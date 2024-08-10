import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './Assets/Styles/Index.css'

// Ovde kasnije dodati auth provider za Oauth

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
	<React.StrictMode>
		<App />
	</React.StrictMode>
);
