import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import '../../Assets/DriverVerification.css';

import { BlockDriverAsync, VerifyDriverApproveAsync, VerifyDriverDenyAsync, GetAllDriversAsync } from '../../Services/adminService';

export default function DriverVerification() {
    const [drivers, setDrivers] = useState([]);

    useEffect(() => {
        fetchDrivers();
    }, []);

    const fetchDrivers = async () => {
        try {
            const response = await GetAllDriversAsync();
            setDrivers(response);
        } catch (error) {
            console.error('Error fetching drivers:', error);
			toast("Error fetching the drivers.");
        }
    };

    const handleVerifyApprove = async (id) => {
        try {
            const response = await VerifyDriverApproveAsync(id);
			toast("Driver verified!");
            fetchDrivers();
        } catch (error) {
            console.error('Error verifying driver:', error);
        }
    };

	const handleVerifyDeny = async (id) => {
        try {
            const response = await VerifyDriverDenyAsync(id);
			toast("Driver verification denied.");
            fetchDrivers();
        } catch (error) {
            console.error('Error verifying driver:', error);
        }
    };

    const handleBlock = async (id) => {
        try {
			const response = await BlockDriverAsync(id);
			fetchDrivers();
        } catch (error) {
            console.error('Error updating driver block status:', error);
			toast("An error has occured.");
        }
    };

    const handleBlockStatusChange = (id, isBlocked) => {
        setDrivers(prevDrivers =>
            prevDrivers.map(driver =>
                driver.id === id ? { ...driver, isBlocked: !isBlocked } : driver
            )
        );
    };

    return (
        <div>
			<div className="grid-container">
				<div className="grid-item header" style={{ gridColumn: 1, gridRow: 1 }}>Email</div>
				<div className="grid-item header" style={{ gridColumn: 2, gridRow: 1 }}>First Name</div>
				<div className="grid-item header" style={{ gridColumn: 3, gridRow: 1 }}>Last Name</div>
				<div className="grid-item header" style={{ gridColumn: 4, gridRow: 1 }}>Average Rating</div>
				<div className="grid-item header" style={{ gridColumn: 5, gridRow: 1 }}>Verify</div>
				<div className="grid-item header" style={{ gridColumn: 6, gridRow: 1 }}>Options</div>
				{drivers.map((driver, index) => (
					<React.Fragment key={driver.id}>
						<div className="grid-item" style={{ gridColumn: 1, gridRow: index + 2 }}>
							{driver.email}
						</div>
						<div className="grid-item" style={{ gridColumn: 2, gridRow: index + 2 }}>
							{driver.firstname}
						</div>
						<div className="grid-item" style={{ gridColumn: 3, gridRow: index + 2 }}>
							{driver.lastname}
						</div>
						<div className="grid-item" style={{ gridColumn: 4, gridRow: index + 2 }}>
							{driver.driverRating}
						</div>
						<div className="grid-item" style={{ gridColumn: 5, gridRow: index + 2 }}>
							<span className={`verification-status ${driver.isVerified ? 'verified' : 'not-verified'}`}>
								{driver.verificationStatus ? 'Verified' : 'Not Verified'}
							</span>
						</div>
						<div className="grid-item" style={{ gridColumn: 6, gridRow: index + 2 }}>
							<button
								onClick={() => handleVerifyApprove(driver.id)}
								className="verify-button"
							>
								Verify
							</button>
							<button
								onClick={() => handleVerifyDeny(driver.id)}
								className="verify-button"
							>
								Deny
							</button>
							<span className="block-status">
								{driver.isBlocked ? 'Blocked' : 'Unblocked'}
							</span>
							<button
								onClick={() => {
									handleBlock(driver.id);
									handleBlockStatusChange(driver.id, driver.isBlocked);
								}}
								className={driver.isBlocked ? 'unblock-button' : 'block-button'}
							>
								{driver.isBlocked ? 'Unblock' : 'Block'}
							</button>
						</div>
					</React.Fragment>
				))}
			</div>
        </div>
    );
};
