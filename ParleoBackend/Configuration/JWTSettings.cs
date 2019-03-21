using Microsoft.Extensions.Configuration;

namespace ParleoBackend.Configuration
{
    public class JWTSettings: IJWTSettings
    {
        private readonly IConfiguration _configuration;

        public JWTSettings(
            IConfiguration configuration
        )
        {
            _configuration = configuration.GetSection(nameof(JWTSettings));
        }

        public string JWTKey => _configuration.GetValue<string>(nameof(JWTKey));
    }
}
