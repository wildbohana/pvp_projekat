using System.Runtime.Serialization;

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
        [DataMember]
        public string? VerificationStatus { get; set; }
        [DataMember]
        public bool? IsBlocked { get; set; }
    }
}
