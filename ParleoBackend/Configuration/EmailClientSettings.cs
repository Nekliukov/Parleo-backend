using Microsoft.Extensions.Configuration;
using Parleo.BLL.Interfaces;

namespace ParleoBackend.Configuration
{
    public class EmailClientSettings : IEmailClientSettings
    {
        private readonly IConfiguration _configuration;

        public EmailClientSettings(
            IConfiguration configuration
        )
        {
            _configuration = configuration.GetSection(nameof(EmailClientSettings));
        }

        public string Sender => _configuration.GetValue<string>(nameof(Sender));

        public string UserName => _configuration.GetValue<string>(nameof(UserName));

        public string Password => _configuration.GetValue<string>(nameof(Password));

        public string Host => _configuration.GetValue<string>(nameof(Host));

        public int Port => _configuration.GetValue<int>(nameof(Port));
    }
}