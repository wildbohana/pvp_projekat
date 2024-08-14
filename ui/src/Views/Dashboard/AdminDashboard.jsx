import React from 'react';
import { Link } from 'react-router-dom';
import '../../Assets/SectionStyles.css';

export function AdminDashboard() {
    return (
        <div className="user-section">
            <h2>Admin Dashboard</h2>
            <Link to="/admin/drivers" className="profile-link">View Drivers</Link>
            <Link to="/admin/rides" className="profile-link">View Rides</Link>
        </div>
    );
}
