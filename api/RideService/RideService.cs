using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace RideService
{
    internal sealed class RideService : StatefulService, IRideService
    {
        public RideService(StatefulServiceContext context) : base(context) { }

        #region IRideService Implementation
        public void AcceptRide()
        {
            throw new NotImplementedException();
        }

        public void CompleteRide()
        {
            throw new NotImplementedException();
        }

        public void CreateNewRide()
        {
            throw new NotImplementedException();
        }

        public void GetAllRidesAdmin()
        {
            throw new NotImplementedException();
        }

        public void GetPreviousRidesCustomer()
        {
            throw new NotImplementedException();
        }

        public void GetPreviousRidesDriver()
        {
            throw new NotImplementedException();
        }

        public void GetRideInfo()
        {
            throw new NotImplementedException();
        }

        public void RateRide()
        {
            throw new NotImplementedException();
        }
        #endregion IRideService Implementation

        #region Default Methods
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[0];
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
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
        #endregion Default Methods
    }
}
