import { axiosClient } from "./axiosClient";

// Get ride info
export const GetRideInfoAsync = async (id) => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/ride/ride-info/${id}`
	);
};

// New ride
export const NewRideRequestAsync = async (data) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/ride/new-ride`,
		data
	);
};

// Get ride estimate
export const GetRideEstimateAsync = async (id) => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/ride/ride-estimate/${id}`
	);
};

// Get ride estimate for user
// Takođe služi za proveru da li je već poslat zahtev
// Ako jeste, onemogućava se pravljenje novog zahteva za vožnju
export const GetRideEstimateUserAsync = async () => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/ride/ride-estimate-user`
	);
};

// Confirm ride request
export const ConfirmRideRequestAsync = async (id) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/ride/confirm-request/${id}`
	);
};

// Delete ride request
export const DeleteRideRequestAsync = async (id) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/ride/delete-request/${id}`
	);
};

// Previous rides customer
export const GetPreviousRidesCustomerAsync = async () => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/ride/previous-rides`
	);
};

// Accept ride (driver)
export const AcceptRideAsync = async (id) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/ride/accept-ride/${id}`
	);
};

// Complete ride (driver)
export const CompleteRideAsync = async (id) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/ride/complete-ride/${id}`
	);
};

// Get all pending rides (driver)
export const GetAllPendingRidesAsync = async () => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/ride/pending-rides`
	);
};

// Get all completed rides (driver)
export const GetAllCompletedRidesAsync = async () => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/ride/completed-rides`
	);
};

// Get all rides (admin)
export const GetAllRidesAsync = async () => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/ride/all-rides}`
	);
};
