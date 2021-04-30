using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KioskZakat.Migrations
{
    public partial class studentdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    noMatric = table.Column<string>(nullable: false),
                    nama = table.Column<string>(nullable: true),
                    noBilik = table.Column<string>(nullable: true),
                    kodProgram = table.Column<string>(nullable: true),
                    semester = table.Column<int>(nullable: false),
                    checkout = table.Column<bool>(nullable: false),
                    kunci = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.noMatric);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}
