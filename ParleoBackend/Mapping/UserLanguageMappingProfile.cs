using AutoMapper;
using System;
using System.Linq;
using DataAssesLanguage = Parleo.DAL.Entities.UserLanguage;

namespace ParleoBackend.Mapping
{
    public class UserLanguageMappingProfile : Profile
    {
        public UserLanguageMappingProfile()
        {
            CreateMap<DataAssesLanguage, UserLanguageViewModel>()
                .ForMember(ul => ul.Id, opt => opt.MapFrom(ulvm => ulvm.UserId))
                .ForMember(ul => ul.Name, opt => opt.MapFrom(ulvm => ulvm.Language.Name));

            CreateMap<UserLanguageViewModel, DataAssesLanguage>();
        }
    }
}
