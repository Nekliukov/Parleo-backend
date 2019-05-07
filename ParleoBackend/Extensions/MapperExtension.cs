using AutoMapper;
using Microsoft.Extensions.Configuration;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using ParleoBackend.Configuration;
using ParleoBackend.Contracts;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Filters;
using ParleoBackend.ViewModels.Pages;
using System.IO;
using System.Linq;

namespace ParleoBackend.Extensions
{
    public static class MapperExtension
    {
        public static IMapper GetConfiguredMapper(IConfiguration configuration)
        {
            IImageSettings imageSettings = new ImageSettings(configuration);
            var mappingConfig = new MapperConfiguration(mc =>
            {
            // entities

            mc.CreateMap<UserLoginModel, UserLoginViewModel>();
            mc.CreateMap<UserLoginViewModel, UserLoginModel>();

            mc.CreateMap<UserRegistrationModel, UserRegistrationViewModel>();
            mc.CreateMap<UserRegistrationViewModel, UserRegistrationModel>();

            mc.CreateMap<UserViewModel, UserModel>();
            mc.CreateMap<UserModel, UserViewModel>()
            .ForMember(uvm => uvm.AccountImage, opt =>
                opt.MapFrom(um =>
                    FileExtension.GetFullFilePath(
                        imageSettings.BaseUrl,
                        imageSettings.AccountSourceUrl,
                        um.AccountImage)
                    )
            );

            mc.CreateMap<EventModel, EventViewModel>()
            .ForMember(uvm => uvm.Image, opt =>
                opt.MapFrom(um =>
                    FileExtension.GetFullFilePath(
                        imageSettings.BaseUrl,
                        imageSettings.EventSourceUrl,
                        um.Image)
                    )
            );
            mc.CreateMap<EventViewModel, EventModel>();

            mc.CreateMap<UpdateEventViewModel, UpdateEventModel>();
            mc.CreateMap<UpdateEventModel, UpdateEventViewModel>();

            mc.CreateMap<CreateEventViewModel, CreateEventModel>();
            mc.CreateMap<CreateEventModel, CreateEventViewModel>();

            mc.CreateMap<LanguageModel, LanguageViewModel>()
                .ForMember(lvm => lvm.Id, opt => opt.MapFrom(lm => lm.Code));

            mc.CreateMap<LanguageViewModel, LanguageModel>();

            mc.CreateMap<HobbyModel, HobbyViewModel>();
            mc.CreateMap<HobbyViewModel, HobbyModel>();

            mc.CreateMap<UserLanguageModel, UserLanguageViewModel>();
            mc.CreateMap<UserLanguageViewModel, UserLanguageModel>();

            mc.CreateMap<MiniatureModel, UserMiniatureViewModel>()
            .ForMember(mm => mm.Image, opt =>
                opt.MapFrom(mvm =>
                    FileExtension.GetFullFilePath(
                        imageSettings.BaseUrl,
                        imageSettings.AccountSourceUrl,
                        mvm.Image)
                    )
            );
            mc.CreateMap<UserMiniatureViewModel, MiniatureModel>();

            mc.CreateMap<MiniatureModel, EventMiniatureViewModel>()
                .ForMember(mm => mm.Image, opt =>
                    opt.MapFrom(mvm =>
                        FileExtension.GetFullFilePath(
                            imageSettings.BaseUrl,
                            imageSettings.EventSourceUrl,
                            mvm.Image)
                        )
                );
            mc.CreateMap<EventMiniatureViewModel, MiniatureModel>();

            mc.CreateMap<UpdateUserViewModel, UpdateUserModel>();
            mc.CreateMap<UpdateUserModel, UpdateUserViewModel>();

            mc.CreateMap<MessageModel, MessageViewModel>();
            mc.CreateMap<MessageViewModel, MessageModel>();

            mc.CreateMap<ChatModel, ChatViewModel>()
                .ForMember(ecvm => ecvm.Image, opt =>
                    opt.MapFrom(cm => cm.EventMiniature != null ?
                        FileExtension.GetFullFilePath(
                                imageSettings.BaseUrl,
                                imageSettings.EventSourceUrl,
                                cm.EventMiniature.Image)
                        : FileExtension.GetFullFilePath(
                        imageSettings.BaseUrl,
                        imageSettings.EventSourceUrl,
                        cm.Image)));

            mc.CreateMap<ChatViewModel, ChatModel>();

            mc.CreateMap<CreateGroupChatViewModel, ChatModel>()
                .ForMember(cm => cm.Members,
                    opt => opt.MapFrom(cgcvm =>
                        cgcvm.Members.Select(id => new MiniatureModel() {Id = id}))
                    );
                mc.CreateMap<EventMiniatureViewModel, MiniatureModel>();

                mc.CreateMap<UpdateUserViewModel, UpdateUserModel>();
                mc.CreateMap<UpdateUserModel, UpdateUserViewModel>();

                mc.CreateMap<MessageModel, MessageViewModel>();
                mc.CreateMap<MessageViewModel, MessageModel>();

                mc.CreateMap<ChatModel, ChatViewModel>()
                    .ForMember(ecvm => ecvm.Image, opt =>
                        opt.MapFrom(cm => cm.EventMiniature != null ?
                            FileExtension.GetFullFilePath(
                                    imageSettings.BaseUrl,
                                    imageSettings.EventSourceUrl,
                                    cm.EventMiniature.Image)
                            : FileExtension.GetFullFilePath(
                            imageSettings.BaseUrl,
                            imageSettings.AccountSourceUrl,
                            cm.Image)));

                mc.CreateMap<ChatViewModel, ChatModel>();


                mc.CreateMap<HobbyModel, HobbyViewModel>();
                mc.CreateMap<HobbyViewModel, HobbyModel>();

                // filters
                mc.CreateMap<ChatFilterViewModel, ChatFilterModel>();
                mc.CreateMap<ChatFilterModel, ChatFilterViewModel>();

                mc.CreateMap<EventFilterViewModel, EventFilterModel>();
                mc.CreateMap<EventFilterModel, EventFilterViewModel>();

                mc.CreateMap<UserFilterViewModel, UserFilterModel>();
                mc.CreateMap<UserFilterModel, UserFilterViewModel>();

                // pages
                mc.CreateMap(typeof(PageViewModel<>), typeof(PageModel<>));
                mc.CreateMap(typeof(PageModel<>), typeof(PageViewModel<>));

                mc.CreateMap<PageRequestViewModel, PageRequestModel>();
                mc.CreateMap<PageRequestModel, PageRequestViewModel>();
            });

            return mappingConfig.CreateMapper();
        }
    }
}
