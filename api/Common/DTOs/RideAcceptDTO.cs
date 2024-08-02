using System.Runtime.Serialization;

// Koristi vozač kada potvrđuje vožnju

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
