import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useCart } from '../contexts/CartContext';
import { useUI } from '../contexts/UIContext';
import api from '../services/api';
import {
    Container,
    Typography,
    Button,
    Card,
    CardMedia,
    Box,
    Chip,
    Divider,
    Paper,
    Avatar
} from '@mui/material';
import {
    Person as PersonIcon,
    AccessTime as AccessTimeIcon,
    Category as CategoryIcon,
    DateRange as DateRangeIcon
} from '@mui/icons-material';
import { useCourse } from '../contexts/CourseContext';

const DEFAULT_IMAGE = 'https://placehold.co/800x400/e9ecef/495057?text=Kurs+Görseli&font=roboto';

const CourseDetail = () => {
    const { id } = useParams();
    const { addToCart, cartItems } = useCart();
    const { addNotification, showLoading, hideLoading } = useUI();
    const [course, setCourse] = useState(null);
    const [error, setError] = useState('');
    const { isCoursePurchased, loadPurchasedCourses } = useCourse();

    useEffect(() => {
        const fetchCourse = async () => {
            showLoading();
            try {
                const response = await api.course.getCourseById(id);
                if (response && response.data) {
                    setCourse(response.data);
                    await loadPurchasedCourses();
                }
            } catch (error) {
                setError('Kurs detayları yüklenirken bir hata oluştu');
                console.error('Error fetching course:', error);
            } finally {
                hideLoading();
            }
        };

        fetchCourse();
    }, [id, showLoading, hideLoading, loadPurchasedCourses]);

    const handleAddToCart = () => {
        if (isCoursePurchased(course.id)) {
            addNotification('Bu kursu zaten satın aldınız', 'warning');
            return;
        }

        if (cartItems.some(item => item.id === course.id)) {
            addNotification('Bu kurs zaten sepetinizde', 'warning');
            return;
        }
        addToCart(course);
    };

    const formatDate = (dateString) => {
        if (!dateString) return '';
        return dateString.split('T')[0].split('-').reverse().join('/');
    };

    if (error) {
        return (
            <Container maxWidth="lg" sx={{ mt: 12, textAlign: 'center' }}>
                <Typography color="error" variant="h6">
                    {error}
                </Typography>
            </Container>
        );
    }

    if (!course) {
        return null;
    }

    return (
        <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
            <Box display="grid" gridTemplateColumns="repeat(12, 1fr)" gap={4}>
                <Box gridColumn={{ xs: "span 12", md: "span 8" }}>
                    <Card elevation={0} sx={{ mb: 4 }}>
                        <CardMedia
                            component="img"
                            height="400"
                            image={course.imageUrl || DEFAULT_IMAGE}
                            alt={course.name}
                            sx={{ borderRadius: 2 }}
                        />
                    </Card>

                    <Typography variant="h4" component="h1" gutterBottom sx={{ fontWeight: 700 }}>
                        {course.name}
                    </Typography>

                    <Box sx={{ mb: 4, display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                        <Chip
                            icon={<CategoryIcon />}
                            label={course.category}
                            variant="outlined"
                        />
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                            <Avatar sx={{ width: 24, height: 24, bgcolor: 'primary.main' }}>
                                <PersonIcon sx={{ fontSize: '1rem' }} />
                            </Avatar>
                            <Typography variant="body2">
                                {course.instructorName}
                            </Typography>
                        </Box>
                        <Chip
                            icon={<DateRangeIcon />}
                            label={`Oluşturulma: ${formatDate(course.createdDate)}`}
                            variant="outlined"
                        />
                        {course.updatedDate && (
                            <Chip
                                icon={<DateRangeIcon />}
                                label={`Son Güncelleme: ${formatDate(course.updatedDate)}`}
                                variant="outlined"
                            />
                        )}
                    </Box>

                    <Typography variant="h6" gutterBottom sx={{ fontWeight: 600 }}>
                        Kurs Açıklaması
                    </Typography>
                    <Typography variant="body1" paragraph sx={{ color: 'text.secondary' }}>
                        {course.description}
                    </Typography>
                </Box>

                <Box gridColumn={{ xs: "span 12", md: "span 4" }}>
                    <Paper elevation={2} sx={{ p: 3, borderRadius: 2, position: 'sticky', top: '100px' }}>
                        <Typography variant="h4" gutterBottom sx={{ fontWeight: 700, color: 'primary.main' }}>
                            {course.price} TL
                        </Typography>
                        
                        {isCoursePurchased(course.id) ? (
                            <Button
                                variant="contained"
                                color="success"
                                fullWidth
                                disabled
                                sx={{ mt: 2 }}
                            >
                                Bu Kursa Sahipsiniz
                            </Button>
                        ) : (
                            <Button
                                variant="contained"
                                color="primary"
                                fullWidth
                                onClick={() => addToCart(course)}
                                sx={{ mt: 2 }}
                            >
                                Sepete Ekle
                            </Button>
                        )}

                        <Divider sx={{ my: 3 }} />

                        <Typography variant="subtitle2" gutterBottom sx={{ fontWeight: 600 }}>
                            Bu kursa dahil olanlar:
                        </Typography>
                        <Box sx={{ mt: 2 }}>
                            <Box sx={{ display: 'flex', alignItems: 'center', mb: 1, gap: 1 }}>
                                <AccessTimeIcon fontSize="small" color="action" />
                                <Typography variant="body2" color="text.secondary">
                                    Ömür boyu erişim
                                </Typography>
                            </Box>
                            <Box sx={{ display: 'flex', alignItems: 'center', mb: 1, gap: 1 }}>
                                <PersonIcon fontSize="small" color="action" />
                                <Typography variant="body2" color="text.secondary">
                                    Deneyimli eğitmen
                                </Typography>
                            </Box>
                        </Box>
                    </Paper>
                </Box>
            </Box>
        </Container>
    );
};

export default CourseDetail; 