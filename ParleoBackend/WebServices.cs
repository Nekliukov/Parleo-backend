using Microsoft.Extensions.DependencyInjection;
using ParleoBackend.Configuration;
using ParleoBackend.Contracts;

namespace ParleoBackend
{
    public static class WebServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAccountImageSettings, AccountImageSettings>();
        }
    }
}
