using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ParleoBackend.Mapping;

namespace Parleo.BLL.Extensions
{
    public static class MapperExtension
    {
        public static void Configure(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserAuthMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
