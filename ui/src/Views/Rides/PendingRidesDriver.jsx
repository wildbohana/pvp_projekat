import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

import '../../Assets/Rides.css';

import { 
    GetAllPendingRidesAsync, 
    AcceptRideAsync, 
    GetRideEstimateDriverAsync,
    CompleteRideAsync
} from '../../Services/rideService';
import { 
    GetUserProfileAsync 
} from '../../Services/userService';

const PendingRides = () => {
    const [rides, setRides] = useState([]);
    const [isDriverBlocked, setIsDriverBlocked] = useState(false);
    const [isDriverBusy, setIsDriverBusy] = useState(false);
    const [ride, setActiveRide] = useState();
    const navigate = useNavigate();

    useEffect(() => {
        fetchAcceptedRide();
        fetchPendingRides();
		checkDriverStatus(); 
    }, []);

    const fetchPendingRides = async () => {
        try {
			// userId se iz tokena čita
            const response = await GetAllPendingRidesAsync();
            setRides(response.data);
        } catch (error) {
            console.error('Error fetching completed rides:', error);
			toast("Error fetching rides.");
        }
    };

    const fetchAcceptedRide = async () => {
        try {
            const response = await GetRideEstimateDriverAsync();
            if (response.data)
            {
                localStorage.setItem('confirmedRide', response.data.id);
                setActiveRide(response.data);
            }
        }
        catch (error) {
            console.error('Error fetching active ride:', error);
            toast("Error fetching active ride!");
        } finally {
            checkBusyStatus();
        }
    }

    const acceptRide = async (rideId) => {
        try {
            const response = await AcceptRideAsync(rideId);

            // vraća bool
            if (response.data) {
                localStorage.setItem('confirmedRide', rideId);
                toast("Ride accepted.");
                setTimeout(`window.location.reload()`, 2000);
            }
        } catch (error) {
            console.error('Error accepting ride:', error);
            toast("Error accepting ride!");
        }
    };
    
    const completeRide = async () => {
        try {
            const rideId = ride.id;
            const response = await CompleteRideAsync(rideId);

            // vraća bool
            if (response.data) {
                localStorage.removeItem('confirmedRide', rideId);
                toast("Ride completed.");
                setTimeout(`window.location.reload()`, 2000);
            }
        } catch (error) {
            console.error('Error accepting ride:', error);
            toast("Error accepting ride!");
        }
    };

    const checkBusyStatus = async() => {
        var rideLocal = localStorage.getItem('confirmedRide');
        if (rideLocal === null) {
            setIsDriverBusy(false);
        }
        else {
            setIsDriverBusy(true);
        }
    }

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

    // TODO: ako ima aktivnu vožnju, dodati navigaciju do statusa te vožnje?
    // Ili do odbrojavanja
    // I tu dodati dugme za MarkRideAsCompleted

    return (
        <div>
            {isDriverBusy ? (
                <div className="grid-container-narrow">
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 1 }}>Start Address</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 2 }}>Final Address</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 3 }}>Price</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 4 }}>Distance</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 5 }}>ETA</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 7 }}>Status</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 8 }}>Accept ride</div>
                
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
                {new Date(ride.startTime).getDate()}/{new Date(ride.startTime).getMonth() + 1}/{new Date(ride.startTime).getFullYear()} {new Date(ride.startTime).getHours()}:{new Date(ride.startTime).getMinutes()}
                </div>
                <div className="grid-item" style={{ gridColumn: 2, gridRow: 7 }}>
                    { ride.status }
                </div>

                <div className="grid-item" style={{ gridColumn: 2, gridRow: 8 }}>
                    { ride.status === 'InProgress' ? ( 
                        <button onClick={() => completeRide()} className="action-button-narrow">
                            Complete Ride
                        </button>
                    ) : (
                        <span>Blocked</span>
                    )}
                </div>
                </div>
            ) : (
                <div className="grid-container">
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 1 }}>Customer</div>
                    <div className="grid-item header" style={{ gridColumn: 2, gridRow: 1 }}>Start Address</div>
                    <div className="grid-item header" style={{ gridColumn: 3, gridRow: 1 }}>Final Address</div>
                    <div className="grid-item header" style={{ gridColumn: 4, gridRow: 1 }}>Price</div>
                    <div className="grid-item header" style={{ gridColumn: 5, gridRow: 1 }}>Distance</div>
                    <div className="grid-item header" style={{ gridColumn: 6, gridRow: 1 }}>Requested at</div>
                    <div className="grid-item header" style={{ gridColumn: 7, gridRow: 1 }}>Pickup Time</div>
                    <div className="grid-item header" style={{ gridColumn: 8, gridRow: 1 }}>Status</div>
                    <div className="grid-item header" style={{ gridColumn: 9, gridRow: 1 }}>Accept ride</div>

                    {rides.map((ride, index) => (
                        <React.Fragment key={ride.id}>
                            <div className="grid-item" style={{ gridColumn: 1, gridRow: index + 2 }}>
                                {ride.customerId}
                            </div>
                            <div className="grid-item" style={{ gridColumn: 2, gridRow: index + 2 }}>
                                {ride.startAddress}
                            </div>
                            <div className="grid-item" style={{ gridColumn: 3, gridRow: index + 2 }}>
                                {ride.finalAddress}
                            </div>
                            <div className="grid-item" style={{ gridColumn: 4, gridRow: index + 2 }}>
                                {ride.price} din
                            </div>
                            <div className="grid-item" style={{ gridColumn: 5, gridRow: index + 2 }}>
                                {ride.distance} km
                            </div>
                            <div className="grid-item" style={{ gridColumn: 6, gridRow: index + 2 }}>
                            {new Date(ride.startTime).getDate()}/{new Date(ride.startTime).getMonth() + 1}/{new Date(ride.startTime).getFullYear()} {new Date(ride.startTime).getHours()}:{new Date(ride.startTime).getMinutes()}
                            </div>
                            <div className="grid-item" style={{ gridColumn: 7, gridRow: index + 2 }}>
                                {ride.pickUpTime} min
                            </div>
                            <div className="grid-item" style={{ gridColumn: 8, gridRow: index + 2 }}>
                                { ride.status }
                            </div>

                            <div className="grid-item" style={{ gridColumn: 9, gridRow: index + 2 }}>
                                {!isDriverBlocked || !isDriverBusy ? ( 
                                    <button onClick={() => acceptRide(ride.id)} className="action-button">
                                        Accept Ride
                                    </button>
                                ) : (
                                    <span>Blocked</span>
                                )}
                            </div>
                        </React.Fragment>
                    ))}
                </div>
            )}
        </div>
    );
};

export default PendingRides;
