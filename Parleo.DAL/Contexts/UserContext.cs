using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Contexts
{
    public class UserContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Credentials> Credentials { get; set; }

        public UserContext() : base()
        {
        }

        public UserContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<User>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Language>().Property(e => e.Id).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Credentials>()
                .HasOne(c => c.User)
                .WithOne(ui => ui.Credentials)
                .HasForeignKey<Credentials>(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedEvents)
                .WithOne(e => e.Creator);
            modelBuilder.Entity<Language>()
                .HasMany(lng => lng.Events)
                .WithOne(e => e.Language);

            #region User-Language m2m
            modelBuilder.Entity<UserLanguage>().HasKey(k => new { k.UserId, k.LanguageId });
            modelBuilder.Entity<UserLanguage>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.Languages)
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
                .WithMany(u => u.Friends)
                .HasForeignKey(fs => fs.UserToId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserFriends>()
                .HasOne(fs => fs.UserFrom)
                .WithMany()
                .HasForeignKey(fs => fs.UserFromId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region User-Event m2m
            modelBuilder.Entity<UserEvent>().HasKey(k => new { k.UserId, k.EventId });
            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.AttendingEvents)
                .HasForeignKey(ue => ue.UserId);
            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ue => ue.EventId);
            #endregion
        }
    }
}
