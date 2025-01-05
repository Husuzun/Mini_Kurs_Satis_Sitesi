// src/components/Navbar.js
import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import { useCart } from '../contexts/CartContext';
import { useCourse } from '../contexts/CourseContext';
import {
    AppBar,
    Toolbar,
    Typography,
    TextField,
    InputAdornment,
    Box,
    Button,
    IconButton,
    Badge,
    Stack
} from '@mui/material';
import {
    Search as SearchIcon,
    ShoppingCart as ShoppingCartIcon,
    Person as PersonIcon
} from '@mui/icons-material';
import LoginModal from './LoginModal';
import SignupModal from './SignupModal';

const Navbar = () => {
    const navigate = useNavigate();
    const { cartCount } = useCart();
    const { searchCourses } = useCourse();
    const [searchTerm, setSearchTerm] = useState('');
    const [showLoginModal, setShowLoginModal] = useState(false);
    const [showSignupModal, setShowSignupModal] = useState(false);
    
    const getUserFromStorage = () => {
        try {
            const userStr = localStorage.getItem('user');
            return userStr ? JSON.parse(userStr) : null;
        } catch (error) {
            console.error('Error parsing user data:', error);
            return null;
        }
    };

    const isInstructor = () => {
        try {
            const token = localStorage.getItem('token');
            if (!token) return false;
            
            const decodedToken = jwtDecode(token);
            return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Instructor';
        } catch (error) {
            console.error('Error checking instructor role:', error);
            return false;
        }
    };

    const user = getUserFromStorage();

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        localStorage.removeItem('refreshToken');
        navigate('/');
    };

    const handleSearch = (e) => {
        const value = e.target.value;
        setSearchTerm(value);
        searchCourses(value);
    };

    const handleLoginClick = (e) => {
        e.preventDefault();
        setShowLoginModal(true);
    };

    const handleSignupClick = (e) => {
        e.preventDefault();
        setShowSignupModal(true);
    };

    const switchToSignup = () => {
        setShowLoginModal(false);
        setShowSignupModal(true);
    };

    const switchToLogin = () => {
        setShowSignupModal(false);
        setShowLoginModal(true);
    };

    return (
        <>
            <AppBar position="fixed" sx={{ backgroundColor: 'white', boxShadow: 1 }}>
                <Toolbar sx={{ justifyContent: 'space-between' }}>
                    <Typography
                        variant="h6"
                        component={Link}
                        to="/"
                        sx={{
                            textDecoration: 'none',
                            color: 'primary.main',
                            fontWeight: 700,
                            '&:hover': {
                                color: 'primary.dark'
                            }
                        }}
                    >
                        Kurs Platformu
                    </Typography>

                    <Box sx={{ flexGrow: 1, display: 'flex', justifyContent: 'center' }}>
                        <TextField
                            size="small"
                            variant="outlined"
                            placeholder="Kurs ara..."
                            value={searchTerm}
                            onChange={handleSearch}
                            InputProps={{
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <SearchIcon />
                                    </InputAdornment>
                                ),
                                sx: {
                                    backgroundColor: 'white',
                                    borderRadius: 1,
                                    width: '350px',
                                    '& fieldset': {
                                        borderColor: 'rgba(0,0,0,0.1)',
                                    },
                                    '&:hover fieldset': {
                                        borderColor: 'rgba(0,0,0,0.2) !important',
                                    }
                                }
                            }}
                        />
                    </Box>

                    <Stack direction="row" spacing={2} alignItems="center">
                        <IconButton 
                            component={Link} 
                            to="/cart"
                            color="primary"
                            sx={{ position: 'relative' }}
                        >
                            <Badge badgeContent={cartCount} color="error">
                                <ShoppingCartIcon />
                            </Badge>
                        </IconButton>

                        {user ? (
                            <>
                                <Button
                                    component={Link}
                                    to="/profile"
                                    startIcon={<PersonIcon />}
                                    color="primary"
                                    sx={{ textTransform: 'none' }}
                                >
                                    {user.userName}
                                </Button>
                                {isInstructor() && (
                                    <Button
                                        component={Link}
                                        to="/create-course"
                                        variant="outlined"
                                        color="primary"
                                        sx={{ textTransform: 'none' }}
                                    >
                                        Kurs Oluştur
                                    </Button>
                                )}
                                <Button
                                    onClick={handleLogout}
                                    color="primary"
                                    sx={{ textTransform: 'none' }}
                                >
                                    Çıkış Yap
                                </Button>
                            </>
                        ) : (
                            <>
                                <Button
                                    onClick={handleSignupClick}
                                    variant="outlined"
                                    color="primary"
                                    sx={{ textTransform: 'none' }}
                                >
                                    Kayıt Ol
                                </Button>
                                <Button
                                    onClick={handleLoginClick}
                                    variant="contained"
                                    color="primary"
                                    sx={{ textTransform: 'none' }}
                                >
                                    Giriş Yap
                                </Button>
                            </>
                        )}
                    </Stack>
                </Toolbar>
            </AppBar>
            <Toolbar /> {/* Spacing element to prevent content from hiding under AppBar */}

            <LoginModal 
                open={showLoginModal} 
                onClose={() => setShowLoginModal(false)}
                onSwitchToSignup={switchToSignup}
            />

            <SignupModal 
                open={showSignupModal} 
                onClose={() => setShowSignupModal(false)}
                onSwitchToLogin={switchToLogin}
            />
        </>
    );
};

export default Navbar;