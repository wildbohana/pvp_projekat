using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IRideService : IService
    {
        Task<bool> CreateNewRideAsync();
        Task<bool> AcceptRideAsync();
        Task<bool> CompleteRideAsync();
        Task<bool> GetRideInfoAsync();
        Task<bool> GetAllRidesAdminAsync();
        Task<bool> GetPreviousRidesDriverAsync();
        Task<bool> GetPreviousRidesCustomerAsync();
    }
}
