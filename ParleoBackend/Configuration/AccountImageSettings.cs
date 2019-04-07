using Microsoft.Extensions.Configuration;
using ParleoBackend.Contracts;

namespace ParleoBackend.Configuration
{
    public class AccountImageSettings: IAccountImageSettings
    {
        private readonly IConfiguration _configuration;

        public AccountImageSettings(
            IConfiguration configuration    
        )
        {
            _configuration = configuration.GetSection(nameof(AccountImageSettings));
        }

        public string SourceUrl => _configuration.GetValue<string>(nameof(SourceUrl));
        public string DestPath => _configuration.GetValue<string>(nameof(DestPath));
    }
}
