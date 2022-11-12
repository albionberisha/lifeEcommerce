using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lifeEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_CoverTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CoverId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CoverTypeId",
                table: "Products",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CoverTypeId",
                table: "Products",
                newName: "IX_Products_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Products",
                newName: "CoverTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UnitId",
                table: "Products",
                newName: "IX_Products_CoverTypeId");

            migrationBuilder.AddColumn<int>(
                name: "CoverId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_CoverTypeId",
                table: "Products",
                column: "CoverTypeId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
