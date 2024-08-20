import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import '../../Assets/DriverVerification.css';

import { 
	BlockDriverAsync, 
	VerifyDriverApproveAsync, 
	VerifyDriverDenyAsync, 
	GetAllDriversAsync 
} from '../../Services/adminService';

export default function DriverVerification() {
    const [drivers, setDrivers] = useState([]);

    useEffect(() => {
        fetchDrivers();
    }, []);

    const fetchDrivers = async () => {
        try {
            const response = await GetAllDriversAsync();
            setDrivers(response.data);
        } catch (error) {
            console.error('Error fetching drivers:', error);
			toast("Error fetching the drivers.");
        }
    };

    const handleVerifyApprove = async (id) => {
        try {
            const response = await VerifyDriverApproveAsync(id);
			if (response.data) {
				toast("Driver verified!");
				await fetchDrivers();
			} else {
				toast("Bad request");
			}
        } catch (error) {
            console.error('Error verifying driver:', error);
			toast("Error while verifying driver.");
        }
    };

	const handleVerifyDeny = async (id) => {
        try {
            const response = await VerifyDriverDenyAsync(id);
			if (response.data) {
				toast("Driver verification denied.");
				fetchDrivers();
			} else {
				toast("Bad request!");
			}			
        } catch (error) {
            console.error('Error verifying driver:', error);
			toast("Error while verifying driver.");
        }
    };

    const handleBlock = async (id) => {
        try {
			const response = await BlockDriverAsync(id);
			if (response.data) {
				toast("Success!");
				fetchDrivers();
			} else {
				toast("Bad request!");
			}
        } catch (error) {
            console.error('Error updating driver block status:', error);
			toast("Error while blocking driver.");
        }
    };

    const handleBlockStatusChange = (id, isBlocked) => {
        setDrivers(prevDrivers =>
            prevDrivers.map(driver =>
                driver.email === id ? { ...driver, isBlocked: !isBlocked } : driver
            )
        );
    };

    return (
        <div>
			<div className="driver-container">
				<div className="driver-item header" style={{ gridColumn: 1, gridRow: 1 }}>Email</div>
				<div className="driver-item header" style={{ gridColumn: 2, gridRow: 1 }}>First Name</div>
				<div className="driver-item header" style={{ gridColumn: 3, gridRow: 1 }}>Last Name</div>
				<div className="driver-item header" style={{ gridColumn: 4, gridRow: 1 }}>Average Rating</div>
				<div className="driver-item header" style={{ gridColumn: 5, gridRow: 1 }}>Status</div>
				<div className="driver-item header" style={{ gridColumn: 6, gridRow: 1 }}>Verify</div>
				<div className="driver-item header" style={{ gridColumn: 7, gridRow: 1 }}>Block status</div>

				{drivers.map((driver, index) => (
					<React.Fragment key={driver.id}>
						<div className="driver-item" style={{ gridColumn: 1, gridRow: index + 2 }}>
							{driver.email}
						</div>
						<div className="driver-item" style={{ gridColumn: 2, gridRow: index + 2 }}>
							{driver.firstname}
						</div>
						<div className="driver-item" style={{ gridColumn: 3, gridRow: index + 2 }}>
							{driver.lastname}
						</div>
						<div className="driver-item" style={{ gridColumn: 4, gridRow: index + 2 }}>
							{Math.round((driver.averageRating + Number.EPSILON) * 100) / 100}
						</div>
						<div className="driver-item" style={{ gridColumn: 5, gridRow: index + 2 }}>
							{driver.verificationStatus}
						</div>
						<div className="driver-item" style={{ gridColumn: 6, gridRow: index + 2 }}>
							<div className="display-vertical" >
								<button
									onClick={() => handleVerifyApprove(driver.email)}
									disabled = {driver.verificationStatus === 'Approved' || driver.verificationStatus === 'Denied' }
									className={driver.verificationStatus === 'Approved' || driver.verificationStatus === 'Denied' ? "verify-button-disabled" : "verify-button"}>
									Verify
								</button>
								<button
									onClick={() => handleVerifyDeny(driver.email)}
									disabled = {driver.verificationStatus === 'Approved' || driver.verificationStatus === 'Denied' }
									className={driver.verificationStatus === 'Approved' || driver.verificationStatus === 'Denied' ? "verify-button-disabled" : "verify-button"}>
									Deny
								</button>
							</div>
						</div>
						<div className="driver-item" style={{ gridColumn: 7, gridRow: index + 2 }}>
							<div className="display-vertical" >
								<span className="block-status">
									{driver.isBlocked ? 'Blocked' : 'Unblocked'}
								</span>
								<button
									onClick={() => {
										handleBlock(driver.email);
										handleBlockStatusChange(driver.email, driver.isBlocked);
									}}
									disabled={driver.verificationStatus !== 'Approved'}
									className={driver.isBlocked ? 'unblock-button' : 'block-button'}>
									{driver.isBlocked ? 'Unblock' : 'Block'}
								</button>
							</div>
						</div>
					</React.Fragment>
				))}
			</div>
        </div>
    );
};
