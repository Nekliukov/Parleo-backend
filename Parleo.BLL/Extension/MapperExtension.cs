using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Models.Entities;
using DataAccessAuth = Parleo.DAL.Entities.Credentials;
using DataAccessUser = Parleo.DAL.Entities.User;
using DataAccessLanguage = Parleo.DAL.Entities.Language;
using DataAccessEvent = Parleo.DAL.Entities.Event;
using DataAccessUserLanguage = Parleo.DAL.Entities.UserLanguage;

namespace Parleo.BLL.Extensions
{
    public static class MapperExtension
    {
        public static void Configure(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<DataAccessAuth, AuthorizationModel>();
                mc.CreateMap<AuthorizationModel, DataAccessAuth>();

                mc.CreateMap<UserModel, DataAccessUser>();
                mc.CreateMap<DataAccessUser, UserModel>()
                    .ForMember(ui => ui.Email, 
                        opt => opt.MapFrom(uivm => uivm.Credentials.Email));

                mc.CreateMap<DataAccessLanguage, LanguageModel>();
                mc.CreateMap<LanguageModel, DataAccessLanguage>();

                mc.CreateMap<DataAccessEvent, EventModel>()
                    .ForMember(em => em.ParticipantsCount,
                        opt => opt.MapFrom(e => e.Participants.Count))
                    .ForMember(em => em.CreatorId,
                        opt => opt.MapFrom(e => e.Creator.Id));
                mc.CreateMap<EventModel, DataAccessEvent>()
                    .ForMember(e => e.Creator,
                        opt => opt.MapFrom(em => new DataAccessUser()
                        {
                            Id = em.Id
                        }));

                mc.CreateMap<DataAccessUserLanguage, UserLanguageModel>()
                    .ForMember(ul => ul.Id, opt => opt.MapFrom(ulvm => ulvm.UserId))
                    .ForMember(ul => ul.Name, 
                        opt => opt.MapFrom(ulvm => ulvm.Language.Name));

                mc.CreateMap<UserLanguageModel, DataAccessLanguage>();
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
