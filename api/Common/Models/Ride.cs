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
        public string Id { get; set; }
        [DataMember]
        public string StartAddress { get; set; }
        [DataMember]
        public string FinalAddress { get; set; }
        [DataMember]
        public int Distance { get; set; }           // u kilometrima
        [DataMember]
        public int Price { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }     // Prvi unos kada se zatraži vožnja, drugi unos kada vozač započne vožnju
        [DataMember]
        public DateTime ArrivalTime { get; set; }   // Prvi unos je procenjeno vreme dolaska vozača, drugi unos kada vozač završi vožnju
        [DataMember]
        public ERideStatus Status { get; set; }     // default - Pending
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string? DriverId { get; set; }
        [DataMember]
        public int Rating { get; set; }

        public Ride(RideEntity entity)
        {
            Id = entity.Id;
            StartAddress = entity.StartAddress;
            FinalAddress = entity.FinalAddress;
            Distance = entity.Distance;
            Price = entity.Price;
            StartTime = entity.StartTime;
            ArrivalTime = entity.ArrivalTime;
            Status = entity.Status;
            CustomerId = entity.CustomerId;
            DriverId = entity.DriverId;
            Rating = entity.Rating;
        }

        public Ride(string startAddress, string finalAddress, string customerId)
        {
            Random rand = new Random();
            int waitTime = rand.Next(4, 12);
            int kilometers = rand.Next(1, 10);

            Id = Guid.NewGuid().ToString();
            StartAddress = startAddress;
            FinalAddress = finalAddress;
            Distance = kilometers;
            Price = kilometers * 60;
            StartTime = DateTime.UtcNow;                           // ažuriraj još jednom kada vozač potvrdi vožnju
            ArrivalTime = DateTime.UtcNow.AddMinutes(waitTime);    // ažuriraj još jednom kada vozač potvrdi vožnju
            Status = ERideStatus.Pending;
            CustomerId = customerId;
            DriverId = null;
            Rating = 0;
        }

        // RideDTO -> Ride manuelno pravi (šta je sigurno sigurno je)
    }
}
