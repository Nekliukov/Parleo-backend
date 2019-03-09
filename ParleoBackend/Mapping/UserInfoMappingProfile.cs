using AutoMapper;
using ParleoBackend.ViewModels;
using System;
using System.Linq;
using DataAccessUserInfo = Parleo.DAL.Entities.UserInfo;

namespace ParleoBackend.Mapping
{
    public class UserInfoMappingProfile : Profile
    {
        public UserInfoMappingProfile()
        {
            CreateMap<DataAccessUserInfo, UserInfoViewModel>()
                .ForMember(ui => ui.Email, opt => opt.MapFrom(uivm => uivm.UserAuth.Email));

            CreateMap<UserInfoViewModel, DataAccessUserInfo>();            
        }
    }
}
