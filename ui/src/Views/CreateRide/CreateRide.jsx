import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { 
    NewRideRequestAsync, 
    GetRideEstimateUserAsync, 
    ConfirmRideRequestAsync, 
    DeleteRideRequestAsync 
} from '../../Services/rideService';

import '../../Assets/Rides.css';

const CreateRide = () => {
	const [startAddress, setStartAddress] = useState('');
    const [finalAddress, setFinalAddress] = useState('');
    const [isCustomerBusy, setIsCustomerBusy] = useState(false);
    const [ride, setRideRequest] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        fetchAcceptedRide();
    }, []);
    
    const fetchAcceptedRide = async () => {
        try {
            const response = await GetRideEstimateUserAsync();
            if (response.data)
            {
                localStorage.setItem('requestedRide', response.data.id);
                setRideRequest(response.data);
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
    const handleRideRequest = async () => {
        if (startAddress === '' || finalAddress === '') return;
        
        try {
            const request = {
                StartAddress: startAddress,
                FinalAddress: finalAddress,
            };
			const response = await NewRideRequestAsync(request);

            if (response.status === 200) {
				localStorage.setItem('requestedRide', response.data.id);
                toast("Ride sucessfully requested.");
                setTimeout(`window.location.reload()`, 2000);
            }
        } catch (error) {
            console.error('Error predicting ride:', error);
            toast("Error requesting ride.");
        }
    };

    const confirmRide = async () => {
        try {
            const rideId = ride.id;
            const response = await ConfirmRideRequestAsync(rideId);

            if (response.status === 200) {
				toast("Ride request confirmed.");
                setTimeout(`window.location.reload()`, 2000);
			}
        } catch (error) {
            console.error('Error confirming ride:', error);
            toast("Error confirming ride!");
        }
    };
    
    const deleteRide = async () => {
        try {
            const rideId = ride.id;
            const response = await DeleteRideRequestAsync(rideId);

            if (response.status === 200) {
				toast("Ride request deleted.");
                localStorage.removeItem('requestedRide');
                setTimeout(`window.location.reload()`, 2000);
			}
        } catch (error) {
            console.error('Error deleting request:', error);
            toast("Error deleting request!");
        }
    };
    
    return (
        <div>
            {!isCustomerBusy ? (
                <div className="grid-container-narrow">
                    <h2>Create New Ride</h2>
                    <form className="new-ride-form">
                        <input
                            type="text"
                            placeholder="Start Address"
                            value={startAddress}
                            onChange={(e) => setStartAddress(e.target.value)}
                            required 
                        />
                        <input
                            type="text"
                            placeholder="End Address"
                            value={finalAddress}
                            onChange={(e) => setFinalAddress(e.target.value)}
                            required 
                        />
                        <button className="action-button-narrow" onClick={handleRideRequest}>Order</button>
                    </form>
                </div>
            ) : (
                <div className="grid-container-narrow">
                    <div className="grid-item header" style={{ gridColumn: 1, gridRow: 1 }}>Start Address</div>
                    <div className="grid-item header" style={{ gridColumn: 1, gridRow: 2 }}>Final Address</div>
                    <div className="grid-item header" style={{ gridColumn: 1, gridRow: 3 }}>Price</div>
                    <div className="grid-item header" style={{ gridColumn: 1, gridRow: 4 }}>Distance</div>
                    <div className="grid-item header" style={{ gridColumn: 1, gridRow: 5 }}>Driver arrival</div>
                    <div className="grid-item header" style={{ gridColumn: 1, gridRow: 7 }}>Status</div>
                    <div className="grid-item header" style={{ gridColumn: 1, gridRow: 8 }}>Ride action</div>
                 
                    <div className="grid-item" style={{ gridColumn: 2, gridRow: 1 }}>
                        {ride.startAddress}
                    </div>
                    <div className="grid-item" style={{ gridColumn: 2, gridRow: 2 }}>
                        {ride.finalAddress}
                    </div>
                    <div className="grid-item" style={{ gridColumn: 2, gridRow: 3 }}>
                        {ride.price} din
                    </div>
                    <div className="grid-item" style={{ gridColumn: 2, gridRow: 4 }}>
                        {ride.distance} km
                    </div>
                    <div className="grid-item" style={{ gridColumn: 2, gridRow: 5 }}>
                    {new Date(ride.arrivalTime).getDate()}/{new Date(ride.arrivalTime).getMonth() + 1}/{new Date(ride.arrivalTime).getFullYear()} {new Date(ride.arrivalTime).getHours()}:{new Date(ride.arrivalTime).getMinutes()}
                    </div>
                    <div className="grid-item" style={{ gridColumn: 2, gridRow: 7 }}>
                        { ride.status }
                    </div>

                    <div className="grid-item" style={{ gridColumn: 2, gridRow: 8 }}>
                        { ride.status === 'Pending' ? ( 
                            <div>
                                <button onClick={() => confirmRide()} className="action-button-narrow-two">
                                    Accept Ride
                                </button>
                                <button onClick={() => deleteRide()} className="action-button-narrow-two">
                                Delete Ride
                                </button>
                            </div>
                        ) : (
                            <span>/</span>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default CreateRide;
