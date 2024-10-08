﻿using Common.DTOs;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IRideService : IService
    {
        // Svi
        Task<RideInfoDTO?> GetRideInfoAsync(string rideId);

        // Korisnici
        Task<RideInfoDTO?> CreateRideRequestAsync(RideNewDTO data, string customerId);
        Task<RideInfoDTO?> GetRideEstimationAsync(string rideId, string customerId);
        Task<RideInfoDTO?> GetRideEstimationForUserAsync(string customerId);
        Task<bool> ConfirmRideRequestAsync(string rideId, string customerId);
        Task<bool> DeleteRideRequestAsync(string rideId, string customerId);
        Task<IEnumerable<RideInfoDTO>> GetPreviousRidesCustomerAsync(string customerId);

        // Vozači
        Task<bool> AcceptRideAsync(string rideId, string driverId);
        Task<bool> CompleteRideAsync(string rideId, string driverId);
        Task<RideInfoDTO?>GetAcceptedRideForDriverAsync(string driverId);
        Task<IEnumerable<RideInfoDTO>> GetAllPendingRidesAsync();   // one koje su potvrđene od korisnika
        Task<IEnumerable<RideInfoDTO>> GetPreviousRidesDriverAsync(string driverId);

        // Admin
        Task<IEnumerable<RideInfoDTO>> GetAllRidesAdminAsync();

        // Ocenjivanje
        Task<bool> RateRideAsync(RatingDTO rate, string customerId);
        Task<double> GetAverageDriverRateAsync(string rideId);
    }
}
