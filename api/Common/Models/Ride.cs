using Common.Enums;
using Common.TableEntites;
using System.Runtime.Serialization;

// In-memory model (čuva se u rečniku)

namespace Common.Models
{
    [DataContract]
    public class Ride
    {
        // ili int ili string (GUID)
        [DataMember]
        public int Id { get; set; }
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
        public int RideDuration { get; set; }   // u minutama
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public ERideStatus Status { get; set; }

        [DataMember]
        public string? CustomerId { get; set; }
        [DataMember]
        public UserEntity? Customer { get; set; }

        [DataMember]
        public string? DriverId { get; set; }
        [DataMember]
        public UserEntity? Driver { get; set; }

        // TODO constructors (za RideDTO i RideEntity)
    }
}
