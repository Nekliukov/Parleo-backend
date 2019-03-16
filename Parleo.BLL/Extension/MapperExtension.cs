using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Models;
using DataAccessAuth = Parleo.DAL.Entities.Credentials;
using DataAccessUser = Parleo.DAL.Entities.User;
using DataAccessLanguage = Parleo.DAL.Entities.Language;
using DataAccessEvent = Parleo.DAL.Entities.Event;

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
                        opt => opt.MapFrom(e => e.Participants.Count));
                mc.CreateMap<EventModel, DataAccessEvent>();


            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
