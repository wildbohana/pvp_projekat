using System.Runtime.Serialization;

namespace Common.DTOs
{
    [DataContract]
    public class RideNewDTO
    {
        [DataMember]
        public string? StartAddress { get; set; }
        [DataMember]
        public string? FinalAddress { get; set; }
    }
}
