﻿using Microsoft.Extensions.DependencyInjection;
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
            MapperExtension.Configure(services);
        }
    }
}