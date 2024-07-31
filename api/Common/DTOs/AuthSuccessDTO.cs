using System.Runtime.Serialization;
using System.ServiceModel;

namespace Common.DTOs
{
    public class AuthSuccessDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
    }
}
