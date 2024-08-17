import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { jwtDecode } from "jwt-decode";
import axios from "axios";

import { RegisterAsync } from "../../Services/userService";
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';

function Register() {
    const [firstname, setFirstName] = useState('');
    const [lastname, setLastName] = useState('');
    const [address, setAddress] = useState('');
    const [dob, setDob] = useState('');
    const [usertype, setType] = useState('');
    const [email, setEmail] = useState('');
	const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
	const [password2, setPassword2] = useState('');
    const [image, setImage] = useState('');
	const navigate = useNavigate();

	const handleImageUploaded = (e) => {
		const file = e.target.files[0];
		const reader = new FileReader();
		reader.onloadend = () => {
			setImage(reader.result);
		}
		reader.readAsDataURL(file);
	}

	const handleSubmit = async (e) => {
		e.preventDefault();
		const userData = {
            Firstname: firstname,
			Lastname: lastname,
            Address: address,
            DateOfBirth: dob,
            Role: usertype,
			Email: email,
            Username: username,
			Password: password,
            ConfirmPassword: password2,
			PhotoUrl: image,
		};

		try {
			const response = await RegisterAsync(userData);
			console.log(response);
			if (response.data)
			{
				navigate("/login");
			}
			else
			{
				console.log("That email has allready been taken!");
				toast("That email has allready been taken!");
			}
		} catch (error) {
			console.error("There was an error!", error);
			toast(error.message);
		}
	};
	
	const navigateToLogin = () => {
		navigate('/login');
	};
    
	const handleGoogleSuccess = async (response) => {
		const { email, name, picture } = jwtDecode(response.credential);
		//console.log(jwtDecode(response.credential));

		try {
			const fullName = name.trim().split(' ');
			const firstName = fullName[0];
			const lastName = fullName.length > 1 ? fullName[fullName.length - 1] : '/';

			const userData = {
				Email: email,
				Password: "123",
				ConfirmPassword: "123",
				Username: firstName,
				Firstname: firstName,
				Lastname: lastName,
				DateOfBirth: "01-01-1999",
				Address: "/",
				Role: "Customer",
				PhotoUrl: "",  
				// https://media.istockphoto.com/id/1300845620/vector/user-icon-flat-isolated-on-white-background-user-symbol-vector-illustration.jpg?s=612x612&w=0&k=20&c=yBeyba0hUkh14_jgv1OKqIH0CCSWU_4ckRkAoy2p73o=
			};

			console.log(userData);
	
			// Make a POST request to your backend API
			const response1 = await RegisterAsync(userData);
			console.log(response1.data);

			if (response1.data)
			{
				navigate("/login");
			}
			else
			{
				toast("That email address has allready been taken.");
				console.log("Email is allready taken.");
			}
		} catch (error) {
			console.error('Error fetching image or registering user:', error);
		}
	};
	
    const handleGoogleFailure = (response) => {
        console.error("Google login failed!", response);
    };

	// TODO  dodati GoogleOAuthProvider? kao u index.js
    return (
		<GoogleOAuthProvider clientId={process.env.REACT_APP_GOOGLE_CLIENTID}>
        <div className="App">
		<div className="auth-form-container">
			<h1>Register</h1>
			<form className="register-form" onSubmit={handleSubmit}>
				<label htmlFor="firstname">First Name</label>
				<input value={firstname} onChange={(e) => setFirstName(e.target.value)} id="firstname" placeholder="First Name" required />

				<label htmlFor="lastname">Last Name</label>
				<input value={lastname} onChange={(e) => setLastName(e.target.value)} id="lastname" placeholder="Last Name" required />

                <label htmlFor="username">Username</label>
				<input value={username} onChange={(e) => setUsername(e.target.value)} id="username" placeholder="Username" required />

				<label htmlFor="email">Email</label>
				<input value={email} onChange={(e) => setEmail(e.target.value)} type="email" placeholder="youremail@gmail.com" id="email" required />

				<label htmlFor="password">Password</label>
				<input value={password} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="********" id="password" required />

				<label htmlFor="password">Confirm Password</label>
				<input value={password2} onChange={(e) => setPassword2(e.target.value)} type="password" placeholder="********" id="password2" required />

				<label htmlFor="address">Address</label>
				<input value={address} onChange={(e) => setAddress(e.target.value)} id="address" placeholder="Address" required />

                <label htmlFor="dob">Date of Birth</label>
				<input type="date" placeholder='Enter your date of birth' value={dob} onChange={(e) => setDob(e.target.value)} id="dob" required />

                <label htmlFor="usertype">User role</label>
				<select value={usertype} onChange={(e) => setType(e.target.value)} id="usertype" >
					<option value="Customer">Customer</option>
					<option value="Driver">Driver</option>
				</select>

				<label htmlFor="image">Image</label>
				<input type="file" onChange={handleImageUploaded} required />

				<button type="submit" className="auth-button">Register</button>
			</form>

			<div className="google-button-container">
				<GoogleLogin
					onSuccess={handleGoogleSuccess}
					onFailure={handleGoogleFailure}
					buttonText="Register with Google"
				/>
			</div>

			<button className="link-btn" onClick={navigateToLogin}>Already have an account? Login here.</button>

		</div>
		</div>
		</GoogleOAuthProvider>

    );
};

export default Register;
