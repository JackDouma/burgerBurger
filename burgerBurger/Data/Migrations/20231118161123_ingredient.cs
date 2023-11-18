using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class ingredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ingredient",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ingredient",
                table: "Inventory");
        }
    }
}
