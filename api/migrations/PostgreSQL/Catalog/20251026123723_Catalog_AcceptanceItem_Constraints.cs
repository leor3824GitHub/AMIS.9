using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class Catalog_AcceptanceItem_Constraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AcceptanceItems_AcceptanceId",
                schema: "catalog",
                table: "AcceptanceItems");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_AcceptanceId_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems",
                columns: new[] { "AcceptanceId", "PurchaseItemId" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_AcceptanceItems_QtyAccepted_NonNegative",
                schema: "catalog",
                table: "AcceptanceItems",
                sql: "\"QtyAccepted\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AcceptanceItems_AcceptanceId_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AcceptanceItems_QtyAccepted_NonNegative",
                schema: "catalog",
                table: "AcceptanceItems");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_AcceptanceId",
                schema: "catalog",
                table: "AcceptanceItems",
                column: "AcceptanceId");
        }
    }
}
