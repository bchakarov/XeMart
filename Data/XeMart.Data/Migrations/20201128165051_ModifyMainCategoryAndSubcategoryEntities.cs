using Microsoft.EntityFrameworkCore.Migrations;

namespace XeMart.Data.Migrations
{
    public partial class ModifyMainCategoryAndSubcategoryEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MainCategories");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Subcategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Subcategories");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MainCategories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
