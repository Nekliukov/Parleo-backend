using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Extensions
{
    public static class MapperExtension
    {
        public static void Configure(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<AuthorizationModel, AuthorizationViewModel>();
                mc.CreateMap<AuthorizationViewModel, AuthorizationModel>();

                mc.CreateMap<UserViewModel, UserModel>();
                mc.CreateMap<UserModel, UserViewModel>();

                mc.CreateMap<EventModel, EventViewModel>();
                mc.CreateMap<EventViewModel, EventModel>();

                mc.CreateMap<LanguageModel, LanguageViewModel>();
                mc.CreateMap<LanguageViewModel, LanguageModel>();

                mc.CreateMap<UserLanguageModel, UserLanguageViewModel>();
                mc.CreateMap<UserLanguageViewModel, UserLanguageModel>();
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
