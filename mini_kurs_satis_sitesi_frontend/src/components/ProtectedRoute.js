import { Navigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

export const ProtectedInstructorRoute = ({ children }) => {
    const isInstructor = () => {
        try {
            const token = localStorage.getItem('token');
            if (!token) return false;
            const decodedToken = jwtDecode(token);
            return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Instructor';
        } catch {
            return false;
        }
    };

    return isInstructor() ? children : <Navigate to="/login" />;
}; 