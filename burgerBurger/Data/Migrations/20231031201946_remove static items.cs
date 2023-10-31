using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class removestaticitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_StaticItem_StaticItemId",
                table: "Inventory");

            migrationBuilder.DropTable(
                name: "StaticItem");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_StaticItemId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "StaticItemId",
                table: "Inventory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaticItemId",
                table: "Inventory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StaticItem",
                columns: table => new
                {
                    StaticItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Calories = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
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
    }
}
