using Microsoft.Extensions.DependencyInjection;
using Parleo.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Parleo.BLL;
using Parleo.DAL;

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
            BLServices.AddServices(services);
            DalServices.AddServices(services);
            services.AddDbContext<UserContext>(
                options => options.UseSqlServer(connectionString));
        }
    }
}
