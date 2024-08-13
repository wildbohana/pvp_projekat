import { axiosClient } from "./axiosClient";

// Approve driver verification
export const VerifyDriverApproveAsync = async (id) => {
	try {
		const response = await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/admin/verify-appprove/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Deny driver verification
export const VerifyDriverDenyAsync = async (id) => {
	try {
		const response = await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/admin/verify-deny/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Block/unblock driver
export const BlockDriverAsync = async (id) => {
	try {
		const response = await axiosClient.post(
			`${process.env.REACT_APP_API_URL}/admin/block/${id}`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};

// Get all drivers
export const GetAllDriversAsync = async () => {
	try {
		const response = await axiosClient.get(
			`${process.env.REACT_APP_API_URL}/admin/all-drivers`
		);
		return response.data;
	}
	catch (error) {
		throw new Error(error.response.data.error);
	}
};
