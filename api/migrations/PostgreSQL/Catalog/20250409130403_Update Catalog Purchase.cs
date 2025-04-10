using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class UpdateCatalogPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItems_Purchases_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems",
                column: "PurchaseId",
                principalSchema: "catalog",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItems_Purchases_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseItems_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems");
        }
    }
}
