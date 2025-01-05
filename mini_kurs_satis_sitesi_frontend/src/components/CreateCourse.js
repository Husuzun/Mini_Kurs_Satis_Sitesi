import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
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
    Alert
} from '@mui/material';

const CreateCourse = () => {
    const navigate = useNavigate();
    const { addNotification, showLoading, hideLoading } = useUI();
    const [error, setError] = useState('');
    const [formData, setFormData] = useState({
        name: '',
        description: '',
        price: 0,
        category: ''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: name === 'price' ? Number(value) : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        showLoading();

        const courseData = {
            ...formData,
            price: Number(formData.price)
        };

        try {
            console.log('Sending course data:', courseData);
            const response = await api.course.createCourse(courseData);
            
            if (response.isSuccessful) {
                addNotification('Kurs başarıyla oluşturuldu', 'success');
                navigate('/profile');
            } else {
                const errorMessage = response.error?.message || response.error || 'Kurs oluşturulurken bir hata oluştu';
                setError(errorMessage);
                addNotification(errorMessage, 'error');
            }
        } catch (error) {
            console.error('Course creation error:', error);
            const errorMessage = error.message || 'Kurs oluşturulurken bir hata oluştu';
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
                    <Typography variant="h4" component="h1" gutterBottom sx={{ mb: 4 }}>
                        Yeni Kurs Oluştur
                    </Typography>
                </Box>
                <Box gridColumn="span 12">
                    {error && (
                        <Alert severity="error" sx={{ mb: 3 }}>
                            {error}
                        </Alert>
                    )}
                    
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
                                    label="Fiyat (TL)"
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
                                            step: "0.01",
                                            pattern: "\\d*\\.?\\d{0,2}"
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
                                    <MenuItem value="" disabled>Kategori Seçin</MenuItem>
                                    {CATEGORIES.filter(category => category !== 'all').map((category) => (
                                        <MenuItem key={category} value={category}>
                                            {category}
                                        </MenuItem>
                                    ))}
                                </TextField>
                            </Box>

                            <Box gridColumn="span 12">
                                <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                                    <Button
                                        variant="outlined"
                                        onClick={() => navigate(-1)}
                                    >
                                        İptal
                                    </Button>
                                    <Button
                                        type="submit"
                                        variant="contained"
                                        color="primary"
                                    >
                                        Kurs Oluştur
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

export default CreateCourse; 