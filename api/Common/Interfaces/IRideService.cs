using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IRideService : IService
    {
        void CreateNewRide();
        void AcceptRide();
        void RateRide();
        void CompleteRide();
        void GetRideInfo();
        void GetAllRidesAdmin();
        void GetPreviousRidesDriver();
        void GetPreviousRidesCustomer();

    }
}
