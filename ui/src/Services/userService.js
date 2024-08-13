import { axiosClient } from "./axiosClient";

// Register
export const RegisterAsync = async (requestBody) => {
	try {
		const response = await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/auth/register`,
			requestBody
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Login
export const LoginAsync = async (requestBody) => {
	try {
		const response = await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/auth/login`, 
			requestBody
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Google login
//export const GoogleLogin = async (requestBody) => {
//  console.log("google");
//  return await axiosClient.post(
//    `${process.env.REACT_APP_API_URL}/users/external-login`,
//    requestBody
//	);
//};

// MyProfile
export const GetUserProfileAsync = async () => {
	try {
		const response = await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/users/profile`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Update profile
export const UpdateUserAsync = async (requestBody) => {
	try {
		const response = await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/users/update`,
			requestBody
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Get busy status
// UserId se čita iz tokena
export const GetBusyStatusAsync = async () => {
	try {
		const response = await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/users/busy`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Get verification status
export const GetVerificationStatusAsync = async (id) => {
	try {
		const response = await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/users/verified-check/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};
