import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Dialog,
    DialogContent,
    DialogTitle,
    TextField,
    Button,
    Box,
    Typography,
    IconButton,
    InputAdornment,
    CircularProgress
} from '@mui/material';
import { Close as CloseIcon, Visibility, VisibilityOff } from '@mui/icons-material';
import { useUI } from '../contexts/UIContext';
import api from '../services/api';

const SignupModal = ({ open, onClose, onSwitchToLogin }) => {
    const navigate = useNavigate();
    const { addNotification } = useUI();
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        userName: '',
        email: '',
        password: '',
        confirmPassword: '',
        city: ''
    });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        // Form validation
        const form = e.target;
        const inputs = form.querySelectorAll('input[required]');
        let isValid = true;

        inputs.forEach(input => {
            if (!input.value) {
                setError(`Lütfen ${input.labels[0].textContent} alanını doldurun`);
                isValid = false;
                return;
            }
        });

        if (!isValid) return;

        if (formData.password !== formData.confirmPassword) {
            setError('Şifreler eşleşmiyor');
            return;
        }

        setLoading(true);

        try {
            const { confirmPassword, ...registerData } = formData;
            const response = await api.user.register(registerData);
            
            if (response.isSuccessful) {
                addNotification('Kayıt başarılı! Giriş yapabilirsiniz.', 'success');
                onSwitchToLogin();
            } else {
                if (response.error?.errors && Array.isArray(response.error.errors)) {
                    setError(response.error.errors.join('\n'));
                } else {
                    setError(response.error || 'Kayıt başarısız');
                }
            }
        } catch (error) {
            console.error('Signup error:', error);
            if (error.data?.error?.errors && Array.isArray(error.data.error.errors)) {
                setError(error.data.error.errors.join('\n'));
            } else {
                setError(error.message || 'Kayıt işlemi sırasında bir hata oluştu');
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <Dialog 
            open={open} 
            onClose={onClose}
            maxWidth="xs"
            fullWidth
            PaperProps={{
                sx: {
                    borderRadius: 2,
                    boxShadow: '0 8px 32px rgba(0,0,0,0.1)'
                }
            }}
        >
            <DialogTitle sx={{ m: 0, p: 2, pb: 1 }}>
                <Typography variant="h6" component="div" sx={{ fontWeight: 600 }}>
                    Kayıt Ol
                </Typography>
                <IconButton
                    onClick={onClose}
                    sx={{
                        position: 'absolute',
                        right: 8,
                        top: 8,
                        color: 'grey.500'
                    }}
                >
                    <CloseIcon />
                </IconButton>
            </DialogTitle>
            <DialogContent>
                <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }} noValidate>
                    {error && (
                        <Box sx={{ mb: 2 }}>
                            {error.split('\n').map((err, index) => (
                                <Typography key={index} color="error" variant="body2" sx={{ mb: 0.5 }}>
                                    {err}
                                </Typography>
                            ))}
                        </Box>
                    )}
                    <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
                        <TextField
                            required
                            fullWidth
                            id="firstName"
                            label="Ad"
                            name="firstName"
                            value={formData.firstName}
                            onChange={handleChange}
                        />
                        <TextField
                            required
                            fullWidth
                            id="lastName"
                            label="Soyad"
                            name="lastName"
                            value={formData.lastName}
                            onChange={handleChange}
                        />
                    </Box>
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        id="userName"
                        label="Kullanıcı Adı"
                        name="userName"
                        autoComplete="username"
                        value={formData.userName}
                        onChange={handleChange}
                        sx={{ mb: 2 }}
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        id="email"
                        label="Email Adresi"
                        name="email"
                        autoComplete="email"
                        value={formData.email}
                        onChange={handleChange}
                        sx={{ mb: 2 }}
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="password"
                        label="Şifre"
                        type={showPassword ? 'text' : 'password'}
                        id="password"
                        autoComplete="new-password"
                        value={formData.password}
                        onChange={handleChange}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <IconButton
                                        onClick={() => setShowPassword(!showPassword)}
                                        edge="end"
                                    >
                                        {showPassword ? <VisibilityOff /> : <Visibility />}
                                    </IconButton>
                                </InputAdornment>
                            ),
                        }}
                        sx={{ mb: 2 }}
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="confirmPassword"
                        label="Şifre Tekrar"
                        type={showConfirmPassword ? 'text' : 'password'}
                        id="confirmPassword"
                        autoComplete="new-password"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <IconButton
                                        onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                                        edge="end"
                                    >
                                        {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
                                    </IconButton>
                                </InputAdornment>
                            ),
                        }}
                        sx={{ mb: 2 }}
                    />
                    <TextField
                        margin="normal"
                        fullWidth
                        name="city"
                        label="Şehir"
                        value={formData.city}
                        onChange={handleChange}
                        sx={{ mb: 2 }}
                    />
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        disabled={loading}
                        sx={{ mt: 3, mb: 2, py: 1.2 }}
                    >
                        {loading ? <CircularProgress size={24} /> : 'Kayıt Ol'}
                    </Button>
                    <Box sx={{ textAlign: 'center', mt: 2 }}>
                        <Typography variant="body2" color="text.secondary">
                            Zaten hesabınız var mı?{' '}
                            <Button 
                                color="primary" 
                                onClick={onSwitchToLogin}
                                sx={{ textTransform: 'none', fontWeight: 600 }}
                            >
                                Giriş Yap
                            </Button>
                        </Typography>
                    </Box>
                </Box>
            </DialogContent>
        </Dialog>
    );
};

export default SignupModal; 