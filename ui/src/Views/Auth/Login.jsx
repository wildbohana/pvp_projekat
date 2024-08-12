import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';
import axiosInstance from '../../Utils/axiosInstance';
import Cookies from 'js-cookie';

function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    
	const handleSubmit = async (e) => {
		e.preventDefault();

		const loginData = {
			Email: email,
			Password: password
		};
		axiosInstance.post("/auth/login", loginData)
			.then(response => {
				const token = response.data.accessToken;
				const usertype = response.data.usertype;
				Cookies.set('jwt-token', token, { expires: 7, secure: true, sameSite: 'Strict' });
				localStorage.setItem('usertype', usertype);
				navigate('/');
			})
			.catch(error => {
				console.error('Error during login:', error);
			});
	}

	const navigateToRegister = () => {
		navigate('/register');
	};
    
	return (
		<div className="App">
		<div className="auth-form-container">
			<h2>Login</h2>
			<form className="login-form" onSubmit={handleSubmit}>
				<label htmlFor="email">email</label>
				<input value={email} onChange={(e) => setEmail(e.target.value)} type="email" placeholder="youremail@gmail.com" id="email" name="email" />
				<label htmlFor="password">password</label>
				<input value={password} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="********" id="password" name="password" />
				<button type="submit">Log In</button>
			</form>
			<button className="link-btn" onClick={navigateToRegister}>Don't have an account? Register here.</button>
		</div>
		</div>
	)
}

export default Login;
