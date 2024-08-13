import React, { useState, useEffect } from 'react';
import { GetUserProfileAsync, UpdateUserAsync } from '../../Services/userService';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../../Assets/MyProfile.css';

import { axiosClient } from '../../Services/axiosClient';

function MyProfile() {
    const [profile, setProfile] = useState({
		Email: '',
		Username: '',
		ConfirmOldPassword: '',
		NewPassword: '',
		ConfirmNewPassword: '',
        Firstname: '',
        Lastname: '',
        Address: '',
        DateOfBirth: '',
        Role: '',
		PhotoUrl: '',
        VerificationStatus: '',
        IsBlocked: false
    });
    
    useEffect(() => {
        axiosClient.get(`${process.env.REACT_APP_API_URL}/users/profile`).then(response => {
            setProfile(response.data);
            console.log(response.data);
        }).catch(error => {
            console.error("Error fetching profile: ", error);
            toast("Error fetching profile");
        });
    }, []);

    const handleInputChange = (e) => {
        setProfile({
            ...profile,
            [e.target.name]: e.target.value
        });
    };

    const handleImageUploaded = (e) => {
        const file = e.target.files[0];
        const reader = new FileReader();
        reader.onloadend = () => {
            setProfile({
                ...profile,
                PhotoUrl: reader.result,
            })
        }
        reader.readAsDataURL(file);
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            console.log(profile);

            Object.keys(profile).forEach(field => {
                if (profile[field] === '' || profile[field] === undefined) {
                    delete profile[field];
                }
            })

            console.log(profile);
            const response = await UpdateUserAsync(profile);
            if (response.status === 200) {
                toast("Profile updated successfully");
                setTimeout(`window.location.reload()`, 2000);   // dobra fora (ali mora se izmeniti)
            }
        } catch (error) {
            toast("Error updating profile");
            console.error("Error updating profile: ", error);
        }
    };

    const labelMap = {
        firstname: 'First Name',
        lastname: 'Last Name',
        confirmOldPassword: 'Old password',
		newPassword: 'New password',
        confirmNewPassword: 'Confirm new password',
		username: 'Userame',
		email: 'Email',
        address: 'Address',
        dateOfBirth: 'Date of birth',
		role: 'User type',
        photoUrl: 'Image',
        verificationStatus: 'Verification status',
        isBlocked: 'Blocked?',
    };

    return (		
        <div className="profile-container">
            {/* <ToastContainer position="top-right" autoClose={2000} hideProgressBar={false} /> */}
            <h1>Edit Profile</h1>
            <form onSubmit={handleSubmit} className="profile-form">
                {Object.entries(labelMap).map(([labelKey, labelValue]) => (
                    <div key={labelKey} className="form-group">
                        <label htmlFor={labelKey}>{labelValue}</label>
                        {labelKey === 'Image' || labelKey === 'photoUrl' ? (
                            <div>
                            <img src={profile[labelKey]} alt="profile image" className="profile-image" />
                            <input type="file" id={labelKey} name={labelKey} onChange={handleImageUploaded} />
                            </div>
                        ) : labelKey === 'confirmOldPassword' || labelKey === 'confirmNewPassword' || labelKey === 'newPassword' ? (
                            <input
                                type = "password"
                                id = {labelKey}
                                name = {labelKey}
                                value = {profile[labelKey]}
                                onChange = {handleInputChange}
                                className = "form-control"
                            />
                        ) : (
                            <input
                                type = "text"
                                id = {labelKey}
                                name = {labelKey}
                                value = {profile[labelKey]}
                                onChange = {handleInputChange}
                                className = "form-control"
                                disabled = {
                                    labelKey === "Id" || 
                                    labelKey === "userId" || 
                                    labelKey === "isBlocked" || 
                                    labelKey === "verificationStatus" || 
                                    labelKey === "role" 
                                }
                            />
                        )}
                    </div>
                ))}
                <button type="submit" className="submit-button">Update Profile</button>
            </form>
        </div>
    );
}

export default MyProfile;
