using Common.Models;
using System.Runtime.Serialization;

namespace Common.DTOs
{
    [DataContract]
    public class RideInfoDTO
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string StartAddress { get; set; }
        [DataMember]
        public string FinalAddress { get; set; }
        [DataMember]
        public double Distance { get; set; }    // u kilometrima
        [DataMember]
        public int Price { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime ArrivalTime { get; set; }
        [DataMember]
        public string Status { get; set; }     // default - Pending
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string? DriverId { get; set; } = null;
        [DataMember]
        public int Rating { get; set; }

        public RideInfoDTO(Ride ride)
        {
            Id = ride.Id;
            StartAddress = ride.StartAddress;
            FinalAddress = ride.FinalAddress;
            Distance = ride.Distance;
            Price = ride.Price;
            StartTime = ride.StartTime;
            ArrivalTime = ride.ArrivalTime;
            Status = ride.Status.ToString();
            CustomerId = ride.CustomerId;
            DriverId = ride.DriverId;
            Rating = ride.Rating;
        }
    }
}
