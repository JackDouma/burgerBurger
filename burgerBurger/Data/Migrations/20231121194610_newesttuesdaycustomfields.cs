using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class newesttuesdaycustomfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomItemId",
                table: "ItemInventory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "AspNetUsers",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "balance",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true,
                defaultValue: 0.00m);

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "locationIdentifier",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "province",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "balance",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "city",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "locationIdentifier",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "province",
                table: "AspNetUsers");
        }
    }
}
