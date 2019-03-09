using AutoMapper;
using Parleo.BLL.Models;
using DataAccessAuth = Parleo.DAL.Entities.UserAuth;

namespace ParleoBackend.Mapping
{
    public class UserAuthMappingProfile : Profile
    {
        public UserAuthMappingProfile()
        {
            CreateMap<DataAccessAuth, AuthorizationModel>();

            CreateMap<AuthorizationModel, DataAccessAuth>();
        }
    }
}
