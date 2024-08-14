import React from 'react';
import { Link } from 'react-router-dom';
import '../../Assets/SectionStyles.css';

export function DriverDashboard() {
    return (
        <div className="user-section">
            <h2>Driver Dashboard</h2>
            <Link to="/driver/new-rides" className="profile-link">View New Rides</Link>
            <Link to="/driver/my-rides" className="profile-link">View My Completed Rides</Link>
        </div>
    );
}