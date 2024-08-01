using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

// TODO JWT tokene

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("admin/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpPost("verify-approve")]
        public async Task<IActionResult> VerifyApproveAsync(string driverId)
        {
            try
            {
                // check jwt token (provera da li je admin poslao zahtev)

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
                // check jwt token (provera da li je admin poslao zahtev)

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
                // check jwt token (provera da li je admin poslao zahtev)

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
                // check jwt token (dobavi userId iz njega)

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
