namespace Common.Models
{
    public class Rating
    {
        // int ili string (GUID)
        public int Id { get; set; }

        public int Rate { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerUsername { get; set; }
        public int DriverId { get; set; }
        public string? DriverUsername { get; set; }
    }
}
