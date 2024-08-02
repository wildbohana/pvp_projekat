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
        // TODO sve -_-
        public async Task<RideEstimateDTO> CreateRideRequestAsync(RideNewDTO data, string customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<RideEstimateDTO> GetRideEstimationAsync(string rideId, string customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ConfirmRideRequestAsync(RideEstimateDTO data, string customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteRideRequest(RideEstimateDTO data, string customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RideInfoDTO>> GetPreviousRidesCustomerAsync(string customerId)
        {
            throw new NotImplementedException();
        }
        #endregion Customer methods

        #region  Driver methods
        public async Task<bool> AcceptRideAsync(RideAcceptDTO data, string driverId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var rideResult = await rideDictionary.TryGetValueAsync(tx, data.Id);

                if (rideResult.HasValue)
                {
                    var ride = rideResult.Value;

                    if (rideResult.Value.Status == ERideStatus.ConfirmedByCustomer && ride.DriverId.Equals(driverId))
                    {
                        var acceptedRide = ride;
                        acceptedRide.Status = ERideStatus.ConfirmedByDriver;

                        try
                        {
                            await rideDictionary.TryUpdateAsync(tx, data.Id, acceptedRide, ride);
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

        public async Task<bool> CompleteRideAsync(string rideId, string driverId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var rideResult = await rideDictionary.TryGetValueAsync(tx, rideId);

                if (rideResult.HasValue)
                {
                    var ride = rideResult.Value;

                    if (rideResult.Value.Status == ERideStatus.ConfirmedByDriver && ride.DriverId.Equals(driverId))
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
