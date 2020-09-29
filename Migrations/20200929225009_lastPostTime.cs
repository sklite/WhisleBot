using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WhisleBotConsole.Migrations
{
    public partial class lastPostTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastNotifiedPostTime",
                table: "Preferences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastNotifiedPostTime",
                table: "Preferences");
        }
    }
}
