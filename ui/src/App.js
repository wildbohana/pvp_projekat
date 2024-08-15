import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import PropTypes from 'prop-types';
import { ToastContainer } from 'react-toastify';

import './Assets/App.css';
import Navbar from "./Components/Navbar";

import Login from './Views/Auth/Login';
import Register from './Views/Auth/Register';
import MyProfile from './Views/Profile/MyProfile';
import Dashboard from './Views/Dashboard/Dashboard';
import DriverVerification from './Views/Verification/DriverVerification'
import RateRide from './Views/RateRide/RateRide';
import CreateRide from './Views/CreateRide/CreateRide';
import AllRides from './Views/Rides/AllRidesAdmin';
import CompletedRides from './Views/Rides/CompletedRidesCustomer';
import MyRides from './Views/Rides/MyRidesDriver';
import PendingRides from './Views/Rides/PendingRidesDriver';

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

				{ /* TODO dodaj protected route! */}
				<Route path="/admin/drivers" element={<DriverVerification />} />	
				<Route path="/admin/rides" element={<AllRides />} />	
				<Route path="/driver/new-rides" element={<PendingRides />} />	
				<Route path="/user/completed-rides" element={<CompletedRides />} />	
				<Route path="/user/create-ride" element={<CreateRide />} />	
				<Route path="/driver/my-rides"element={<MyRides />} />	
				<Route path="/user/rate-driver/:rideId" element={<RateRide />} />	

			</Routes>
		</Router>
	);
}

export default App;
