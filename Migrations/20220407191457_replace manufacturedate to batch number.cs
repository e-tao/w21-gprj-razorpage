using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace w21_gprj_razorpage.Migrations
{
    public partial class replacemanufacturedatetobatchnumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManufactureDate",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                table: "Product",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchNumber",
                table: "Product");

            migrationBuilder.AddColumn<DateTime>(
                name: "ManufactureDate",
                table: "Product",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
