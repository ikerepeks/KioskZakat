using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KioskZakat.Migrations
{
    public partial class UpdatePurchaseItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cardNum",
                table: "Purchase");

            migrationBuilder.DropColumn(
                name: "details",
                table: "Purchase");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Purchase");

            migrationBuilder.DropColumn(
                name: "imgPath",
                table: "Item");

            migrationBuilder.AddColumn<string>(
                name: "couponNum",
                table: "Purchase",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "validUntil",
                table: "Purchase",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "couponNum",
                table: "Purchase");

            migrationBuilder.DropColumn(
                name: "validUntil",
                table: "Purchase");

            migrationBuilder.AddColumn<string>(
                name: "cardNum",
                table: "Purchase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "details",
                table: "Purchase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "price",
                table: "Purchase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imgPath",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
