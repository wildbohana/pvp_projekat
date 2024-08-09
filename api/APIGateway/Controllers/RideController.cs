﻿using Common.DTOs;
using Common.Enums;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Security.Claims;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("ride")]
    public class RideController : ControllerBase
    {
        // svi
        [HttpGet]
        public async Task<IActionResult> GetRideInfo(string rideId)
        {
            try
            {
                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.GetRideInfoAsync(rideId);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        // korisnici
        [HttpPost("new-ride")]
        public async Task<IActionResult> NewRideRequest(RideNewDTO data)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || role.Equals(EUserType.Driver.ToString()))
                {
                    return BadRequest("Driver can't request a ride!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var isBusy = await proxyUser.GetBusyStatusAsync(emailFromToken);

                if (isBusy) return BadRequest();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.CreateRideRequestAsync(data, emailFromToken);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpGet("ride-estimate")]
        public async Task<IActionResult> GetRideEstimation(string rideId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || role.Equals(EUserType.Driver.ToString()))
                {
                    return BadRequest("Driver can't request a ride!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var isBusy = await proxyUser.GetBusyStatusAsync(emailFromToken);

                if (isBusy) return BadRequest();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.GetRideEstimationAsync(rideId, emailFromToken);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpPost("confirm-request")]
        public async Task<IActionResult> ConfirmRideRequest(string data)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || role.Equals(EUserType.Driver.ToString()))
                {
                    return BadRequest("Driver can't request a ride!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var isBusy = await proxyUser.GetBusyStatusAsync(emailFromToken);

                if (isBusy) return Unauthorized();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.ConfirmRideRequestAsync(data, emailFromToken);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpPost("delete-request")]
        public async Task<IActionResult> DeleteRideRequest(string data)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || role.Equals(EUserType.Driver.ToString()))
                {
                    return BadRequest("Driver can't request a ride!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var isBusy = await proxyUser.GetBusyStatusAsync(emailFromToken);

                if (isBusy) return BadRequest();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.DeleteRideRequestAsync(data, emailFromToken);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpGet("previous-rides")]
        public async Task<IActionResult> GetAllPreviousRides()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || !role.Equals(EUserType.Customer.ToString()))
                {
                    return BadRequest("You are not a customer!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.GetPreviousRidesCustomerAsync(emailFromToken);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        // vozač
        [HttpPost("accept-ride")]
        public async Task<IActionResult> AcceptRide(string rideId)
        {
            try
            {

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || !role.Equals(EUserType.Driver.ToString()))
                {
                    return BadRequest("You are not a driver!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var verified = await proxyUser.IsDriverVerifiedCheckAsync(emailFromToken);
                var blocked = await proxyUser.IsDriverBlockedCheckAsync(emailFromToken);
                var isBusy = await proxyUser.GetBusyStatusAsync(emailFromToken);

                if (!verified || blocked || isBusy)
                {
                    return Unauthorized();
                }

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.AcceptRideAsync(rideId, emailFromToken);
                var rideInfo = await proxy.GetRideInfoAsync(rideId);

                if (temp)
                {
                    await proxyUser.ChangeBusyStatusAsync(rideInfo.DriverId, true);
                    await proxyUser.ChangeBusyStatusAsync(rideInfo.CustomerId, true);
                }
                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpPost("complete-ride")]
        public async Task<IActionResult> CompleteRide(string rideId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || !role.Equals(EUserType.Driver.ToString()))
                {
                    return BadRequest("You are not a  driver!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var verified = await proxyUser.IsDriverVerifiedCheckAsync(emailFromToken);
                var blocked = await proxyUser.IsDriverBlockedCheckAsync(emailFromToken);

                if (!verified || blocked)
                {
                    return Unauthorized();
                }

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.CompleteRideAsync(rideId, emailFromToken);
                var rideInfo = await proxy.GetRideInfoAsync(rideId);

                if (temp)
                {
                    await proxyUser.ChangeBusyStatusAsync(rideInfo.DriverId, false);
                    await proxyUser.ChangeBusyStatusAsync(rideInfo.CustomerId, false);
                }

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpGet("pending-requests")]
        public async Task<IActionResult> GetAllPendingRequests()
        {
            try
            {

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || !role.Equals(EUserType.Driver.ToString()))
                {
                    return BadRequest("You are not a driver!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var isBusy = await proxyUser.GetBusyStatusAsync(emailFromToken);

                if (isBusy) return BadRequest();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.GetAllPendingRidesAsync();

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpGet("completed-rides")]
        public async Task<IActionResult> GetAllCompletedRides()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var emailFromToken = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (role == null || !role.Equals(EUserType.Driver.ToString()))
                {
                    return BadRequest("You are not a driver!");
                }
                else if (emailFromToken == null)
                {
                    return Unauthorized();
                }

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var verified = await proxyUser.IsDriverVerifiedCheckAsync(emailFromToken);

                if (!verified) return Unauthorized();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.GetPreviousRidesDriverAsync(emailFromToken);

                return Ok(temp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        // admin

        [HttpGet("all-rides")]
        public async Task<IActionResult> GetAllRides()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;

                if (role == null || !role.Equals(EUserType.Administrator.ToString()))
                {
                    return Unauthorized("You are not admin!");
                }

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.GetAllRidesAdminAsync();

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
