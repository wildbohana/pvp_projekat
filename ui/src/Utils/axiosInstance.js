import axios from 'axios';
import Cookies from 'js-cookie';

// Na svaku promenu vrednosti iz .env moraš ponovo pokrenuti ceo front
const api_url = process.env.REACT_APP_API_URL;

const axiosInstance = axios.create({
	baseURL: api_url,  
});

axiosInstance.interceptors.request.use((config) => {
	const token = Cookies.get('jwt-token');
	if (token) {
		config.headers.Authorization = `Bearer ${token}`;
	}
	return config;
}, (error) => {
	return Promise.reject(error);
});

export default axiosInstance;
