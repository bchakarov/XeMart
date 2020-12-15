namespace XeMart.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddRelationBetweenSupplierAndOrderProductEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "OrderProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_SupplierId",
                table: "OrderProducts",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Suppliers_SupplierId",
                table: "OrderProducts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Suppliers_SupplierId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_SupplierId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "OrderProducts");
        }
    }
}
