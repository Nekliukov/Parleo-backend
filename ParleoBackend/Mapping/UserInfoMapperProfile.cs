using AutoMapper;
using System;
using System.Linq;
using DataAssesUserInfo = Parleo.DAL.Entities.UserInfo;

namespace ParleoBackend.Mapping
{
    public class UserInfoMapperProfile : Profile
    {
        public UserInfoMapperProfile()
        {
            CreateMap<DataAssesUserInfo, UserInfoViewModel>()
                .ForMember(ui => ui.Languages, opt => opt.MapFrom(ui => 
                    ui.Languages.Select(l => l.UserId == ui.Id)));
        }
    }
}
