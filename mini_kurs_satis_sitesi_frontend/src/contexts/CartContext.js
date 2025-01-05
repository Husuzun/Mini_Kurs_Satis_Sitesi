import React, { createContext, useContext, useState, useEffect } from 'react';
import { useUI } from './UIContext';
import { useCourse } from './CourseContext';
import api from '../services/api';
import { handleError, ERROR_TYPES } from '../utils/errorHandler';
import { LOADING_TYPES } from './UIContext';
import { storage } from '../utils/storage';

const CartContext = createContext();

export const CartProvider = ({ children }) => {
    const [cartItems, setCartItems] = useState(() => {
        try {
            return storage.getCart();
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.VALIDATION);
            console.error('Error loading cart from storage:', handledError);
            return [];
        }
    });

    const { addNotification, showLoading, hideLoading } = useUI();
    const { isCoursePurchased, loadPurchasedCourses } = useCourse();

    useEffect(() => {
        try {
            storage.setCart(cartItems);
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.VALIDATION);
            console.error('Error saving cart to storage:', handledError);
        }
    }, [cartItems]);

    const addToCart = (course) => {
        try {
            if (!course || !course.id) {
                throw new Error('Invalid course data');
            }

            if (isCoursePurchased(course.id)) {
                addNotification('Bu kursu zaten satın aldınız', 'warning');
                return;
            }

            if (cartItems.some(item => item.id === course.id)) {
                addNotification('Bu kurs zaten sepetinizde', 'warning');
                return;
            }

            setCartItems(prev => [...prev, course]);
            addNotification('Kurs sepete eklendi', 'success');
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.VALIDATION);
            addNotification(handledError.message, 'error');
        }
    };

    const removeFromCart = (courseId) => {
        try {
            if (!courseId) {
                throw new Error('Invalid course ID');
            }

            setCartItems(prev => prev.filter(item => item.id !== courseId));
            addNotification('Kurs sepetten çıkarıldı', 'info');
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.VALIDATION);
            addNotification(handledError.message, 'error');
        }
    };

    const clearCart = async () => {
        showLoading(LOADING_TYPES.CART);
        try {
            setCartItems([]);
            storage.clearCart();
            await loadPurchasedCourses();
            addNotification('Sepet temizlendi', 'info');
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.UNKNOWN);
            addNotification(handledError.message, 'error');
        } finally {
            hideLoading(LOADING_TYPES.CART);
        }
    };

    const getCartTotal = () => {
        try {
            return cartItems.reduce((total, item) => {
                if (!item.price || typeof item.price !== 'number') {
                    throw new Error('Invalid price data');
                }
                return total + item.price;
            }, 0);
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.VALIDATION);
            addNotification(handledError.message, 'error');
            return 0;
        }
    };

    const checkout = async () => {
        showLoading(LOADING_TYPES.PAYMENT);
        try {
            const orderData = {
                courses: cartItems.map(item => item.id),
                totalAmount: getCartTotal()
            };

            const response = await api.order.createOrder(orderData);
            
            if (response.isSuccessful) {
                await clearCart();
                addNotification('Siparişiniz başarıyla tamamlandı', 'success');
                return { success: true };
            } else {
                throw new Error(response.error || 'Sipariş işlemi başarısız oldu');
            }
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.API);
            addNotification(handledError.message, 'error');
            return { success: false, error: handledError };
        } finally {
            hideLoading(LOADING_TYPES.PAYMENT);
        }
    };

    return (
        <CartContext.Provider value={{ 
            cartItems, 
            addToCart, 
            removeFromCart, 
            clearCart,
            getCartTotal,
            checkout,
            cartCount: cartItems.length 
        }}>
            {children}
        </CartContext.Provider>
    );
};

export const useCart = () => useContext(CartContext); 