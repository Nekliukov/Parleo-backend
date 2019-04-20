using AutoMapper;
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
using System.Linq;

namespace Parleo.BLL.Extensions
{
    public static class MapperExtension
    {
        public static IMapper GetConfiguredMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                // entities
                mc.CreateMap<DataAccessAuth, UserLoginModel>();
                mc.CreateMap<UserLoginModel, DataAccessAuth>();

                mc.CreateMap<DataAccessAuth, UserRegistrationModel>();
                mc.CreateMap<UserRegistrationModel, DataAccessAuth>();

                mc.CreateMap<AccountTokenModel, AccountToken>();
                mc.CreateMap<AccountToken, AccountTokenModel>();

                mc.CreateMap<UserModel, DataAccessUser>();
                mc.CreateMap<DataAccessUser, UserModel>()
                    .ForMember(ui => ui.Email,
                        opt => opt.MapFrom(uivm => uivm.Credentials.Email))
                    .ForMember(um => um.CreatedEvents,
                        opt => opt.MapFrom(u =>
                            u.CreatedEvents.Select(e => new MiniatureModel
                            {
                                Id = e.Id,
                                // TODO for Ксюшенька
                                // Image = e.EventImage,
                                Image = "FakeImage",
                                Name = e.Name
                            })))
                    .ForMember(um => um.Friends,
                        opt => opt.MapFrom(u =>
                            u.Friends.Select(f => new MiniatureModel
                            {
                                Id = f.UserToId,
                                Image = f.UserTo.AccountImage,
                                Name = f.UserTo.Name
                            })))
                     .ForMember(um => um.AttendingEvents, 
                        opt => opt.MapFrom(u => 
                        u.AttendingEvents.Select(e => new MiniatureModel
                            {
                                Id = e.EventId,
                                // TODO for Ксюшенька
                                // Image = e.Event.EventImage,
                                Image = "FakeImage",
                                Name = e.Event.Name
                            })));

                mc.CreateMap<UpdateUserModel, DataAccessUser>();

                mc.CreateMap<DataAccessLanguage, LanguageModel>();
                mc.CreateMap<LanguageModel, DataAccessLanguage>();

                mc.CreateMap<DataAccessEvent, EventModel>()
                    .ForMember(em => em.Participants,
                        opt => opt.MapFrom(e => 
                            e.Participants.Select(p => new MiniatureModel
                            {
                                Id = p.UserId,
                                Image = p.User.AccountImage,
                                Name = p.User.Name
                            })));

                mc.CreateMap<CreateOrUpdateEventModel, DataAccessEvent>();

                mc.CreateMap<DataAccessUserLanguage, UserLanguageModel>()
                    .ForMember(ul => ul.Code, opt => opt.MapFrom(ulvm => ulvm.LanguageCode));

                mc.CreateMap<UserLanguageModel, DataAccessLanguage>();
                mc.CreateMap<UserLanguageModel, DataAccessUserLanguage>()
                    .ForMember(ul => ul.LanguageCode, opt => opt.MapFrom(l => l.Code));

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

            return mappingConfig.CreateMapper();
        }
    }
}
