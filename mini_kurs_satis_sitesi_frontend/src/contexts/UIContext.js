import React, { createContext, useContext, useState, useCallback } from 'react';
import '../styles/UIContext.css';
import { storage } from '../utils/storage';

const UIContext = createContext();

export const LOADING_TYPES = {
    AUTH: 'auth',
    COURSES: 'courses',
    CART: 'cart',
    PAYMENT: 'payment',
    PROFILE: 'profile',
    GLOBAL: 'global'
};

export const UIProvider = ({ children }) => {
    const [notifications, setNotifications] = useState([]);
    const [loadingStates, setLoadingStates] = useState({
        [LOADING_TYPES.AUTH]: false,
        [LOADING_TYPES.COURSES]: false,
        [LOADING_TYPES.CART]: false,
        [LOADING_TYPES.PAYMENT]: false,
        [LOADING_TYPES.PROFILE]: false,
        [LOADING_TYPES.GLOBAL]: false
    });
    const [theme, setTheme] = useState(() => storage.getTheme());

    const addNotification = useCallback((message, type = 'info') => {
        const id = Date.now();
        const notification = { id, message, type };
        setNotifications(prev => [...prev, notification]);
        setTimeout(() => removeNotification(id), 3000);
        return notification;
    }, []);

    const removeNotification = useCallback((id) => {
        setNotifications(prev => prev.filter(n => n.id !== id));
    }, []);

    const showLoading = useCallback((type = LOADING_TYPES.GLOBAL) => {
        setLoadingStates(prev => ({
            ...prev,
            [type]: true
        }));
    }, []);

    const hideLoading = useCallback((type = LOADING_TYPES.GLOBAL) => {
        setLoadingStates(prev => ({
            ...prev,
            [type]: false
        }));
    }, []);

    const isLoading = useCallback((type = LOADING_TYPES.GLOBAL) => {
        return loadingStates[type];
    }, [loadingStates]);

    const isAnyLoading = useCallback(() => {
        return Object.values(loadingStates).some(state => state);
    }, [loadingStates]);

    const handleThemeChange = useCallback((newTheme) => {
        setTheme(newTheme);
        storage.setTheme(newTheme);
    }, []);

    return (
        <UIContext.Provider value={{
            notifications,
            addNotification,
            removeNotification,
            showLoading,
            hideLoading,
            isLoading,
            isAnyLoading,
            theme,
            setTheme: handleThemeChange
        }}>
            {children}
            <div className="notifications-container">
                {notifications.map(notification => (
                    <div 
                        key={notification.id} 
                        className={`notification ${notification.type}`}
                        onClick={() => removeNotification(notification.id)}
                    >
                        {notification.message}
                    </div>
                ))}
            </div>
            {isAnyLoading() && (
                <div className="loading-overlay">
                    <div className="spinner"></div>
                </div>
            )}
        </UIContext.Provider>
    );
};

export const useUI = () => useContext(UIContext); 