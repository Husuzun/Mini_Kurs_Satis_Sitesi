import React from 'react';
import { Navigate } from 'react-router-dom';
import { useApp } from '../contexts/AppContext';
import { useUI } from '../contexts/UIContext';
import { jwtDecode } from 'jwt-decode';

const InstructorRoute = ({ children }) => {
    const { isAuthenticated } = useApp();
    const { addNotification } = useUI();

    const isInstructor = () => {
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                console.error('Token bulunamadı - Instructor kontrolü');
                return false;
            }
            
            const decodedToken = jwtDecode(token);
            console.log('Decoded Token:', decodedToken);
            
            const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            console.log('Kullanıcı Rolü:', role);
            
            return role === 'Instructor';
        } catch (error) {
            console.error('Instructor rolü kontrol hatası:', error);
            return false;
        }
    };

    if (!isAuthenticated) {
        addNotification('Lütfen önce giriş yapın', 'warning');
        return <Navigate to="/" replace />;
    }

    if (!isInstructor()) {
        addNotification('Bu sayfaya erişim yetkiniz yok', 'error');
        return <Navigate to="/profile" replace />;
    }

    return children;
};

export default InstructorRoute; 