using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class customerIdoncustomitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "OrderItem",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OrderItem");
        }
    }
}
