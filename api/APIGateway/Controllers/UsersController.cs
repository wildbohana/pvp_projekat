using Common.DTOs;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Security.Claims;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                
                if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.GetUserDataAsync(emailFromToken);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UserDTO data)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (emailFromToken == null || !emailFromToken.Equals(data.Email))
                {
                    return Unauthorized();
                }

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

        [HttpGet("busy")]
        public async Task<IActionResult> GetBusyStatus()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                // I za vozače i za korisnike kad se voze busy je true
                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.GetBusyStatusAsync(emailFromToken);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpGet("verified-check")]
        public async Task<IActionResult> GetDriverVerifiedCheck([FromBody] string driverId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.IsDriverVerifiedCheckAsync(emailFromToken);

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
