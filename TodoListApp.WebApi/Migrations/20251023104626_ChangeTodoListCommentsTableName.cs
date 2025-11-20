using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTodoListCommentsTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todo_list_comment_AspNetUsers_u_id",
                table: "todo_list_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_todo_list_comment_todo_lists_tl_id",
                table: "todo_list_comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_todo_list_comment",
                table: "todo_list_comment");

            migrationBuilder.RenameTable(
                name: "todo_list_comment",
                newName: "todo_list_comments");

            migrationBuilder.RenameIndex(
                name: "IX_todo_list_comment_u_id",
                table: "todo_list_comments",
                newName: "IX_todo_list_comments_u_id");

            migrationBuilder.RenameIndex(
                name: "IX_todo_list_comment_tl_id",
                table: "todo_list_comments",
                newName: "IX_todo_list_comments_tl_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_todo_list_comments",
                table: "todo_list_comments",
                column: "tlc_id");

            migrationBuilder.AddForeignKey(
                name: "FK_todo_list_comments_AspNetUsers_u_id",
                table: "todo_list_comments",
                column: "u_id",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_todo_list_comments_todo_lists_tl_id",
                table: "todo_list_comments",
                column: "tl_id",
                principalTable: "todo_lists",
                principalColumn: "td_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todo_list_comments_AspNetUsers_u_id",
                table: "todo_list_comments");

            migrationBuilder.DropForeignKey(
                name: "FK_todo_list_comments_todo_lists_tl_id",
                table: "todo_list_comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_todo_list_comments",
                table: "todo_list_comments");

            migrationBuilder.RenameTable(
                name: "todo_list_comments",
                newName: "todo_list_comment");

            migrationBuilder.RenameIndex(
                name: "IX_todo_list_comments_u_id",
                table: "todo_list_comment",
                newName: "IX_todo_list_comment_u_id");

            migrationBuilder.RenameIndex(
                name: "IX_todo_list_comments_tl_id",
                table: "todo_list_comment",
                newName: "IX_todo_list_comment_tl_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_todo_list_comment",
                table: "todo_list_comment",
                column: "tlc_id");

            migrationBuilder.AddForeignKey(
                name: "FK_todo_list_comment_AspNetUsers_u_id",
                table: "todo_list_comment",
                column: "u_id",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_todo_list_comment_todo_lists_tl_id",
                table: "todo_list_comment",
                column: "tl_id",
                principalTable: "todo_lists",
                principalColumn: "td_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
