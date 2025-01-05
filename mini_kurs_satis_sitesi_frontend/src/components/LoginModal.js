import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
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
import api from '../services/api';

const LoginModal = ({ open, onClose, onLoginSuccess, onSwitchToSignup }) => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [showPassword, setShowPassword] = useState(false);

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

        setLoading(true);

        try {
            const response = await api.auth.login(formData);
            const { accessToken, refreshToken } = response.data;
            
            localStorage.setItem('token', accessToken);
            localStorage.setItem('refreshToken', refreshToken);
            
            const decodedToken = jwtDecode(accessToken);
            
            try {
                const userResponse = await api.user.getCurrentUser();
                const userWithRole = {
                    ...userResponse.data,
                    role: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
                };
                localStorage.setItem('user', JSON.stringify(userWithRole));
                
                onClose();
                if (onLoginSuccess) {
                    onLoginSuccess();
                } else {
                    const redirectPath = localStorage.getItem('redirectAfterLogin');
                    if (redirectPath) {
                        localStorage.removeItem('redirectAfterLogin');
                        navigate(redirectPath);
                    }
                }
            } catch (userError) {
                console.error('User fetch error:', userError);
                if (userError.data?.error?.errors && Array.isArray(userError.data.error.errors)) {
                    setError(userError.data.error.errors.join('\n'));
                } else {
                    setError('Kullanıcı bilgileri alınamadı');
                }
            }
        } catch (error) {
            console.error('Login error:', error);
            if (error.data?.error?.errors && Array.isArray(error.data.error.errors)) {
                setError(error.data.error.errors.join('\n'));
            } else {
                setError(error.message || 'Giriş başarısız');
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
                    Giriş Yap
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
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        id="email"
                        label="Email Adresi"
                        name="email"
                        autoComplete="email"
                        autoFocus
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
                        autoComplete="current-password"
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
                    />
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        disabled={loading}
                        sx={{ mt: 3, mb: 2, py: 1.2 }}
                    >
                        {loading ? <CircularProgress size={24} /> : 'Giriş Yap'}
                    </Button>
                    <Box sx={{ textAlign: 'center', mt: 2 }}>
                        <Typography variant="body2" color="text.secondary">
                            Hesabınız yok mu?{' '}
                            <Button 
                                color="primary" 
                                onClick={onSwitchToSignup}
                                sx={{ textTransform: 'none', fontWeight: 600 }}
                            >
                                Kayıt Ol
                            </Button>
                        </Typography>
                    </Box>
                </Box>
            </DialogContent>
        </Dialog>
    );
};

export default LoginModal; 