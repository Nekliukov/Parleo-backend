using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class ChangeLanguagePrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Language_LanguageId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_Language_LanguageId",
                table: "UserLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLanguage",
                table: "UserLanguage");

            migrationBuilder.DropIndex(
                name: "IX_UserLanguage_LanguageId",
                table: "UserLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Language",
                table: "Language");

            migrationBuilder.DropIndex(
                name: "IX_Event_LanguageId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "UserLanguage");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Language");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Language",
                newName: "Image");

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "UserLanguage",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Language",
                type: "varchar(2)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "Event",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLanguage",
                table: "UserLanguage",
                columns: new[] { "UserId", "LanguageCode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Language",
                table: "Language",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguage_LanguageCode",
                table: "UserLanguage",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LanguageCode",
                table: "Event",
                column: "LanguageCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Language_LanguageCode",
                table: "Event",
                column: "LanguageCode",
                principalTable: "Language",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_Language_LanguageCode",
                table: "UserLanguage",
                column: "LanguageCode",
                principalTable: "Language",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Language_LanguageCode",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_Language_LanguageCode",
                table: "UserLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLanguage",
                table: "UserLanguage");

            migrationBuilder.DropIndex(
                name: "IX_UserLanguage_LanguageCode",
                table: "UserLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Language",
                table: "Language");

            migrationBuilder.DropIndex(
                name: "IX_Event_LanguageCode",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "UserLanguage");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Language",
                newName: "Name");

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "UserLanguage",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Language",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLanguage",
                table: "UserLanguage",
                columns: new[] { "UserId", "LanguageId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Language",
                table: "Language",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguage_LanguageId",
                table: "UserLanguage",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LanguageId",
                table: "Event",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Language_LanguageId",
                table: "Event",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_Language_LanguageId",
                table: "UserLanguage",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
