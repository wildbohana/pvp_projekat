import React, { useState, useEffect } from 'react';
import axiosInstance from '../../Utils/axiosInstance';
import { toast, ToastContainer } from 'react-toastify';
import { useNavigate } from 'react-router-dom';
import 'react-toastify/dist/ReactToastify.css';
import '../../Assets/Styles/MyProfile.css';
import '../../Assets/Styles/HomePage.css';

// Ako je vozač - navigacija do: pending rides, all my previous rides
// Ako je user - navigacija do: new ride, rate ride, all my previous rides

function HomePage() {
    const navigate = useNavigate();

    return (
        <div className="homepage-container">
            <ToastContainer position="top-right" autoClose={2000} hideProgressBar={false} newestOnTop={false} closeOnClick rtl={false} pauseOnFocusLoss draggable pauseOnHover />
            
            <div>
                <button>Create new ride</button>
                <button>Rate previous ride</button>
                <button>Show all previous rides</button>
            </div>
            
            
    	</div>
    );    
}

export default HomePage;
