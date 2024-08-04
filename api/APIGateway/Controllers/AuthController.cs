using Common.DTOs;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

// TODO JWT tokene
// TODO OAuth registracija/login (samo na frontu)

// Login vraća jwt token i userType (ili i njega kao i userId ubaciti u token?)
// UserType ti treba na frontu da bi znala da li je admin/driver/customer

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        #region Config
        private IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }
        #endregion Config

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
