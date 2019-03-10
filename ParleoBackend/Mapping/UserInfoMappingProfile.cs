using AutoMapper;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels;

namespace ParleoBackend.Mapping
{
    public class UserInfoMappingProfile : Profile
    {
        public UserInfoMappingProfile()
        {
            CreateMap<UserInfoViewModel, UserInfoModel>();
            CreateMap<UserInfoModel, UserInfoViewModel>();
        }
    }
}
