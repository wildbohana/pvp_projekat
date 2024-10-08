﻿using Common.DTOs;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly string _validAudience = "";
        private readonly string _validIssuer = "";
        private readonly string _secretKey = "";

        public AuthController(IConfiguration configuration)
        {
            _validAudience = configuration["JwtSettings:ValidAudience"];
            _validIssuer = configuration["JwtSettings:ValidIssuer"];
            _secretKey = configuration["JwtSettings:SecretKey"];
        }
        #endregion Fields

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO data)
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
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO data)
        {
            try
            {
                IUserService proxy = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var temp = await proxy.LoginAsync(data);

                if (temp)
                {
                    var userType = await proxy.GetUserTypeFromEmail(data.Email);
                    if (String.IsNullOrEmpty(userType)) return BadRequest("Something's not adding up.");

                    var token = GenerateAccessToken(data.Email, userType);

                    return Ok(new 
                    { 
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        Usertype = userType
                    });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        #region Generate token
        private JwtSecurityToken GenerateAccessToken(string userId, string userRole)
        {
            // Create user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.Role, userRole)
            };

            // Create a JWT
            var token = new JwtSecurityToken(
                issuer: _validIssuer,
                audience: _validAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60), // Token expiration time
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_secretKey)), 
                    SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
        #endregion Generate token
    }
}
