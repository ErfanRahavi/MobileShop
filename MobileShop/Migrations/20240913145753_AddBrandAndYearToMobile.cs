using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileShop.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandAndYearToMobile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Mobiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Mobiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Mobiles");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Mobiles");
        }
    }
}
