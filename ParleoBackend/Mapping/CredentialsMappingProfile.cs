using AutoMapper;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels;
using DataAccessUser = Parleo.DAL.Entities.User;

namespace ParleoBackend.Mapping
{
    public class CredentialsMappingProfile : Profile
    {
        public CredentialsMappingProfile()
        {
            CreateMap<AuthorizationModel, AuthorizationViewModel>();

            CreateMap<AuthorizationViewModel, AuthorizationModel>();

            CreateMap<AuthUserViewModel, DataAccessUser>();
        }
    }
}
