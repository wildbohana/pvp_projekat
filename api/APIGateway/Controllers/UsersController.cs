using Common.DTOs;
using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Security.Claims;

// TODO JWT tokene

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfileAsync(string testId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                
                string userId = testId;     // samo za testiranje

                // TODO token (dobavi userId)

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.GetUserDataAsync(userId);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(UserDTO data)
        {
            try
            {
                // TODO token (dobavi userId)

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.UpdateProfileAsync(data);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        // I za vozače i za korisnike kad se voze busy je true
        [HttpGet("busy")]
        public async Task<IActionResult> GetBusyStatus(string userId)
        {
            try
            {
                // TODO token (dobavi userId)

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.GetBusyStatusAsync(userId);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpGet("verified-check")]
        public async Task<IActionResult> GetDriverVerifiedCheck(string driverId)
        {
            try
            {
                // TODO token (dobavi userId)

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.IsDriverVerifiedCheckAsync(driverId);

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
