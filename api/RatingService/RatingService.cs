using Azure.Data.Tables;
using Common.DTOs;
using Common.Interfaces;
using Common.Models;
using Common.TableEntites;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace RatingService
{
    internal sealed class RatingService : StatefulService, IRatingService
    {
        #region Fields
        private TableClient ratingTable = null!;
        private Thread ratingTableThread = null!;
        private IReliableDictionary<string, Rating> ratingDictionary = null!;   // Init u RunAsync    

        public RatingService(StatefulServiceContext context) : base(context) { }
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
            await SetRatingTableAsync();
            ratingDictionary = await StateManager.GetOrAddAsync<IReliableDictionary<string, Rating>>("RatingDictionary");
            await PopulateRatingDictionary();

            ratingTableThread = new Thread(new ThreadStart(RatingTableWriteThread));
            ratingTableThread.Start();
        }


        private async Task SetRatingTableAsync()
        {
            var tableServiceClient = new TableServiceClient("UseDevelopmentStorage=true");
            await tableServiceClient.CreateTableIfNotExistsAsync("Rating");
            ratingTable = tableServiceClient.GetTableClient("Rating");
        }

        private async Task PopulateRatingDictionary()
        {
            var entities = ratingTable.QueryAsync<RatingEntity>(x => true).GetAsyncEnumerator();

            using (var tx = StateManager.CreateTransaction())
            {
                while (await entities.MoveNextAsync())
                {
                    var rating = new Rating(entities.Current);
                    await ratingDictionary.TryAddAsync(tx, rating.Id, rating);
                }

                await tx.CommitAsync();
            }
        }

        private async void RatingTableWriteThread()
        {
            while (true)
            {
                using (var tx = StateManager.CreateTransaction())
                {
                    var enumerator = (await ratingDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        var rating = enumerator.Current.Value;
                        var ratingEntity = new RatingEntity(rating);
                        await ratingTable.UpsertEntityAsync(ratingEntity, TableUpdateMode.Merge, CancellationToken.None);
                    }
                }

                Thread.Sleep(5000);
            }
        }
        #endregion RunAsync

        #region Rating methods
        public async Task<bool> RateRideAsync(RatingDTO data, string customerId)
        {
            bool status = false;

            using (var tx = StateManager.CreateTransaction())
            {
                // Provera da li je neko već ocenio tu vožnju
                var ratingResult = await ratingDictionary.TryGetValueAsync(tx, data.RideId);

                if (!ratingResult.HasValue)
                {
                    IRideService proxy = ServiceProxy.Create<IRideService>(new Uri("fabric:/api/RideService"), new ServicePartitionKey(1));
                    RideInfoDTO ride = await proxy.GetRideInfoAsync(data.RideId);
                   
                    if (ride != null && !String.IsNullOrEmpty(ride.DriverId))
                    {
                        Rating newRating = new Rating(data, ride.CustomerId, ride.DriverId);

                        try
                        {
                            await ratingDictionary.AddAsync(tx, data.RideId, newRating);
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

        public async Task<float> GetAverageDriverRateAsync(string driverId)
        {
            float suma = 0;
            float broj = 0;
            float prosek = 0;  // Ako ne postoje ocene, vraća se 0

            using (var tx = StateManager.CreateTransaction())
            {
                var enumerator = (await ratingDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var tmp = enumerator.Current.Value;
                    if (tmp.DriverId.Equals(driverId))
                    {
                        suma += tmp.Rate;
                        broj++;
                    }
                }
            }

            if (broj > 0) prosek = suma / broj;
            return prosek;
        }

        public async Task<bool> HasBeenRatedCheckAsync(string rideId)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var ratingResult = await ratingDictionary.TryGetValueAsync(tx, rideId);

                if (ratingResult.HasValue)
                    return true;
                return false;
            }
        }
        #endregion Rating methods
    }
}
