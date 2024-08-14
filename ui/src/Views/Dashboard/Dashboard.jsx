import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import { toast } from 'react-toastify';

function Dashboard() {    
	return (
		<div className="App">
		<div className="auth-form-container">
			<h2>Dashboard</h2>
			<p> HI MOM! </p>
		</div>
		</div>
	)
}

export default Dashboard;
