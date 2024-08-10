import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosInstance from "../../Utils/axiosInstance";

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

    // Function to convert array buffer to base64 string with header
	const arrayBufferToBase64 = (buffer, mimeType) => {
		let binary = '';
		const bytes = new Uint8Array(buffer);
		const len = bytes.byteLength;
		for (let i = 0; i < len; i++) {
			binary += String.fromCharCode(bytes[i]);
		}
		const base64String = btoa(binary);
		return `data:${mimeType};base64,${base64String}`;
	};
	
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
			PhoroUrl: image,
		};

        // login je rutiran na /
		try {
			const response = await axiosInstance.post("auth/register", {...userData});
			navigate("/")
		} catch (error) {
			console.error("There was an error!", error);
		}
	};  
	
	const navigateToLogin = () => {
		navigate('/');
	};
    
    return (
        <div className="App">
		<div className="auth-form-container">
			<h1>Register</h1>
			<form className="register-form" onSubmit={handleSubmit}>
				<label htmlFor="firstname">First Name</label>
				<input value={firstname} onChange={(e) => setFirstName(e.target.value)} id="firstname" placeholder="First Name" />

				<label htmlFor="lastname">Last Name</label>
				<input value={lastname} onChange={(e) => setLastName(e.target.value)} id="lastname" placeholder="Last Name" />

                <label htmlFor="username">Username</label>
				<input value={username} onChange={(e) => setUsername(e.target.value)} id="username" placeholder="First Name" />

				<label htmlFor="email">Email</label>
				<input value={email} onChange={(e) => setEmail(e.target.value)} type="email" placeholder="youremail@gmail.com" id="email" />

				<label htmlFor="password">Password</label>
				<input value={password} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="********" id="password" />

				<label htmlFor="password">Confirm Password</label>
				<input value={password2} onChange={(e) => setPassword2(e.target.value)} type="password" placeholder="********" id="password2" />

				<label htmlFor="address">Address</label>
				<input value={address} onChange={(e) => setAddress(e.target.value)} id="address" placeholder="Address" />

                <label htmlFor="dob">Date of Birth</label>
				<input type="date" placeholder='Enter your date of birth' value={dob} onChange={(e) => setDob(e.target.value)} id="dob" />

                <label htmlFor="usertype">User role</label>
				<input value={usertype} onChange={(e) => setType(e.target.value)} id="usertype" placeholder="Driver/Cusomer" />

				<label htmlFor="image">Image</label>
				<input type="file" onChange={handleImageUploaded} />

				<button type="submit" className="register-button">Register</button>
			</form>
			
			<button className="link-btn" onClick={navigateToLogin}>Already have an account? Login here.</button>
		</div>
		</div>
    );
};

export default Register;
