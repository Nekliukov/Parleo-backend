﻿using Parleo.BLL.Interfaces;
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

        public async Task SendEmailConfirmationLinkAsync(string to, string token)
        {
            string message = (await File.ReadAllTextAsync(_accountConfirmationMessageSettings.Message))
                .Replace(
                    "{{url}}",
                    _accountConfirmationMessageSettings.InvitationUrl.Replace("{{token}}", $"{token}"
                )
            );

            await SendAsync(to, _accountConfirmationMessageSettings.Subject, message);
        }

        private async Task SendAsync(string to, string subject, string body)
        {
            MailMessage message = new MailMessage(_emailClientSettings.Sender, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            SmtpClient client = new SmtpClient(_emailClientSettings.Host, _emailClientSettings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _emailClientSettings.UserName,
                    _emailClientSettings.Password
                )
            };

            await client.SendMailAsync(message);
        }
    }
}
