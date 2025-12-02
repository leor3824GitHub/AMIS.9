using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddProductEstimatedUsefulLife : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstimatedUsefulLife",
                schema: "catalog",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 12,
                comment: "Estimated useful life in months");

            migrationBuilder.AddColumn<int>(
                name: "PropertyClassification",
                schema: "catalog",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                comment: "1=Consumable, 2=SemiExpendable, 3=PPE");

            migrationBuilder.CreateTable(
                name: "AssetClassificationRules",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RuleName = table.Column<string>(type: "text", nullable: false),
                    Classification = table.Column<int>(type: "integer", nullable: false),
                    MinimumCost = table.Column<decimal>(type: "numeric", nullable: false),
                    MaximumCost = table.Column<decimal>(type: "numeric", nullable: false),
                    MinimumUsefulLifeMonths = table.Column<int>(type: "integer", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RCAAccountCode = table.Column<string>(type: "text", nullable: false),
                    ExpenseAccountCode = table.Column<string>(type: "text", nullable: false),
                    DocumentType = table.Column<string>(type: "text", nullable: false),
                    COAReference = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetClassificationRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsumableInventories",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StockNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WeightedAverageCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ReorderLevel = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
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
                    table.PrimaryKey("PK_ConsumableInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumableInventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalAssets",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyCode = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AcquisitionCost = table.Column<decimal>(type: "numeric", nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    ModelNumber = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Condition = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "text", nullable: false),
                    EstimatedUsefulLife = table.Column<int>(type: "integer", nullable: false),
                    DisposalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DisposalReason = table.Column<string>(type: "text", nullable: true),
                    CurrentClassification = table.Column<int>(type: "integer", nullable: false),
                    PreviousClassification = table.Column<int>(type: "integer", nullable: true),
                    LastReclassificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReclassificationReason = table.Column<string>(type: "text", nullable: true),
                    PPEType = table.Column<string>(type: "text", nullable: true),
                    AccumulatedDepreciation = table.Column<decimal>(type: "numeric", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalAssets_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetAssignmentHistories",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetNumber = table.Column<string>(type: "text", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: false),
                    DocumentNumber = table.Column<string>(type: "text", nullable: false),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    AssignmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignmentType = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    AssetClassification = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    TransferredToEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransferredToDocumentNumber = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Condition = table.Column<string>(type: "text", nullable: true),
                    AcceptedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    AcceptanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetAssignmentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetAssignmentHistories_Employees_AcceptedBy",
                        column: x => x.AcceptedBy,
                        principalSchema: "catalog",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignmentHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "catalog",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignmentHistories_Employees_TransferredToEmployeeId",
                        column: x => x.TransferredToEmployeeId,
                        principalSchema: "catalog",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignmentHistories_PhysicalAssets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "catalog",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetReclassificationHistories",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetNumber = table.Column<string>(type: "text", nullable: false),
                    OldClassification = table.Column<int>(type: "integer", nullable: false),
                    NewClassification = table.Column<int>(type: "integer", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    AcquisitionCostAtReclassification = table.Column<decimal>(type: "numeric", nullable: false),
                    COAReference = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetReclassificationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetReclassificationHistories_PhysicalAssets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "catalog",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_PropertyClassification",
                schema: "catalog",
                table: "Products",
                column: "PropertyClassification");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignmentHistories_AcceptedBy",
                schema: "catalog",
                table: "AssetAssignmentHistories",
                column: "AcceptedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignmentHistories_AssetId",
                schema: "catalog",
                table: "AssetAssignmentHistories",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignmentHistories_EmployeeId",
                schema: "catalog",
                table: "AssetAssignmentHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignmentHistories_TransferredToEmployeeId",
                schema: "catalog",
                table: "AssetAssignmentHistories",
                column: "TransferredToEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetReclassificationHistories_AssetId",
                schema: "catalog",
                table: "AssetReclassificationHistories",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumableInventories_ProductId",
                schema: "catalog",
                table: "ConsumableInventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumableInventories_StockNumber",
                schema: "catalog",
                table: "ConsumableInventories",
                column: "StockNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_ProductId",
                schema: "catalog",
                table: "PhysicalAssets",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetAssignmentHistories",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "AssetClassificationRules",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "AssetReclassificationHistories",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "ConsumableInventories",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "PhysicalAssets",
                schema: "catalog");

            migrationBuilder.DropIndex(
                name: "IX_Products_PropertyClassification",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EstimatedUsefulLife",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PropertyClassification",
                schema: "catalog",
                table: "Products");
        }
    }
}
