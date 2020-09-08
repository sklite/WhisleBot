using Microsoft.EntityFrameworkCore.Migrations;

namespace WhisleBotConsole.Migrations
{
    public partial class userTableAddGroupName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentGroupName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "Preferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentGroupName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "Preferences");
        }
    }
}
