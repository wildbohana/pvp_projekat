using Azure.Data.Tables;
using Common.DTOs;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using Common.TableEntites;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace RideService
{
    internal sealed class RideService : StatefulService, IRideService
    {
        #region Fields
        private TableClient rideTable = null!;
        private Thread rideTableThread = null!;
        private IReliableDictionary<string, Ride> rideDictionary = null!;   // Init u RunAsync

        public RideService(StatefulServiceContext context) : base(context) { }
        #endregion Fields

        #region Create listeners
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }
        #endregion Create listeners

        #region RunAsync
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await SetRideTableAsync();
            rideDictionary = await StateManager.GetOrAddAsync<IReliableDictionary<string, Ride>>("RideDictionary");
            await PopulateRideDictionary();

            rideTableThread = new Thread(new ThreadStart(RideTableWriteThread));
            rideTableThread.Start();
        }

        private async Task SetRideTableAsync()
        {
            var tableServiceClient = new TableServiceClient("UseDevelopmentStorage=true");
            await tableServiceClient.CreateTableIfNotExistsAsync("Ride");
            rideTable = tableServiceClient.GetTableClient("Ride");
        }

        private async Task PopulateRideDictionary()
        {
            var entities = rideTable.QueryAsync<RideEntity>(x => true).GetAsyncEnumerator();

            using (var tx = StateManager.CreateTransaction())
            {
                while (await entities.MoveNextAsync())
                {
                    var ride = new Ride(entities.Current);
                    await rideDictionary.TryAddAsync(tx, ride.Id, ride);
                }

                await tx.CommitAsync();
            }
        }

        private async void RideTableWriteThread()
        {
            while (true)
            {
                using (var tx = StateManager.CreateTransaction())
                {
                    var enumerator = (await rideDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        var ride = enumerator.Current.Value;
                        var rideEntity = new RideEntity(ride);
                        await rideTable.UpsertEntityAsync(rideEntity, TableUpdateMode.Merge, CancellationToken.None);
                    }
                }

                Thread.Sleep(5000);
            }
        }
        #endregion RunAsync

        #region Customer methods
        public async Task<RideEstimateDTO?> CreateRideRequestAsync(RideNewDTO data, string customerId)
        {
            // Ako je korisnik već poslao zahtev, ne može da zahteva novu vožnju
            RideEstimateDTO pendingRideCheck = await GetRideEstimationForUserAsync(customerId);
            if (pendingRideCheck != null)
            {
                return null;
            }

            using (var tx = StateManager.CreateTransaction())
            {
                Random rand = new Random();
                Ride newRide = new Ride(data.StartAddress, data.FinalAddress, customerId);

                try
                {
                    await rideDictionary.AddAsync(tx, newRide.Id, newRide);
                    await tx.CommitAsync();
                    return new RideEstimateDTO(newRide);
                }
                catch (Exception)
                {
                    tx.Abort();
                }                
            }

            return null;
        }

        public async Task<RideEstimateDTO?> GetRideEstimationAsync(string rideId, string customerId)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var rideResult = await rideDictionary.TryGetValueAsync(tx, rideId);

                if (rideResult.HasValue || rideResult.Value.CustomerId.Equals(customerId))
                {
                    RideEstimateDTO rideInfo = new RideEstimateDTO(rideResult.Value);
                    return rideInfo;
                }

                return null;
            }
        }

        public async Task<RideEstimateDTO?> GetRideEstimationForUserAsync(string customerId)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var enumerator = (await rideDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var tmp = enumerator.Current.Value;
                    if (tmp.Status != ERideStatus.Completed && tmp.CustomerId.Equals(customerId))
                    {
                        var ride = new RideEstimateDTO(tmp);
                        return ride;
                    }
                }
            }

            return null;
        }

        public async Task<bool> ConfirmRideRequestAsync(string rideId, string customerId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var rideResult = await rideDictionary.TryGetValueAsync(tx, rideId);

                if (rideResult.HasValue)
                {
                    var ride = rideResult.Value;

                    if (rideResult.Value.Status == ERideStatus.Pending && ride.CustomerId.Equals(customerId))
                    {
                        var acceptedRide = ride;
                        acceptedRide.Status = ERideStatus.ConfirmedByCustomer;

                        try
                        {
                            await rideDictionary.TryUpdateAsync(tx, rideId, acceptedRide, ride);
                            await tx.CommitAsync();
                            status = true;
                        }
                        catch (Exception)
                        {
                            status = false;
                            tx.Abort();
                        }
                    }
                }
            }
            
            return status;
        }

        public async Task<bool> DeleteRideRequestAsync(string rideId, string customerId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var rideResult = await rideDictionary.TryGetValueAsync(tx, rideId);

                if (rideResult.HasValue)
                {
                    try
                    {
                        await rideDictionary.TryRemoveAsync(tx, rideId);
                        await tx.CommitAsync();
                        status = true;
                    }
                    catch (Exception)
                    {
                        status = false;
                        tx.Abort();
                    }
                }
            }

            return status;
        }

        // Na frontu prikazati listu gotovih vožnji, i pored njih dugme "OCENI"
        // Ako je vožnja već ocenjena, prikazati ocenu (ili broj zvezdica, svejedno)
        public async Task<IEnumerable<RideInfoDTO>> GetPreviousRidesCustomerAsync(string customerId)
        {
            var rides = new List<RideInfoDTO>();

            using (var tx = StateManager.CreateTransaction())
            {
                var enumerator = (await rideDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var tmp = enumerator.Current.Value;
                    if (tmp.Status == ERideStatus.Completed && tmp.CustomerId.Equals(customerId))
                    {
                        rides.Add(new RideInfoDTO(tmp));
                    }
                }
            }

            return rides;
        }
        #endregion Customer methods

        #region  Driver methods
        public async Task<bool> AcceptRideAsync(string rideId, string driverId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var rideResult = await rideDictionary.TryGetValueAsync(tx, rideId);

                if (rideResult.HasValue)
                {
                    var ride = rideResult.Value;

                    if (rideResult.Value.Status == ERideStatus.ConfirmedByCustomer && ride.DriverId.Equals(driverId))
                    {
                        var acceptedRide = ride;
                        acceptedRide.Status = ERideStatus.InProgress;
                        acceptedRide.StartTime = DateTime.UtcNow;

                        try
                        {
                            await rideDictionary.TryUpdateAsync(tx, rideId, acceptedRide, ride);
                            await tx.CommitAsync();
                            status = true;
                        }
                        catch (Exception)
                        {
                            status = false;
                            tx.Abort();
                        }
                    }
                }
            }

            return status;
        }

        // Manuelno ili automatski da se pozove ova metoda?
        public async Task<bool> CompleteRideAsync(string rideId, string driverId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var rideResult = await rideDictionary.TryGetValueAsync(tx, rideId);

                if (rideResult.HasValue)
                {
                    var ride = rideResult.Value;

                    if (rideResult.Value.Status == ERideStatus.InProgress && ride.DriverId.Equals(driverId))
                    {
                        var completedRide = ride;
                        completedRide.Status = ERideStatus.Completed;

                        try
                        {
                            await rideDictionary.TryUpdateAsync(tx,  rideId, completedRide, ride);
                            await tx.CommitAsync();
                            status = true;
                        }
                        catch (Exception)
                        {
                            status = false;
                            tx.Abort();
                        }
                    }
                }
            }

            return status;
        }

        public async Task<IEnumerable<RideInfoDTO>> GetAllPendingRidesAsync()
        {
            var rides = new List<RideInfoDTO>();

            using (var tx = StateManager.CreateTransaction())
            {
                var enumerator = (await rideDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var tmp = enumerator.Current.Value;
                    if (tmp.Status == ERideStatus.ConfirmedByCustomer)
                    {
                        rides.Add(new RideInfoDTO(tmp));
                    }
                }
            }

            return rides;
        }

        public async Task<IEnumerable<RideInfoDTO>> GetPreviousRidesDriverAsync(string driverId)
        {
            var rides = new List<RideInfoDTO>();

            using (var tx = StateManager.CreateTransaction())
            {
                var enumerator = (await rideDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var tmp = enumerator.Current.Value;
                    if (tmp.Status == ERideStatus.Completed && tmp.DriverId.Equals(driverId))
                    {
                        rides.Add(new RideInfoDTO(tmp));
                    }
                }
            }

            return rides;
        }
        #endregion Driver methods

        #region Admin methods
        public async Task<RideInfoDTO?> GetRideInfoAsync(string rideId)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var rideResult = await rideDictionary.TryGetValueAsync(tx, rideId);

                if (rideResult.HasValue)
                {
                    Ride ride = rideResult.Value;
                    RideInfoDTO rideInfo = new RideInfoDTO(ride);

                    return rideInfo;
                }

                return null;
            }
        }

        public async Task<IEnumerable<RideInfoDTO>> GetAllRidesAdminAsync()
        {
            var rides = new List<RideInfoDTO>();

            using (var tx = StateManager.CreateTransaction())
            {
                var enumerator = (await rideDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var tmp = enumerator.Current.Value;
                    var ride = new RideInfoDTO(tmp);
                    rides.Add(ride);
                }
            }

            return rides;
        }
        #endregion Admin methods
    }
}
