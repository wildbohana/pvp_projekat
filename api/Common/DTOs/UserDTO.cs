using Common.Enums;
using Common.Models;

// Za prikaz profila i za ažuriranje podataka
// Po potrebi u kontroleru menjati polja (kao u RCA)

namespace Common.DTOs
{
    public class UserDTO
    {
        public string? Email { get; set; }   // Ne može da se menja (jer je ID) 
        public string? Username { get; set; }
        public string? ConfirmOldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Address { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Role { get; set; }
        public string? Photo { get; set; }

        public UserDTO(User user)
        {
            Email = user.Email;
            Username = user.Username;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            Address = user.Address;
            ConfirmNewPassword = "";
            ConfirmOldPassword = "";
            NewPassword = "";
            DateOfBirth = user.DateOfBirth;
            Role = user.UserType.ToString();
            Photo = user.PhotoUrl;
        }
    }
}
