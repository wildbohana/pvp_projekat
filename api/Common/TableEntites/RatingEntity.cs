using Azure;
using Azure.Data.Tables;
using Common.Models;

namespace Common.TableEntites
{
    public class RatingEntity : ITableEntity
    {
        public string? Id { get; set; }
        public int Rate { get; set; }
        public string? CustomerId { get; set; }
        public string? DriverId { get; set; }

        // ITableEntity implementation
        public string PartitionKey { get; set; } = "Rating";
        public string RowKey { get; set; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public RatingEntity()
        {
            PartitionKey = "Rating";
            Timestamp = DateTimeOffset.Now;
            ETag = ETag.All;
            //RowKey = new Guid().ToString();
        }

        public RatingEntity(Rating rating)
        {
            RowKey = rating.Id;
            Id = rating.Id;
            Rate = rating.Rate;
            CustomerId = rating.CustomerId;
            DriverId = rating.DriverId;
        }
    }
}
