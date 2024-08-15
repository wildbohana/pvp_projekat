import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { NewRideRequestAsync, GetRideEstimateAsync, GetRideEstimateUserAsync, ConfirmRideRequestAsync, DeleteRideRequestAsync } from '../../Services/rideService';

const CreateRide = () => {
	const [startAddress, setStartAddress] = useState('');
    const [finalAddress, setFinalAddress] = useState('');
    const [isCustomerBusy, setIsCustomerBusy] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        fetchAcceptedRide();
    }, []);
    
    const fetchAcceptedRide = async () => {
        try {
            const response = await GetRideEstimateUserAsync();
            if (response.data)
            {
                localStorage.setItem('requestedRide', response.data.id)
            }
        }
        catch (error) {
            console.error('Error fetching active ride:', error);
            toast("Error fetching active ride!");
        } finally {
            checkBusyStatus();
        }
    }

    const checkBusyStatus = async() => {
        var ride = localStorage.getItem('requestedRide');
        if (ride === null) {
            setIsCustomerBusy(false);
        }
        else {
            setIsCustomerBusy(true);
        }
    }

    // RequestRide
    const handlePredictRide = async () => {
        try {
            const request = {
                StartAddress: startAddress,
                FinalAddress: finalAddress,
            };
			const response = await NewRideRequestAsync(request);

            if (response.status === 200) {
				localStorage.setItem('requestedRide', response.data.id);
            }
        } catch (error) {
            console.error('Error predicting ride:', error);
            toast("Error requesting ride.");
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

    // TODO dodati proveru da li ima nešto u requestedRide, i ako ima, dobaviti info o njoj

    return (
        <div className="create-ride-container">
            {!isCustomerBusy ? (
                <div>
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
                        value={finalAddress}
                        onChange={(e) => setFinalAddress(e.target.value)}
                    />
                    <button className="submit" onClick={handlePredictRide}>Order</button>
                </div>
            ) : (
                <div className="grid-container">
                <div className="grid-item"> 
                    You have allready requested a ride!
                </div>
                </div>
            )}
        </div>
    );
};

export default CreateRide;
