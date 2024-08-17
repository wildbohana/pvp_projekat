import React from 'react';
import { Link } from 'react-router-dom';
import '../../Assets/Dashboard.css';

export function AdminDashboard() {
    return (
        <div className="user-section">
            <h2>Admin Dashboard</h2>
            <Link to="/admin/drivers" >View Drivers</Link>
            <Link to="/admin/rides" >View Rides</Link>
        </div>
    );
}
