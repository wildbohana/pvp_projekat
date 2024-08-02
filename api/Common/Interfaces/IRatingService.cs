using Common.DTOs;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IRatingService : IService
    {
        Task<bool> RateRideAsync(RatingDTO rate);
        Task<float> GetAverageDriverRateAsync(string rideId);
        Task<bool> HasBeenRatedCheckAsync(string rideId);
    }
}
