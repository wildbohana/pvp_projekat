namespace Common.DTOs
{
    public class UpdateUserDTO
    {
        public string Email { get; set; }   // Ne može da se menja (jer je ID)
        
        public string Username { get; set; }

        public string ConfirmOldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Address { get; set; }

        //public IFormFile Photo { get; set; }
    }
}
