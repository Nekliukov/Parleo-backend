using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Interfaces;
using ParleoBackend.Configuration;
using ParleoBackend.Contracts;
using ParleoBackend.Extensions;
using ParleoBackend.Hubs;
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
            services.AddScoped<IEmailClientSettings, EmailClientSettings>();
            services.AddScoped<IAccountConfirmationMessageSettings, AccountConfirmationMessageSettings>();
            services.AddScoped<IImageSettings, ImageSettings>();
            services.AddScoped<IChatHub, ChatHub>();
            services.AddSingleton<BackgroundWorkerRegistry>();
        }

        public static IMapper GetMapper(IConfiguration configuration)
        {
            return MapperExtension.GetConfiguredMapper(configuration);
        }
    }
}
