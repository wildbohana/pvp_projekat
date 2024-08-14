import { axiosClient } from "./axiosClient";

// Average driver rating (admin only)
export const GetAverageRatingAsync = async (id) => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/rating/get-rating/${id}`
	);
};

// Check if ride has been rated
export const RatedCheckAsync = async (id) => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/rating/rated-check/${id}`
	);
};

// Rate ride
export const RateRideAsync = async (data) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/rating/rate-ride`,
		data
	);
};
