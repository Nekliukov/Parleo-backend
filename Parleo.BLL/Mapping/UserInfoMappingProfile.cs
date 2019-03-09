using AutoMapper;
using Parleo.BLL.Models;
using DataAccessUserInfo = Parleo.DAL.Entities.UserInfo;

namespace ParleoBackend.Mapping
{
    public class UserInfoMappingProfile : Profile
    {
        public UserInfoMappingProfile()
        {
            CreateMap<DataAccessUserInfo, UserInfoModel>()
                .ForMember(ui => ui.Email, opt => opt.MapFrom(uivm => uivm.UserAuth.Email));

            CreateMap<UserInfoModel, DataAccessUserInfo>();            
        }
    }
}
