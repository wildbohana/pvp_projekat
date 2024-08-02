using Common.Models;
using System.Runtime.Serialization;

namespace Common.DTOs
{
    [DataContract]
    public class RideEstimateDTO
    {
        [DataMember]
        public string? Id { get; set; }
        [DataMember]
        public string? StartAddress { get; set; }
        [DataMember]
        public string? FinalAddress { get; set; }
        [DataMember]
        public double Distance { get; set; }    // u kilometrima
        [DataMember]
        public float Price { get; set; }
        [DataMember]
        public int PickUpTime { get; set; }     // u minutama
        [DataMember]
        public string? CustomerId { get; set; }

        public RideEstimateDTO(Ride ride)
        {
            Id = ride.Id;
            StartAddress = ride.StartAddress;
            FinalAddress = ride.FinalAddress;
            Distance = ride.Distance;
            Price = ride.Price;
            PickUpTime = ride.PickUpTime;
            CustomerId = ride.CustomerId;
        }
    }
}
