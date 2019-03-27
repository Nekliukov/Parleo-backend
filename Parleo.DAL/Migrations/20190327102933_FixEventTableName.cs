using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class FixEventTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_User_CreatorId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Language_LanguageId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvent_Events_EventId",
                table: "UserEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameIndex(
                name: "IX_Events_LanguageId",
                table: "Event",
                newName: "IX_Event_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_CreatorId",
                table: "Event",
                newName: "IX_Event_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_User_CreatorId",
                table: "Event",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Language_LanguageId",
                table: "Event",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvent_Event_EventId",
                table: "UserEvent",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_User_CreatorId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Language_LanguageId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvent_Event_EventId",
                table: "UserEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_Event_LanguageId",
                table: "Events",
                newName: "IX_Events_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_Event_CreatorId",
                table: "Events",
                newName: "IX_Events_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_User_CreatorId",
                table: "Events",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Language_LanguageId",
                table: "Events",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvent_Events_EventId",
                table: "UserEvent",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
