using Common.DTOs;
using Common.Enums;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Security.Claims;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("rating")]
    public class RatingController : ControllerBase
    {
        [HttpPost("rate-ride")]
        public async Task<IActionResult> RateRideAsync([FromBody] RatingDTO data)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || role.Equals(EUserType.Driver.ToString()))
                {
                    return BadRequest("Driver can't rate a ride!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IRatingService proxy = ServiceProxy.Create<IRatingService>(new Uri("fabric:/api/RatingService"), new ServicePartitionKey(1));
                var temp = await proxy.RateRideAsync(data, emailFromToken);

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
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;

                if (role == null || !role.Equals(EUserType.Administrator.ToString()))
                {
                    return Unauthorized("You are not administrator!");
                }

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
