using Common.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using MimeKit;
using System.Fabric;

namespace EmailService
{
    internal sealed class EmailService : StatelessService, IEmailService
    {
        // TODO premesti u app.config
        private const string fromAddress = "drs.projekat.tim12@gmail.com";
        private const string appPassword = "aetu jlgc mmnz svvh";

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
        public async Task SendEmail(string emailAddress, bool isApproved)
        {
            var messageBody = "Hello!\n\nYour account has been ";
            if (isApproved)
            {
                messageBody += "approved.\nHappy riding!";
            }
            else
            {
                messageBody += "denied.\nSorry to hear that.";
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Administrator", fromAddress));
            message.To.Add(new MailboxAddress("", emailAddress));
            message.Subject = "Driver verification status";
            message.Body = new TextPart("plain") { Text = messageBody };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(fromAddress, appPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        #endregion IEmailService Implementation
    }
}
