using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddUIWorkflowEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new columns to PhysicalAssets table
            migrationBuilder.AddColumn<string>(
                name: "QRCodeData",
                schema: "catalog",
                table: "PhysicalAssets",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyNumber",
                schema: "catalog",
                table: "PhysicalAssets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QRGeneratedDate",
                schema: "catalog",
                table: "PhysicalAssets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentCustodianId",
                schema: "catalog",
                table: "PhysicalAssets",
                type: "uuid",
                nullable: true);

            // Add new columns to Issuances table
            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "catalog",
                table: "Issuances",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CustodianId",
                schema: "catalog",
                table: "Issuances",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "catalog",
                table: "Issuances",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedOn",
                schema: "catalog",
                table: "Issuances",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                schema: "catalog",
                table: "Issuances",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptanceSignature_SignatureData",
                schema: "catalog",
                table: "Issuances",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptanceSignature_SignedOn",
                schema: "catalog",
                table: "Issuances",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AcceptanceSignature_SignedByEmployeeId",
                schema: "catalog",
                table: "Issuances",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptanceSignature_IpAddress",
                schema: "catalog",
                table: "Issuances",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptanceSignature_UserAgent",
                schema: "catalog",
                table: "Issuances",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptanceSignature_DeviceFingerprint",
                schema: "catalog",
                table: "Issuances",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            // Update Acceptance Status column to support new enum values
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "catalog",
                table: "Acceptances",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            // Create AssetRequisitions table
            migrationBuilder.CreateTable(
                name: "AssetRequisitions",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequisitionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AcceptanceSignature_SignatureData = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    AcceptanceSignature_SignedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AcceptanceSignature_SignedByEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    AcceptanceSignature_IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    AcceptanceSignature_UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AcceptanceSignature_DeviceFingerprint = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRequisitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetRequisitions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "catalog",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetRequisitions_Issuances_IssuanceId",
                        column: x => x.IssuanceId,
                        principalSchema: "catalog",
                        principalTable: "Issuances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create DepreciationSchedules table
            migrationBuilder.CreateTable(
                name: "JournalEntryVouchers",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VoucherNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    VoucherDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    DepreciationMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalDebitAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCreditAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExportedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExportFormat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ExportFileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryVouchers", x => x.Id);
                });

            // Create DepreciationSchedules table
            migrationBuilder.CreateTable(
                name: "DepreciationSchedules",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhysicalAssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    MonthlyDepreciationAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AccumulatedDepreciationAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    JournalEntryVoucherId = table.Column<Guid>(type: "uuid", nullable: true),
                    PostedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepreciationSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepreciationSchedules_JournalEntryVouchers_JournalEntryVoucherId",
                        column: x => x.JournalEntryVoucherId,
                        principalSchema: "catalog",
                        principalTable: "JournalEntryVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepreciationSchedules_PhysicalAssets_PhysicalAssetId",
                        column: x => x.PhysicalAssetId,
                        principalSchema: "catalog",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create JournalEntries table
            migrationBuilder.CreateTable(
                name: "JournalEntries",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JournalEntryVoucherId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    AccountCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    DebitAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreditAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_JournalEntryVouchers_JournalEntryVoucherId",
                        column: x => x.JournalEntryVoucherId,
                        principalSchema: "catalog",
                        principalTable: "JournalEntryVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_PropertyNumber",
                schema: "catalog",
                table: "PhysicalAssets",
                column: "PropertyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_CurrentCustodianId",
                schema: "catalog",
                table: "PhysicalAssets",
                column: "CurrentCustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_Issuances_Status",
                schema: "catalog",
                table: "Issuances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Issuances_CustodianId",
                schema: "catalog",
                table: "Issuances",
                column: "CustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequisitions_Status",
                schema: "catalog",
                table: "AssetRequisitions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequisitions_ExpirationDate",
                schema: "catalog",
                table: "AssetRequisitions",
                column: "ExpirationDate");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequisitions_EmployeeId",
                schema: "catalog",
                table: "AssetRequisitions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequisitions_IssuanceId",
                schema: "catalog",
                table: "AssetRequisitions",
                column: "IssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryVouchers_VoucherNumber",
                schema: "catalog",
                table: "JournalEntryVouchers",
                column: "VoucherNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryVouchers_Year_Month",
                schema: "catalog",
                table: "JournalEntryVouchers",
                columns: new[] { "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryVouchers_Status",
                schema: "catalog",
                table: "JournalEntryVouchers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationSchedules_PhysicalAssetId_Year_Month",
                schema: "catalog",
                table: "DepreciationSchedules",
                columns: new[] { "PhysicalAssetId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationSchedules_Status",
                schema: "catalog",
                table: "DepreciationSchedules",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationSchedules_JournalEntryVoucherId",
                schema: "catalog",
                table: "DepreciationSchedules",
                column: "JournalEntryVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_JournalEntryVoucherId_LineNumber",
                schema: "catalog",
                table: "JournalEntries",
                columns: new[] { "JournalEntryVoucherId", "LineNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_AccountCode",
                schema: "catalog",
                table: "JournalEntries",
                column: "AccountCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop tables
            migrationBuilder.DropTable(
                name: "JournalEntries",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "DepreciationSchedules",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "AssetRequisitions",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "JournalEntryVouchers",
                schema: "catalog");

            // Drop columns from PhysicalAssets
            migrationBuilder.DropIndex(
                name: "IX_PhysicalAssets_PropertyNumber",
                schema: "catalog",
                table: "PhysicalAssets");

            migrationBuilder.DropIndex(
                name: "IX_PhysicalAssets_CurrentCustodianId",
                schema: "catalog",
                table: "PhysicalAssets");

            migrationBuilder.DropColumn(
                name: "QRCodeData",
                schema: "catalog",
                table: "PhysicalAssets");

            migrationBuilder.DropColumn(
                name: "PropertyNumber",
                schema: "catalog",
                table: "PhysicalAssets");

            migrationBuilder.DropColumn(
                name: "QRGeneratedDate",
                schema: "catalog",
                table: "PhysicalAssets");

            migrationBuilder.DropColumn(
                name: "CurrentCustodianId",
                schema: "catalog",
                table: "PhysicalAssets");

            // Drop columns from Issuances
            migrationBuilder.DropIndex(
                name: "IX_Issuances_Status",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropIndex(
                name: "IX_Issuances_CustodianId",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "CustodianId",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "AcceptedOn",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "AcceptanceSignature_SignatureData",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "AcceptanceSignature_SignedOn",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "AcceptanceSignature_SignedByEmployeeId",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "AcceptanceSignature_IpAddress",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "AcceptanceSignature_UserAgent",
                schema: "catalog",
                table: "Issuances");

            migrationBuilder.DropColumn(
                name: "AcceptanceSignature_DeviceFingerprint",
                schema: "catalog",
                table: "Issuances");

            // Revert Acceptance Status column
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "catalog",
                table: "Acceptances",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
