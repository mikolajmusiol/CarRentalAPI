using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class PriceDependencyChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Prices_PriceId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_PriceId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_CarId",
                table: "Prices",
                column: "CarId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Cars_CarId",
                table: "Prices",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Cars_CarId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_CarId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Prices");

            migrationBuilder.AddColumn<int>(
                name: "PriceId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_PriceId",
                table: "Cars",
                column: "PriceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Prices_PriceId",
                table: "Cars",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
