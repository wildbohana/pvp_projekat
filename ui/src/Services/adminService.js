import { axiosClient } from "./axiosClient";

// Approve driver verification
export const VerifyDriverApproveAsync = async (id) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/admin/verify-approve?driverId=${id}`
	);
};

// Deny driver verification
export const VerifyDriverDenyAsync = async (id) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/admin/verify-deny?driverId=${id}`
	);
};

// Block/unblock driver
export const BlockDriverAsync = async (id) => {
	return await axiosClient.post(
		`${process.env.REACT_APP_API_URL}/admin/block?driverId=${id}`
	);
};

// Get all drivers
export const GetAllDriversAsync = async () => {
	return await axiosClient.get(
		`${process.env.REACT_APP_API_URL}/admin/all-drivers`
	);
};
