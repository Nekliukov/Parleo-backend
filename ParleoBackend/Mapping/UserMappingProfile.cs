using AutoMapper;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels;

namespace ParleoBackend.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserViewModel, UserModel>();
            CreateMap<UserModel, UserViewModel>();
        }
    }
}
