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

namespace UserService
{
    internal sealed class UserService : StatefulService, IUserService
    {
        public UserService(StatefulServiceContext context) : base(context) { }

        #region IUserService Implementation
        public void CreateNewUser()
        {
            throw new NotImplementedException();
        }

        public void GetUserInfo()
        {
            throw new NotImplementedException();
        }

        public void UpdateUser()
        {
            throw new NotImplementedException();
        }
        #endregion IUserService Implementation

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
