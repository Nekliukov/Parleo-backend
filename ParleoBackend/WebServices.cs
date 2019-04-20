using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Interfaces;
using ParleoBackend.Configuration;
using ParleoBackend.Contracts;
using ParleoBackend.Extensions;
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
            services.AddScoped<IAccountImageSettings, AccountImageSettings>();
        }

        public static IMapper GetMapper()
        {
            return MapperExtension.GetConfiguredMapper();
        }
    }
}
