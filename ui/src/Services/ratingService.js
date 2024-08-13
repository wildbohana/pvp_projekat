import { axiosClient } from "./axiosClient";

// Average driver rating (admin only)
export const GetAverageRatingAsync = async (id) => {
  try {
		const response = await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/rating/get-rating/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Check if ride has been rated
export const RatedCheckAsync = async (id) => {
  try {
		const response = axiosClient.get(
			`${process.env.REACT_APP_API_URL}/rating/rated-check/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Rate ride
export const RateRideAsync = async (data) => {
  try {
		const response =  await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/rating/rate-ride`,
			data
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};
