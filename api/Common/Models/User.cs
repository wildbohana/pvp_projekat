using Common.Enums;

namespace Common.Models
{
    public class User
    {
        // Email je ID korisnika (ne menja se)
        public string? Email { get; set; } 
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? Lastname { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Address { get; set; }
        public EUserType UserType { get; set; }
        public string? PhotoUrl { get; set; }
        public EVerificationStatus VerificationStatus { get; set; }
        public bool IsBlocked { get; set; }
        public bool Busy { get; set; }
        public double AvgRate { get; set; } = 0;
    }
}
