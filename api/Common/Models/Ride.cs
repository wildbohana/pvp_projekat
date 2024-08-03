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

        public Ride(string? startAddress, string? finalAddress, string? customerId)
        {
            Random rand = new Random();

            Id = new Guid().ToString();
            StartAddress = startAddress;
            FinalAddress = finalAddress;
            Distance = rand.Next(1, 10);
            Price = rand.Next(300, 1000);
            PickUpTime = rand.Next(4, 12);
            RideDuration = rand.Next(5, 15);
            StartTime = DateTime.Now.AddMinutes(PickUpTime);    // ažuriraj još jednom kada vozač potvrdi vožnju
            Status = ERideStatus.Pending;
            CustomerId = customerId;
            DriverId = null;
        }

        // RideDTO -> Ride manuelno pravi (šta je sigurno sigurno je)
    }
}
