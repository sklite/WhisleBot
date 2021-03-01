using Microsoft.EntityFrameworkCore.Migrations;

namespace Wbcl.DAL.Migrations
{
    public partial class nameHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistory_Users_UserId",
                schema: "monitor",
                table: "ChatHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatHistory",
                schema: "monitor",
                table: "ChatHistory");

            migrationBuilder.RenameTable(
                name: "ChatHistory",
                schema: "monitor",
                newName: "MessagesLog",
                newSchema: "monitor");

            migrationBuilder.RenameIndex(
                name: "IX_ChatHistory_UserId",
                schema: "monitor",
                table: "MessagesLog",
                newName: "IX_MessagesLog_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesLog",
                schema: "monitor",
                table: "MessagesLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesLog_Users_UserId",
                schema: "monitor",
                table: "MessagesLog",
                column: "UserId",
                principalSchema: "monitor",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesLog_Users_UserId",
                schema: "monitor",
                table: "MessagesLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesLog",
                schema: "monitor",
                table: "MessagesLog");

            migrationBuilder.RenameTable(
                name: "MessagesLog",
                schema: "monitor",
                newName: "ChatHistory",
                newSchema: "monitor");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesLog_UserId",
                schema: "monitor",
                table: "ChatHistory",
                newName: "IX_ChatHistory_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatHistory",
                schema: "monitor",
                table: "ChatHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistory_Users_UserId",
                schema: "monitor",
                table: "ChatHistory",
                column: "UserId",
                principalSchema: "monitor",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
