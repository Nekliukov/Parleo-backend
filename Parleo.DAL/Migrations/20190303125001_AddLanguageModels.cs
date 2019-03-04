using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class AddLanguageModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "UserInfo",
                type: "decimal(11, 8)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "UserInfo",
                type: "decimal(10, 2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserInfo",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserInfo",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid));

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLanguage",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    Level = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguage", x => new { x.UserId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_UserLanguage_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLanguage_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguage_LanguageId",
                table: "UserLanguage",
                column: "LanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLanguage");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "UserInfo",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11, 8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "UserInfo",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserInfo",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserInfo",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "NEWID()");
        }
    }
}
