using System.Runtime.Serialization;

// In-memory model (čuva se u rečniku)

namespace Common.Models
{
    [DataContract]
    public class Rating
    {
        // int ili string (GUID)
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int RideId { get; set; }
        [DataMember]
        public int Rate { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string? CustomerUsername { get; set; }
        [DataMember]
        public int DriverId { get; set; }
        [DataMember]
        public string? DriverUsername { get; set; }

        // TODO constructors (za RatingDTO i RatingEntity)
    }
}
