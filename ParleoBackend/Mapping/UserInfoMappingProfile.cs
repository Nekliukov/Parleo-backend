using AutoMapper;
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
                .ForMember(ui => ui.Languages, opt => opt.MapFrom(ui => 
                    ui.Languages.Select(l => l.UserId == ui.Id)));
        }
    }
}
