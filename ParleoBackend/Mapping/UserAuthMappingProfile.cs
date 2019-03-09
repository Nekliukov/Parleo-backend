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
            CreateMap<DataAccessAuth, AuthorizationRequest>();

            CreateMap<AuthorizationRequest, DataAccessAuth>();

            CreateMap<AuthUserInfoViewModel, DataAccessUserInfo>();
        }
    }
}
