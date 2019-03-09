using AutoMapper;
using ParleoBackend.ViewModels;
using DataAccessAuth = Parleo.DAL.Entities.UserAuth;
using DataAccessUserInfo = Parleo.DAL.Entities.UserInfo;

namespace ParleoBackend.Mapping
{
    public class UserAuthMappingProfile : Profile
    {
        public UserAuthMappingProfile()
        {
            CreateMap<DataAccessAuth, UserAuthViewModel>();

            CreateMap<UserAuthViewModel, DataAccessAuth>();

            CreateMap<AuthUserInfoViewModel, DataAccessUserInfo>();
        }
    }
}
