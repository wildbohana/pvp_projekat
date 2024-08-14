import { axiosClient } from "./axiosClient";

// Register
export const RegisterAsync = async (requestBody) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/auth/register`,
		requestBody
	);
};

// Login
export const LoginAsync = async (requestBody) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/auth/login`, 
		requestBody
	);
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
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/users/profile`
	);
};

// Update profile
export const UpdateUserAsync = async (requestBody) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/users/update`,
		requestBody
	);
};

// Get busy status
// UserId se čita iz tokena
export const GetBusyStatusAsync = async () => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/users/busy`
	);
};

// Get verification status
export const GetVerificationStatusAsync = async (id) => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/users/verified-check/${id}`
	);
};
