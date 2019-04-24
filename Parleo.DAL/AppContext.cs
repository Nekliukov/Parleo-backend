using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Models.Entities;

namespace Parleo.DAL
{
    public class AppContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Credentials> Credentials { get; set; }

        public DbSet<Event> Event { get; set; }

        public DbSet<AccountToken> AccountToken { get; set; }

        public DbSet<Chat> Chat { get; set; }

        public DbSet<Language> Language { get; set; }

        public DbSet<Hobby> Hobby { get; set; }

        public AppContext() : base()
        {
        }

        public AppContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<User>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Chat>().Property(c => c.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            #region One-to-One
            modelBuilder.Entity<Credentials>()
                .HasOne(c => c.User)
                .WithOne(ui => ui.Credentials)
                .HasForeignKey<Credentials>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Chat>()
                .HasMany(chat => chat.Messages)
                .WithOne(message => message.Chat)
                .HasForeignKey(message => message.ChatId);

            modelBuilder.Entity<Event>()
                .HasOne(ev => ev.Chat)
                .WithOne();  //Or withMany()

            modelBuilder.Entity<AccountToken>()
                .HasOne(c => c.User)
                .WithOne(ui => ui.AccountToken)
                .HasForeignKey<AccountToken>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region One-to-Many
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedEvents)
                .WithOne(e => e.Creator);
            modelBuilder.Entity<Language>()
                .HasMany(lng => lng.Events)
                .WithOne(e => e.Language);
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Hobbies)
                .WithOne(h => h.Category);
            #endregion

            #region User-Language m2m
            modelBuilder.Entity<UserLanguage>().HasKey(k => new { k.UserId, k.LanguageCode });
            modelBuilder.Entity<UserLanguage>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.Languages)
                .HasForeignKey(ul => ul.UserId);
            modelBuilder.Entity<UserLanguage>()
                .HasOne(ul => ul.Language)
                .WithMany(lng => lng.UserLanguages)
                .HasForeignKey(ul => ul.LanguageCode);
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
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ue => ue.EventId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Chat-User m2m
            modelBuilder.Entity<ChatUser>().HasKey(chatUser => new { chatUser.ChatId, chatUser.UserId });
            modelBuilder.Entity<ChatUser>()
                .HasOne(chatUser => chatUser.Chat)
                .WithMany(user => user.Members)
                .HasForeignKey(chatUser => chatUser.ChatId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ChatUser>()
                .HasOne(chatUser => chatUser.User)
                .WithMany(user => user.Chats)
                .HasForeignKey(chatUser => chatUser.UserId);
            #endregion
            #region User-Hobby m2m
            modelBuilder.Entity<UserHobby>().HasKey(k => new { k.UserId, k.HobbyName });
            modelBuilder.Entity<UserHobby>()
                .HasOne(uh => uh.User)
                .WithMany(u => u.Hobbies)
                .HasForeignKey(uh => uh.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserHobby>()
                .HasOne(uh => uh.Hobby)
                .WithMany(h => h.Users)
                .HasForeignKey(uh => uh.HobbyName)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
