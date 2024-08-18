import React, { useState } from 'react'

import '../../Assets/Dashboard.css';

import { AdminDashboard } from './AdminDashboard';
import { DriverDashboard } from './DriverDashboard';
import { CustomerDashboard } from './CustomerDashboard';

function Dashboard() {
	const usertype = localStorage.getItem('usertype');
		
	function renderSection() {
		switch (usertype) {
		  case "Administrator":
			return <AdminDashboard />;
		  case "Driver":
			return <DriverDashboard />;
		  default:
			return <CustomerDashboard />;
		}
	}

	return (
		<div className="dashboard-container">
			<div className="dashboard-links">
				{renderSection()}
			</div>
		</div>
	);
}

export default Dashboard;
