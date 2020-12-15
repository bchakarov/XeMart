namespace XeMart.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ModifyUserFavouriteProductEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserFavouriteProducts_IsDeleted",
                table: "UserFavouriteProducts");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserFavouriteProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserFavouriteProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserFavouriteProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserFavouriteProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserFavouriteProducts_IsDeleted",
                table: "UserFavouriteProducts",
                column: "IsDeleted");
        }
    }
}
