using Common.Interfaces;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace EmailService
{
    internal sealed class EmailService : StatelessService, IEmailService
    {
        public EmailService(StatelessServiceContext context) : base(context) { }

        #region Create listeners
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }
        #endregion Create listeners

        #region RunAsync
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO ne znam da li ovde treba nešto menjati
            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
        #endregion RunAsync

        #region IEmailService Implementation
        public Task<bool> SendEmail()
        {
            throw new NotImplementedException();
        }
        #endregion IEmailService Implementation

    }
}
