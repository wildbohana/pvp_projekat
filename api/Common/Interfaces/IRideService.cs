namespace Common.Interfaces
{
    public interface IRideService
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
