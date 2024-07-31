using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public string? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }

        public string? Photo { get; set; }
    }
}
