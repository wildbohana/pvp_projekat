import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

import '../../Assets/Rides.css';

import { GetAllCompletedRidesAsync } from '../../Services/rideService';

const MyRides = () => {
    const [rides, setRides] = useState([]);

    useEffect(() => {
        fetchCompletedRides();
    }, []);

    const fetchCompletedRides = async () => {
        try {
			// userId se iz tokena čita
            const response = await GetAllCompletedRidesAsync();
            setRides(response.data);
        } catch (error) {
            console.error('Error fetching completed rides:', error);
			toast("Error fetching rides.");
        }
    };

	// TODO change
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

export default MyRides;
