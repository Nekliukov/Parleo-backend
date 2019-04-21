using Microsoft.Extensions.Configuration;
using ParleoBackend.Contracts;

namespace ParleoBackend.Configuration
{
    public class EventImageSettings: IImageSettings
    {
        private readonly IConfiguration _configuration;

        public EventImageSettings(
            IConfiguration configuration
        )
        {
            _configuration = configuration.GetSection(nameof(EventImageSettings));
        }

        public string SourceUrl => _configuration.GetValue<string>(nameof(SourceUrl));
        public string DestPath => _configuration.GetValue<string>(nameof(DestPath));
    }
}
