using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Contexts
{
    public class UserContext : DbContext
    {
        //public DbSet<UserInfo> UserInfo { get; set; }
        //public DbSet<UserAuth> UserAuth { get; set; }

        public UserContext() : base()
        {
        }

        public UserContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<UserInfo>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Language>().Property(e => e.Id).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<UserInfo>().Property(ui => ui.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<UserAuth>()
                .HasOne(ua => ua.UserInfo)
                .WithOne(ui => ui.UserAuth)
                .HasForeignKey<UserAuth>(ua => ua.UserInfoId);

            modelBuilder.Entity<UserInfo>()
                .HasMany(ui => ui.Events)
                .WithOne(e => e.Creator);
            modelBuilder.Entity<Language>()
                .HasMany(lng => lng.Events)
                .WithOne(e => e.Language);

            #region User-Language m2m
            modelBuilder.Entity<UserLanguage>().HasKey(k => new { k.UserId, k.LanguageId });
            modelBuilder.Entity<UserLanguage>()
                .HasOne(ul => ul.UserInfo)
                .WithMany(ui => ui.Languages)
                .HasForeignKey(ul => ul.UserId);
            modelBuilder.Entity<UserLanguage>()
                .HasOne(ul => ul.Language)
                .WithMany(lng => lng.UserLanguages)
                .HasForeignKey(ul => ul.LanguageId);
            #endregion

            #region User-User m2m
            modelBuilder.Entity<UserFriends>().HasKey(k => new { k.UserToId, k.UserFromId });
            modelBuilder.Entity<UserFriends>()
                .HasOne(fs => fs.UserTo)
                .WithMany()
                .HasForeignKey(fs => fs.UserToId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserFriends>()
                .HasOne(fs => fs.UserFrom)
                .WithMany()
                .HasForeignKey(fs => fs.UserFromId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
