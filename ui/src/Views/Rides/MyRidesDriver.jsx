import React, { useState, useEffect } from 'react';
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

    return (
        <div>
			 <div className="grid-container">
				<div className="grid-item header" style={{ gridColumn: 1, gridRow: 1 }}>Start Address</div>
				<div className="grid-item header" style={{ gridColumn: 2, gridRow: 1 }}>Final Address</div>
				<div className="grid-item header" style={{ gridColumn: 3, gridRow: 1 }}>Price</div>
				<div className="grid-item header" style={{ gridColumn: 4, gridRow: 1 }}>Start Time</div>
				<div className="grid-item header" style={{ gridColumn: 5, gridRow: 1 }}>Arrival Time</div>
				<div className="grid-item header" style={{ gridColumn: 6, gridRow: 1 }}>Status</div>
				<div className="grid-item header" style={{ gridColumn: 7, gridRow: 1 }}>Customer</div>
				<div className="grid-item header" style={{ gridColumn: 8, gridRow: 1 }}>Rate</div>

				{rides.map((ride, index) => (
					<React.Fragment key={ride.id}>
						<div className="grid-item" style={{ gridColumn: 1, gridRow: index + 2 }}>
							{ride.startAddress}
						</div>
						<div className="grid-item" style={{ gridColumn: 2, gridRow: index + 2 }}>
							{ride.finalAddress}
						</div>
						<div className="grid-item" style={{ gridColumn: 3, gridRow: index + 2 }}>
							{ride.price} din
						</div>
						<div className="grid-item" style={{ gridColumn: 4, gridRow: index + 2 }}>
						{new Date(ride.startTime).getDate()}/{new Date(ride.startTime).getMonth() + 1}/{new Date(ride.startTime).getFullYear()} {new Date(ride.startTime).getHours()}:{new Date(ride.startTime).getMinutes()}
						</div>
						<div className="grid-item" style={{ gridColumn: 5, gridRow: index + 2 }}>
						{new Date(ride.arrivalTime).getDate()}/{new Date(ride.arrivalTime).getMonth() + 1}/{new Date(ride.arrivalTime).getFullYear()} {new Date(ride.arrivalTime).getHours()}:{new Date(ride.arrivalTime).getMinutes()}
						</div>
						<div className="grid-item" style={{ gridColumn: 6, gridRow: index + 2 }}>
							{ ride.status }
						</div>
						<div className="grid-item" style={{ gridColumn: 7, gridRow: index + 2 }}>
							{ ride.customerId }
						</div>
						<div className="grid-item" style={{ gridColumn: 8, gridRow: index + 2 }}>
							{ ride.rating }
						</div>
					</React.Fragment>
				))}
			</div>
        </div>
    );
};

export default MyRides;
