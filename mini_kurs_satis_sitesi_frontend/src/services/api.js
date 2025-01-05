const BASE_URL = 'https://localhost:7261/api';

// Public endpoints that don't require authentication
const PUBLIC_ENDPOINTS = [
    '/Courses',
    '/Courses/category',
    '/Courses/'  // Course detail endpoints that start with /Courses/
];

const isPublicEndpoint = (endpoint) => {
    return PUBLIC_ENDPOINTS.some(publicPath => {
        if (publicPath.endsWith('/')) {
            return endpoint.startsWith(publicPath);
        }
        return endpoint === publicPath;
    });
};

let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
    failedQueue.forEach(prom => {
        if (error) {
            prom.reject(error);
        } else {
            prom.resolve(token);
        }
    });
    failedQueue = [];
};

// Token yenileme interceptor'ı
const refreshTokenInterceptor = async (originalRequest) => {
    try {
        if (isRefreshing) {
            // Token yenilenirken bekle
            return new Promise((resolve, reject) => {
                failedQueue.push({ resolve, reject });
            }).then(token => {
                originalRequest.headers['Authorization'] = `Bearer ${token}`;
                return fetch(new Request(originalRequest, {
                    headers: {
                        ...originalRequest.headers,
                        'Authorization': `Bearer ${token}`
                    }
                }));
            });
        }

        isRefreshing = true;
        const refreshToken = localStorage.getItem('refreshToken');

        if (!refreshToken) {
            throw new Error('Refresh token not found');
        }

        const refreshResponse = await fetch(`${BASE_URL}/Auth/CreateTokenByRefreshToken`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ token: refreshToken })
        });

        const refreshData = await refreshResponse.json();

        if (refreshData.isSuccessful && refreshData.data) {
            const { accessToken, refreshToken: newRefreshToken } = refreshData.data;
            
            // Yeni token ve refresh token'ı kaydet
            localStorage.setItem('token', accessToken);
            localStorage.setItem('refreshToken', newRefreshToken);

            // Bekleyen istekleri işle
            processQueue(null, accessToken);

            // Orijinal isteği yeni token ile tekrar dene
            return fetch(new Request(originalRequest, {
                headers: {
                    ...originalRequest.headers,
                    'Authorization': `Bearer ${accessToken}`
                }
            }));
        } else {
            // Token yenileme başarısız - sessiz çıkış yap
            localStorage.removeItem('token');
            localStorage.removeItem('refreshToken');
            processQueue(new Error('Token refresh failed'), null);
            throw new Error('Token refresh failed');
        }
    } catch (error) {
        processQueue(error, null);
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        throw error;
    } finally {
        isRefreshing = false;
    }
};

// Request interceptor
const requestInterceptor = (url, options = {}) => {
    const endpoint = new URL(url).pathname.replace(BASE_URL, '');
    const token = localStorage.getItem('token');

    // Public endpoint'ler için token eklemeye gerek yok
    if (isPublicEndpoint(endpoint)) {
        return {
            ...options,
            headers: {
                'Content-Type': 'application/json',
                ...options.headers
            }
        };
    }

    return {
        ...options,
        headers: {
            'Content-Type': 'application/json',
            ...(token && { 'Authorization': `Bearer ${token}` }),
            ...options.headers
        }
    };
};

// Special endpoints that should fail silently without redirect
const SILENT_FAIL_ENDPOINTS = [
    '/User/purchased-courses'
];

const isSilentFailEndpoint = (endpoint) => {
    return SILENT_FAIL_ENDPOINTS.includes(endpoint);
};

// Response interceptor
const responseInterceptor = async (response, request) => {
    const endpoint = new URL(request.url).pathname.replace(BASE_URL, '');
    
    // Token geçersiz ve henüz retry yapılmamışsa
    if (response.status === 401 && !request._retry) {
        // Public endpoint'ler için auth gerekmez
        if (isPublicEndpoint(endpoint)) {
            return response;
        }

        // Önce token'ın varlığını kontrol et
        const token = localStorage.getItem('token');
        const refreshToken = localStorage.getItem('refreshToken');
        
        // Eğer hiç token yoksa
        if (!token && !refreshToken) {
            throw new Error('User not authenticated');
        }

        try {
            request._retry = true;
            const newResponse = await refreshTokenInterceptor(request);
            
            // Eğer token yenileme başarılıysa yeni response'u dön
            if (newResponse.ok) {
                return newResponse;
            }
            
            return newResponse;
        } catch (error) {
            throw error;
        }
    }
    return response;
};

