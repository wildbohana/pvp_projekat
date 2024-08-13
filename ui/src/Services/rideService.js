import { axiosClient } from "./axiosClient";

// Get ride info
export const GetRideInfoAsync = async (id) => {
	try {
		const response =  await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/ride/ride-info/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// New ride
export const NewRideRequestAsync = async (data) => {
	try {
		const response =  await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/ride/new-ride`,
			data
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Get ride estimate
export const GetRideEstimateAsync = async (id) => {
	try {
		const response =  await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/ride/ride-estimate/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Confirm ride request
export const ConfirmRideRequestAsync = async (id) => {
	try {
		const response =  await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/ride/confirm-request/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Delete ride request
export const DeleteRideRequestAsync = async (id) => {
	try {
		const response =  await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/ride/delete-request/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Previous rides customer
export const GetPreviousRidesCustomerAsync = async () => {
	try {
		const response =  await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/ride/previous-rides`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Accept ride (driver)
export const AcceptRideAsync = async (id) => {
	try {
		const response =  await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/ride/accept-ride/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Complete ride (driver)
export const CompleteRideAsync = async (id) => {
	try {
		const response =  await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/ride/complete-ride/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Get all pending rides (driver)
export const GetAllPendingRidesAsync = async () => {
	try {
		const response =  await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/ride/pending-rides`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Get all completed rides (driver)
export const GetAllCompletedRidesAsync = async () => {
	try {
		const response =  await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/ride/completed-rides`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Get all rides (admin)
export const GetAllRidesAsync = async () => {
	try {
		const response =  await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/ride/all-rides}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};
