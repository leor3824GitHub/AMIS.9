using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class UpdateCatalogVariousTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issuances_Products_ProductId",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItems_Purchases_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseItems_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems");

            migrationBuilder.DropIndex(
                name: "IX_Issuances_ProductId",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "Qty",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "catalog",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                schema: "catalog",
                table: "Issuances",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssuanceDate",
                schema: "catalog",
                table: "Issuances",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "catalog",
                table: "Issuances",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Qty",
                schema: "catalog",
                table: "Inventories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "catalog",
                table: "Inventories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IssuanceItem",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuanceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuanceItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceItem_ProductId",
                schema: "catalog",
                table: "IssuanceItem",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuanceItem",
                schema: "catalog");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IssuanceDate",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "catalog",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                schema: "catalog",
                table: "Issuances",
                newName: "UnitPrice");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "catalog",
                table: "Issuances",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Qty",
                schema: "catalog",
                table: "Issuances",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "catalog",
                table: "Issuances",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                schema: "catalog",
                table: "Inventories",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "catalog",
                table: "Inventories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_PurchaseId",
                schema: "catalog",
                table: "PurchaseItems",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Issuances_ProductId",
                schema: "catalog",
                table: "Issuances",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issuances_Products_ProductId",
                schema: "catalog",
                table: "Issuances",
                column: "ProductId",
                principalSchema: "catalog",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
