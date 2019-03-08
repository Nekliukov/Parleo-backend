using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class FixUserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cln_firstname",
                table: "tbl_user_info");

            migrationBuilder.RenameColumn(
                name: "cln_lastname",
                table: "tbl_user_info",
                newName: "cln_name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cln_name",
                table: "tbl_user_info",
                newName: "cln_lastname");

            migrationBuilder.AddColumn<string>(
                name: "cln_firstname",
                table: "tbl_user_info",
                nullable: true);
        }
    }
}
