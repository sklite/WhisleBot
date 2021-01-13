using Microsoft.EntityFrameworkCore.Migrations;

namespace WhisleBotConsole.Migrations
{
    public partial class UserTagets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "CurrentGroupId",
            //    table: "Users",
            //    newName: "CurrentTargetId");

            //migrationBuilder.RenameColumn(
            //    name: "CurrentGroupName",
            //    table: "Users",
            //    newName: "CurrentTargetName");

            //migrationBuilder.RenameColumn(
            //    name: "GroupId",
            //    table: "Preferences",
            //    newName: "TargetId"
            //    );

            //migrationBuilder.RenameColumn(
            //    name: "GroupName",
            //    table: "Preferences",
            //    newName: "TargetName");

            //migrationBuilder.AddColumn<int>(
            //    name: "CurrentTargetType",
            //    table: "Users",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "TargetType",
            //    table: "Preferences",
            //    nullable: false,
            //    defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTargetId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentTargetName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentTargetType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Preferences");

            migrationBuilder.DropColumn(
                name: "TargetName",
                table: "Preferences");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "Preferences");

            migrationBuilder.AddColumn<long>(
                name: "CurrentGroupId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentGroupName",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "Preferences",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "Preferences",
                type: "TEXT",
                nullable: true);
        }
    }
}
