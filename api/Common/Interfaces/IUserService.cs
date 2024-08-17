using Common.DTOs;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IUserService : IService
    {
        Task<bool> LoginAsync(LoginDTO credentials);
        Task<bool> RegisterAsync(RegisterDTO credentials);
        Task<UserDTO?> GetUserDataAsync(string email);
        Task<bool> UpdateProfileAsync(UserDTO credentials);
        Task<bool> UserExistsAsync(string email);
        Task<bool> IsDriverVerifiedCheckAsync(string email);
        Task<bool> IsDriverBlockedCheckAsync(string email);
        Task<string> GetUserTypeFromEmail(string email);

        // Samo za admina
        Task<IEnumerable<DriverDTO>> GetAllDriversAsync();
        Task<bool> BlockDriverAsync(string driverId);
        Task<bool> ApproveDriverAsync(string driverId);
        Task<bool> DenyDriverAsync(string driverId);
    }
}
