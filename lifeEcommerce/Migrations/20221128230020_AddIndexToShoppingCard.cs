using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lifeEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToShoppingCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingCards_UserId",
                table: "ShoppingCards");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCards_UserId_ProductId",
                table: "ShoppingCards",
                columns: new[] { "UserId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingCards_UserId_ProductId",
                table: "ShoppingCards");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCards_UserId",
                table: "ShoppingCards",
                column: "UserId");
        }
    }
}
