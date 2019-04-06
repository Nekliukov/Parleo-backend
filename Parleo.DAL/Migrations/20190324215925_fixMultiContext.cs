using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class fixMultiContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_User_UserId",
                table: "Credentials");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvent_Event_EventId",
                table: "UserEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvent_User_UserId",
                table: "UserEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFriends_User_UserFromId",
                table: "UserFriends");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFriends_User_UserToId",
                table: "UserFriends");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_User_UserId",
                table: "UserLanguage");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: true),
                    Birthdate = table.Column<DateTime>(type: "Date", nullable: false),
                    Gender = table.Column<bool>(nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11, 8)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MaxParticipants = table.Column<int>(nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11, 8)", nullable: false),
                    IsFinished = table.Column<bool>(nullable: false),
                    StartTime = table.Column<DateTimeOffset>(nullable: false),
                    EndDate = table.Column<DateTimeOffset>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatorId",
                table: "Events",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_LanguageId",
                table: "Events",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Users_UserId",
                table: "Credentials",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvent_Events_EventId",
                table: "UserEvent",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvent_Users_UserId",
                table: "UserEvent",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriends_Users_UserFromId",
                table: "UserFriends",
                column: "UserFromId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriends_Users_UserToId",
                table: "UserFriends",
                column: "UserToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_Users_UserId",
                table: "UserLanguage",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Users_UserId",
                table: "Credentials");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvent_Events_EventId",
                table: "UserEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvent_Users_UserId",
                table: "UserEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFriends_Users_UserFromId",
                table: "UserFriends");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFriends_Users_UserToId",
                table: "UserFriends");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_Users_UserId",
                table: "UserLanguage");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Birthdate = table.Column<DateTime>(type: "Date", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Gender = table.Column<bool>(nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11, 8)", nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    CreatorId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTimeOffset>(nullable: true),
                    IsFinished = table.Column<bool>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11, 8)", nullable: false),
                    MaxParticipants = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_CreatorId",
                table: "Event",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LanguageId",
                table: "Event",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_User_UserId",
                table: "Credentials",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvent_Event_EventId",
                table: "UserEvent",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvent_User_UserId",
                table: "UserEvent",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriends_User_UserFromId",
                table: "UserFriends",
                column: "UserFromId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriends_User_UserToId",
                table: "UserFriends",
                column: "UserToId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_User_UserId",
                table: "UserLanguage",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
