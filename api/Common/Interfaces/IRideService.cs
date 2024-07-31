using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IRideService : IService
    {
        Task<bool> CreateNewRide();
        Task<bool> AcceptRide();
        Task<bool> RateRide();
        Task<bool> CompleteRide();
        Task<bool> GetRideInfo();
        Task<bool> GetAllRidesAdmin();
        Task<bool> GetPreviousRidesDriver();
        Task<bool> GetPreviousRidesCustomer();

    }
}
