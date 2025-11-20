using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTodoListComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "todo_list_comment",
                columns: table => new
                {
                    tlc_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tlc_text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    tlc_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    u_id = table.Column<string>(type: "text", nullable: false),
                    tl_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo_list_comment", x => x.tlc_id);
                    table.ForeignKey(
                        name: "FK_todo_list_comment_AspNetUsers_u_id",
                        column: x => x.u_id,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_todo_list_comment_todo_lists_tl_id",
                        column: x => x.tl_id,
                        principalTable: "todo_lists",
                        principalColumn: "td_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_todo_list_comment_tl_id",
                table: "todo_list_comment",
                column: "tl_id");

            migrationBuilder.CreateIndex(
                name: "IX_todo_list_comment_u_id",
                table: "todo_list_comment",
                column: "u_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "todo_list_comment");
        }
    }
}
