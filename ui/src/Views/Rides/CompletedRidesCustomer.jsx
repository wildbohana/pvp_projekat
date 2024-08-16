import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import { toast } from 'react-toastify';

import '../../Assets/Rides.css';

import { Ride } from '../../Models/Ride';
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
            setRides(response.data);
        } catch (error) {
            console.error('Error fetching rides:', error);
			toast("Error fetching previous rides.");
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
				<div className="grid-item" style={{ gridColumn: 7, gridRow: 1 }}>Requested at</div>
				<div className="grid-item" style={{ gridColumn: 8, gridRow: 1 }}>Status</div>
				<div className="grid-item" style={{ gridColumn: 9, gridRow: 1 }}>Rate ride</div>

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
							{ ride.status }
						</div>
						<div className="grid-item" style={{ gridColumn: 9, gridRow: index + 2 }}>
							{ ride.rating === 0 ? (
								<button className="action-button" onClick={() => navigate(`/user/rate-ride/${ride.id}`)}>Rate ride</button>
							) : (
								ride.rating
							)}
						</div>
					</React.Fragment>
				))}
			</div>
        </div>
    );
};

export default CompletedRides;
