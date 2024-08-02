using Common.DTOs;
using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.Win32;

namespace Common.Interfaces
{
    public interface IUserService : IService
    {
        Task<bool> LoginAsync(LoginDTO credentials);
        Task<bool> RegisterAsync(RegisterDTO credentials);
        Task<RideDTO?> GetUserDataAsync(string email);
        Task<bool> UpdateProfileAsync(RideDTO credentials);
        Task<bool> UserExistsAsync(string email);
        Task<bool> GetBusyStatusAsync(string email);

        // Samo za admina
        Task<IEnumerable<DriverDTO>> GetAllDriversAsync();
        Task<bool> BlockDriverAsync(string driverId);
        Task<bool> ApproveDriverAsync(string driverId);
        Task<bool> DenyDriverAsync(string driverId);
    }
}
