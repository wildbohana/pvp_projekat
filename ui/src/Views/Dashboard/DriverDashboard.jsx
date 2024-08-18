import React from 'react';
import { Link } from 'react-router-dom';
import '../../Assets/Dashboard.css';

export function DriverDashboard() {
    return (
        <div className="user-section">
            <h2>Driver Dashboard</h2>
            <Link to="/driver/new-rides">View New Rides</Link>
            <Link to="/driver/my-rides">View My Completed Rides</Link>
        </div>
    );
}