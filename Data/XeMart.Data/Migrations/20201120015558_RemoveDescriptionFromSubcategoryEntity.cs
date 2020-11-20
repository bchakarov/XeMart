using Microsoft.EntityFrameworkCore.Migrations;

namespace XeMart.Data.Migrations
{
    public partial class RemoveDescriptionFromSubcategoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Subcategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Subcategories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
