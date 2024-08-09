using Common.Enums;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Security.Claims;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        [HttpPost("verify-approve")]
        public async Task<IActionResult> VerifyApproveAsync(string driverId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;

                if (role == null || !role.Equals(EUserType.Administrator.ToString()))
                {
                    return Unauthorized("You are not administrator!");
                }

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.ApproveDriverAsync(driverId);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpPost("verify-deny")]
        public async Task<IActionResult> VerifyDenyAsync(string driverId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;

                if (role == null || !role.Equals(EUserType.Administrator.ToString()))
                {
                    return Unauthorized("You are not administrator!");
                }

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.DenyDriverAsync(driverId);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockAsync(string driverId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;

                if (role == null || !role.Equals(EUserType.Administrator.ToString()))
                {
                    return Unauthorized("You are not administrator!");
                }

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.BlockDriverAsync(driverId);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpGet("all-drivers")]
        public async Task<IActionResult> GetAllDriversAsync()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;

                if (role == null || !role.Equals(EUserType.Administrator.ToString()))
                {
                    return Unauthorized("You are not administrator!");
                }

                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.GetAllDriversAsync();

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
