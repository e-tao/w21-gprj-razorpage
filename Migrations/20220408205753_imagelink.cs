using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace w21_gprj_razorpage.Migrations
{
    public partial class imagelink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "Product",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "Product");
        }
    }
}
