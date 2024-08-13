import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import PropTypes from 'prop-types';

import './Assets/App.css';
import Login from './Views/Auth/Login';
import Register from './Views/Auth/Register';
import Navbar from "./Components/Navbar";

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
		<Navbar />
			<Routes>
				<Route path="/login" element={<Login />} />	
				<Route path="/register" element={<Register />} />	
			</Routes>
		</Router>
	);
}

export default App;
