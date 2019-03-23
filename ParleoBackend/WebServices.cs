using Microsoft.Extensions.DependencyInjection;
using ParleoBackend.Configuration;
using ParleoBackend.Contracts;
using ParleoBackend.Services;

namespace ParleoBackend
{
    public static class WebServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IClaimsService, ClaimsService>();
            services.AddSingleton<IJwtSettings, JwtSettings>();
            services.AddSingleton<IJwtService, JwtService>();
        }
    }
}
