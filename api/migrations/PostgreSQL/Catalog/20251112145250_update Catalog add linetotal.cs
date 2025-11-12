using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class updateCatalogaddlinetotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionItems_Inspections_InspectionId1",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItems_Purchases_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems");

            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_InspectionId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_InspectionId1",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.DropColumn(
                name: "InspectionId1",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_InspectionId_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems",
                columns: new[] { "InspectionId", "PurchaseItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems",
                column: "PurchaseItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItems_Purchases_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems",
                column: "PurchaseId",
                principalSchema: "catalog",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItems_Purchases_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems");

            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_InspectionId_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.AddColumn<Guid>(
                name: "InspectionId1",
                schema: "catalog",
                table: "InspectionItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_InspectionId",
                schema: "catalog",
                table: "InspectionItems",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_InspectionId1",
                schema: "catalog",
                table: "InspectionItems",
                column: "InspectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems",
                column: "PurchaseItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionItems_Inspections_InspectionId1",
                schema: "catalog",
                table: "InspectionItems",
                column: "InspectionId1",
                principalSchema: "catalog",
                principalTable: "Inspections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItems_Purchases_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems",
                column: "PurchaseId",
                principalSchema: "catalog",
                principalTable: "Purchases",
                principalColumn: "Id");
        }
    }
}
