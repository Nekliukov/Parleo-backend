using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Interfaces;
using ParleoBackend.Configuration;
using ParleoBackend.Contracts;
using ParleoBackend.Extensions;
using ParleoBackend.Hubs;
using ParleoBackend.Services;
using ParleoBackend.Validators.Common;
using ParleoBackend.Validators.Event;
using ParleoBackend.Validators.User;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Pages;
using System.Globalization;

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
            services.AddTransient<IValidator<UpdateEventViewModel>, UpdateEventViewModelValidator>();
            services.AddTransient<IValidator<CreateEventViewModel>, CreateEventViewModelValidator>();
            services.AddTransient<IValidator<UserRegistrationViewModel>, UserRegistrationViewModelValidator>();
            services.AddTransient<IValidator<UserLoginViewModel>, UserLoginViewModelValidator>();
            services.AddTransient<IValidator<UpdateUserViewModel>, UpdateUserViewModelValidator>();
            services.AddTransient<IValidator<LocationViewModel>, LocationViewModelValidator>();
            services.AddTransient<IValidator<PageRequestViewModel>, PageRequestViewModelValidator>();
            ValidatorOptions.LanguageManager.Culture = new CultureInfo("en-GB");
            services.AddTransient<ClearExpiredTokenJob>();
        }

        public static IMapper GetMapper(IConfiguration configuration)
        {
            return MapperExtension.GetConfiguredMapper(configuration);
        }
    }
}
