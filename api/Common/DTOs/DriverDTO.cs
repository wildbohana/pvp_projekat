using Common.Models;
using System.Runtime.Serialization;

// Ovo je za admina

namespace Common.DTOs
{
    [DataContract]
    public class DriverDTO
    {
        [DataMember]
        public string? Email { get; set; }
        [DataMember]
        public string? Username { get; set; }
        [DataMember]
        public string? Firstname { get; set; }
        [DataMember]
        public string? Lastname { get; set; }
        [DataMember]
        public string? VerificationStatus { get; set; }
        [DataMember]
        public bool? IsBlocked { get; set; }
        [DataMember]
        public double AvgRate { get; set; } = 0;

        public DriverDTO(User driver)
        {
            Email = driver.Email;
            Username = driver.Username;
            Firstname = driver.Firstname;
            Lastname = driver.Lastname;
            VerificationStatus = driver.VerificationStatus.ToString();
            IsBlocked = driver.IsBlocked;
            AvgRate = driver.AvgRate;
        }
    }
}
