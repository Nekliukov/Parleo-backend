using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Contexts
{
    class UserContext : DbContext
    {
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserAuth> UserAuth { get; set; }

        public UserContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserAuth>()
                .HasOne(ua => ua.UserInfo)
                .WithOne(ui => ui.UserAuth)
                .HasForeignKey<UserAuth>(ua => ua.UserInfoId);
        }
    }
}
