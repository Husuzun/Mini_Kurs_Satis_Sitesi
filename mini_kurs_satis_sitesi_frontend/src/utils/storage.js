import { handleError, ERROR_TYPES } from './errorHandler';

export const STORAGE_KEYS = {
    CART: 'cart',
    TOKEN: 'token',
    REFRESH_TOKEN: 'refreshToken',
    THEME: 'theme',
    USER_PREFERENCES: 'userPreferences'
};

class StorageService {
    setItem(key, value) {
        try {
            const serializedValue = typeof value === 'string' ? value : JSON.stringify(value);
            localStorage.setItem(key, serializedValue);
            return true;
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.VALIDATION);
            console.error(`Error setting storage item [${key}]:`, handledError);
            return false;
        }
    }

    getItem(key, defaultValue = null) {
        try {
            const item = localStorage.getItem(key);
            if (item === null) return defaultValue;

            try {
                return JSON.parse(item);
            } catch {
                return item;
            }
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.VALIDATION);
            console.error(`Error getting storage item [${key}]:`, handledError);
            return defaultValue;
        }
    }

    removeItem(key) {
        try {
            localStorage.removeItem(key);
            return true;
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.VALIDATION);
            console.error(`Error removing storage item [${key}]:`, handledError);
            return false;
        }
    }

    clear() {
        try {
            localStorage.clear();
            return true;
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.VALIDATION);
            console.error('Error clearing storage:', handledError);
            return false;
        }
    }

    // Cart specific methods
    getCart() {
        return this.getItem(STORAGE_KEYS.CART, []);
    }

    setCart(cart) {
        return this.setItem(STORAGE_KEYS.CART, cart);
    }

    clearCart() {
        return this.removeItem(STORAGE_KEYS.CART);
    }

    // Auth specific methods
    getToken() {
        return this.getItem(STORAGE_KEYS.TOKEN);
    }

    setToken(token) {
        return this.setItem(STORAGE_KEYS.TOKEN, token);
    }

    getRefreshToken() {
        return this.getItem(STORAGE_KEYS.REFRESH_TOKEN);
    }

    setRefreshToken(token) {
        return this.setItem(STORAGE_KEYS.REFRESH_TOKEN, token);
    }

    clearAuth() {
        this.removeItem(STORAGE_KEYS.TOKEN);
        this.removeItem(STORAGE_KEYS.REFRESH_TOKEN);
    }

    // Theme methods
    getTheme() {
        return this.getItem(STORAGE_KEYS.THEME, 'light');
    }

    setTheme(theme) {
        return this.setItem(STORAGE_KEYS.THEME, theme);
    }
}

export const storage = new StorageService(); 