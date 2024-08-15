import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

import '../../Assets/Rides.css';

import { GetAllPendingRidesAsync, AcceptRideAsync } from '../../Services/rideService';
import { GetUserProfileAsync } from '../../Services/userService';

const PendingRides = () => {
    const [rides, setRides] = useState([]);
    const [isDriverBlocked, setIsDriverBlocked] = useState(true);
	//const { setDeliveryTime, setShowOverlay } = useCountdownContext();
    const navigate = useNavigate();

    useEffect(() => {
        fetchPendingRides();
		checkDriverStatus(); 
    }, []);

    const fetchPendingRides = async () => {
        try {
			// userId se iz tokena čita
            const response = await GetAllPendingRidesAsync();
            setRides(response);
        } catch (error) {
            console.error('Error fetching completed rides:', error);
			toast("Error fetching rides.");
        }
    };

    const acceptRide = async (rideId) => {
        try {
            const response = await AcceptRideAsync(rideId);

            if (response) {
				// TODO promeni ovo ili na BE ili na FE
                const deliveryDateTime = new Date(response.data);
                //setDeliveryTime(deliveryDateTime);
                //setShowOverlay(true);
            }
            navigate('/dashboard');
            fetchPendingRides();
        } catch (error) {
            console.error('Error accepting ride:', error);
        }
    };

	// TODO promeni
    /*
    function renderStatus(status) {
        console.log(status)
        switch (status) {
            case RideStatus.Pending:
                return 'Pending';
            case RideStatus.InProgress:
                return 'In Progress';
            case RideStatus.Completed:
                return 'Completed';
            default:
                return 'Unknown';
        }
    };
    */

    const checkDriverStatus = async () => {
        try {

            const response = await GetUserProfileAsync();
            const user = response.data; 
            setIsDriverBlocked(user.isBlocked); 
        } catch (error) {
            console.error('Error checking driver status:', error);
			toast("Error checking driver status.");
            setIsDriverBlocked(true); 
        }
    };

    return (
        <div>
			 <div className="grid-container">
				<div className="grid-item" style={{ gridColumn: 1, gridRow: 1 }}>
					Start Address
				</div>
				<div className="grid-item" style={{ gridColumn: 2, gridRow: 1 }}>
					End Address
				</div>
				<div className="grid-item" style={{ gridColumn: 3, gridRow: 1 }}>
					Price
				</div>
				<div className="grid-item" style={{ gridColumn: 4, gridRow: 1 }}>
					Delivery Time
				</div>
				<div className="grid-item" style={{ gridColumn: 5, gridRow: 1 }}>
					Status
				</div>
				{rides.map((ride, index) => (
					<React.Fragment key={ride.id}>
						<div className="grid-item" style={{ gridColumn: 1, gridRow: index + 2 }}>
							{ride.startAddress}
						</div>
						<div className="grid-item" style={{ gridColumn: 2, gridRow: index + 2 }}>
							{ride.endAddress}
						</div>
						<div className="grid-item" style={{ gridColumn: 3, gridRow: index + 2 }}>
							{ride.price}
						</div>
						<div className="grid-item" style={{ gridColumn: 4, gridRow: index + 2 }}>
						{new Date(ride.deliveryTime).getDate()}/{new Date(ride.deliveryTime).getMonth()}/{new Date(ride.deliveryTime).getFullYear()} {new Date(ride.deliveryTime).getHours()}:{new Date(ride.deliveryTime).getMinutes()}
						</div>
						<div className="grid-item" style={{ gridColumn: 5, gridRow: index + 2 }}>
							{ /*renderStatus(ride.status)*/ }
                            { ride.status }
						</div>
					</React.Fragment>
				))}
			</div>
        </div>
    );
};

export default PendingRides;
