using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ParleoBackend.Mapping
{
    public static class MapperExtension
    {
        public static void Configure(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserLanguageMappingProfile());
                mc.AddProfile(new EventMappingProfile());
                mc.AddProfile(new UserInfoMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
