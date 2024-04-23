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

	// TODO promeni polja u formi
	// TODO promeni da user type bude checkbox, promeni da DoB bude dateTime
    return (
        <div className={styles.container}>
            <div className={styles.form}>
                <h1>Register</h1>
                <form>					
                    <input type="text" className={styles['form-input']} placeholder='Enter your first name' value={firstname} onChange={(e) => setFirstName(e.target.value)} />
                    <br />
                    <input type="text" className={styles['form-input']} placeholder='Enter your last name' value={lastname} onChange={(e) => setLastName(e.target.value)} />
                    <br />
                    <input type="text" className={styles['form-input']} placeholder='Enter your address' value={address} onChange={(e) => setAddress(e.target.value)} />
                    <br />
                    <input type="text" className={styles['form-input']} placeholder='Enter your date of birth' value={dob} onChange={(e) => setDob(e.target.value)} />
                    <br />
                    <input type="text" className={styles['form-input']} placeholder='Choose your user type' value={usertype} onChange={(e) => setType(e.target.value)} />
                    <br />
                    <input type="text" className={styles['form-input']} placeholder='Enter your username' value={username} onChange={(e) => setUsername(e.target.value)} />
                    <br />
                    <input type="email" className={styles['form-input']} placeholder='Enter your email' value={email} onChange={(e) => setEmail(e.target.value)} />
                    <br />
                    <input type="password" className={styles['form-input']} placeholder='Enter your password' value={password} onChange={(e) => setPassword(e.target.value)} />
                    <br />
                    <button className={styles.button} onClick={(e) => {
                        e.preventDefault();
                        if (firstname.trim() === '' || lastname.trim() === '' || 
							address.trim() === '' || dob.trim() === '' || usertype.trim() === '' || 
							username.trim() === '' || email.trim() === '' || password.trim() === '') {
                            setErrorMessage('Please fill input fields!');
                        } else {
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
