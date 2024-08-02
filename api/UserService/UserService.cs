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
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Net;
using Common.Helpers;

// TODO Upload slike u Blob
// TODO OAuth registracija/login (samo na frontu)
// TODO slanje email-a kada admin odobri/odbije vozača

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

        #region Auth actions
        public async Task<bool> LoginAsync(LoginDTO credentials)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, credentials.Email);

                if (userResult.HasValue && userResult.Value.Password != null)
                {
                    string hashedPassword = HashHelper.HashPassword(credentials.Password);
                    if (userResult.Value.Password.Equals(hashedPassword))
                    {
                        status = true;
                    }
                }
            }

            return status;
        }

        public async Task<bool> RegisterAsync(RegisterDTO credentials)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                // Provera da li je neko već iskoristio tu email adresu
                var userResult = await userDictionary.TryGetValueAsync(tx, credentials.Email);

                if (!userResult.HasValue)
                {
                    if (!credentials.Password.Equals(credentials.ConfirmPassword))
                    {
                        status = false;
                    }
                    else
                    {
                        User newUser = new User(credentials);

                        // Heširanje lozinke
                        newUser.Password = HashHelper.HashPassword(credentials.ConfirmPassword);

                        // TODO
                        // Upload slike
                        //if (credentials.Photo != "")
                        //{
                        //    Image image;
                        //    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(credentials.Photo.Split(',')[1])))
                        //    {
                        //        image = Image.FromStream(ms);
                        //    }
                        //    newUser.PhotoUrl = new BlobHelper().UploadImage(image, "slike", Guid.NewGuid().ToString() + ".jpg");
                        //}
                        //else
                        //{
                        //    newUser.PhotoUrl = "";
                        //}

                        try
                        {
                            await userDictionary.AddAsync(tx, credentials.Email, newUser);
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
        #endregion Auth actions

        #region User actions
        public async Task<UserDTO?> GetUserDataAsync(string email)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, email);
                
                if (userResult.HasValue)
                {
                    User user = userResult.Value;
                    UserDTO userInfo = new()
                    {
                        Email = user.Email,
                        Username = user.Username,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth,
                        ConfirmNewPassword = "",
                        ConfirmOldPassword = "",
                        NewPassword = "",
                        Role = user.UserType.ToString(),
                        PhotoUrl = user.PhotoUrl
                    };

                    return userInfo;
                }
                return null;
            }
        }

        public async Task<bool> GetBusyStatusAsync(string email)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, email);

                if (userResult.HasValue)
                    return userResult.Value.Busy;
                return false;
            }
        }

        public async Task<bool> UpdateProfileAsync(UserDTO credentials)
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

                    // TODO ostavi ili obriši? ovako korisnik uvek mora da menja lozinku?
                    string hashedOldPassword = HashHelper.HashPassword(credentials.ConfirmOldPassword);
                    if (!hashedOldPassword.Equals(user.Password))
                    {
                        status = false;
                    }
                    else if (!credentials.NewPassword.Equals(credentials.ConfirmNewPassword))
                    {
                        status = false;
                    }
                    else
                    {
                        User newUser = user;

                        newUser.Firstname = credentials.Firstname ?? newUser.Firstname;
                        newUser.Lastname = credentials.Lastname ?? newUser.Lastname;
                        newUser.Address = credentials.Address ?? newUser.Address;
                        newUser.Username = credentials.Username ?? newUser.Username;
                        newUser.DateOfBirth = credentials.DateOfBirth ?? newUser.DateOfBirth;

                        // Promena lozinke
                        if (!string.IsNullOrEmpty(credentials.ConfirmNewPassword))
                        {
                            newUser.Password = HashHelper.HashPassword(credentials.ConfirmNewPassword);
                        }

                        // TODO
                        // Promena slike
                        //if (!credentials.Photo.StartsWith("http"))
                        //{
                        //    Image image;
                        //    string slikaB64 = credentials.Photo;
                        //    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(slikaB64.Split(',')[1])))
                        //    {
                        //        image = Image.FromStream(ms);
                        //    }
                        //    newUser.PhotoUrl = new BlobHelper().UploadImage(image, "slike", Guid.NewGuid().ToString() + ".jpg");
                        //}

                        // Upis promene u rečnik
                        try
                        {
                            await userDictionary.TryUpdateAsync(tx, credentials.Email, newUser, user);
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
        #endregion User actions

        #region Admin actions
        public async Task<IEnumerable<DriverDTO>> GetAllDriversAsync()
        {
            var drivers = new List<DriverDTO>();

            using (var tx = StateManager.CreateTransaction())
            {
                var enumerator = (await userDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var tmp = enumerator.Current.Value;
                    if (tmp.UserType == EUserType.Driver)
                    {
                        drivers.Add(new DriverDTO(tmp));
                    }
                }
            }

            return drivers;
        }

        public async Task<bool> BlockDriverAsync(string driverId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, driverId);

                if (!userResult.HasValue)
                {
                    status = false;
                }
                else if (userResult.Value.UserType != EUserType.Driver)
                {
                    status = false;
                }
                else
                {
                    var oldUser = userResult.Value;
                    var updatedUser = userResult.Value;

                    if (!oldUser.IsBlocked)
                    {
                        updatedUser.IsBlocked = true;
                    }
                    else
                    {
                        updatedUser.IsBlocked = false;
                    }

                    try
                    {
                        await userDictionary.TryUpdateAsync(tx, driverId, updatedUser, oldUser);
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

            // if (status) sendEmail();
            return status;
        }

        public async Task<bool> ApproveDriverAsync(string driverId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, driverId);

                if (!userResult.HasValue)
                {
                    status = false;
                }
                else if (userResult.Value.VerificationStatus != EVerificationStatus.Pending)
                {
                    status = false;
                }
                else
                {
                    var user = userResult.Value;
                    user.VerificationStatus = EVerificationStatus.Approved;

                    try
                    {
                        await userDictionary.TryUpdateAsync(tx, driverId, user, user);
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

            // if (status) sendEmail();
            return status;
        }

        public async Task<bool> DenyDriverAsync(string driverId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                var userResult = await userDictionary.TryGetValueAsync(tx, driverId);

                if (!userResult.HasValue)
                {
                    status = false;
                }
                else if (userResult.Value.VerificationStatus != EVerificationStatus.Pending)
                {
                    status = false;
                }
                else
                {
                    var user = userResult.Value;
                    user.VerificationStatus = EVerificationStatus.Denied;

                    try
                    {
                        await userDictionary.TryUpdateAsync(tx, driverId, user, user);
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

            // if (status) sendEmail();
            return status;
        }
        #endregion Admin actions
    }
}
