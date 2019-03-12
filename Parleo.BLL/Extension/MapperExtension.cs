using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Models;
using DataAccessAuth = Parleo.DAL.Entities.Credentials;
using DataAccessUser = Parleo.DAL.Entities.User;

namespace Parleo.BLL.Extensions
{
    public static class MapperExtension
    {
        public static void Configure(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<DataAccessAuth, AuthorizationModel>();
                mc.CreateMap<AuthorizationModel, DataAccessAuth>();
                mc.CreateMap<UserModel, DataAccessUser>();
                mc.CreateMap<DataAccessUser, UserModel>()
                    .ForMember(ui => ui.Email, opt => opt.MapFrom(uivm => uivm.Credentials.Email));
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
