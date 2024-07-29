using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public int Age { get; set; }        // umesto DoB
        public string Address { get; set; }
        public string VerificationStatus { get; set; }
        public bool Busy { get; set; }      // za vozače
        public bool IsBlocked { get; set; } // za vozače
    }
}
