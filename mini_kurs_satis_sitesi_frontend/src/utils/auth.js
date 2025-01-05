import { jwtDecode } from 'jwt-decode';

export const getToken = () => localStorage.getItem('token');

export const getDecodedToken = () => {
    try {
        const token = getToken();
        return token ? jwtDecode(token) : null;
    } catch (error) {
        console.error('Token decode error:', error);
        return null;
    }
};

export const isInstructor = () => {
    try {
        const decodedToken = getDecodedToken();
        if (!decodedToken) return false;
        
        const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        return role === 'Instructor';
    } catch (error) {
        console.error('Instructor role check error:', error);
        return false;
    }
}; 