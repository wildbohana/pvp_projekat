using Common.Enums;
using Common.TableEntites;
using System.Runtime.Serialization;

// In-memory model (čuva se u rečniku)

namespace Common.Models
{
    [DataContract]
    public class Ride
    {
        [DataMember]
        public string? Id { get; set; }
        [DataMember]
        public string? StartAddress { get; set; }
        [DataMember]
        public string? FinalAddress { get; set; }
        [DataMember]
        public double Distance { get; set; }        // u kilometrima
        [DataMember]
        public float Price { get; set; }
        [DataMember]
        public int PickUpTime { get; set; }         // u minutama
        [DataMember]
        public int RideDuration { get; set; }       // u minutama
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public ERideStatus Status { get; set; }     // default - Pending
        [DataMember]
        public string? CustomerId { get; set; }
        [DataMember]
        public string? DriverId { get; set; } = null;

        public Ride(RideEntity entity)
        {
            Id = entity.Id;
            StartAddress = entity.StartAddress;
            FinalAddress = entity.FinalAddress;
            Distance = entity.Distance;
            Price = entity.Price;
            PickUpTime = entity.PickUpTime;
            RideDuration = entity.RideDuration;
            StartTime = entity.StartTime;
            Status = entity.Status;
            CustomerId = entity.CustomerId;
            DriverId = entity.DriverId;
        }

        // RideDTO -> Ride manuelno pravi (šta je sigurno sigurno je)
    }
}
