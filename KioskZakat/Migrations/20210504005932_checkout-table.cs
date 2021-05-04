using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KioskZakat.Migrations
{
    public partial class checkouttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "Checkout",
                columns: table => new
                {
                    noMatric = table.Column<string>(nullable: false),
                    nama = table.Column<string>(nullable: true),
                    noBilik = table.Column<string>(nullable: true),
                    kodProgram = table.Column<string>(nullable: true),
                    semester = table.Column<string>(nullable: true),
                    checkout = table.Column<DateTime>(nullable: false),
                    kunci = table.Column<bool>(nullable: false),
                    tag = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkout", x => x.noMatric);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checkout");
        }
    }
}
