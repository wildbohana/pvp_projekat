using Common.Enums;
using Common.TableEntites;

namespace Common.Models
{
    public class Ride
    {
        // ili int ili string (GUID)
        public int Id { get; set; }

        public string? StartAddress { get; set; }
        public string? FinalAddress { get; set; }
        public double Distance { get; set; }    // u kilometrima
        public float Price { get; set; }
        public int PickUpTime { get; set; }     // u minutama
        public int RideDuration { get; set; }   // u minutama
        public DateTime StartTime { get; set; }
        public ERideStatus Status { get; set; }

        public int CustomerId { get; set; }
        public UserEntity? Customer { get; set; }

        public int? DriverId { get; set; }
        public UserEntity? Driver { get; set; }
    }
}
