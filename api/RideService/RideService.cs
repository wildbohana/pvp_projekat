using Azure.Data.Tables;
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

        #region IRideService Implementation
        public async Task<bool> AcceptRideAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CompleteRideAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateNewRideAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetAllRidesAdminAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetPreviousRidesCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetPreviousRidesDriverAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetRideInfoAsync()
        {
            throw new NotImplementedException();
        }
        #endregion IRideService Implementation
    }
}
