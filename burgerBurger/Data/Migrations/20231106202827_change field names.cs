using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class changefieldnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderItem_ItemId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderItem_ItemId",
                table: "OrderDetails",
                column: "ItemId",
                principalTable: "OrderItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderItem_ItemId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderItem_ItemId",
                table: "OrderDetails",
                column: "ItemId",
                principalTable: "OrderItem",
                principalColumn: "Id");
        }
    }
}
