﻿using AutoMapper;
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
using DataAccessUserHobby = Parleo.DAL.Models.Entities.UserHobby;
using Parleo.DAL.Models.Entities;
using System.Linq;
using System.Collections.Generic;

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
                    .ForMember(um => um.Friends,
                        opt => opt.MapFrom(u =>
                            u.Friends.Select(f => new MiniatureModel
                            {
                                Id = f.UserToId,
                                Image = f.UserTo.AccountImage,
                                Name = f.UserTo.Name
                            })));

                mc.CreateMap<UpdateUserModel, DataAccessUser>()
                    .ForMember(u => u.Hobbies,
                        opt => opt.MapFrom(um =>
                            um.Hobbies.Select(h => new UserHobby()
                            {
                                HobbyName = h,
                            })));

                mc.CreateMap<DataAccessLanguage, LanguageModel>();
                mc.CreateMap<LanguageModel, DataAccessLanguage>();

                mc.CreateMap<DataAccessEvent, EventModel>()
                    .ForMember(em => em.Creator, 
                        opt => opt.MapFrom(c => new MiniatureModel()
                        {
                            Id = c.Creator.Id,
                            Image = c.Creator.AccountImage,
                            Name = c.Creator.Name
                        }))
                    .ForMember(em => em.Participants,
                        opt => opt.MapFrom(e => 
                            e.Participants.Select(p => new MiniatureModel
                            {
                                Id = p.UserId,
                                Image = p.User.AccountImage,
                                Name = p.User.Name
                            })));

                mc.CreateMap<UpdateEventModel, DataAccessEvent>();
                mc.CreateMap<CreateEventModel, DataAccessEvent>()
                    .ForMember(e => e.Participants,
                        opt => opt.MapFrom(ce => new List<UserEvent>()
                        {
                            new UserEvent()
                            {
                                UserId = ce.CreatorId
                            }
                        }));

                mc.CreateMap<DataAccessUserLanguage, UserLanguageModel>()
                    .ForMember(ul => ul.Code, opt => opt.MapFrom(ulvm => ulvm.LanguageCode));
                mc.CreateMap<UserLanguageModel, DataAccessUserLanguage>()
                    .ForMember(ul => ul.LanguageCode, opt => opt.MapFrom(l => l.Code));

                mc.CreateMap<Hobby, HobbyModel>()
                    .ForMember(hm => hm.Name, opt => opt.MapFrom(h => h.Name))
                    .ForMember(hm => hm.Category, opt => opt.MapFrom(h => h.Category.Name))
                    .ForAllOtherMembers(opt => opt.Ignore());
                mc.CreateMap<HobbyModel, Hobby>()
                    .ForMember(h => h.Name, opt => opt.MapFrom(hm => hm.Name))
                    .ForAllOtherMembers(opt => opt.Ignore());

                mc.CreateMap<UserLanguageModel, DataAccessLanguage>();

                mc.CreateMap<MessageModel, Message>();
                mc.CreateMap<Message, MessageModel>();

                mc.CreateMap<Chat, ChatModel>()
                    .ForMember(cm => cm.LastMessage,
                        opt => opt.MapFrom(c => c.Messages != null ? c.Messages.FirstOrDefault() : null))
                    .ForMember(cm => cm.Members, opt => opt.MapFrom(c => c.Members
                        .Select(m => new MiniatureModel()
                        {
                            Id = m.User.Id,
                            Image = m.User.AccountImage,
                            Name = m.User.Name
                        })))
                    .ForMember(cm => cm.Event, 
                    opt => opt.MapFrom(c => c.Event != null 
                    ? new MiniatureModel()
                    {
                        Id = c.Event.Id,
                        Image = c.Event.Image,
                        Name = c.Event.Name
                    } : null));
                mc.CreateMap<ChatModel, Chat>();

                mc.CreateMap<DataAccessUserHobby, HobbyModel>()
                    .ForMember(hm => hm.Category, opt => opt.MapFrom(uh => uh.Hobby.Category.Name))
                    .ForMember(hm => hm.Name, opt => opt.MapFrom(uh => uh.HobbyName));
                mc.CreateMap<HobbyModel, DataAccessUserHobby>()
                    .ForMember(uh => uh.HobbyName, opt => opt.MapFrom(hm => hm.Name));

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
