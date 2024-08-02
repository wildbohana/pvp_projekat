using Common.DTOs;
using Common.Enums;
using Common.TableEntites;
using System.Runtime.Serialization;

// In memory model (čuva se u rečniku)

namespace Common.Models
{
    [DataContract]
    public class User
    {
        // Email je ID korisnika (ne menja se)
        [DataMember]
        public string? Email { get; set; }
        [DataMember]
        public string? Username { get; set; }
        [DataMember]
        public string? Password { get; set; }
        [DataMember]
        public string? Firstname { get; set; }
        [DataMember]
        public string? Lastname { get; set; }
        [DataMember]
        public string? DateOfBirth { get; set; }
        [DataMember]
        public string? Address { get; set; }
        [DataMember]
        public EUserType UserType { get; set; }
        [DataMember]
        public string? PhotoUrl { get; set; }
        [DataMember]
        public EVerificationStatus VerificationStatus { get; set; }
        [DataMember]
        public bool IsBlocked { get; set; }
        [DataMember]
        public bool Busy { get; set; }

        public User(UserEntity user)
        {
            Email = user.Email;
            Username = user.Username;
            Password = user.Password;
            Address = user.Address;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            DateOfBirth = user.DateOfBirth;
            UserType = user.UserType;
            PhotoUrl = user.PhotoUrl;
            VerificationStatus = user.VerificationStatus;
            IsBlocked = user.IsBlocked;
            Busy = user.Busy;
        }

        public User(RegisterDTO user)
        {
            Email = user.Email;
            Username = user.Username;
            Password = user.Password;
            Address = user.Address;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            DateOfBirth = user.DateOfBirth;
            UserType = user.Role == EUserType.Customer.ToString() ? EUserType.Customer : EUserType.Driver;
            PhotoUrl = user.PhotoUrl;
            VerificationStatus = (UserType == EUserType.Customer) ? EVerificationStatus.Approved : EVerificationStatus.Pending;
            IsBlocked = false;
            Busy = false;
        }
    }
}
