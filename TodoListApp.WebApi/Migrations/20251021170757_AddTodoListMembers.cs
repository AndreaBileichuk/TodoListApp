using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTodoListMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todo_lists_AspNetUsers_u_id",
                table: "todo_lists");

            migrationBuilder.DropIndex(
                name: "IX_todo_lists_u_id",
                table: "todo_lists");

            migrationBuilder.DropColumn(
                name: "u_id",
                table: "todo_lists");

            migrationBuilder.CreateTable(
                name: "todo_lists_members",
                columns: table => new
                {
                    u_id = table.Column<string>(type: "text", nullable: false),
                    td_id = table.Column<int>(type: "integer", nullable: false),
                    tlm_role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo_lists_members", x => new { x.u_id, x.td_id });
                    table.ForeignKey(
                        name: "FK_todo_lists_members_AspNetUsers_u_id",
                        column: x => x.u_id,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_todo_lists_members_todo_lists_td_id",
                        column: x => x.td_id,
                        principalTable: "todo_lists",
                        principalColumn: "td_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_todo_lists_members_td_id",
                table: "todo_lists_members",
                column: "td_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "todo_lists_members");

            migrationBuilder.AddColumn<string>(
                name: "u_id",
                table: "todo_lists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_todo_lists_u_id",
                table: "todo_lists",
                column: "u_id");

            migrationBuilder.AddForeignKey(
                name: "FK_todo_lists_AspNetUsers_u_id",
                table: "todo_lists",
                column: "u_id",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
