using Microsoft.Extensions.DependencyInjection;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Repositories;

namespace Parleo.DAL
{
    public static class DalServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IEventsRepository, EventsRepository>();
        }
    }
}
