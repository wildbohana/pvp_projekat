import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { NewRideRequestAsync, GetRideEstimateAsync, GetRideEstimateUserAsync, ConfirmRideRequestAsync, DeleteRideRequestAsync } from '../../Services/rideService';

const CreateRide = () => {
	const [startAddress, setStartAddress] = useState('');
    const [endAddress, setEndAddress] = useState('');
    const [price, setPrice] = useState(0);
    const [predicted, setPredicted] = useState(false);
	// wtf is this
	const [pickupTime, setPickupTime] = useState(0);
    const navigate = useNavigate();

    const handlePredictRide = async () => {
        try {
            const request = {
                StartAddress: startAddress,
                EndAddress: endAddress,
            };
			const response = await NewRideRequestAsync(request);

            if (response.status === 200) {
                setPrice(response.price);
                setPickupTime(response.pickUpTime);	// za X minuta
                setPredicted(true);
				localStorage.setItem('requestedRide', response.data.id);
            }
        } catch (error) {
            console.error('Error predicting ride:', error);
        }
    };

    const handleConfirmRide = async () => {
        try {
			const rideId = localStorage.getItem('requestedRide');
			const response = await ConfirmRideRequestAsync(rideId);
			if (response.status === 200) {
				toast("Ride request confirmed.");
			}
        } catch (error) {
            console.error('Error occurred', error);
			toast("Error requesting a ride.");
        }
    };

    const handleDeleteRide = async () => {
        try {
			const rideId = localStorage.getItem('requestedRide');
			const response = await DeleteRideRequestAsync(rideId);
			if (response.status === 200) {
				toast("Ride request deleted.");
				localStorage.removeItem('requestedRide');
			}
        } catch (error) {
            console.error('Error occurred', error);
			toast("Error requesting a ride.");
        }
    };

    return (
        <div className="create-ride-container">
            <h2>Create New Ride</h2>
            <input
                type="text"
                placeholder="Start Address"
                value={startAddress}
                onChange={(e) => setStartAddress(e.target.value)}
            />
            <input
                type="text"
                placeholder="End Address"
                value={endAddress}
                onChange={(e) => setEndAddress(e.target.value)}
            />
            <button className="submit" onClick={handlePredictRide}>Order</button>
            {predicted && (
                <div className="prediction-results">
                    <p>Price: ${price}</p>
                    <p>Estimated Arrival: {pickupTime ? pickupTime.getHours() + ":" + pickupTime.getMinutes() : ''}</p>
                    <button className="submit" onClick={handleConfirmRide}>Confirm</button>
					<button className="submit" onClick={handleDeleteRide}>Delete</button>
                </div>
            )}
        </div>
    );
};

export default CreateRide;
