using AutoMapper;
using ParleoBackend.ViewModels;
using System;
using System.Linq;
using DataAssesUserInfo = Parleo.DAL.Entities.UserInfo;

namespace ParleoBackend.Mapping
{
    public class UserInfoMappingProfile : Profile
    {
        public UserInfoMappingProfile()
        {
            CreateMap<DataAssesUserInfo, UserInfoViewModel>()
                .ForMember(ui => ui.Mail, opt => opt.MapFrom(uivm => uivm.UserAuth.Email));

            CreateMap<UserInfoViewModel, DataAssesUserInfo>();            
        }
    }
}
