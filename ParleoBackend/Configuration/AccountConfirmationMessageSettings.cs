using Microsoft.Extensions.Configuration;
using Parleo.BLL.Interfaces;

namespace ParleoBackend.Configuration
{
    public class AccountConfirmationMessageSettings : IAccountConfirmationMessageSettings
    {
        private readonly IConfiguration _configuration;

        public AccountConfirmationMessageSettings(
            IConfiguration configuration
        )
        {
            _configuration = configuration.GetSection(nameof(AccountConfirmationMessageSettings));
        }

        public string InvitationUrl => _configuration.GetValue<string>(nameof(InvitationUrl));

        public string Message => _configuration.GetValue<string>(nameof(Message));

        public string Subject => _configuration.GetValue<string>(nameof(Subject));
    }
}
