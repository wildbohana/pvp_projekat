using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IEmailService : IService
    {
        Task<bool> SendEmail();
    }
}
