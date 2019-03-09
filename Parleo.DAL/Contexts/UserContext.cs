using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Entities;
using Parleo.DAL.Entities.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Contexts
{
    public class UserContext : DbContext
    {
        public DbSet<UserInfo> UserInfo { get; set; }

        public DbSet<UserAuth> UserAuth { get; set; }

        public UserContext() : base()
        {
        }

        public UserContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new UserAuthConfiguration());
            modelBuilder.ApplyConfiguration(new UserFriendsConfiguration());
            modelBuilder.ApplyConfiguration(new UserInfoConfiguration());
            modelBuilder.ApplyConfiguration(new UserLanguageConfiguration());
        }
    }
}
