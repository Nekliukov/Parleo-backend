using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace Parleo.DAL.Contexts
{
    class UserContext : DbContext
    {
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserAuth> UserAuth { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserAuth>()
                .HasRequired(ua => ua.UserInfo)
                .WithRequiredPrincipal(ui => ui.UserAuth);
        }
    }
}
