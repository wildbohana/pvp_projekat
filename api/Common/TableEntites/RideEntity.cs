using Azure;
using Azure.Data.Tables;
using Common.Enums;
using Common.Models;
using System.Runtime.Serialization;

namespace Common.TableEntites
{
    public class RideEntity : ITableEntity
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string StartAddress { get; set; }
        [DataMember]
        public string FinalAddress { get; set; }
        [DataMember]
        public int Distance { get; set; }    // u kilometrima
        [DataMember]
        public int Price { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime ArrivalTime { get; set; }
        [DataMember]
        public ERideStatus Status { get; set; }     // default - Pending
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string? DriverId { get; set; }
        [DataMember]
        public int Rating { get; set; }

        // ITableEntity implementation
        public string PartitionKey { get; set; } = "Ride";
        public string RowKey { get; set; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public RideEntity()
        {
            PartitionKey = "Ride";
            Timestamp = DateTimeOffset.Now;
            ETag = ETag.All;
            //RowKey = new Guid().ToString();
        }

        public RideEntity(Ride ride)
        {
            RowKey = ride.Id;
            Id = ride.Id;
            StartAddress = ride.StartAddress;
            FinalAddress = ride.FinalAddress;
            Distance = ride.Distance;
            Price = ride.Price;
            StartTime = ride.StartTime;
            ArrivalTime = ride.ArrivalTime;
            Status = ride.Status;
            CustomerId = ride.CustomerId;
            DriverId = ride.DriverId;
            Rating = ride.Rating;
        }
    }
}
