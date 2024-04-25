import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import styles from './Register.module.css'

export const Register = () => {
    const [firstname, setFirstName] = useState('');
    const [lastname, setLastName] = useState('');
    const [address, setAddress] = useState('');
    const [dob, setDob] = useState('');
    const [usertype, setType] = useState('');
    const [email, setEmail] = useState('');
	const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
	const [password2, setPassword2] = useState('');
    const [errorMessage, setErrorMessage] = useState('');

    const navigate = useNavigate();

    const handleRegister = async () => {
        try {
            const response = await axios.post('/auth/register', {
                firstname,
                lastname,
                address,
                dob,
                usertype,
                email,
				username,
                password,
            });
            const successMessage = response.data;
            console.log(successMessage);

            setErrorMessage('');
            window.alert('Registration successful!');
        } catch (error) {
            console.error('Registration failed:', error.response ? error.response.data : error.message);
            setErrorMessage('Registration failed. Please check your details and try again.');
        }
    };

    const handleRegisterClick = () => {
        navigate('/');
    }

    return (
        <div className={styles.container}>
            <div className={styles.form}>
                <h1>Register</h1>
                <form>	
					<table>
						<tr>
							<td><input type="text" className={styles['form-input']} placeholder='Enter your first name' value={firstname} onChange={(e) => setFirstName(e.target.value)} /></td>
							<td><input type="text" className={styles['form-input']} placeholder='Enter your last name' value={lastname} onChange={(e) => setLastName(e.target.value)} /></td>
						</tr>
						<tr>
							<td><input type="text" className={styles['form-input']} placeholder='Enter your address' value={address} onChange={(e) => setAddress(e.target.value)} /></td>
							<td><input type="date" className={styles['form-input']} placeholder='Enter your date of birth' value={dob} onChange={(e) => setDob(e.target.value)} /></td>
						</tr>
						<tr>
							<td><input type="email" className={styles['form-input']} placeholder='Enter your email' value={email} onChange={(e) => setEmail(e.target.value)} /></td>
							<td><input type="text" className={styles['form-input']} placeholder='Enter your username' value={username} onChange={(e) => setUsername(e.target.value)} /></td>
						</tr>
						<tr>
							<td><input type="password" className={styles['form-input']} placeholder='Enter your password' value={password} onChange={(e) => setPassword(e.target.value)} /></td>
							<td><input type="password" className={styles['form-input']} placeholder='Confirm password' value={password2} onChange={(e) => setPassword2(e.target.value)} /></td>
						</tr>
						<tr>
							<td>
								<select value={usertype} onChange={(e) => setType(e.target.value)}>
									<option value="driver">Driver</option>
									<option value="customer">Customer</option>
								</select>
							</td>
						</tr>
					</table>
					
                    <button className={styles.button} onClick={(e) => {
                        e.preventDefault();
                        if (firstname.trim() === '' || lastname.trim() === '' || address.trim() === '' || dob.trim() === '' || usertype.trim() === '' || username.trim() === '' || email.trim() === '' || password.trim() === '' || password2.trim() === '') {
                            setErrorMessage('Please fill input fields!');
                        }
						// Samo na frontu se proverava da li se poklapaju lozinke
						else if (password !== password2) {
							setErrorMessage('Passwords do not match!');
						} 						
						else {
                            handleRegister();
                            handleRegisterClick();
                            setErrorMessage('');
                        }
                    }}>Register</button>
                    {errorMessage && <p style={{ color: 'red', textAlign: 'center', marginBottom: 20 }} > {errorMessage}</p>}
                </form>
            </div>
        </div>
    );
};
