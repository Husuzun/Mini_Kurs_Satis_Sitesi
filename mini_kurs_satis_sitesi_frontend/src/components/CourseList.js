import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import LoadingSpinner from './LoadingSpinner';
import { 
    Card, 
    CardMedia, 
    CardContent, 
    CardActionArea,
    Typography,
    Box,
    Chip,
    Avatar,
    Divider,
    Container
} from '@mui/material';
import { Person as PersonIcon } from '@mui/icons-material';
import { useCourse } from '../contexts/CourseContext';
import { useUI } from '../contexts/UIContext';
import api from '../services/api';
import { CATEGORIES } from '../constants/categories';
import { DEFAULT_COURSE_IMAGE } from '../constants/images';

const CourseList = () => {
    const { courses, setCourses } = useCourse();
    const { loading, showLoading, hideLoading } = useUI();
    const [selectedCategory, setSelectedCategory] = useState('all');
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchCourses = async () => {
            showLoading();
            try {
                let response;
                if (selectedCategory && selectedCategory !== 'all') {
                    response = await api.course.getCoursesByCategory(selectedCategory);
                } else {
                    response = await api.course.getAllCourses();
                }

                if (response.isSuccessful) {
                    // Sadece aktif kursları filtrele
                    const activeCourses = response.data.filter(course => course.isActive);
                    setCourses(activeCourses);
                } else {
                    setError('Kurslar yüklenirken bir hata oluştu');
                }
            } catch (error) {
                console.error('Error fetching courses:', error);
                setError('Kurslar yüklenirken bir hata oluştu');
            } finally {
                hideLoading();
            }
        };

        fetchCourses();
    }, [selectedCategory, showLoading, hideLoading, setCourses]);

    const handleCategoryClick = async (category) => {
        setSelectedCategory(category);
        showLoading();
        setError('');
        
        try {
            let response;
            
            if (category === 'all') {
                response = await api.course.getAllCourses();
            } else {
                response = await api.course.getCoursesByCategory(category);
            }
            
            if (response.isSuccessful) {
                // Sadece aktif kursları filtrele
                const activeCourses = response.data.filter(course => course.isActive);
                setCourses(activeCourses);
            } else {
                setError('Kurslar yüklenirken bir hata oluştu');
            }
        } catch (error) {
            console.error('Error fetching courses:', error);
            setError('Kurslar yüklenirken bir hata oluştu');
        } finally {
            hideLoading();
        }
    };

    if (error) return (
        <Box sx={{ 
            color: '#dc3545',
            textAlign: 'center',
            padding: '20px',
            backgroundColor: '#fff',
            borderRadius: '8px',
            boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
            margin: '20px auto',
            maxWidth: '600px'
        }}>
            {error}
        </Box>
    );

    return (
        <Container maxWidth="xl" sx={{ mt: 2, pb: 6 }}>
            {/* Categories Section */}
            <Box 
                sx={{ 
                    display: 'flex',
                    justifyContent: 'center',
                    mb: 4,
                    gap: 1,
                    flexWrap: 'wrap',
                    mt: 2
                }}
            >
                {CATEGORIES.map(category => (
                    <Box
                        key={category}
                        onClick={() => handleCategoryClick(category)}
                        sx={{
                            px: 2,
                            py: 1,
                            cursor: 'pointer',
                            fontSize: '0.85rem',
                            color: selectedCategory === category ? 'white' : 'text.secondary',
                            backgroundColor: selectedCategory === category ? 'primary.main' : 'transparent',
                            transition: 'all 0.2s ease-in-out',
                            '&:hover': {
                                backgroundColor: selectedCategory === category 
                                    ? 'primary.dark'
                                    : 'rgba(0, 0, 0, 0.04)',
                            },
                            borderRadius: 1
                        }}
                    >
                        {category === 'all' ? 'Tüm Kurslar' : category}
                    </Box>
                ))}
            </Box>

            {/* Courses Grid with Localized Loading */}
            {loading ? (
                <LoadingSpinner 
                    size={30} 
                    minHeight="400px"
                    containerStyle={{
                        backgroundColor: 'rgba(255, 255, 255, 0.8)',
                        borderRadius: 1,
                        my: 2
                    }}
                />
            ) : (
                <Box display="grid" gridTemplateColumns="repeat(12, 1fr)" gap={3}>
                    {courses.map(course => (
                        <Box gridColumn={{ xs: "span 12", sm: "span 6", md: "span 4", lg: "span 3" }} key={course.id}>
                            <Card 
                                component={Link} 
                                to={`/course/${course.id}`}
                                sx={{ 
                                    height: '100%',
                                    display: 'flex',
                                    flexDirection: 'column',
                                    transition: 'all 0.3s ease',
                                    textDecoration: 'none',
                                    '&:hover': {
                                        transform: 'translateY(-8px)',
                                        boxShadow: '0 8px 20px rgba(0,0,0,0.1)'
                                    }
                                }}
                            >
                                <CardActionArea>
                                    <CardMedia
                                        component="img"
                                        height="180"
                                        image={DEFAULT_COURSE_IMAGE}
                                        alt={course.name}
                                        sx={{
                                            objectFit: 'cover',
                                            backgroundColor: '#e9ecef'
                                        }}
                                    />
                                    <CardContent>
                                        <Typography 
                                            gutterBottom 
                                            variant="h6" 
                                            component="h2"
                                            sx={{ 
                                                fontWeight: 700,
                                                fontSize: '1.1rem',
                                                mb: 1,
                                                color: 'text.primary'
                                            }}
                                        >
                                            {course.name}
                                        </Typography>
                                        <Typography 
                                            variant="body2" 
                                            color="text.secondary"
                                            sx={{ 
                                                mb: 2,
                                                minHeight: '40px',
                                                display: '-webkit-box',
                                                WebkitLineClamp: 2,
                                                WebkitBoxOrient: 'vertical',
                                                overflow: 'hidden',
                                                textOverflow: 'ellipsis'
                                            }}
                                        >
                                            {course.description}
                                        </Typography>
                                        <Box sx={{ mb: 2 }}>
                                            <Chip 
                                                label={course.category}
                                                size="small"
                                                variant="outlined"
                                                sx={{ 
                                                    borderRadius: '12px',
                                                    fontSize: '0.75rem'
                                                }}
                                            />
                                        </Box>
                                        <Divider sx={{ my: 1 }} />
                                        <Box 
                                            sx={{ 
                                                mt: 2,
                                                display: 'flex',
                                                justifyContent: 'space-between',
                                                alignItems: 'center'
                                            }}
                                        >
                                            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                                <Avatar 
                                                    sx={{ 
                                                        width: 24, 
                                                        height: 24,
                                                        bgcolor: 'primary.main',
                                                        fontSize: '0.875rem'
                                                    }}
                                                >
                                                    <PersonIcon sx={{ fontSize: '1rem' }} />
                                                </Avatar>
                                                <Typography 
                                                    variant="body2" 
                                                    color="text.secondary"
                                                    sx={{ fontSize: '0.875rem' }}
                                                >
                                                    {course.instructorName}
                                                </Typography>
                                            </Box>
                                            <Typography 
                                                variant="h6" 
                                                color="primary"
                                                sx={{ 
                                                    fontWeight: 700,
                                                    fontSize: '1.25rem'
                                                }}
                                            >
                                                {course.price} TL
                                            </Typography>
                                        </Box>
                                    </CardContent>
                                </CardActionArea>
                            </Card>
                        </Box>
                    ))}
                </Box>
            )}
        </Container>
    );
};

export default React.memo(CourseList); 