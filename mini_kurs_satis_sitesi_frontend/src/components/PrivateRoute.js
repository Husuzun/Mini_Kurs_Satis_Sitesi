import React from 'react';
import { Navigate } from 'react-router-dom';
import { useApp } from '../contexts/AppContext';

const PrivateRoute = ({ children }) => {
    const { isAuthenticated } = useApp();

    if (!isAuthenticated) {
        return <Navigate to="/" replace />;
    }

    return children;
};

export default PrivateRoute; 