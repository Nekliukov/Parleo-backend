using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Repositories;

namespace Parleo.DAL
{
    public static class DalServices
    {
        public static void AddServices(IServiceCollection services, string connectionString)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUtilityRepository, UtilityRepository>();
            services.AddDbContext<AppContext>(
                options => options.UseSqlServer(connectionString));            
        }
    }
}
