using Common.DTOs;
using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.Win32;

namespace Common.Interfaces
{
    public interface IUserService : IService
    {
        // Dodati još i metode za blokiranje i verifikaciju vozača
        Task<bool> LoginAsync(LoginDTO credentials);

        Task<bool> RegisterAsync(RegisterDTO credentials);

        Task<UpdateUserDTO?> GetUserDataAsync(string email);

        Task<bool> UpdateProfileAsync(UpdateUserDTO credentials);

        Task<bool> UserExistsAsync(string email);

        // TODO remove
        Task<bool> Test();

    }
}
