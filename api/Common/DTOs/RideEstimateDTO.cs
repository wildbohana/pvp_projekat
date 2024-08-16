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
        public DateTime EstimatedArrivalTime { get; set; }      // ovo je UTC, koristi lokalizaciju na FE?
        [DataMember]
        public double Distance { get; set; }    // u kilometrima
        [DataMember]
        public int Price { get; set; }
        [DataMember]
        public string? CustomerId { get; set; }
        [DataMember]
        public string? Status { get; set; }

        public RideEstimateDTO(Ride ride)
        {
            Id = ride.Id;
            StartAddress = ride.StartAddress;
            FinalAddress = ride.FinalAddress;
            Distance = ride.Distance;
            Price = ride.Price;
            EstimatedArrivalTime = ride.StartTime.AddMinutes(ride.PickUpTime);
            CustomerId = ride.CustomerId;
            Status = ride.Status.ToString();
        }
    }
}
