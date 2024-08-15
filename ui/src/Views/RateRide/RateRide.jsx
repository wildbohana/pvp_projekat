import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { toast } from 'react-toastify';

import { GetAverageRatingAsync, RateRideAsync, RatedCheckAsync } from '../../Services/ratingService';

const RateRide = () => {
    const [rating, setRating] = useState(0);
    const { rideId } = useParams();
    const navigate = useNavigate();

    const handleSubmitRating = async () => {
        try {
			const request = {
				RideId: rideId,
				Rate: rating
			};
			const response = await RateRideAsync(request);
            navigate('/');
        } catch (error) {
            console.error('Error submitting rating:', error.message);
			toast("Error submiting ride rating.");
        }
    };

    return (
        <div className="driver-rating-page">
            <h2>Rate Your Drive</h2>
            <div className="rating-container">
                {[1, 2, 3, 4, 5].map((star) => (
                    <span
                        key={star}
                        className={star <= rating ? 'star active' : 'star'}
                        data-star={star}
                        onClick={() => setRating(star)}
                    >
                        &#9733;
                    </span>
                ))}
            </div>
            <button onClick={handleSubmitRating}>Submit Rating</button>
        </div>
    );
};

export default RateRide;
