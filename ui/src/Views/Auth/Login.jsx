import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import { toast } from 'react-toastify';
import { LoginAsync } from '../../Services/userService';

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

		try 
		{
			const response = await LoginAsync(loginData);
			if (response.status === 200)
			{
				const token = response.data.accessToken;
				const userType = response.data.usertype;
				Cookies.set('jwt-token', token, { expires: 7, secure: true, sameSite: 'Strict' });
				localStorage.setItem('usertype', userType);
				navigate('/');
			}
			else
			{
				toast("Wrong credentials!");
			}
		}
		catch (error)
		{
			console.log(error.message);
			toast(error.message);
		}
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
				<input value={email} onChange={(e) => setEmail(e.target.value)} type="email" placeholder="youremail@gmail.com" id="email" name="email" required />
				<label htmlFor="password">password</label>
				<input value={password} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="********" id="password" name="password" required />
				<button type="submit">Log In</button>
			</form>
			<button className="link-btn" onClick={navigateToRegister}>Don't have an account? Register here.</button>
		</div>
		</div>
	)
}

export default Login;
