import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import PropTypes from 'prop-types';

import './Assets/App.css';
import Navbar from "./Components/Navbar";

import Login from './Views/Auth/Login';
import Register from './Views/Auth/Register';
import MyProfile from './Views/Profile/MyProfile';
import Dashboard from './Views/Dashboard/Dashboard';
import { ToastContainer } from 'react-toastify';

function App() {

	const checkAuth = () => {
		const token = Cookies.get('jwt-token');
		return token && token !== '';
	};

	const ProtectedRoute = ({ children }) => {
		if (!checkAuth()) {
			return <Navigate to="/login" />;
		}
		return children;
	};

	ProtectedRoute.propTypes = {
		children: PropTypes.node.isRequired,
	};

	return (
		<Router>
		<ToastContainer position="top-right" autoClose={1600} hideProgressBar={false} />
		<Navbar />
			<Routes>
				<Route path="/login" element={<Login />} />	
				<Route path="/register" element={<Register />} />
				<Route path="/" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
				<Route path="/profile" element={<ProtectedRoute><MyProfile /></ProtectedRoute>} />
			</Routes>
		</Router>
	);
}

export default App;
