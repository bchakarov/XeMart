using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XeMart.Data.Migrations
{
    public partial class ModifyUserProductReviewEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProductReviews_IsDeleted",
                table: "UserProductReviews");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserProductReviews");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserProductReviews");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserProductReviews",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserProductReviews");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserProductReviews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserProductReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserProductReviews_IsDeleted",
                table: "UserProductReviews",
                column: "IsDeleted");
        }
    }
}
