﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parleo.DAL.Contexts;

namespace Parleo.DAL.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20190304103233_FixUserPasswordHash")]
    partial class FixUserPasswordHash
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Parleo.DAL.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid?>("CreatorId");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EndDate");

                    b.Property<bool>("IsFinished");

                    b.Property<Guid?>("LanguageId");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(11, 8)");

                    b.Property<int>("MaxParticipants");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("LanguageId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("Parleo.DAL.Entities.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("Parleo.DAL.Entities.UserAuth", b =>
                {
                    b.Property<Guid>("UserInfoId");

                    b.Property<string>("Email");

                    b.Property<DateTime>("LastLogin");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.HasKey("UserInfoId");

                    b.ToTable("UserAuth");
                });

            modelBuilder.Entity("Parleo.DAL.Entities.UserFriends", b =>
                {
                    b.Property<Guid>("UserToId");

                    b.Property<Guid>("UserFromId");

                    b.Property<int>("Status");

                    b.HasKey("UserToId", "UserFromId");

                    b.HasIndex("UserFromId");

                    b.ToTable("UserFriends");
                });

            modelBuilder.Entity("Parleo.DAL.Entities.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("Date");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("FirstName");

                    b.Property<bool>("Gender");

                    b.Property<string>("LastName");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(11, 8)");

                    b.HasKey("Id");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("Parleo.DAL.Entities.UserLanguage", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("LanguageId");

                    b.Property<byte>("Level");

                    b.HasKey("UserId", "LanguageId");

                    b.HasIndex("LanguageId");

                    b.ToTable("UserLanguage");
                });

            modelBuilder.Entity("Parleo.DAL.Entities.Event", b =>
                {
                    b.HasOne("Parleo.DAL.Entities.UserInfo", "Creator")
                        .WithMany("Events")
                        .HasForeignKey("CreatorId");

                    b.HasOne("Parleo.DAL.Entities.Language", "Language")
                        .WithMany("Events")
                        .HasForeignKey("LanguageId");
                });

            modelBuilder.Entity("Parleo.DAL.Entities.UserAuth", b =>
                {
                    b.HasOne("Parleo.DAL.Entities.UserInfo", "UserInfo")
                        .WithOne("UserAuth")
                        .HasForeignKey("Parleo.DAL.Entities.UserAuth", "UserInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Parleo.DAL.Entities.UserFriends", b =>
                {
                    b.HasOne("Parleo.DAL.Entities.UserInfo", "UserFrom")
                        .WithMany()
                        .HasForeignKey("UserFromId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Parleo.DAL.Entities.UserInfo", "UserTo")
                        .WithMany()
                        .HasForeignKey("UserToId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Parleo.DAL.Entities.UserLanguage", b =>
                {
                    b.HasOne("Parleo.DAL.Entities.Language", "Language")
                        .WithMany("UserLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Parleo.DAL.Entities.UserInfo", "UserInfo")
                        .WithMany("Languages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
