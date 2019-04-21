using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class AddAboutPropertyToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "User");
        }
    }
}
