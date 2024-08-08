using Common.Helpers;
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
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        [HttpPost("verify-approve")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> VerifyApproveAsync(string driverId)
        {
            try
            {
                // TODO token

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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> VerifyDenyAsync(string driverId)
        {
            try
            {
                // TODO token (da li je admin)

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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> BlockAsync(string driverId)
        {
            try
            {
                // TODO token (da li je admin)

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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllDriversAsync()
        {
            try
            {
                // TODO token (da li je admin)

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
