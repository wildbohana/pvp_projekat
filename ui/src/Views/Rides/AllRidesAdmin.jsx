import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';

import '../../Assets/Rides.css';

import { GetAllRidesAsync } from '../../Services/rideService';

const AllRides = () => {
    const [rides, setRides] = useState([]);

    useEffect(() => {
        fetchRides();
    }, []);

    const fetchRides = async () => {
        try {
			const response = await GetAllRidesAsync();
            setRides(response.data);
        } catch (error) {
            console.error('Error fetching rides:', error);
			toast("Error fetching rides");
        }
    };

    return (
        <div>
			<div className="grid-container">
				<div className="grid-item" style={{ gridColumn: 1, gridRow: 1 }}>Customer</div>
				<div className="grid-item" style={{ gridColumn: 2, gridRow: 1 }}>Driver</div>
				<div className="grid-item" style={{ gridColumn: 3, gridRow: 1 }}>Start Address</div>
				<div className="grid-item" style={{ gridColumn: 4, gridRow: 1 }}>Final Address</div>
				<div className="grid-item" style={{ gridColumn: 5, gridRow: 1 }}>Price</div>
				<div className="grid-item" style={{ gridColumn: 6, gridRow: 1 }}>Distance</div>
				<div className="grid-item" style={{ gridColumn: 7, gridRow: 1 }}>Start Time</div>
				<div className="grid-item" style={{ gridColumn: 8, gridRow: 1 }}>Pickup Time</div>
				<div className="grid-item" style={{ gridColumn: 9, gridRow: 1 }}>Ride Duration</div>
				<div className="grid-item" style={{ gridColumn: 10, gridRow: 1 }}>Status</div>
				<div className="grid-item" style={{ gridColumn: 11, gridRow: 1 }}>Rating</div>

				{rides.map((ride, index) => (
					<React.Fragment key={ride.id}>
						<div className="grid-item" style={{ gridColumn: 1, gridRow: index + 2 }}>
							{ride.customerId}
						</div>
						<div className="grid-item" style={{ gridColumn: 2, gridRow: index + 2 }}>
							{ride.driverId}
						</div>
						<div className="grid-item" style={{ gridColumn: 3, gridRow: index + 2 }}>
							{ride.startAddress}
						</div>
						<div className="grid-item" style={{ gridColumn: 4, gridRow: index + 2 }}>
							{ride.finalAddress}
						</div>
						<div className="grid-item" style={{ gridColumn: 5, gridRow: index + 2 }}>
							{ride.price} din
						</div>
						<div className="grid-item" style={{ gridColumn: 6, gridRow: index + 2 }}>
							{ride.distance} km
						</div>
						<div className="grid-item" style={{ gridColumn: 7, gridRow: index + 2 }}>
						{new Date(ride.startTime).getDate()}/{new Date(ride.startTime).getMonth()}/{new Date(ride.startTime).getFullYear()} {new Date(ride.startTime).getHours()}:{new Date(ride.startTime).getMinutes()}
						</div>
						<div className="grid-item" style={{ gridColumn: 8, gridRow: index + 2 }}>
							{ride.pickUpTime} min
						</div>
						<div className="grid-item" style={{ gridColumn: 9, gridRow: index + 2 }}>
							{ride.rideDuration} min
						</div>
						<div className="grid-item" style={{ gridColumn: 10, gridRow: index + 2 }}>
							{ ride.status }
						</div>
						<div className="grid-item" style={{ gridColumn: 11, gridRow: index + 2 }}>
							{ ride.rating }
						</div>
					</React.Fragment>
				))}
			</div>
        </div>
    );
};

export default AllRides;
