using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class Catalog_SingleShot_UniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // AcceptanceItems: make PurchaseItemId index unique
            migrationBuilder.DropIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems",
                column: "PurchaseItemId",
                unique: true);

            // InspectionItems: make PurchaseItemId index unique
            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems",
                column: "PurchaseItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // AcceptanceItems: revert to non-unique index
            migrationBuilder.DropIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems",
                column: "PurchaseItemId");

            // InspectionItems: revert to non-unique index
            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems",
                column: "PurchaseItemId");
        }
    }
}
