using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class FixUserPasswordHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "UserAuth");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "UserAuth",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "UserAuth",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "UserAuth");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "UserAuth");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "UserAuth",
                nullable: true);
        }
    }
}
