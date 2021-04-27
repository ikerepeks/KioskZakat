using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KioskZakat.Migrations
{
    public partial class editstudentdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "checkoutTime",
                table: "Student",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "tag",
                table: "Student",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "checkoutTime",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "tag",
                table: "Student");
        }
    }
}
