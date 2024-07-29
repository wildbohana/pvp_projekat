using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("auth/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Hi mom!");
        }

        // TODO remove
        [HttpGet("testRemoting")]
        public async Task<IActionResult> TestRemoting()
        {
            try
            {
                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"));
                User temp = await proxy.GetUserInfo();

                return Ok(temp.Username);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }
    }
}
