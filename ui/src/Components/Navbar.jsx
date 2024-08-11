import React from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import '../Assets/Styles/Navbar.css';
import Cookies from 'js-cookie';

const handleLogout = () => {
    Cookies.remove('jwt-token');
	//localStorage.removeItem('currentUser');
};

const Navbar = () => {
    const location = useLocation();

    return (
        <nav className="navbar">
            <NavLink to="/" className="nav-link">Home</NavLink>
            <NavLink to="/profile" className="nav-link">Profile</NavLink>
            {Cookies.get('jwt-token') && location.pathname !== '/login' && (
                <NavLink to="/login" className="nav-link logout-button" onClick={handleLogout}> Log Out </NavLink>
            )}
        </nav>
    );
};

export default Navbar;
