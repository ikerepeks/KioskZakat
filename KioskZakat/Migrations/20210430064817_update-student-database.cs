using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KioskZakat.Migrations
{
    public partial class updatestudentdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "time",
                table: "Student");

            migrationBuilder.AlterColumn<string>(
                name: "semester",
                table: "Student",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "noMatric",
                table: "Student",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Student",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "checkoutTime",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "tag",
                table: "Student");

            migrationBuilder.AlterColumn<int>(
                name: "semester",
                table: "Student",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "noMatric",
                table: "Student",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "time",
                table: "Student",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "noMatric");
        }
    }
}
