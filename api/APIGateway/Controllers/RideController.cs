using Common.DTOs;
using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

// Provera koji je tip korisnika kad se šalju zahtevi (npr. vozač ne može da poruči vožnju)

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
                // TODO token (dobavi userId, provera da li je Customer)
                string customerId = "izvuci-iz-tokena";

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var isBusy = await proxyUser.GetBusyStatusAsync(customerId);

                if (isBusy) return Unauthorized();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.CreateRideRequestAsync(data, customerId);

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
                // TODO token (dobavi userId, provera da li je Customer)
                string customerId = "izvuci-iz-tokena";

                //IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                //var isBusy = await proxyUser.GetBusyStatusAsync(customerId);

                //if (isBusy) return Unauthorized();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.GetRideEstimationAsync(rideId, customerId);

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
                // TODO token (dobavi userId, provera da li je Customer)
                string userId = "izvuci-iz-tokena";


                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var isBusy = await proxyUser.GetBusyStatusAsync(userId);

                if (isBusy) return Unauthorized();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.ConfirmRideRequestAsync(data, userId);

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
                // check jwt token (provera da li je korisnik poslao zahtev)
                string userId = "izvuci-iz-tokena";

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var isBusy = await proxyUser.GetBusyStatusAsync(userId);

                if (isBusy) return Unauthorized();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.DeleteRideRequestAsync(data, userId);

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
                // check jwt token (dobavi userId iz njega)
                string customerId = "izvuci-iz-tokena";

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.GetPreviousRidesCustomerAsync(customerId);

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
                // check jwt token (provera da li je korisnik poslao zahtev)
                string driverId = "izvuci-iz-tokena";

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var verified = await proxyUser.IsDriverVerifiedCheckAsync(driverId);
                var blocked = await proxyUser.IsDriverBlockedCheckAsync(driverId);
                var isBusy = await proxyUser.GetBusyStatusAsync(driverId);

                if (!verified || blocked || isBusy)
                {
                    return Unauthorized();
                }

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.AcceptRideAsync(rideId, driverId);
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
                // check jwt token (provera da li je korisnik poslao zahtev)
                string driverId = "izvuci-iz-tokena";

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var verified = await proxyUser.IsDriverVerifiedCheckAsync(driverId);
                var blocked = await proxyUser.IsDriverBlockedCheckAsync(driverId);

                if (!verified || blocked)
                {
                    return Unauthorized();
                }

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.CompleteRideAsync(rideId, driverId);
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
                // check jwt token (dobavi userId iz njega)
                string driverId = "izvuci-iz-tokena";

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var isBusy = await proxyUser.GetBusyStatusAsync(driverId);

                if (isBusy) return Unauthorized();

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
                // check jwt token (dobavi userId iz njega)
                string driverId = "izvuci-iz-tokena";

                IUserService proxyUser = ServiceProxy.Create<IUserService>(new Uri("fabric:/api/UserService"), new ServicePartitionKey(1));
                var verified = await proxyUser.IsDriverVerifiedCheckAsync(driverId);

                if (!verified) return Unauthorized();

                IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                var temp = await proxy.GetPreviousRidesDriverAsync(driverId);

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
                // check jwt token (dobavi userId iz njega)

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
