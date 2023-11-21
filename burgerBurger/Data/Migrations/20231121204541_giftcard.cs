using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class giftcard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemInventory_CustomItem_CustomItemId",
                table: "ItemInventory");

            migrationBuilder.DropTable(
                name: "CustomItem");

            migrationBuilder.DropIndex(
                name: "IX_ItemInventory_CustomItemId",
                table: "ItemInventory");

            migrationBuilder.DropColumn(
                name: "CustomItemId",
                table: "ItemInventory");

            migrationBuilder.CreateTable(
                name: "GiftCards",
                columns: table => new
                {
                    GiftCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    code = table.Column<int>(type: "int", nullable: false),
                    redeemed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftCards", x => x.GiftCardId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiftCards");

            migrationBuilder.AddColumn<int>(
                name: "CustomItemId",
                table: "ItemInventory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomItem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemInventory_CustomItemId",
                table: "ItemInventory",
                column: "CustomItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemInventory_CustomItem_CustomItemId",
                table: "ItemInventory",
                column: "CustomItemId",
                principalTable: "CustomItem",
                principalColumn: "Id");
        }
    }
}
