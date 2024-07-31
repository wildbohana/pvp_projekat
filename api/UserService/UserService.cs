using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using Common.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.DTOs;
using Common.Enums;
using Azure.Data.Tables;
using Common.TableEntites;
using Common.Models;

/*
TODO:
 - JWT tokens
 - hash passwords
 - upload slike u Blob
 - ostale funkcije za korisnike (block/verify - updateUser)
*/

namespace UserService
{
    internal sealed class UserService : StatefulService, IUserService
    {
        #region Fields
        private TableClient userTable = null!;
        private Thread userTableThread = null!;
        private IReliableDictionary<string, User> userDictionary = null!;   // Init u RunAsync    

        public UserService(StatefulServiceContext context) : base(context) { }
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
            await SetUserTableAsync();
            userDictionary = await StateManager.GetOrAddAsync<IReliableDictionary<string, User>>("UserDictionary");
            await PopulateUserDictionary();

            userTableThread = new Thread(new ThreadStart(UserTableWriteThread));
            userTableThread.Start();

        }

        private async Task SetUserTableAsync()
        {
            var tableServiceClient = new TableServiceClient("UseDevelopmentStorage=true");
            await tableServiceClient.CreateTableIfNotExistsAsync("User");
            userTable = tableServiceClient.GetTableClient("User");
        }

        private async Task PopulateUserDictionary()
        {
            var entities = userTable.QueryAsync<UserEntity>(x => true).GetAsyncEnumerator();

            using (var tx = StateManager.CreateTransaction())
            {
                while (await entities.MoveNextAsync())
                {
                    var user = new User(entities.Current);
                    await userDictionary.TryAddAsync(tx, user.Email, user);
                }

                await tx.CommitAsync();
            }
        }

        private async void UserTableWriteThread()
        {
            while (true)
            {
                using (var tx = StateManager.CreateTransaction())
                {
                    var enumerator = (await userDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        var user = enumerator.Current.Value;
                        var userEntity = new UserEntity(user);
                        await userTable.UpsertEntityAsync(userEntity, TableUpdateMode.Merge, CancellationToken.None);
                    }
                }

                Thread.Sleep(5000);
            }
        }
        #endregion RunAsync

        #region IUserService Implementation
        public async Task<bool> LoginAsync(LoginDTO credentials)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, credentials.Email);

                if (userResult.HasValue && userResult.Value.Password != null)
                {
                    if (userResult.Value.Password.Equals(credentials.Password)) status = true;
                }
            }

            return status;
        }

        public async Task<bool> RegisterAsync(RegisterDTO credentials)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, credentials.Email);

                if (!userResult.HasValue)
                {
                    if (!credentials.Password.Equals(credentials.ConfirmPassword))
                    {
                        status = false;
                    }
                    else
                    {
                        try
                        {
                            await userDictionary.AddAsync(tx, credentials.Email, new User(credentials));
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

        public async Task<UpdateUserDTO?> GetUserDataAsync(string email)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, email);

                if (userResult.HasValue)
                    return new UpdateUserDTO(userResult.Value);
                return null;
            }
        }

        public async Task<bool> UpdateProfileAsync(UpdateUserDTO credentials)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, credentials.Email);

                if (!userResult.HasValue)
                {
                    status = false;
                }
                else
                {
                    var user = userResult.Value;

                    if (!credentials.ConfirmOldPassword.Equals(user.Password))
                    {
                        status = false;
                    }
                    else if (!credentials.NewPassword.Equals(credentials.ConfirmNewPassword))
                    {
                        status = false;
                    }
                    else
                    {
                        try
                        {
                            await userDictionary.TryUpdateAsync(tx, credentials.Email, new User(credentials), user);
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

        public async Task<bool> UserExistsAsync(string email)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, email);

                if (userResult.HasValue) status = true;
            }

            return status;
        }
        #endregion IUserService Implementation
    }
}
