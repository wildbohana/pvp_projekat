using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IUserService : IService
    {
        bool CreateNewUser();
        bool UpdateUser();
        Task<User> GetUserInfo();

        // TODO remove
        Task<bool> Test();

    }
}