const apiCall = async (endpoint, options = {}) => {
    try {
        const url = `${BASE_URL}${endpoint}`;
        console.log('API Call:', { url, method: options.method || 'GET', headers: options.headers, body: options.body });

        // Request interceptor'ı uygula
        const interceptedOptions = requestInterceptor(url, options);
        const request = new Request(url, interceptedOptions);

        // İsteği yap
        let response = await fetch(request);

        // Response interceptor'ı uygula
        try {
            response = await responseInterceptor(response, request);
        } catch (error) {
            // Eğer sessiz hata vermesi gereken endpoint ise ve authentication hatası ise
            if (error.message === 'User not authenticated' && isSilentFailEndpoint(endpoint)) {
                return {
                    data: [],
                    statusCode: 401,
                    isSuccessful: false,
                    error: null
                };
            }
            if (error.message === 'User not authenticated') {
                throw {
                    status: 401,
                    error: 'Lütfen giriş yapın',
                    data: null
                };
            }
            throw error;
        }

        const data = await response.json();
        console.log('API Response:', data);

        if (!response.ok) {
            throw {
                status: response.status,
                error: data.error?.errors?.[0] || data.error || 'API error',
                data
            };
        }

        return {
            ...data,
            statusCode: response.status
        };
    } catch (error) {
        console.error('API Call Failed:', error);
        
        // Eğer sessiz hata vermesi gereken endpoint ise boş data dön
        if (endpoint === '/User/purchased-courses' && (error.status === 401 || error.status === 403)) {
            return {
                data: [],
                statusCode: error.status,
                isSuccessful: false,
                error: null
            };
        }
        
        throw {
            status: error.status || 500,
            error: error.error || 'Sunucu ile iletişim kurulamadı',
            data: error.data
        };
    }
};

// Auth endpoints
export const authAPI = {
    login: async (credentials) => {
        const response = await apiCall('/Auth/CreateToken', {
            method: 'POST',
            body: JSON.stringify(credentials)
        });
        
        // API yanıt yapısını kontrol et
        if (response.isSuccessful && response.data) {
            const { accessToken, refreshToken } = response.data;
            if (accessToken && refreshToken) {
                localStorage.setItem('token', accessToken);
                localStorage.setItem('refreshToken', refreshToken);
            }
        }
        
        return response;
    },
    refreshToken: async (token) => {
        const response = await apiCall('/Auth/CreateTokenByRefreshToken', {
            method: 'POST',
            body: JSON.stringify({ token })
        });
        
        // API yanıt yapısını kontrol et
        if (response.isSuccessful && response.data) {
            const { accessToken, refreshToken } = response.data;
            if (accessToken && refreshToken) {
                localStorage.setItem('token', accessToken);
                localStorage.setItem('refreshToken', refreshToken);
            }
        }
        
        return response;
    },
    logout: () => {
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
    }
};

// User endpoints
export const userAPI = {
    getCurrentUser: () => apiCall('/User'),
    register: (userData) => apiCall('/User', {
        method: 'POST',
        body: JSON.stringify(userData)
    }),
    updateProfile: (updateData) => apiCall('/User/profile', {
        method: 'PUT',
        body: JSON.stringify(updateData)
    }),
    getPurchasedCourses: () => apiCall('/User/purchased-courses')
};

// Course endpoints
export const courseAPI = {
    getInstructorCourses: () => apiCall('/Courses/instructor-courses'),
    createCourse: (courseData) => apiCall('/Courses', {
        method: 'POST',
        body: JSON.stringify(courseData)
    }),
    updateCourse: (id, courseData) => apiCall(`/Courses/${id}`, {
        method: 'PUT',
        body: JSON.stringify(courseData)
    }),
    getCoursesByCategory: (category) => apiCall('/Courses/category', {
        method: 'POST',
        body: JSON.stringify({ category })
    }),
    getAllCourses: () => apiCall('/Courses'),
    getCourseById: (id) => apiCall(`/Courses/${id}`)
};

// Order endpoints
export const orderAPI = {
    getOrderHistory: () => apiCall('/Orders'),
    createOrder: (orderData) => {
        console.log('Creating order with API:', orderData);
        return apiCall('/Orders', {
            method: 'POST',
            body: JSON.stringify(orderData)
        });
    }
};

// Payment endpoints
export const paymentAPI = {
    processPayment: (paymentData) => {
        console.log('Processing payment with API:', paymentData);
        return apiCall('/Payments', {
            method: 'POST',
            body: JSON.stringify(paymentData)
        });
    }
};

export default {
    auth: authAPI,
    user: userAPI,
    course: courseAPI,
    order: orderAPI,
    payment: paymentAPI
}; 