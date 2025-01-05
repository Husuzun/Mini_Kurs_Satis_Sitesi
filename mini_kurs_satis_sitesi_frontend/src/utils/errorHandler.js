export const ERROR_TYPES = {
    AUTH: 'auth',
    API: 'api',
    NETWORK: 'network',
    VALIDATION: 'validation',
    UNKNOWN: 'unknown'
};

export const ERROR_MESSAGES = {
    [ERROR_TYPES.AUTH]: {
        default: 'Oturum hatası oluştu',
        login: 'Giriş yapılırken bir hata oluştu',
        token: 'Oturum süreniz doldu',
        permission: 'Bu işlem için yetkiniz bulunmuyor'
    },
    [ERROR_TYPES.API]: {
        default: 'Sunucu hatası oluştu',
        notFound: 'İstenilen kaynak bulunamadı',
        serverError: 'Sunucu hatası'
    },
    [ERROR_TYPES.NETWORK]: {
        default: 'Bağlantı hatası oluştu',
        offline: 'İnternet bağlantınızı kontrol edin'
    },
    [ERROR_TYPES.VALIDATION]: {
        default: 'Geçersiz veri',
        required: 'Bu alan zorunludur',
        invalid: 'Geçersiz değer'
    },
    [ERROR_TYPES.UNKNOWN]: {
        default: 'Beklenmeyen bir hata oluştu'
    }
};

export const handleError = (error, type = ERROR_TYPES.UNKNOWN) => {
    // API yanıtlarından gelen hataları işle
    if (error?.response?.data?.error) {
        return {
            type,
            message: error.response.data.error,
            details: error.response.data
        };
    }

    // Network hatalarını işle
    if (error?.message === 'Network Error') {
        return {
            type: ERROR_TYPES.NETWORK,
            message: ERROR_MESSAGES[ERROR_TYPES.NETWORK].offline,
            details: error
        };
    }

    // 401 hatalarını işle
    if (error?.response?.status === 401) {
        return {
            type: ERROR_TYPES.AUTH,
            message: ERROR_MESSAGES[ERROR_TYPES.AUTH].token,
            details: error
        };
    }

    // 403 hatalarını işle
    if (error?.response?.status === 403) {
        return {
            type: ERROR_TYPES.AUTH,
            message: ERROR_MESSAGES[ERROR_TYPES.AUTH].permission,
            details: error
        };
    }

    // 404 hatalarını işle
    if (error?.response?.status === 404) {
        return {
            type: ERROR_TYPES.API,
            message: ERROR_MESSAGES[ERROR_TYPES.API].notFound,
            details: error
        };
    }

    // 500 hatalarını işle
    if (error?.response?.status >= 500) {
        return {
            type: ERROR_TYPES.API,
            message: ERROR_MESSAGES[ERROR_TYPES.API].serverError,
            details: error
        };
    }

    // Diğer tüm hataları işle
    return {
        type,
        message: ERROR_MESSAGES[type]?.default || ERROR_MESSAGES[ERROR_TYPES.UNKNOWN].default,
        details: error
    };
};

export const isApiError = (error) => {
    return error?.type === ERROR_TYPES.API;
};

export const isAuthError = (error) => {
    return error?.type === ERROR_TYPES.AUTH;
};

export const isNetworkError = (error) => {
    return error?.type === ERROR_TYPES.NETWORK;
};

export const isValidationError = (error) => {
    return error?.type === ERROR_TYPES.VALIDATION;
}; 