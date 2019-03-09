using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parleo.DAL.Migrations
{
    public partial class AddDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_language",
                columns: table => new
                {
                    cln_id = table.Column<Guid>(nullable: false),
                    cln_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_language", x => x.cln_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_info",
                columns: table => new
                {
                    cln_id = table.Column<Guid>(nullable: false),
                    cln_firstname = table.Column<string>(nullable: true),
                    cln_lastname = table.Column<string>(nullable: true),
                    cln_birth_date = table.Column<DateTime>(type: "Date", nullable: false),
                    cln_gender = table.Column<bool>(nullable: false),
                    cln_latitude = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    cln_longitude = table.Column<decimal>(type: "decimal(11, 8)", nullable: false),
                    cln_created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_info", x => x.cln_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_event",
                columns: table => new
                {
                    cln_id = table.Column<Guid>(nullable: false),
                    cln_name = table.Column<string>(nullable: true),
                    cln_description = table.Column<string>(nullable: true),
                    cln_max_participants = table.Column<int>(nullable: false),
                    cln_latitude = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    cln_longitude = table.Column<decimal>(type: "decimal(11, 8)", nullable: false),
                    cln_is_finished = table.Column<bool>(nullable: false),
                    cln_start_time = table.Column<DateTime>(nullable: false),
                    cln_end_date = table.Column<DateTime>(nullable: true),
                    cln_creator_id = table.Column<Guid>(nullable: false),
                    cln_language_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_event", x => x.cln_id);
                    table.ForeignKey(
                        name: "FK_tbl_event_tbl_user_info_cln_creator_id",
                        column: x => x.cln_creator_id,
                        principalTable: "tbl_user_info",
                        principalColumn: "cln_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_event_tbl_language_cln_language_id",
                        column: x => x.cln_language_id,
                        principalTable: "tbl_language",
                        principalColumn: "cln_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_auth",
                columns: table => new
                {
                    cln_user_info_id = table.Column<Guid>(nullable: false),
                    cln_email = table.Column<string>(nullable: true),
                    cln_password_hash = table.Column<byte[]>(nullable: true),
                    cln_password_salt = table.Column<byte[]>(nullable: true),
                    cln_last_login = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_auth", x => x.cln_user_info_id);
                    table.ForeignKey(
                        name: "FK_tbl_user_auth_tbl_user_info_cln_user_info_id",
                        column: x => x.cln_user_info_id,
                        principalTable: "tbl_user_info",
                        principalColumn: "cln_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_friends",
                columns: table => new
                {
                    cln_user_to = table.Column<Guid>(nullable: false),
                    cln_user_from = table.Column<Guid>(nullable: false),
                    cln_status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_friends", x => new { x.cln_user_from, x.cln_user_to });
                    table.ForeignKey(
                        name: "FK_tbl_user_friends_tbl_user_info_cln_user_from",
                        column: x => x.cln_user_from,
                        principalTable: "tbl_user_info",
                        principalColumn: "cln_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_user_friends_tbl_user_info_cln_user_to",
                        column: x => x.cln_user_to,
                        principalTable: "tbl_user_info",
                        principalColumn: "cln_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_language",
                columns: table => new
                {
                    cln_user_id = table.Column<Guid>(nullable: false),
                    cln_language_id = table.Column<Guid>(nullable: false),
                    cln_level = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_language", x => new { x.cln_user_id, x.cln_language_id });
                    table.ForeignKey(
                        name: "FK_tbl_user_language_tbl_language_cln_language_id",
                        column: x => x.cln_language_id,
                        principalTable: "tbl_language",
                        principalColumn: "cln_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_user_language_tbl_user_info_cln_user_id",
                        column: x => x.cln_user_id,
                        principalTable: "tbl_user_info",
                        principalColumn: "cln_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_event_cln_creator_id",
                table: "tbl_event",
                column: "cln_creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_event_cln_language_id",
                table: "tbl_event",
                column: "cln_language_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_friends_cln_user_to",
                table: "tbl_user_friends",
                column: "cln_user_to");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_language_cln_language_id",
                table: "tbl_user_language",
                column: "cln_language_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_event");

            migrationBuilder.DropTable(
                name: "tbl_user_auth");

            migrationBuilder.DropTable(
                name: "tbl_user_friends");

            migrationBuilder.DropTable(
                name: "tbl_user_language");

            migrationBuilder.DropTable(
                name: "tbl_language");

            migrationBuilder.DropTable(
                name: "tbl_user_info");
        }
    }
}
