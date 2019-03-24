using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Filters;
using ParleoBackend.ViewModels.Pages;

namespace ParleoBackend.Extensions
{
    public static class MapperExtension
    {
        public static void Configure(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                // entities
                mc.CreateMap<AuthorizationModel, AuthorizationViewModel>();
                mc.CreateMap<AuthorizationViewModel, AuthorizationModel>();

                mc.CreateMap<UserViewModel, UserModel>();
                mc.CreateMap<UserModel, UserViewModel>();

                mc.CreateMap<EventModel, EventViewModel>();
                mc.CreateMap<EventViewModel, EventModel>();

                mc.CreateMap<LanguageModel, LanguageViewModel>();
                mc.CreateMap<LanguageViewModel, LanguageModel>();

                mc.CreateMap<UserLanguageModel, UserLanguageViewModel>();
                mc.CreateMap<UserLanguageViewModel, UserLanguageModel>();

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
