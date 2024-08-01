using Common.Models;

// Ovo je za admina

namespace Common.DTOs
{
    public class DriverDTO
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? VerificationStatus { get; set; }
        public bool? IsBlocked { get; set; }
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
