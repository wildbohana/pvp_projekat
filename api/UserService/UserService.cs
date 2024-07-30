using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using Common.Interfaces;
using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace UserService
{
    internal sealed class UserService : StatefulService, IUserService
    {
        public UserService(StatefulServiceContext context) : base(context) { }

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

        #region IUserService Implementation

        public bool CreateNewUser()
        {
            return false;
        }

        public async Task<User> GetUserInfo()
        {
            return new User() { Username = "testing" };
        }

        public bool UpdateUser()
        {
            throw new NotImplementedException();
        }

        // TODO remove
        public async Task<bool> Test()
        {
            bool retval = true;
            return retval;
        }
        #endregion IUserService Implementation

    }
}
