using Parleo.BLL.Interfaces;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Parleo.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailClientSettings _emailClientSettings;
        private readonly IAccountConfirmationMessageSettings _accountConfirmationMessageSettings;

        public EmailService(
            IEmailClientSettings emailClientSettings,
            IAccountConfirmationMessageSettings accountConfirmationMessageSettings
        )
        {
            _emailClientSettings = emailClientSettings;
            _accountConfirmationMessageSettings = accountConfirmationMessageSettings;
        }

        private async Task Send(string to, string subject, string body)
        {
            MailMessage message = new MailMessage(_emailClientSettings.Sender, to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(_emailClientSettings.Host, _emailClientSettings.Port);

            client.Credentials = new NetworkCredential(
                _emailClientSettings.UserName,
                _emailClientSettings.Password
            );

            client.EnableSsl = true;

            await client.SendMailAsync(message);
        }

        public async Task SendEmailConfirmationLink(string to, string token)
        {
            string message = (await File.ReadAllTextAsync(_accountConfirmationMessageSettings.Message))
                .Replace(
                    "{{url}}",
                    _accountConfirmationMessageSettings.InvitationUrl.Replace("{{token}}", $"{token}"
                )
            );

            await Send(to, _accountConfirmationMessageSettings.Subject, message);
        }
    }
}
