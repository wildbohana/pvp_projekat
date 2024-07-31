using Common.Enums;
using Azure.Data.Tables;
using Azure;
using Common.Models;

namespace Common.TableEntites
{
    public class UserEntity : ITableEntity
    {
        // Email je ID korisnika (ne menja se)
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public EUserType UserType { get; set; }
        public string? PhotoUrl { get; set; }
        public EVerificationStatus VerificationStatus { get; set; }
        public bool IsBlocked { get; set; }
        public bool Busy { get; set; }
        public double AvgRate { get; set; } = 0;

        // ITableEntity implementation
        public string PartitionKey { get; set; } = "User";
        public string RowKey { get; set; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public UserEntity()
        {
            PartitionKey = "User";
            Timestamp = DateTimeOffset.Now;
            ETag = ETag.All;
        }

        public UserEntity(User user) : base()
        {
            RowKey = user.Email;

            Username = user.Username;
            Email = user.Email;
            Password = user.Password;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            DateOfBirth = user.DateOfBirth;
            Address = user.Address;
            UserType = user.UserType;
            PhotoUrl = user.PhotoUrl;
            VerificationStatus = user.VerificationStatus;
            IsBlocked = user.IsBlocked;
            Busy = user.Busy;
            AvgRate = user.AvgRate;
        }
    }
}
