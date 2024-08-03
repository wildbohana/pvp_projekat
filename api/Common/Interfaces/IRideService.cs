using Common.DTOs;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IRideService : IService
    {
        // Svi
        Task<RideInfoDTO?> GetRideInfoAsync(string rideId);

        // Korisnici
        Task<RideEstimateDTO?> CreateRideRequestAsync(RideNewDTO data, string customerId);     // vraća ili string rideId, ili RideEstimateDTO
        Task<RideEstimateDTO?> GetRideEstimationAsync(string rideId, string customerId);
        Task<bool> ConfirmRideRequestAsync(string rideId, string customerId);
        Task<bool> DeleteRideRequestAsync(string rideId, string customerId);      // šalje se ili rideId, ili RideEstimateDTO
        Task<IEnumerable<RideInfoDTO>> GetPreviousRidesCustomerAsync(string customerId);

        // Vozači
        Task<bool> AcceptRideAsync(string rideId, string driverId);
        Task<bool> CompleteRideAsync(string rideId, string driverId);
        Task<IEnumerable<RideInfoDTO>> GetAllPendingRidesAsync();   // one koje su potvrđene od korisnika
        Task<IEnumerable<RideInfoDTO>> GetPreviousRidesDriverAsync(string driverId);

        // Admin
        Task<IEnumerable<RideInfoDTO>> GetAllRidesAdminAsync();
    }
}
