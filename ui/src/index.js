import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './Assets/Index.css'
import { GoogleOAuthProvider } from "@react-oauth/google"

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
	<GoogleOAuthProvider clientId={process.env.REACT_APP_GOOGLE_CLIENTID}>
		<React.StrictMode>
			<App />
		</React.StrictMode>
	</GoogleOAuthProvider>
);
