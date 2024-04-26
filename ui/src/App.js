// [RUN WITH] npm start
/* 
Pre prvog pokretanja u terminalu uneti: 
npm install react-scripts --save
npm install react-router-dom
npm install axios
*/

// TODO: ne zaboravi da dodaš .env i varijable u fajl !!!

import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import './App.css';
import { Login } from './Pages/Auth/Login';
import { Register } from './Pages/Auth/Register';
import { Home } from './Pages/Home/Home';

function App() {
	return (
		<Router>
			<Routes>
				<Route path="/register" element={<Register />} />
				<Route path="/" element={<Login />} />
				<Route path="/home" element={<Home />} />
			</Routes>
		</Router>
	);
}

export default App;
