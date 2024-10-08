import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import Countdown from 'react-countdown';

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
    const Completionist = () => <span>Better hurry up!</span>;

    useEffect(() => {
        checkDriverStatus(); 
        fetchAcceptedRide();
    }, []);

    const fetchPendingRides = async () => {
        try {
            const response = await GetAllPendingRidesAsync();
            setRides(response.data);
        } catch (error) {
            console.error('Error fetching completed rides:', error);
			toast("Error fetching rides.");
        }
    };

    const fetchAcceptedRide = async () => {
        try {
            localStorage.removeItem('confirmedRide');
            const response = await GetRideEstimateDriverAsync();
            if (response.data)
            {
                localStorage.setItem('confirmedRide', response.data.id);
                setActiveRide(response.data);
            }
        } catch (error) {
            console.error('Error fetching active ride:', error);
            toast("Error fetching active ride!");
        } finally {
            await checkBusyStatus();
        }
    }

    const acceptRide = async (rideId) => {
        try {
            const response = await AcceptRideAsync(rideId);

            // vraća bool
            if (response.data) {
                //localStorage.setItem('confirmedRide', rideId);
                toast("Ride accepted.");
                await fetchAcceptedRide();
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
                localStorage.removeItem('confirmedRide');
                toast("Ride completed.");
                await fetchAcceptedRide();
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
            await fetchPendingRides();
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

    return (
        <div>
            {/* Kada vozač ima aktivnu vožnju */}
            {isDriverBusy ? (
                <div className="grid-container-narrow">
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 1 }}>Start Address</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 2 }}>Final Address</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 3 }}>Price</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 4 }}>Distance</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 5 }}>Start time</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 6 }}>Expected arrival</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 7 }}>Status</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 8 }}>Countdown</div>
                <div className="grid-item header" style={{ gridColumn: 1, gridRow: 9 }}>Ride action</div>
                
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
                <div className="grid-item" style={{ gridColumn: 2, gridRow: 6 }}>
                {new Date(ride.arrivalTime).getDate()}/{new Date(ride.arrivalTime).getMonth() + 1}/{new Date(ride.arrivalTime).getFullYear()} {new Date(ride.arrivalTime).getHours()}:{new Date(ride.arrivalTime).getMinutes()}
                </div>
                <div className="grid-item" style={{ gridColumn: 2, gridRow: 7 }}>
                    { ride.status }
                </div>
                <div className="grid-item" style={{ gridColumn: 2, gridRow: 8 }}>
                    <Countdown date={ride.arrivalTime}>
                        <Completionist />
                    </Countdown>
                </div>
                <div className="grid-item" style={{ gridColumn: 2, gridRow: 9 }}>
                    { ride.status === 'InProgress' ? ( 
                        <button onClick={() => completeRide()} className="action-button-table">
                            Complete Ride
                        </button>
                    ) : (
                        <span>/</span>
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
                    <div className="grid-item header" style={{ gridColumn: 7, gridRow: 1 }}>Status</div>
                    <div className="grid-item header" style={{ gridColumn: 8, gridRow: 1 }}>Ride action</div>

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
                                { ride.status }
                            </div>

                            <div className="grid-item" style={{ gridColumn: 8, gridRow: index + 2 }}>
                                {!isDriverBlocked || !isDriverBusy ? ( 
                                    <button onClick={() => acceptRide(ride.id)} className="action-button-table">
                                        Accept Ride
                                    </button>
                                ) : (
                                    <span>/</span>
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
