import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useCart } from '../contexts/CartContext';
import { useApp } from '../contexts/AppContext';
import { useUI } from '../contexts/UIContext';
import {
    Container,
    Typography,
    Button,
    Card,
    CardContent,
    CardMedia,
    Box,
    IconButton,
    Divider
} from '@mui/material';
import { Delete as DeleteIcon } from '@mui/icons-material';
import LoginModal from './LoginModal';
import PaymentForm from './PaymentForm';
import api from '../services/api';
import { DEFAULT_COURSE_IMAGE } from '../constants/images';

const Cart = () => {
    const { cartItems, removeFromCart, getCartTotal, clearCart } = useCart();
    const { isAuthenticated, checkAuth } = useApp();
    const { addNotification } = useUI();
    const navigate = useNavigate();
    const [showLoginModal, setShowLoginModal] = React.useState(false);
    const [showPaymentModal, setShowPaymentModal] = React.useState(false);
    const [orderId, setOrderId] = React.useState(null);

    const handleCheckout = async () => {
        const authStatus = await checkAuth();
        if (!authStatus) {
            setShowLoginModal(true);
            return;
        }

        setShowPaymentModal(true);
    };

    const handlePaymentSuccess = async (paymentData) => {
        try {
            const orderData = {
                orderItems: cartItems.map(item => ({
                    courseId: item.id,
                    price: item.price
                }))
            };
            
            console.log('Creating order:', orderData);
            const orderResponse = await api.order.createOrder(orderData);

            if (orderResponse.isSuccessful) {
                console.log('Order created successfully:', orderResponse.data);
                addNotification('Siparişiniz başarıyla tamamlandı', 'success');
                clearCart();
                navigate('/profile');
            } else {
                const errorMessage = orderResponse.error?.message || 'Sipariş oluşturulurken bir hata oluştu';
                console.error('Order creation failed:', errorMessage);
                addNotification(errorMessage, 'error');
            }
        } catch (error) {
            console.error('Order creation error:', error);
            addNotification('Sipariş oluşturulurken bir hata oluştu. Lütfen daha sonra tekrar deneyin.', 'error');
        }
    };

    const handleLoginSuccess = async () => {
        setShowLoginModal(false);
        const authStatus = await checkAuth();
        if (authStatus) {
            handleCheckout();
        }
    };

    if (cartItems.length === 0) {
        return (
            <Container maxWidth="md" sx={{ mt: 12, textAlign: 'center' }}>
                <Typography variant="h5" gutterBottom>
                    Sepetiniz boş
                </Typography>
                <Button 
                    variant="contained" 
                    color="primary" 
                    onClick={() => navigate('/')}
                    sx={{ mt: 2 }}
                >
                    Alışverişe Başla
                </Button>
            </Container>
        );
    }

    return (
        <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
            <Box display="grid" gridTemplateColumns="repeat(12, 1fr)" gap={3}>
                <Box gridColumn={{ xs: "span 12", md: "span 8" }}>
                    {cartItems.map(item => (
                        <Card key={item.id} sx={{ mb: 2, display: 'flex' }}>
                            <CardMedia
                                component="img"
                                sx={{ width: 140 }}
                                image={item.imageUrl || DEFAULT_COURSE_IMAGE}
                                alt={item.name}
                            />
                            <Box sx={{ display: 'flex', flexDirection: 'column', flexGrow: 1 }}>
                                <CardContent sx={{ flex: '1 0 auto' }}>
                                    <Typography component="h5" variant="h6">
                                        {item.name}
                                    </Typography>
                                    <Typography variant="subtitle1" color="text.secondary" gutterBottom>
                                        {item.instructorName}
                                    </Typography>
                                    <Typography variant="h6" color="primary">
                                        {item.price} TL
                                    </Typography>
                                </CardContent>
                                <Box sx={{ display: 'flex', alignItems: 'center', pl: 1, pb: 1 }}>
                                    <IconButton onClick={() => removeFromCart(item.id)} aria-label="Sil">
                                        <DeleteIcon />
                                    </IconButton>
                                </Box>
                            </Box>
                        </Card>
                    ))}
                </Box>
                
                <Box gridColumn={{ xs: "span 12", md: "span 4" }}>
                    <Card>
                        <CardContent>
                            <Typography variant="h6" gutterBottom>
                                Sipariş Özeti
                            </Typography>
                            <Divider sx={{ my: 2 }} />
                            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
                                <Typography variant="subtitle1">Toplam</Typography>
                                <Typography variant="h6">{getCartTotal()} TL</Typography>
                            </Box>
                            <Button
                                variant="contained"
                                color="primary"
                                fullWidth
                                size="large"
                                onClick={handleCheckout}
                            >
                                {isAuthenticated ? 'Ödemeye Geç' : 'Giriş Yap ve Devam Et'}
                            </Button>
                        </CardContent>
                    </Card>
                </Box>
            </Box>

            <LoginModal 
                open={showLoginModal} 
                onClose={() => setShowLoginModal(false)}
                onLoginSuccess={handleLoginSuccess}
            />

            {showPaymentModal && (
                <PaymentForm
                    open={showPaymentModal}
                    onClose={() => setShowPaymentModal(false)}
                    onSuccess={handlePaymentSuccess}
                    onError={(error) => {
                        addNotification(error, 'error');
                    }}
                />
            )}
        </Container>
    );
};

export default Cart; 