import { CircularProgress, Box } from '@mui/material';

const LoadingSpinner = ({ size = 40, containerStyle = {}, minHeight = "200px" }) => (
    <Box 
        display="flex" 
        justifyContent="center" 
        alignItems="center" 
        minHeight={minHeight}
        {...containerStyle}
    >
        <CircularProgress size={size} />
    </Box>
);

export default LoadingSpinner; 