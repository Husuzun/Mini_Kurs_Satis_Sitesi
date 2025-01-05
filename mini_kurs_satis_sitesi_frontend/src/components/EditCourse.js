import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useUI } from '../contexts/UIContext';
import api from '../services/api';
import { CATEGORIES } from '../constants/categories';
import {
    Container,
    Typography,
    TextField,
    Button,
    Card,
    CardContent,
    MenuItem,
    Box,
    Alert,
    FormControlLabel,
    Switch
} from '@mui/material';

const EditCourse = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const { addNotification, showLoading, hideLoading } = useUI();
    const [error, setError] = useState('');
    const [formData, setFormData] = useState({
        name: '',
        description: '',
        price: '',
        category: '',
        isActive: true
    });

    useEffect(() => {
        const fetchCourse = async () => {
            showLoading();
            try {
                const response = await api.course.getCourseById(id);
                if (response.isSuccessful) {
                    setFormData({
                        name: response.data.name,
                        description: response.data.description,
                        price: response.data.price,
                        category: response.data.category,
                        isActive: response.data.isActive
                    });
                } else {
                    setError('Kurs bilgileri alınamadı');
                }
            } catch (error) {
                console.error('Error fetching course:', error);
                setError('Kurs bilgileri alınamadı');
            } finally {
                hideLoading();
            }
        };

        fetchCourse();
    }, [id, showLoading, hideLoading]);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : name === 'price' ? Number(value) : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        showLoading();

        try {
            const response = await api.course.updateCourse(id, formData);
            
            if (response.isSuccessful) {
                addNotification('Kurs başarıyla güncellendi', 'success');
                navigate('/profile');
            } else {
                const errorMessage = response.error?.message || response.error || 'Kurs güncellenirken bir hata oluştu';
                setError(errorMessage);
                addNotification(errorMessage, 'error');
            }
        } catch (error) {
            console.error('Course update error:', error);
            const errorMessage = error.message || 'Kurs güncellenirken bir hata oluştu';
            setError(errorMessage);
            addNotification(errorMessage, 'error');
        } finally {
            hideLoading();
        }
    };

    return (
        <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
            <Box display="grid" gridTemplateColumns="repeat(12, 1fr)" gap={3}>
                <Box gridColumn="span 12">
                    <Typography variant="h4" component="h1" gutterBottom>
                        Kurs Düzenle
                    </Typography>
                </Box>
                {error && (
                    <Box gridColumn="span 12">
                        <Alert severity="error">{error}</Alert>
                    </Box>
                )}
                <Box gridColumn="span 12">
                    <form onSubmit={handleSubmit}>
                        <Box display="grid" gridTemplateColumns="repeat(12, 1fr)" gap={3}>
                            <Box gridColumn="span 12">
                                <TextField
                                    fullWidth
                                    label="Kurs Adı"
                                    name="name"
                                    value={formData.name}
                                    onChange={handleChange}
                                    required
                                    variant="outlined"
                                />
                            </Box>
                            <Box gridColumn="span 12">
                                <TextField
                                    fullWidth
                                    label="Kurs Açıklaması"
                                    name="description"
                                    value={formData.description}
                                    onChange={handleChange}
                                    required
                                    multiline
                                    rows={4}
                                    variant="outlined"
                                />
                            </Box>
                            <Box gridColumn={{ xs: "span 12", sm: "span 6" }}>
                                <TextField
                                    fullWidth
                                    label="Fiyat"
                                    name="price"
                                    type="number"
                                    value={formData.price}
                                    onChange={handleChange}
                                    required
                                    variant="outlined"
                                    sx={{
                                        '& input::-webkit-outer-spin-button, & input::-webkit-inner-spin-button': {
                                            '-webkit-appearance': 'none',
                                            margin: 0
                                        }
                                    }}
                                    slotProps={{
                                        input: { 
                                            min: 0, 
                                            step: "0.01"
                                        }
                                    }}
                                />
                            </Box>
                            <Box gridColumn={{ xs: "span 12", sm: "span 6" }}>
                                <TextField
                                    fullWidth
                                    select
                                    label="Kategori"
                                    name="category"
                                    value={formData.category}
                                    onChange={handleChange}
                                    required
                                    variant="outlined"
                                >
                                    {CATEGORIES.filter(category => category !== 'all').map((category) => (
                                        <MenuItem key={category} value={category}>
                                            {category}
                                        </MenuItem>
                                    ))}
                                </TextField>
                            </Box>
                            <Box gridColumn="span 12">
                                <FormControlLabel
                                    control={
                                        <Switch
                                            checked={formData.isActive}
                                            onChange={handleChange}
                                            name="isActive"
                                            color="primary"
                                        />
                                    }
                                    label="Kurs Aktif"
                                />
                            </Box>
                            <Box gridColumn="span 12">
                                <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                                    <Button
                                        variant="outlined"
                                        onClick={() => navigate('/profile')}
                                    >
                                        İptal
                                    </Button>
                                    <Button
                                        type="submit"
                                        variant="contained"
                                        color="primary"
                                    >
                                        Güncelle
                                    </Button>
                                </Box>
                            </Box>
                        </Box>
                    </form>
                </Box>
            </Box>
        </Container>
    );
};

export default EditCourse; 