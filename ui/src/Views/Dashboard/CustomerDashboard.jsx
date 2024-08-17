import React from 'react';
import { Link } from 'react-router-dom';
import '../../Assets/Dashboard.css';

export function CustomerDashboard() {
    return (
        <div className="user-section">
            <h2>Customer Dashboard</h2>
            <Link to="/user/create-ride">Create a Ride</Link>
            <Link to="/user/completed-rides">View Completed Rides</Link>
        </div>
    );
}
