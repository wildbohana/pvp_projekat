using System.Runtime.Serialization;

// Probably will need some changes

namespace Common.DTOs
{
    [DataContract]
    public class RideAcceptDTO
    {
        [DataMember]
        public string? Id { get; set; }
        [DataMember]
        public string? CustomerId { get; set; }
    }
}
