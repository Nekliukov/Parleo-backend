﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parleo.DAL.Contexts;
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
            services.AddDbContext<UserContext>(
                options => options.UseSqlServer(connectionString));
            services.AddDbContext<EventContext>(
                options => options.UseSqlServer(connectionString));
        }
    }
}