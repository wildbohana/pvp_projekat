import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import { 
    GetUserProfileAsync, 
    UpdateUserAsync 
} from '../../Services/userService';

import '../../Assets/MyProfile.css';

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
        fetchProfile();
    }, []);

    const fetchProfile = async() => {
        try {
            const response = await GetUserProfileAsync();
			if (response.data) {
                setProfile(response.data);
			} else {
				toast("Bad request");
			}
        } catch (error) {
            console.error("Error fetching profile: ", error);
            toast("Error fetching profile");
        }
    }

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
            if (profile.NewPassword !== profile.ConfirmNewPassword) {
                console.log("New password doesn't match confimed new password.");
                toast("New passwords don't match.");
                return;
            }

            Object.keys(profile).forEach(field => {
                if (profile[field] === '' || profile[field] === undefined) {
                    delete profile[field];
                }
            })

            const response = await UpdateUserAsync(profile);
            if (response.status === 200) {
                toast("Profile updated successfully");
                await fetchProfile();
            }
        } catch (error) {
            toast("Error updating profile");
            console.error("Error updating profile: ", error);
        }
    };

    const labelMap = {
        firstname: 'First Name',
        lastname: 'Last Name',
        username: 'Userame',
        email: 'Email',
        photoUrl: 'Image',
        address: 'Address',
        dateOfBirth: 'Date of birth',
		role: 'User type',
        verificationStatus: 'Verification status',
        isBlocked: 'Blocked?',
        confirmOldPassword: 'Old password',
		newPassword: 'New password',
        confirmNewPassword: 'Confirm new password',
    };

    return (
        <div className="profile-container">
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
                        ) : labelKey === 'confirmOldPassword' ? (
                            <input
                                type = "password"
                                id = {labelKey}
                                name = {labelKey}
                                value = {profile[labelKey]}
                                onChange = {handleInputChange}
                                className = "form-control"
                                required
                            />
                        ) : labelKey === 'confirmNewPassword' || labelKey === 'newPassword' ? (
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
                                    labelKey === "id" || 
                                    labelKey === "email" || 
                                    labelKey === "isBlocked" || 
                                    labelKey === "verificationStatus" || 
                                    labelKey === "role" ||
                                    labelKey === "dateOfBirth"
                                }
                                required
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
