using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Filters;
using ParleoBackend.ViewModels.Pages;
using System.Globalization;

namespace ParleoBackend.Extensions
{
    public static class MapperExtension
    {
        public static void Configure(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                // entities
                mc.CreateMap<UserLoginModel, UserLoginViewModel>();
                mc.CreateMap<UserLoginViewModel, UserLoginModel>();

                mc.CreateMap<UserRegistrationModel, UserRegistrationViewModel>();
                mc.CreateMap<UserRegistrationViewModel, UserRegistrationModel>();

                mc.CreateMap<UserViewModel, UserModel>();
                mc.CreateMap<UserModel, UserViewModel>();

                mc.CreateMap<EventModel, EventViewModel>();
                mc.CreateMap<EventViewModel, EventModel>();

                mc.CreateMap<CreateOrUpdateEventViewModel, CreateOrUpdateEventModel>();
                mc.CreateMap<CreateOrUpdateEventModel, CreateOrUpdateEventViewModel>();

                mc.CreateMap<LanguageModel, LanguageViewModel>()
                    .ForMember(lvm => lvm.Name, opt => opt.MapFrom(lm => new CultureInfo(lm.Code).NativeName));
                mc.CreateMap<LanguageViewModel, LanguageModel>();

                mc.CreateMap<UserLanguageModel, UserLanguageViewModel>();
                mc.CreateMap<UserLanguageViewModel, UserLanguageModel>();

                mc.CreateMap<MiniatureModel, MiniatureViewModel>();
                mc.CreateMap<MiniatureViewModel, MiniatureModel>();

                mc.CreateMap<UpdateUserViewModel, UpdateUserModel>();
                mc.CreateMap<UpdateUserModel, UpdateUserViewModel>();

                mc.CreateMap<MessageModel, MessageViewModel>();
                mc.CreateMap<MessageViewModel, MessageModel>();

                mc.CreateMap<ChatModel, ChatViewModel>();
                mc.CreateMap<ChatViewModel, ChatModel>();

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

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
