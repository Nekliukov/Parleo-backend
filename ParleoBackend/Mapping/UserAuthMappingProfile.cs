using AutoMapper;
using ParleoBackend.ViewModels;
using DataAssesAuth = Parleo.DAL.Entities.UserAuth;

namespace ParleoBackend.Mapping
{
    public class UserAuthMappingProfile : Profile
    {
        public UserAuthMappingProfile()
        {
            CreateMap<DataAssesAuth, UserAuthViewModel>();

            CreateMap<UserAuthViewModel, DataAssesAuth>();
        }
    }
}
