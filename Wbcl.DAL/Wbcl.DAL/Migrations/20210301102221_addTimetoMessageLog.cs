using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wbcl.DAL.Migrations
{
    public partial class addTimetoMessageLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Sent",
                schema: "monitor",
                table: "MessagesLog",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sent",
                schema: "monitor",
                table: "MessagesLog");
        }
    }
}
