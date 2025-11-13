using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class Catalog_AutoStatusHandlers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.DropIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "catalog",
                table: "Purchases",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                schema: "catalog",
                table: "Purchases",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                schema: "catalog",
                table: "Purchases",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QtyPassed",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QtyInspected",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QtyFailed",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QtyAccepted",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemStatus",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InspectionStatus",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AcceptanceStatus",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InspectionItemStatus",
                schema: "catalog",
                table: "InspectionItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceItems_IssuanceId",
                schema: "catalog",
                table: "IssuanceItems",
                column: "IssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems",
                column: "PurchaseItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Acceptances_InspectionId",
                schema: "catalog",
                table: "Acceptances",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems",
                column: "PurchaseItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Acceptances_Inspections_InspectionId",
                schema: "catalog",
                table: "Acceptances",
                column: "InspectionId",
                principalSchema: "catalog",
                principalTable: "Inspections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuanceItems_Issuances_IssuanceId",
                schema: "catalog",
                table: "IssuanceItems",
                column: "IssuanceId",
                principalSchema: "catalog",
                principalTable: "Issuances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acceptances_Inspections_InspectionId",
                schema: "catalog",
                table: "Acceptances");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuanceItems_Issuances_IssuanceId",
                schema: "catalog",
                table: "IssuanceItems");

            migrationBuilder.DropIndex(
                name: "IX_IssuanceItems_IssuanceId",
                schema: "catalog",
                table: "IssuanceItems");

            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.DropIndex(
                name: "IX_Acceptances_InspectionId",
                schema: "catalog",
                table: "Acceptances");

            migrationBuilder.DropIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Remarks",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "catalog",
                table: "Purchases",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "QtyPassed",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "QtyInspected",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "QtyFailed",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "QtyAccepted",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ItemStatus",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "InspectionStatus",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AcceptanceStatus",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "InspectionItemStatus",
                schema: "catalog",
                table: "InspectionItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems",
                column: "PurchaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems",
                column: "PurchaseItemId");
        }
    }
}
