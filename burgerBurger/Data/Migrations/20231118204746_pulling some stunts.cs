using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class pullingsomestunts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Outline",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InventoryOutline",
                columns: table => new
                {
                    InventoryOutlineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    itemName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    itemDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    calories = table.Column<int>(type: "int", nullable: false),
                    itemCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    itemShelfLife = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryOutline", x => x.InventoryOutlineId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryOutline");

            migrationBuilder.DropColumn(
                name: "Outline",
                table: "Inventory");
        }
    }
}
