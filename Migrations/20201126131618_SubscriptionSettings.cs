using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WhisleBotConsole.Migrations
{
    public partial class SubscriptionSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndOfAdvancedSubscription",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionStatus",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndOfAdvancedSubscription",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionStatus",
                table: "Users");
        }
    }
}
