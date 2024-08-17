using System.Runtime.Serialization;

namespace Common.DTOs
{
    [DataContract]
    public class RatingDTO
    {
        [DataMember]
        public string RideId { get; set; }
        [DataMember]
        public int Rate { get; set; } 
    }
}
