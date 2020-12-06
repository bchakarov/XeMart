using Microsoft.EntityFrameworkCore.Migrations;

namespace XeMart.Data.Migrations
{
    public partial class AddStripeIdToOrderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeId",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeId",
                table: "Orders");
        }
    }
}
