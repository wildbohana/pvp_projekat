using Common.DTOs;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IRideService : IService
    {
        // Svi
        Task<RideInfoDTO?> GetRideInfoAsync(string rideId);

        // Korisnici
        Task<RideEstimateDTO> CreateRideRequestAsync(RideNewDTO data, string customerId);     // vraća ili string rideId, ili RideEstimateDTO
        Task<RideEstimateDTO> GetRideEstimationAsync(string rideId, string customerId);
        Task<bool> ConfirmRideRequestAsync(RideEstimateDTO data, string customerId);
        Task<bool> DeleteRideRequest(RideEstimateDTO data, string customerId);      // šalje se ili rideId, ili RideEstimateDTO

        // Vozači
        Task<bool> AcceptRideAsync(RideAcceptDTO data, string driverId);
        Task<bool> CompleteRideAsync(string rideId, string driverId);
        Task<IEnumerable<RideInfoDTO>> GetAllPendingRidesAsync();   // one koje su potvrđene od korisnika

        // Admin
        Task<IEnumerable<RideInfoDTO>> GetAllRidesAdminAsync();
        Task<IEnumerable<RideInfoDTO>> GetPreviousRidesDriverAsync(string driverId);
        Task<IEnumerable<RideInfoDTO>> GetPreviousRidesCustomerAsync(string customerId);
    }
}
