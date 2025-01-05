import React, { useState } from 'react';
import api from '../services/api';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    TextField,
    Button,
    Box,
    Alert,
    Typography
} from '@mui/material';

const PaymentForm = ({ open, onClose, onSuccess, onError }) => {
    const [formData, setFormData] = useState({
        paymentMethod: 'CreditCard',
        cardNumber: '',
        expiryMonth: '',
        expiryYear: '',
        cvv: '',
        cardHolderName: ''
    });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        setFormData(prev => ({
            ...prev,
            [e.target.name]: e.target.value
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
            const response = await api.payment.processPayment(formData);
            if (response.isSuccessful) {
                onSuccess(response.data);
                onClose();
            } else {
                if (response.error?.errors && Array.isArray(response.error.errors)) {
                    const errorMessage = response.error.errors.join('\n');
                    setError(errorMessage);
                    onError(errorMessage);
                } else {
                    const errorMessage = response.error?.message || response.error || 'Ödeme işlemi başarısız oldu';
                    setError(errorMessage);
                    onError(errorMessage);
                }
            }
        } catch (error) {
            console.error('Payment error:', error);
            if (error.data?.error?.errors && Array.isArray(error.data.error.errors)) {
                const errorMessage = error.data.error.errors.join('\n');
                setError(errorMessage);
                onError(errorMessage);
            } else {
                const errorMessage = 'Ödeme işlemi sırasında bir hata oluştu';
                setError(errorMessage);
                onError(errorMessage);
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
            <DialogTitle>Ödeme Bilgileri</DialogTitle>
            <form onSubmit={handleSubmit} noValidate>
                <DialogContent>
                    {error && (
                        <Box sx={{ mb: 2 }}>
                            {error.split('\n').map((err, index) => (
                                <Typography key={index} color="error" variant="body2" sx={{ mb: 0.5 }}>
                                    {err}
                                </Typography>
                            ))}
                        </Box>
                    )}
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <Box sx={{ width: '100%' }}>
                            <TextField
                                fullWidth
                                label="Kart Üzerindeki İsim"
                                name="cardHolderName"
                                value={formData.cardHolderName}
                                onChange={handleChange}
                                required
                            />
                        </Box>
                        <Box sx={{ width: '100%' }}>
                            <TextField
                                fullWidth
                                label="Kart Numarası"
                                name="cardNumber"
                                value={formData.cardNumber}
                                onChange={handleChange}
                                required
                                maxLength={16}
                                inputMode="numeric"
                            />
                        </Box>
                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Box sx={{ flex: 1 }}>
                                <TextField
                                    fullWidth
                                    label="Ay"
                                    name="expiryMonth"
                                    value={formData.expiryMonth}
                                    onChange={handleChange}
                                    required
                                    maxLength={2}
                                    inputMode="numeric"
                                />
                            </Box>
                            <Box sx={{ flex: 1 }}>
                                <TextField
                                    fullWidth
                                    label="Yıl"
                                    name="expiryYear"
                                    value={formData.expiryYear}
                                    onChange={handleChange}
                                    required
                                    maxLength={4}
                                    inputMode="numeric"
                                />
                            </Box>
                            <Box sx={{ flex: 1 }}>
                                <TextField
                                    fullWidth
                                    label="CVV"
                                    name="cvv"
                                    value={formData.cvv}
                                    onChange={handleChange}
                                    required
                                    maxLength={3}
                                    inputMode="numeric"
                                />
                            </Box>
                        </Box>
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={onClose}>İptal</Button>
                    <Button 
                        type="submit" 
                        variant="contained" 
                        color="primary"
                        disabled={loading}
                    >
                        {loading ? 'İşleniyor...' : 'Ödemeyi Tamamla'}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};

export default PaymentForm; 