import React, { createContext, useContext, useState, useCallback, useMemo, useEffect } from 'react';
import { useUI } from './UIContext';
import api from '../services/api';
import { handleError, ERROR_TYPES } from '../utils/errorHandler';
import { LOADING_TYPES } from './UIContext';

const CourseContext = createContext();

export const CourseProvider = ({ children }) => {
    const [courses, setCourses] = useState([]);
    const [purchasedCourses, setPurchasedCourses] = useState([]);
    const [selectedCourse, setSelectedCourse] = useState(null);
    const [searchResults, setSearchResults] = useState(null);
    const { addNotification, showLoading, hideLoading } = useUI();

    const fetchCourses = useCallback(async (category = null) => {
        showLoading(LOADING_TYPES.COURSES);
        try {
            const response = category 
                ? await api.course.getCoursesByCategory(category)
                : await api.course.getAllCourses();
                
            if (response.isSuccessful) {
                setCourses(response.data);
                setSearchResults(null);
            }
            return response;
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.API);
            addNotification(handledError.message, 'error');
            return { isSuccessful: false, error: handledError };
        } finally {
            hideLoading(LOADING_TYPES.COURSES);
        }
    }, [showLoading, hideLoading, addNotification]);

    const searchCourses = useCallback((searchTerm) => {
        if (!searchTerm.trim()) {
            setSearchResults(null);
            return;
        }

        const searchTermLower = searchTerm.toLowerCase();
        const filtered = courses.filter(course => 
            course.name.toLowerCase().includes(searchTermLower) ||
            course.description.toLowerCase().includes(searchTermLower) ||
            course.category.toLowerCase().includes(searchTermLower)
        );
        
        setSearchResults(filtered);
    }, [courses]);

    const fetchCourseById = useCallback(async (courseId) => {
        showLoading(LOADING_TYPES.COURSES);
        try {
            const response = await api.course.getCourseById(courseId);
            if (response.isSuccessful) {
                setSelectedCourse(response.data);
            }
            return response;
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.API);
            addNotification(handledError.message, 'error');
            return { isSuccessful: false, error: handledError };
        } finally {
            hideLoading(LOADING_TYPES.COURSES);
        }
    }, [showLoading, hideLoading, addNotification]);

    const loadPurchasedCourses = useCallback(async () => {
        showLoading(LOADING_TYPES.COURSES);
        try {
            const response = await api.user.getPurchasedCourses();
            if (response && response.data && response.data.purchasedCourses) {
                setPurchasedCourses(response.data.purchasedCourses);
            } else {
                setPurchasedCourses([]);
            }
        } catch (error) {
            const handledError = handleError(error, ERROR_TYPES.API);
            console.error('Error loading purchased courses:', handledError);
            setPurchasedCourses([]);
        } finally {
            hideLoading(LOADING_TYPES.COURSES);
        }
    }, [showLoading, hideLoading]);

    const isCoursePurchased = useCallback((courseId) => {
        if (!courseId || !purchasedCourses || !Array.isArray(purchasedCourses) || purchasedCourses.length === 0) return false;
        return purchasedCourses.some(course => course.courseId === courseId);
    }, [purchasedCourses]);

    useEffect(() => {
        loadPurchasedCourses();
    }, [loadPurchasedCourses]);

    const contextValue = useMemo(() => ({
        courses: searchResults || courses,
        selectedCourse,
        setCourses,
        fetchCourses,
        fetchCourseById,
        searchCourses,
        isSearching: searchResults !== null,
        isCoursePurchased,
        loadPurchasedCourses
    }), [courses, selectedCourse, setCourses, fetchCourses, fetchCourseById, searchCourses, searchResults, isCoursePurchased, loadPurchasedCourses]);

    return (
        <CourseContext.Provider value={contextValue}>
            {children}
        </CourseContext.Provider>
    );
};

export const useCourse = () => useContext(CourseContext); 