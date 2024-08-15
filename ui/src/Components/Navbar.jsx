import React from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import '../Assets/Navbar.css';
import Cookies from 'js-cookie';

const handleLogout = () => {
    Cookies.remove('jwt-token');
	localStorage.removeItem('usertype');
    localStorage.removeItem('requestedRide');
    localStorage.removeItem('confirmedRide');
};

const Navbar = () => {
    const location = useLocation();

    return (
        <nav className="navbar">
            {location.pathname === '/login' && (
                <NavLink to="/register" className="nav-link">Register</NavLink>
            )}
             {location.pathname === '/register' && (
                <NavLink to="/login" className="nav-link">Log In</NavLink>
            )}
            {Cookies.get('jwt-token') &&
            (
                <>
                    <NavLink to="/" className="nav-link">Dashboard</NavLink>
                    <NavLink to="/profile" className="nav-link">Profile</NavLink>
                </>
            )}
            {Cookies.get('jwt-token') && location.pathname !== '/login' && (
                <NavLink to="/login" className="nav-link logout-button" onClick={handleLogout}> Log Out </NavLink>
            )}
        </nav>
    );
};

export default Navbar;
