using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Extensions;
using Parleo.BLL.Helpers;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Services;

namespace Parleo.BLL
{
    public static class BLServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISecurityHelper, SecurityHelper>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUtilityService, UtilityService>();
            services.AddScoped<IChatService, ChatService>();
        }

        public static IMapper GetMapper()
        {
            return MapperExtension.GetConfiguredMapper();
        }
    }
}
