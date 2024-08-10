import axios from 'axios';
import Cookies from 'js-cookie';

//const api_url = process.env.API_URL;

const api_url = "http://localhost:8102";

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
