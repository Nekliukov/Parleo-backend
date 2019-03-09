using AutoMapper;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels;
using DataAccessUserInfo = Parleo.DAL.Entities.UserInfo;

namespace ParleoBackend.Mapping
{
    public class UserAuthMappingProfile : Profile
    {
        public UserAuthMappingProfile()
        {
            CreateMap<AuthorizationModel, AuthorizationViewModel>();

            CreateMap<AuthorizationViewModel, AuthorizationModel>();

            CreateMap<AuthUserInfoViewModel, DataAccessUserInfo>();
        }
    }
}
