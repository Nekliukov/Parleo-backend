using Microsoft.Extensions.DependencyInjection;
using Parleo.BLL.Services;
using Parleo.DAL.Repositories;
using System;
using System.Linq;
using Parleo.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Parleo.DI
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Method for automatic dependency injection
        /// </summary>
        /// <param name="services">Registered services</param>
        /// <param name="connectionString"></param>
        public static void InjectDependencies(IServiceCollection services, string connectionString)
        {
            RegisterScoped(typeof(UsersService), "Service", services);
            RegisterScoped(typeof(UsersRepository), "Repository", services);

            services.AddDbContext<UserContext>(
                options => options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// This method will auto-inject services and repositories
        /// which names end with "Service" or "Repository"
        /// </summary>
        private static void RegisterScoped(Type anyType, string typesPostfix, IServiceCollection services)
        {
            var types = anyType.Assembly.GetExportedTypes().Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith(typesPostfix));

            foreach (var type in types)
            {
                var interfaceType = type.GetInterface("I" + type.Name);
                services.AddScoped(interfaceType, type);
            }
        }
    }
}
