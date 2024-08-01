using Common.DTOs;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("users/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            try
            {
                // check jwt token (dobavi userId iz njega)
                string userId = "mejl_adresa_iz_tokena";

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
                // check jwt token

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
                // check jwt token

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
    }
}
