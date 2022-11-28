using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lifeEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderData_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderData",
                table: "OrderData");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderData");

            migrationBuilder.AddColumn<string>(
                name: "OrderDataId",
                table: "OrderDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "OrderData",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderData",
                table: "OrderData",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderDataId",
                table: "OrderDetails",
                column: "OrderDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderData_OrderDataId",
                table: "OrderDetails",
                column: "OrderDataId",
                principalTable: "OrderData",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderData_OrderDataId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderDataId",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderData",
                table: "OrderData");

            migrationBuilder.DropColumn(
                name: "OrderDataId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderData");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderData",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderData",
                table: "OrderData",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderData_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "OrderData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
