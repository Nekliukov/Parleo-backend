using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class InitialModelsCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(2)", nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: true),
                    AccountImage = table.Column<string>(nullable: true),
                    Birthdate = table.Column<DateTime>(type: "Date", nullable: false),
                    Gender = table.Column<bool>(nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11, 8)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hobby",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    CategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hobby", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Hobby_Category_CategoryName",
                        column: x => x.CategoryName,
                        principalTable: "Category",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountToken",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountToken", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AccountToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    LastLogin = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Credentials_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFriends",
                columns: table => new
                {
                    UserToId = table.Column<Guid>(nullable: false),
                    UserFromId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriends", x => new { x.UserToId, x.UserFromId });
                    table.ForeignKey(
                        name: "FK_UserFriends_User_UserFromId",
                        column: x => x.UserFromId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFriends_User_UserToId",
                        column: x => x.UserToId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLanguage",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LanguageCode = table.Column<string>(nullable: false),
                    Level = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguage", x => new { x.UserId, x.LanguageCode });
                    table.ForeignKey(
                        name: "FK_UserLanguage_Language_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLanguage_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHobby",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    HobbyName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHobby", x => new { x.UserId, x.HobbyName });
                    table.ForeignKey(
                        name: "FK_UserHobby_Hobby_HobbyName",
                        column: x => x.HobbyName,
                        principalTable: "Hobby",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserHobby_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatUser",
                columns: table => new
                {
                    ChatId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    UnreadMessages = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUser", x => new { x.ChatId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ChatUser_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MaxParticipants = table.Column<int>(nullable: false),
                    ChatId = table.Column<Guid>(nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11, 8)", nullable: false),
                    IsFinished = table.Column<bool>(nullable: false),
                    StartTime = table.Column<DateTimeOffset>(nullable: false),
                    EndDate = table.Column<DateTimeOffset>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    LanguageCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Language_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ViewedOn = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: true),
                    ChatId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserEvent",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvent", x => new { x.UserId, x.EventId });
                    table.ForeignKey(
                        name: "FK_UserEvent_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserEvent_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chat_CreatorId",
                table: "Chat",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_UserId",
                table: "ChatUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ChatId",
                table: "Event",
                column: "ChatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_CreatorId",
                table: "Event",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LanguageCode",
                table: "Event",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Hobby_CategoryName",
                table: "Hobby",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatId",
                table: "Message",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvent_EventId",
                table: "UserEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFriends_UserFromId",
                table: "UserFriends",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHobby_HobbyName",
                table: "UserHobby",
                column: "HobbyName");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguage_LanguageCode",
                table: "UserLanguage",
                column: "LanguageCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountToken");

            migrationBuilder.DropTable(
                name: "ChatUser");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "UserEvent");

            migrationBuilder.DropTable(
                name: "UserFriends");

            migrationBuilder.DropTable(
                name: "UserHobby");

            migrationBuilder.DropTable(
                name: "UserLanguage");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Hobby");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
