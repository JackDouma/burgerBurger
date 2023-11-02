using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class inheritance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropPrimaryKey(
                name: "PK_StaticItem",
                table: "StaticItem");

            migrationBuilder.RenameTable(
                name: "StaticItem",
                newName: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "StaticItemId",
                table: "Inventory",
                newName: "OrderItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventory_StaticItemId",
                table: "Inventory",
                newName: "IX_Inventory_OrderItemId");

            migrationBuilder.RenameColumn(
                name: "StaticItemId",
                table: "OrderItem",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "OrderItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OrderItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "OrderItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_OrderItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ItemId",
                table: "CartItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_OrderItem_OrderItemId",
                table: "Inventory",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_OrderItem_OrderItemId",
                table: "Inventory");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "OrderItem");

            migrationBuilder.RenameTable(
                name: "OrderItem",
                newName: "StaticItem");

            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "Inventory",
                newName: "StaticItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventory_OrderItemId",
                table: "Inventory",
                newName: "IX_Inventory_StaticItemId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StaticItem",
                newName: "StaticItemId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "StaticItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StaticItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StaticItem",
                table: "StaticItem",
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
