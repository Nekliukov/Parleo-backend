using Microsoft.Extensions.Configuration;

namespace ParleoBackend.Configuration
{
    public class JwtSettings : IJwtSettings
    {
        private readonly IConfiguration _configuration;

        public JwtSettings(
            IConfiguration configuration
        )
        {
            _configuration = configuration.GetSection(nameof(JwtSettings));
        }

        public string JWTKey => _configuration.GetValue<string>(nameof(JWTKey));
    }
}
