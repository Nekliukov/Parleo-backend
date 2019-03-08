using AutoMapper;
using ParleoBackend.ViewModels;
using DataAssesAuth = Parleo.DAL.Entities.UserAuth;
using DataAssesUserInfo = Parleo.DAL.Entities.UserInfo;

namespace ParleoBackend.Mapping
{
    public class UserAuthMappingProfile : Profile
    {
        public UserAuthMappingProfile()
        {
            CreateMap<DataAssesAuth, UserAuthViewModel>();

            CreateMap<UserAuthViewModel, DataAssesAuth>();

            CreateMap<AuthUserInfoViewModel, DataAssesUserInfo>();
        }
    }
}
