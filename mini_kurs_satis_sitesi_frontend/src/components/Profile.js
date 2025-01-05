import React, { useState, useEffect, useCallback } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import { useApp } from '../contexts/AppContext';
import LoadingSpinner from './LoadingSpinner';
import api from '../services/api';
import { 
    Dialog, 
    DialogTitle, 
    DialogContent, 
    TextField, 
    Typography, 
    Alert, 
    Box, 
    Button, 
    IconButton,
    Card,
    CardContent,
    CardMedia,
    Grid,
    Avatar,
    Chip,
    Divider,
    Paper,
    Container,
    CardActionArea
} from '@mui/material';
import { 
    Close as CloseIcon, 
    Add as AddIcon,
    Edit as EditIcon,
    Person as PersonIcon,
    Book as BookIcon,
    History as HistoryIcon,
    School as SchoolIcon
} from '@mui/icons-material';
import LoginModal from './LoginModal';
import { useUI } from '../contexts/UIContext';
import { DEFAULT_COURSE_IMAGE } from '../constants/images';

const Profile = () => {
    const { user, isAuthenticated, checkAuth, setUser } = useApp();
    const { addNotification } = useUI();
    const navigate = useNavigate();
    const [activeTab, setActiveTab] = useState('info');
    const [purchasedCourses, setPurchasedCourses] = useState([]);
    const [orderHistory, setOrderHistory] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [editMode, setEditMode] = useState(false);
    const [showLoginModal, setShowLoginModal] = useState(false);
    const [showPasswordModal, setShowPasswordModal] = useState(false);
    const [updateData, setUpdateData] = useState({
        userName: '',
        email: '',
        city: ''
    });
    const [passwordData, setPasswordData] = useState({
        currentPassword: '',
        newPassword: '',
        confirmNewPassword: ''
    });
    const [myCreatedCourses, setMyCreatedCourses] = useState([]);

    const isInstructor = () => {
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                console.error('Token bulunamadı - Instructor kontrolü');
                return false;
            }
            
            const decodedToken = jwtDecode(token);
            console.log('Decoded Token:', decodedToken);
            
            const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            console.log('Kullanıcı Rolü:', role);
            
            return role === 'Instructor';
        } catch (error) {
            console.error('Instructor rolü kontrol hatası:', error);
            return false;
        }
    };

    const fetchUserData = useCallback(async () => {
        try {
            console.log('Fetching user data...');
            const response = await api.user.getCurrentUser();
            console.log('User data response:', response);
            setUser(response.data);
            setUpdateData({
                userName: response.data.userName,
                email: response.data.email,
                city: response.data.city
            });
        } catch (error) {
            console.error('Error fetching user data:', error);
            setError('Kullanıcı bilgileri alınamadı');
        } finally {
            setLoading(false);
        }
    }, [setUser]);

    const fetchPurchasedCourses = useCallback(async () => {
        try {
            const response = await api.user.getPurchasedCourses();
            setPurchasedCourses(response.data.purchasedCourses);
        } catch (error) {
            console.error('Satın alınan kurslar alınamadı:', error);
            setPurchasedCourses([]);
        }
    }, []);

    const fetchOrderHistory = useCallback(async () => {
        try {
            const response = await api.order.getOrderHistory();
            setOrderHistory(response.data);
        } catch (error) {
            console.error('Sipariş geçmişi alınamadı:', error);
            setOrderHistory([]);
        }
    }, []);

    const fetchCreatedCourses = useCallback(async () => {
        try {
            const response = await api.course.getInstructorCourses();
            console.log('API Response Data:', response);
            
            if (!response.data) {
                console.error('API yanıtında data objesi yok');
                return;
            }
            
            if (!response.data.courses) {
                console.error('API yanıtında courses array yok');
                return;
            }

            console.log('Bulunan Kurs Sayısı:', response.data.courses.length);
            setMyCreatedCourses(response.data.courses);
        } catch (error) {
            console.error('Fetch Created Courses Error:', error);
            setMyCreatedCourses([]);
        }
    }, []);

    useEffect(() => {
        const init = async () => {
            try {
                const isAuth = await checkAuth();
                if (!isAuth) {
                    setShowLoginModal(true);
                    return;
                }

                await fetchUserData();
                await fetchPurchasedCourses();
                await fetchOrderHistory();

                const instructorCheck = isInstructor();
                console.log('Is Instructor Check Result:', instructorCheck);

                if (instructorCheck) {
                    console.log('Instructor kursları yükleniyor...');
                    await fetchCreatedCourses();
                } else {
                    console.log('Kullanıcı instructor değil');
                }
            } catch (error) {
                console.error('Profile initialization error:', error);
            }
        };
        init();
    }, [checkAuth, navigate, fetchUserData, fetchPurchasedCourses, fetchOrderHistory, fetchCreatedCourses]);

    if (!isAuthenticated || !user) {
        return (
            <>
                <LoadingSpinner />
                <LoginModal 
                    open={showLoginModal} 
                    onClose={() => {
                        setShowLoginModal(false);
                        navigate('/');
                    }}
                    onSwitchToSignup={() => {
                        setShowLoginModal(false);
                        // Burada signup modalı açılabilir
                    }}
                />
            </>
        );
    }

    const handleUpdateProfile = async (e) => {
        e.preventDefault();
        setError('');

        const updatePayload = {
            userName: updateData.userName,
            email: updateData.email,
            city: updateData.city
        };

        try {
            console.log('Sending update payload:', updatePayload);
            const response = await api.user.updateProfile(updatePayload);
            console.log('Update response:', response);
            
            if (response.isSuccessful) {
                setUser(response.data);
                localStorage.setItem('user', JSON.stringify(response.data));
                setEditMode(false);
                addNotification('Profil başarıyla güncellendi', 'success');
                await fetchUserData();
            } else {
                console.log('Update failed:', response);
                setError(response.error?.errors?.[0] || 'Güncelleme sırasında bir hata oluştu');
            }
        } catch (error) {
            console.error('Update error:', error);
            setError('Güncelleme sırasında bir hata oluştu');
        }
    };

    const handleUpdatePassword = async (e) => {
        e.preventDefault();
        setError('');

        if (passwordData.newPassword !== passwordData.confirmNewPassword) {
            setError('Yeni şifreler eşleşmiyor');
            return;
        }

        const updatePayload = {
            userName: user.userName,
            email: user.email,
            city: user.city,
            currentPassword: passwordData.currentPassword,
            newPassword: passwordData.newPassword
        };

        try {
            const response = await api.user.updateProfile(updatePayload);
            
            if (response.isSuccessful) {
                setPasswordData({
                    currentPassword: '',
                    newPassword: '',
                    confirmNewPassword: ''
                });
                setShowPasswordModal(false);
                addNotification('Şifre başarıyla güncellendi', 'success');
            } else {
                setError(response.error?.errors?.[0] || 'Şifre güncellenirken bir hata oluştu');
            }
        } catch (error) {
            console.error('Password update error:', error);
            if (error.message.includes('Password must be')) {
                setError('Şifre en az 6 karakter uzunluğunda olmalı ve en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.');
            } else {
                setError(error.message || 'Şifre güncellenirken bir hata oluştu');
            }
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setUpdateData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const formatDate = (dateString) => {
        if (!dateString) return '';
        return dateString.split('T')[0].split('-').reverse().join('/');
    };

    const formatPrice = (price) => {
        if (!price) return '0.00';
        return Number(price).toFixed(2);
    };

    return (
        <Container maxWidth="xl" sx={{ mt: 2, mb: 5 }}>
            <Grid container spacing={4}>
                {/* Left Section */}
                <Grid item xs={12} md={3}>
                    <Paper 
                        elevation={2} 
                        sx={{ 
                            position: 'sticky', 
                            top: '100px',
                            borderRadius: 2,
                            overflow: 'hidden'
                        }}
                    >
                        <Box sx={{ p: 2, textAlign: 'center' }}>
                            <Avatar 
                                sx={{ 
                                    width: 80, 
                                    height: 80, 
                                    mx: 'auto',
                                    mb: 1.5,
                                    bgcolor: 'primary.main',
                                    fontSize: '2rem'
                                }}
                            >
                                {user?.firstName?.[0]?.toUpperCase() || <PersonIcon />}
                            </Avatar>
                            <Typography variant="h6" sx={{ mb: 0.5, fontWeight: 600 }}>
                                {user?.firstName} {user?.lastName}
                            </Typography>
                            <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                                {isInstructor() ? 'Eğitmen' : 'Öğrenci'}
                            </Typography>
                            <Box sx={{ mb: 2 }}>
                                <Box sx={{ mb: 1, p: 1, bgcolor: 'grey.50', borderRadius: 1 }}>
                                    <Typography variant="caption" color="text.secondary">
                                        Kullanıcı Adı
                                    </Typography>
                                    <Typography variant="body2">
                                        {user?.userName}
                                    </Typography>
                                </Box>
                                <Box sx={{ mb: 1, p: 1, bgcolor: 'grey.50', borderRadius: 1 }}>
                                    <Typography variant="caption" color="text.secondary">
                                        Email
                                    </Typography>
                                    <Typography variant="body2">
                                        {user?.email}
                                    </Typography>
                                </Box>
                                <Box sx={{ mb: 1, p: 1, bgcolor: 'grey.50', borderRadius: 1 }}>
                                    <Typography variant="caption" color="text.secondary">
                                        Şehir
                                    </Typography>
                                    <Typography variant="body2">
                                        {user?.city || 'Belirtilmemiş'}
                                    </Typography>
                                </Box>
                            {isInstructor() && (
                                    <Box sx={{ mb: 1, p: 1, bgcolor: 'grey.50', borderRadius: 1 }}>
                                        <Typography variant="caption" color="text.secondary">
                                            Oluşturulan Kurs Sayısı
                                        </Typography>
                                        <Typography variant="body2">
                                            {myCreatedCourses.length}
                                        </Typography>
                                    </Box>
                                )}
                            </Box>
                            <Button
                                variant="contained"
                                fullWidth
                                startIcon={<EditIcon />}
                                onClick={() => setEditMode(true)}
                                sx={{ mb: 1, py: 0.5, fontSize: '0.85rem' }}
                                size="small"
                        >
                            Profili Düzenle
                            </Button>
                            <Button
                                variant="outlined"
                                fullWidth
                            onClick={() => setShowPasswordModal(true)}
                                sx={{ py: 0.5, fontSize: '0.85rem' }}
                                size="small"
                        >
                            Şifre Değiştir
                            </Button>
                        </Box>
                    </Paper>
                </Grid>

                {/* Right Section */}
                <Grid item xs={12} md={9}>
                    {/* Created Courses Section (For Instructors) */}
                    {isInstructor() && (
                        <Paper elevation={2} sx={{ p: 3, mb: 3, borderRadius: 2 }}>
                            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                                <Typography variant="h6" sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                    <SchoolIcon color="primary" />
                                    Oluşturduğum Kurslar
                                </Typography>
                                <Button
                                    variant="contained"
                                    color="success"
                                    startIcon={<AddIcon />}
                                    onClick={() => navigate('/create-course')}
                                >
                                    Yeni Kurs Oluştur
                                </Button>
                            </Box>
                            <Grid container spacing={2}>
                                    {myCreatedCourses.map(course => (
                                    <Grid item xs={12} sm={6} lg={4} key={course.id}>
                                        <Card 
                                            sx={{ 
                                                height: '100%',
                                                display: 'flex',
                                                flexDirection: 'column'
                                            }}
                                        >
                                            <CardMedia
                                                component="img"
                                                height="120"
                                                image={DEFAULT_COURSE_IMAGE}
                                                alt={course.name}
                                            />
                                            <CardContent sx={{ p: 2 }}>
                                                <Typography variant="subtitle1" gutterBottom>
                                                    {course.name}
                                                </Typography>
                                                <Typography 
                                                    variant="body2" 
                                                    color="text.secondary"
                                                    sx={{
                                                        mb: 1,
                                                        display: '-webkit-box',
                                                        WebkitLineClamp: 2,
                                                        WebkitBoxOrient: 'vertical',
                                                        overflow: 'hidden',
                                                        fontSize: '0.8rem'
                                                    }}
                                                >
                                                    {course.description}
                                                </Typography>
                                                <Box sx={{ mb: 1 }}>
                                                    <Chip 
                                                        label={course.category}
                                                        size="small"
                                                        variant="outlined"
                                                        sx={{ fontSize: '0.7rem' }}
                                                    />
                                                </Box>
                                                <Box sx={{ mb: 1 }}>
                                                    <Typography variant="body2" color="text.secondary" sx={{ fontSize: '0.75rem' }}>
                                                        Oluşturulma: {formatDate(course.createdDate)}
                                                    </Typography>
                                                    {course.updatedDate && (
                                                        <Typography variant="body2" color="text.secondary" sx={{ fontSize: '0.75rem' }}>
                                                            Güncelleme: {formatDate(course.updatedDate)}
                                                        </Typography>
                                                    )}
                                                </Box>
                                                <Divider sx={{ my: 1 }} />
                                                <Box sx={{ 
                                                    display: 'flex', 
                                                    justifyContent: 'space-between',
                                                    alignItems: 'center'
                                                }}>
                                                    <Typography variant="subtitle2" color="primary">
                                                        {formatPrice(course.price)} TL
                                                    </Typography>
                                                    <Button
                                                        variant="contained"
                                                        size="small"
                                                    onClick={() => navigate(`/edit-course/${course.id}`)}
                                                        sx={{ fontSize: '0.75rem', py: 0.5 }}
                                                >
                                                    Düzenle
                                                    </Button>
                                                </Box>
                                            </CardContent>
                                        </Card>
                                    </Grid>
                                ))}
                            </Grid>
                        </Paper>
                    )}

                    {/* Purchased Courses Section */}
                    <Paper elevation={2} sx={{ p: 3, mb: 3, borderRadius: 2 }}>
                        <Typography variant="h6" sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 3 }}>
                            <BookIcon color="primary" />
                            Satın Aldığım Kurslar
                        </Typography>
                        <Grid container spacing={2}>
                            {purchasedCourses.map(course => (
                                <Grid item xs={12} sm={6} lg={4} key={course.courseId}>
                                    <Card 
                                        component={Link}
                                        to={`/course/${course.courseId}`}
                                        sx={{ 
                                            height: '100%',
                                            display: 'flex',
                                            flexDirection: 'column',
                                            textDecoration: 'none'
                                        }}
                                    >
                                        <CardActionArea>
                                            <CardMedia
                                                component="img"
                                                height="120"
                                                image={DEFAULT_COURSE_IMAGE}
                                                alt={course.courseName}
                                            />
                                            <CardContent sx={{ p: 2 }}>
                                                <Typography variant="subtitle1" gutterBottom>
                                                    {course.courseName}
                                                </Typography>
                                                <Typography 
                                                    variant="body2" 
                                                    color="text.secondary"
                                                    sx={{
                                                        mb: 1,
                                                        display: '-webkit-box',
                                                        WebkitLineClamp: 2,
                                                        WebkitBoxOrient: 'vertical',
                                                        overflow: 'hidden',
                                                        fontSize: '0.8rem'
                                                    }}
                                                >
                                                    {course.description}
                                                </Typography>
                                                <Box sx={{ mb: 1 }}>
                                                    <Chip 
                                                        label={course.category}
                                                        size="small"
                                                        variant="outlined"
                                                        sx={{ fontSize: '0.7rem' }}
                                                    />
                                                </Box>
                                                <Box sx={{ 
                                                    display: 'flex', 
                                                    justifyContent: 'space-between',
                                                    alignItems: 'center'
                                                }}>
                                                    <Typography variant="body2" color="text.secondary" sx={{ fontSize: '0.75rem' }}>
                                                        Satın Alma: {formatDate(course.purchaseDate)}
                                                    </Typography>
                                                    <Chip
                                                        label={course.orderStatus === 'Paid' ? 'Ödendi' : 'Beklemede'}
                                                        color={course.orderStatus === 'Paid' ? 'success' : 'warning'}
                                                        size="small"
                                                        sx={{ fontSize: '0.7rem' }}
                                                    />
                                                </Box>
                                            </CardContent>
                                        </CardActionArea>
                                    </Card>
                                </Grid>
                            ))}
                        </Grid>
                    </Paper>

                    {/* Order History Section */}
                    <Paper elevation={2} sx={{ p: 3, borderRadius: 2 }}>
                        <Typography variant="h6" sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 3 }}>
                            <HistoryIcon color="primary" />
                            Sipariş Geçmişi
                        </Typography>
                        {orderHistory.map((order, index) => (
                            <Box 
                                key={order.id}
                                sx={{
                                    p: 2,
                                    mb: index !== orderHistory.length - 1 ? 2 : 0,
                                    bgcolor: 'grey.50',
                                    borderRadius: 1
                                }}
                            >
                                <Box sx={{ 
                                    display: 'flex', 
                                    justifyContent: 'space-between',
                                    alignItems: 'center',
                                    mb: 1
                                }}>
                                    <Typography variant="subtitle1">
                                        Sipariş #{order.id}
                                    </Typography>
                                    <Chip
                                        label={order.status === 'Paid' ? 'Ödendi' : 'Beklemede'}
                                        color={order.status === 'Paid' ? 'success' : 'warning'}
                                        size="small"
                                    />
                                </Box>
                                <Typography variant="body2" color="text.secondary">
                                    Tarih: {formatDate(order.orderDate)}
                                </Typography>
                                <Box sx={{ my: 1 }}>
                                    <Typography variant="subtitle2" color="text.secondary" gutterBottom>
                                        Satın Alınan Kurslar:
                                    </Typography>
                                    {order.orderItems?.map((item, idx) => (
                                        <Box key={idx} sx={{ ml: 2, mb: 0.5, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                            <Typography variant="body2">
                                                • {item.courseName}
                                            </Typography>
                                            <Typography variant="body2" color="primary">
                                                {formatPrice(item.price)} TL
                                            </Typography>
                                        </Box>
                                    ))}
                                </Box>
                                <Divider sx={{ my: 1 }} />
                                <Typography variant="h6" color="primary" sx={{ mt: 1, textAlign: 'right' }}>
                                    Toplam: {formatPrice(order.totalPrice)} TL
                                </Typography>
                            </Box>
                        ))}
                    </Paper>
                </Grid>
            </Grid>

            {/* Edit Profile Modal */}
                <Dialog
                    open={editMode}
                    onClose={() => setEditMode(false)}
                    maxWidth="sm"
                    fullWidth
                >
                    <DialogTitle>
                    Profili Düzenle
                        <IconButton
                            onClick={() => setEditMode(false)}
                        sx={{ position: 'absolute', right: 8, top: 8 }}
                        >
                            <CloseIcon />
                        </IconButton>
                    </DialogTitle>
                    <DialogContent>
                    <Box component="form" onSubmit={handleUpdateProfile} sx={{ mt: 2 }}>
                        {error && (
                            <Alert severity="error" sx={{ mb: 2 }}>
                                {error}
                            </Alert>
                        )}
                            <TextField
                                fullWidth
                            label="Kullanıcı Adı"
                                name="userName"
                                value={updateData.userName}
                                onChange={handleInputChange}
                            margin="normal"
                            />
                            <TextField
                                fullWidth
                            label="Email"
                                name="email"
                                value={updateData.email}
                                onChange={handleInputChange}
                            margin="normal"
                            />
                            <TextField
                                fullWidth
                            label="Şehir"
                                name="city"
                                value={updateData.city}
                                onChange={handleInputChange}
                            margin="normal"
                        />
                        <Box sx={{ mt: 3, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                            <Button onClick={() => setEditMode(false)}>
                                    İptal
                                </Button>
                            <Button type="submit" variant="contained">
                                Güncelle
                                </Button>
                            </Box>
                        </Box>
                    </DialogContent>
                </Dialog>

            {/* Change Password Modal */}
                <Dialog
                    open={showPasswordModal}
                    onClose={() => setShowPasswordModal(false)}
                    maxWidth="sm"
                    fullWidth
                >
                    <DialogTitle>
                            Şifre Değiştir
                        <IconButton
                            onClick={() => setShowPasswordModal(false)}
                        sx={{ position: 'absolute', right: 8, top: 8 }}
                        >
                            <CloseIcon />
                        </IconButton>
                    </DialogTitle>
                    <DialogContent>
                    <Box component="form" onSubmit={handleUpdatePassword} sx={{ mt: 2 }}>
                        {error && (
                            <Alert severity="error" sx={{ mb: 2 }}>
                                {error}
                            </Alert>
                        )}
                            <TextField
                                fullWidth
                            type="password"
                            label="Mevcut Şifre"
                                name="currentPassword"
                                value={passwordData.currentPassword}
                                onChange={(e) => setPasswordData(prev => ({
                                    ...prev,
                                    currentPassword: e.target.value
                                }))}
                            margin="normal"
                            />
                            <TextField
                                fullWidth
                            type="password"
                            label="Yeni Şifre"
                                name="newPassword"
                                value={passwordData.newPassword}
                                onChange={(e) => setPasswordData(prev => ({
                                    ...prev,
                                    newPassword: e.target.value
                                }))}
                            margin="normal"
                            />
                            <TextField
                                fullWidth
                            type="password"
                            label="Yeni Şifre Tekrar"
                                name="confirmNewPassword"
                                value={passwordData.confirmNewPassword}
                                onChange={(e) => setPasswordData(prev => ({
                                    ...prev,
                                    confirmNewPassword: e.target.value
                                }))}
                            margin="normal"
                        />
                        <Box sx={{ mt: 3, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                            <Button onClick={() => setShowPasswordModal(false)}>
                                İptal
                            </Button>
                            <Button type="submit" variant="contained">
                                    Şifreyi Güncelle
                                </Button>
                            </Box>
                        </Box>
                    </DialogContent>
                </Dialog>

            {/* Login Modal */}
            <LoginModal 
                open={showLoginModal} 
                onClose={() => {
                    setShowLoginModal(false);
                    navigate('/');
                }}
                onSwitchToSignup={() => {
                    setShowLoginModal(false);
                }}
            />
        </Container>
    );
};

export default Profile; 