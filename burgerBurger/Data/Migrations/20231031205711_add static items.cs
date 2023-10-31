using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class addstaticitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaticItemId",
                table: "Inventory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemInventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticItem",
                columns: table => new
                {
                    StaticItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticItem", x => x.StaticItemId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_StaticItemId",
                table: "Inventory",
                column: "StaticItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_StaticItem_StaticItemId",
                table: "Inventory",
                column: "StaticItemId",
                principalTable: "StaticItem",
                principalColumn: "StaticItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_StaticItem_StaticItemId",
                table: "Inventory");

            migrationBuilder.DropTable(
                name: "ItemInventory");

            migrationBuilder.DropTable(
                name: "StaticItem");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_StaticItemId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "StaticItemId",
                table: "Inventory");
        }
    }
}
