using Common.DTOs;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

// TODO JWT tokene

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("rating/[controller]")]
    public class RatingController : ControllerBase
    {
        [HttpPost("rate-ride")]
        public async Task<IActionResult> RateRideAsync(RatingDTO data)
        {
            try
            {
                // check jwt token (provera da li je admin poslao zahtev)

                IRatingService proxy = ServiceProxy.Create<IRatingService>(new Uri("fabric:/api/RatingService"), new ServicePartitionKey(1));
                var temp = await proxy.RateRideAsync(data);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        // Samo admin
        [HttpGet("get-rating")]
        public async Task<IActionResult> GetDriverRatingAsync(string driverId)
        {
            try
            {
                // check jwt token (dobavi userId iz njega)
                // proveri da li je admin u pitanju

                IRatingService proxy = ServiceProxy.Create<IRatingService>(new Uri("fabric:/api/RatingService"), new ServicePartitionKey(1));
                var temp = await proxy.GetAverageDriverRateAsync(driverId);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpGet("rated-check")]
        public async Task<IActionResult> RatedCheckAsync(string rideId)
        {
            try
            {
                // check jwt token (dobavi userId iz njega)

                IRatingService proxy = ServiceProxy.Create<IRatingService>(new Uri("fabric:/api/RatingService"), new ServicePartitionKey(1));
                var temp = await proxy.HasBeenRatedCheckAsync(rideId);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }
    }
}
