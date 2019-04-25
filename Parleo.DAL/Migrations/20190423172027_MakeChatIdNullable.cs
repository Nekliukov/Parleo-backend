using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class MakeChatIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Chat_ChatId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_ChatId",
                table: "Event");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChatId",
                table: "Event",
                nullable: true,
                oldClrType: typeof(Guid));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Chat_ChatId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_ChatId",
                table: "Event");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChatId",
                table: "Event",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_ChatId",
                table: "Event",
                column: "ChatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Chat_ChatId",
                table: "Event",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
