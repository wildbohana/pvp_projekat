using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("auth/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService us)
        {
            _userService = us;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Hi mom!");
        }
    }
}
