import React, { createContext, useContext, useState, useEffect, useCallback } from 'react';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import { useUI } from './UIContext';
import api from '../services/api';
import { handleError, ERROR_TYPES } from '../utils/errorHandler';
import { LOADING_TYPES } from './UIContext';
import { storage } from '../utils/storage';

const AppContext = createContext();

export const AppProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const { addNotification, showLoading, hideLoading } = useUI();

    const fetchUserData = async () => {
        showLoading(LOADING_TYPES.PROFILE);
        try {
            const response = await api.user.getCurrentUser();
            if (response.isSuccessful) {
                const token = storage.getToken();
                if (!token) {
                    throw new Error('Token not found');
                }

                const decodedToken = jwtDecode(token);
                const userWithRole = {
                    ...response.data,
                    role: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
                };
                setUser(userWithRole);
                setIsAuthenticated(true);
                return true;
            }
            return false;
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.AUTH);
            console.error('Error fetching user data:', handledError);
            return false;
        } finally {
            hideLoading(LOADING_TYPES.PROFILE);
        }
    };

    const checkAuth = useCallback(async () => {
        showLoading(LOADING_TYPES.AUTH);
        try {
            const token = storage.getToken();
            if (!token) {
                setIsAuthenticated(false);
                setUser(null);
                return false;
            }

            const success = await fetchUserData();
            if (!success) {
                storage.clearAuth();
                setIsAuthenticated(false);
                setUser(null);
            }
            return success;
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.AUTH);
            console.error('Auth check error:', handledError);
            setIsAuthenticated(false);
            setUser(null);
            return false;
        } finally {
            hideLoading(LOADING_TYPES.AUTH);
        }
    }, []);

    useEffect(() => {
        checkAuth();
    }, [checkAuth]);

    const handleLogin = async (credentials) => {
        showLoading(LOADING_TYPES.AUTH);
        try {
            if (!credentials?.email || !credentials?.password) {
                throw new Error('Invalid credentials');
            }

            const response = await api.auth.login(credentials);
            if (response.isSuccessful) {
                const { accessToken, refreshToken } = response.data;
                storage.setToken(accessToken);
                storage.setRefreshToken(refreshToken);
                
                const success = await fetchUserData();
                if (success) {
                    addNotification('Giriş başarılı', 'success');
                    return { success: true };
                }
                throw new Error('Failed to fetch user data');
            }
            throw new Error(response.error || 'Login failed');
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.AUTH);
            console.error('Login error:', handledError);
            addNotification(handledError.message, 'error');
            return { success: false, error: handledError };
        } finally {
            hideLoading(LOADING_TYPES.AUTH);
        }
    };

    const logout = useCallback(() => {
        showLoading(LOADING_TYPES.AUTH);
        try {
            storage.clearAuth();
            setUser(null);
            setIsAuthenticated(false);
            addNotification('Çıkış yapıldı', 'info');
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.AUTH);
            console.error('Logout error:', handledError);
            addNotification(handledError.message, 'error');
        } finally {
            hideLoading(LOADING_TYPES.AUTH);
        }
    }, [addNotification, showLoading, hideLoading]);

    const isInstructor = useCallback(() => {
        try {
            return user?.role === 'Instructor';
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.AUTH);
            console.error('Role check error:', handledError);
            return false;
        }
    }, [user]);

    return (
        <AppContext.Provider value={{
            user,
            setUser,
            isAuthenticated,
            login: handleLogin,
            logout,
            isInstructor,
            checkAuth
        }}>
            {children}
        </AppContext.Provider>
    );
};

export const useApp = () => useContext(AppContext); 