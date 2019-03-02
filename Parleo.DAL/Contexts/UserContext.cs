﻿using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Contexts
{
    public class UserContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<UserAuth> UserAuths { get; set; }

        public UserContext() : base()
        {
        }

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
