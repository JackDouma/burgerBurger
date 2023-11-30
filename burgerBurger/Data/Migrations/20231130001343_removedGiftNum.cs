using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace burgerBurger.Data.Migrations
{
    public partial class removedGiftNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "giftPhoneNumber",
                table: "GiftCards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "giftPhoneNumber",
                table: "GiftCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
