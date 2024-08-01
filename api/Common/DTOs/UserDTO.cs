using Common.Enums;
using Common.Models;
using System.Runtime.Serialization;

// Za prikaz profila i za ažuriranje podataka
// Po potrebi u kontroleru menjati polja (kao u RCA)

namespace Common.DTOs
{
    [DataContract]
    public class UserDTO
    {
        [DataMember]
        public string? Email { get; set; }   // Ne može da se menja (jer je ID) 
        [DataMember]
        public string? Username { get; set; }
        [DataMember]
        public string? ConfirmOldPassword { get; set; }
        [DataMember]
        public string? NewPassword { get; set; }
        [DataMember]
        public string? ConfirmNewPassword { get; set; }
        [DataMember]
        public string? Firstname { get; set; }
        [DataMember]
        public string? Lastname { get; set; }
        [DataMember]
        public string? Address { get; set; }
        [DataMember]
        public string? DateOfBirth { get; set; }
        [DataMember]
        public string? Role { get; set; }
        [DataMember]
        public string? PhotoUrl { get; set; }
    }
}
