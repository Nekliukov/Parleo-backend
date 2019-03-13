using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels;

namespace ParleoBackend.Mapping
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
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
