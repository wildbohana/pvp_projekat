using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interfaces
{
    public interface IEmailService : IService
    {
        Task SendEmail(string emailAddress, bool isApproved);
    }
}
