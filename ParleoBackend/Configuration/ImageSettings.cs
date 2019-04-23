using Microsoft.Extensions.Configuration;
using ParleoBackend.Contracts;

namespace ParleoBackend.Configuration
{
    public class ImageSettings: IImageSettings
    {
        private readonly IConfiguration _configuration;

        public ImageSettings(
            IConfiguration configuration    
        )
        {
            _configuration = configuration.GetSection(nameof(ImageSettings));
        }

        public string AccountSourceUrl => _configuration.GetValue<string>(nameof(AccountSourceUrl));

        public string AccountDestPath => _configuration.GetValue<string>(nameof(AccountDestPath));

        public string EventSourceUrl => _configuration.GetValue<string>(nameof(EventSourceUrl));

        public string EventDestPath => _configuration.GetValue<string>(nameof(EventDestPath));
    }
}
