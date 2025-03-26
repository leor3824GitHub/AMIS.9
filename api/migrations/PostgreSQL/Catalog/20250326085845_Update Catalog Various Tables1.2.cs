using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class UpdateCatalogVariousTables12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssuanceItem_Products_ProductId",
                schema: "catalog",
                table: "IssuanceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssuanceItem",
                schema: "catalog",
                table: "IssuanceItem");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "catalog",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "catalog",
                table: "IssuanceItem");

            migrationBuilder.RenameTable(
                name: "IssuanceItem",
                schema: "catalog",
                newName: "IssuanceItems",
                newSchema: "catalog");

            migrationBuilder.RenameIndex(
                name: "IX_IssuanceItem_ProductId",
                schema: "catalog",
                table: "IssuanceItems",
                newName: "IX_IssuanceItems_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "Qty",
                schema: "catalog",
                table: "PurchaseItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                schema: "catalog",
                table: "Issuances",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Qty",
                schema: "catalog",
                table: "IssuanceItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssuanceItems",
                schema: "catalog",
                table: "IssuanceItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuanceItems_Products_ProductId",
                schema: "catalog",
                table: "IssuanceItems",
                column: "ProductId",
                principalSchema: "catalog",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssuanceItems_Products_ProductId",
                schema: "catalog",
                table: "IssuanceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssuanceItems",
                schema: "catalog",
                table: "IssuanceItems");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.RenameTable(
                name: "IssuanceItems",
                schema: "catalog",
                newName: "IssuanceItem",
                newSchema: "catalog");

            migrationBuilder.RenameIndex(
                name: "IX_IssuanceItems_ProductId",
                schema: "catalog",
                table: "IssuanceItem",
                newName: "IX_IssuanceItem_ProductId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                schema: "catalog",
                table: "PurchaseItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "catalog",
                table: "Issuances",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "catalog",
                table: "Inventories",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                schema: "catalog",
                table: "IssuanceItem",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "catalog",
                table: "IssuanceItem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssuanceItem",
                schema: "catalog",
                table: "IssuanceItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuanceItem_Products_ProductId",
                schema: "catalog",
                table: "IssuanceItem",
                column: "ProductId",
                principalSchema: "catalog",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
