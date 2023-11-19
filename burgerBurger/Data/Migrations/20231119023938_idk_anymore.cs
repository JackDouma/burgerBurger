using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class idk_anymore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_OrderItem_OrderItemId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_OrderItemId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "Inventory");

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "InventoryOutline",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "itemName",
                table: "Inventory",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "itemDescription",
                table: "Inventory",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryOutline_OrderItemId",
                table: "InventoryOutline",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryOutline_OrderItem_OrderItemId",
                table: "InventoryOutline",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryOutline_OrderItem_OrderItemId",
                table: "InventoryOutline");

            migrationBuilder.DropIndex(
                name: "IX_InventoryOutline_OrderItemId",
                table: "InventoryOutline");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "InventoryOutline");

            migrationBuilder.AlterColumn<string>(
                name: "itemName",
                table: "Inventory",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "itemDescription",
                table: "Inventory",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "Inventory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_OrderItemId",
                table: "Inventory",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_OrderItem_OrderItemId",
                table: "Inventory",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id");
        }
    }
}
