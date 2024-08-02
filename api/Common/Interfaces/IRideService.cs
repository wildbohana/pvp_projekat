using Common.DTOs;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IRideService : IService
    {
        Task<bool> CreateRideRequestAsync(RideNewDTO data, string userId);     // promeniti da vraća string rideId
        Task<RideEstimateDTO> GetRideEstimationAsync(string rideId, string userId);
        Task<bool> ConfirmRideRequestAsync(RideEstimateDTO data, string userId);
        Task<bool> AcceptRideAsync(RideAcceptDTO data, string driverId); // za vozače
        Task<bool> CompleteRideAsync(string rideId, string driverId);    // za vozače
        Task<RideInfoDTO> GetRideInfoAsync(string rideId);
        Task<IEnumerable<RideInfoDTO>> GetAllRidesAdminAsync();
        Task<IEnumerable<RideInfoDTO>> GetAllPendingRidesAsync();    // za vozače, i to one koje su potvrđene od korisnika
        Task<IEnumerable<RideInfoDTO>> GetPreviousRidesDriverAsync(string driverId);
        Task<IEnumerable<RideInfoDTO>> GetPreviousRidesCustomerAsync(string customerId);
    }
}
