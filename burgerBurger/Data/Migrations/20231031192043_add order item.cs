using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class addorderitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaticItemId",
                table: "Inventory",
                type: "int",
                nullable: true);

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

            migrationBuilder.DropIndex(
                name: "IX_Inventory_StaticItemId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "StaticItemId",
                table: "Inventory");
        }
    }
}
