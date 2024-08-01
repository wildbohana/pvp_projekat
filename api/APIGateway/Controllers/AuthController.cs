using Common.DTOs;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("auth/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO data)
        {
            try
            {
                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.RegisterAsync(data);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO data)
        {
            try
            {
                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.LoginAsync(data);

                // TODO
                // if (temp) var token = GenerateJwtToken();
                // return Ok(token);

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
