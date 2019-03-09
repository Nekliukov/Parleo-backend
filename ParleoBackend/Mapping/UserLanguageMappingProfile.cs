using AutoMapper;
using ParleoBackend.ViewModels;
using System;
using System.Linq;
using DataAccessLanguage = Parleo.DAL.Entities.UserLanguage;

namespace ParleoBackend.Mapping
{
    public class UserLanguageMappingProfile : Profile
    {
        public UserLanguageMappingProfile()
        {
            CreateMap<DataAccessLanguage, UserLanguageViewModel>()
                .ForMember(ul => ul.Id, opt => opt.MapFrom(ulvm => ulvm.UserId))
                .ForMember(ul => ul.Name, opt => opt.MapFrom(ulvm => ulvm.Language.Name));

            CreateMap<UserLanguageViewModel, DataAccessLanguage>();
        }
    }
}
