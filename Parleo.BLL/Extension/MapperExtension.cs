using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Models;
using DataAccessAuth = Parleo.DAL.Entities.UserAuth;
using DataAccessUserInfo = Parleo.DAL.Entities.UserInfo;

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
                mc.CreateMap<UserInfoModel, DataAccessUserInfo>();
                mc.CreateMap<DataAccessUserInfo, UserInfoModel>()
                    .ForMember(ui => ui.Email, opt => opt.MapFrom(uivm => uivm.UserAuth.Email));
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
