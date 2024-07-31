using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using Common.Interfaces;
using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.DTOs;
using Common.Enums;

// TODO jwt tokens

namespace UserService
{
    internal sealed class UserService : StatefulService, IUserService
    {
        #region Fields
        //private TableClient userTable = null!;
        //private Thread userDatabaseThread = null!;
        private IReliableDictionary<string, User> userDictionary = null!;   // Init u RunAsync
        #endregion Fields

        public UserService(StatefulServiceContext context) : base(context) { }

        #region Create listeners
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }
        #endregion Create listeners

        #region RunAsync
        // TODO promeni da se koristi baza
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            userDictionary = await StateManager.GetOrAddAsync<IReliableDictionary<string, User>>("UserDictionary");
            //await PopulateUserDictionary();

            //userDatabaseThread = new Thread(new ThreadStart(UserDatabaseWriteThread));
            //userDatabaseThread.Start();
        }


        //private async Task PopulateUserDictionary()
        //{
        //    var entities = userTable.QueryAsync<UsersTable>(x => true).GetAsyncEnumerator();

        //    using (var tx = StateManager.CreateTransaction())
        //    {
        //        while (await entities.MoveNextAsync())
        //        {
        //            var user = new User(entities.Current);
        //            await userDictionary.TryAddAsync(tx, user.Email, user);
        //        }

        //        await tx.CommitAsync();
        //    }
        //}


        //private async void UserDatabaseWriteThread()
        //{
        //    while (true)
        //    {
        //        using (var tx = StateManager.CreateTransaction())
        //        {
        //            var enumerator = (await userDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

        //            while (await enumerator.MoveNextAsync(CancellationToken.None))
        //            {
        //                var user = enumerator.Current.Value;
        //                await userTable.UpsertEntityAsync(new UsersTable(user), TableUpdateMode.Merge, CancellationToken.None);
        //            }
        //        }

        //        Thread.Sleep(5000);
        //    }
        //}
        #endregion RunAsync

        #region IUserService Implementation

        // TODO remove
        public async Task<bool> Test()
        {
            bool retval = true;
            return retval;
        }

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
            // TODO hash password
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, credentials.Email);

                if (!userResult.HasValue)
                {
                    if (!credentials.Password.Equals(credentials.ConfirmPassword)) status = false;
                    else
                    {
                        try
                        {
                            // TODO change
                            var newUser = new User()
                            { 
                                Password = credentials.ConfirmPassword,
                                Username = credentials.Username,
                                Email = credentials.Email,
                                FirstName = credentials.Firstname,
                                Lastname = credentials.Lastname,
                                UserType = credentials.Role == EUserType.Customer.ToString() ? EUserType.Customer : EUserType.Driver,
                                Address = credentials.Address,
                                DateOfBirth = credentials.DateOfBirth,
                                VerificationStatus = EVerificationStatus.Approved,
                                IsBlocked = false,
                                Busy = false,
                                PhotoUrl = ""
                            };
                            await userDictionary.AddAsync(tx, newUser.Email, newUser);
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

                // TODO change return type (UpdateUserDTO)
                //if (userResult.HasValue) return new UpdateUserDTO(userResult.Value);
                if (userResult.HasValue) return null;
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
                            // TODO change construtor for User
                            await userDictionary.TryUpdateAsync(tx, credentials.Email, new User(), user);
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
