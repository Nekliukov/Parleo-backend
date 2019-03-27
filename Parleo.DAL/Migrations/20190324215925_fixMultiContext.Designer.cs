﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Parleo.DAL.Migrations
{
    [DbContext(typeof(AppContext))]
    [Migration("20190324215925_fixMultiContext")]
    partial class fixMultiContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Parleo.DAL.Models.Entities.Credentials", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("Email");

                    b.Property<DateTimeOffset>("LastLogin");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.HasKey("UserId");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("CreatorId");

                    b.Property<string>("Description");

                    b.Property<DateTimeOffset?>("EndDate");

                    b.Property<bool>("IsFinished");

                    b.Property<Guid>("LanguageId");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(11, 8)");

                    b.Property<int>("MaxParticipants");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("LanguageId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("Date");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<bool>("Gender");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(11, 8)");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.UserEvent", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("EventId");

                    b.HasKey("UserId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("UserEvent");
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.UserFriends", b =>
                {
                    b.Property<Guid>("UserToId");

                    b.Property<Guid>("UserFromId");

                    b.Property<int>("Status");

                    b.HasKey("UserToId", "UserFromId");

                    b.HasIndex("UserFromId");

                    b.ToTable("UserFriends");
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.UserLanguage", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("LanguageId");

                    b.Property<byte>("Level");

                    b.HasKey("UserId", "LanguageId");

                    b.HasIndex("LanguageId");

                    b.ToTable("UserLanguage");
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.Credentials", b =>
                {
                    b.HasOne("Parleo.DAL.Models.Entities.User", "User")
                        .WithOne("Credentials")
                        .HasForeignKey("Parleo.DAL.Models.Entities.Credentials", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.Event", b =>
                {
                    b.HasOne("Parleo.DAL.Models.Entities.User", "Creator")
                        .WithMany("CreatedEvents")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Parleo.DAL.Models.Entities.Language", "Language")
                        .WithMany("Events")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.UserEvent", b =>
                {
                    b.HasOne("Parleo.DAL.Models.Entities.Event", "Event")
                        .WithMany("Participants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Parleo.DAL.Models.Entities.User", "User")
                        .WithMany("AttendingEvents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.UserFriends", b =>
                {
                    b.HasOne("Parleo.DAL.Models.Entities.User", "UserFrom")
                        .WithMany()
                        .HasForeignKey("UserFromId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Parleo.DAL.Models.Entities.User", "UserTo")
                        .WithMany("Friends")
                        .HasForeignKey("UserToId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Parleo.DAL.Models.Entities.UserLanguage", b =>
                {
                    b.HasOne("Parleo.DAL.Models.Entities.Language", "Language")
                        .WithMany("UserLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Parleo.DAL.Models.Entities.User", "User")
                        .WithMany("Languages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
