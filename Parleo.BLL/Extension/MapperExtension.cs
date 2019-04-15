using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.DAL.Models.Filters;
using Parleo.DAL.Models.Pages;
using Parleo.BLL.Models.Pages;
using DataAccessAuth = Parleo.DAL.Models.Entities.Credentials;
using DataAccessUser = Parleo.DAL.Models.Entities.User;
using DataAccessLanguage = Parleo.DAL.Models.Entities.Language;
using DataAccessEvent = Parleo.DAL.Models.Entities.Event;
using DataAccessUserLanguage = Parleo.DAL.Models.Entities.UserLanguage;
using Parleo.DAL.Models.Entities;

namespace Parleo.BLL.Extensions
{
    public static class MapperExtension
    {
        public static void Configure(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                // entities
                mc.CreateMap<DataAccessAuth, AuthorizationModel>();
                mc.CreateMap<AuthorizationModel, DataAccessAuth>();

                mc.CreateMap<AccountTokenModel, AccountToken>();

                mc.CreateMap<UserModel, DataAccessUser>();
                mc.CreateMap<DataAccessUser, UserModel>()
                    .ForMember(ui => ui.Email, 
                        opt => opt.MapFrom(uivm => uivm.Credentials.Email));                

                mc.CreateMap<DataAccessLanguage, LanguageModel>();
                mc.CreateMap<LanguageModel, DataAccessLanguage>();

                mc.CreateMap<DataAccessEvent, EventModel>()
                    .ForMember(em => em.ParticipantsCount,
                        opt => opt.MapFrom(e => e.Participants.Count));                           
                
                mc.CreateMap<CreateOrUpdateEventModel, DataAccessEvent>();

                mc.CreateMap<DataAccessUserLanguage, UserLanguageModel>()
                    .ForMember(ul => ul.Id, opt => opt.MapFrom(ulvm => ulvm.UserId))
                    .ForMember(ul => ul.Name, 
                        opt => opt.MapFrom(ulvm => ulvm.Language.Name));

                mc.CreateMap<UserLanguageModel, DataAccessLanguage>();
                mc.CreateMap<UserLanguageModel, DataAccessUserLanguage>()
                    .ForMember(ul => ul.LanguageId, opt => opt.MapFrom(l => l.Id));

                // filters
                mc.CreateMap<ChatFilterModel, ChatFilter>();
                mc.CreateMap<ChatFilter, ChatFilterModel>();

                mc.CreateMap<EventFilterModel, EventFilter>();
                mc.CreateMap<EventFilter, EventFilterModel>();

                mc.CreateMap<UserFilterModel, UserFilter>();
                mc.CreateMap<UserFilter, UserFilterModel>();

                // pages
                mc.CreateMap(typeof(PageModel<>), typeof(Page<>));
                mc.CreateMap(typeof(Page<>), typeof(PageModel<>));

                mc.CreateMap<PageRequestModel, PageRequest>();
                mc.CreateMap<PageRequest, PageRequestModel>();
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
