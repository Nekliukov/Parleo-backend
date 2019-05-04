using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class FixChatRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Chat_ChatId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_ChatId",
                table: "Event");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Chat",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chat_EventId",
                table: "Chat",
                column: "EventId",
                unique: true,
                filter: "[EventId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_Event_EventId",
                table: "Chat",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_Event_EventId",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_EventId",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Chat");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ChatId",
                table: "Event",
                column: "ChatId",
                unique: true,
                filter: "[ChatId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Chat_ChatId",
                table: "Event",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
