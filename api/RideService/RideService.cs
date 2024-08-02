using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace RideService
{
    internal sealed class RideService : StatefulService, IRideService
    {
        public RideService(StatefulServiceContext context) : base(context) { }

        #region Create listeners
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }
        #endregion Create listeners

        #region RunAsync
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO dodaj svoje rečnike
            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
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
