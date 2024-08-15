import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import { toast } from 'react-toastify';

import '../../Assets/Rides.css';

import { GetPreviousRidesCustomerAsync } from '../../Services/rideService';

const CompletedRides = () => {
    const [rides, setRides] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        fetchCompletedRides();
    }, []);

    const fetchCompletedRides = async () => {
        try {
            // userId iz tokena dobija
            const response = await GetPreviousRidesCustomerAsync();
            setRides(response);
        } catch (error) {
            console.error('Error fetching rides:', error);
			toast("Error fetching previous rides.");
        }
    };

	// TODO change
	/*
    function renderStatus(status) {
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
    };*/

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
				<div className="grid-item" style={{ gridColumn: 6, gridRow: 1 }}>
					Options
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
						{!ride.isRated && (
							<button className="action-button" onClick={() => navigate(`/user/rate-driver/${ride.id}`)}>Rate Driver</button>
						)}
					</React.Fragment>
				))}
			</div>
        </div>
    );
};

export default CompletedRides;
