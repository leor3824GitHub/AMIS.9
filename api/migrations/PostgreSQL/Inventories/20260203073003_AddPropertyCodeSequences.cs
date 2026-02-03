using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class AddPropertyCodeSequences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("1e5f25e6-2faa-4099-af37-54f44507dd10"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("35c48950-95a4-4ca7-896a-50bc46c5897d"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("4e555dde-8dca-4939-be0d-9e40ad4cfa2b"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("566ede58-5524-48fa-b89d-654332d86d78"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("5aa867ff-9ad6-4a95-9a5a-217289bed4c5"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("1cd66582-b1dc-4582-89b4-272843acdc0b"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("4dce0b12-27c9-441a-9f76-a1d40c63f40b"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("5505b04c-b00f-46c1-b5a1-8ba7bfb0fc0e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("9ebad020-fed9-4598-aba4-31864f7b767d"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("eb8877f1-4fcc-4473-86c2-e28f81cd2c70"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("199fab06-6944-473e-88f7-f9b9440e7a41"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("19cc3ca7-1ccf-4339-8c55-707056090942"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("1c638566-bbb2-48f2-93d5-518b7d5d4ce7"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("1e41a779-b713-4b05-8b77-d184f8752553"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("20bbf5d1-cae0-4cab-98a3-cf004b78500e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("223478f0-66a2-4209-ac61-1d3430522685"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("38315ae4-79ba-4cd3-8dd5-29c1a6b3104f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("3881abcf-4ca8-4fbc-9f06-e55a37b2fe4f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("433e2dcf-2341-41f5-b7f9-59e9e153b111"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("6f799000-a96b-4e45-94a6-becb63ffa82f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("8102f8f3-f630-4fbd-b3ef-a78f940cd66b"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("94d07398-91a3-496c-9c28-9e75e96cd5ec"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("952d461b-0d59-4000-9a97-df7d1d905f99"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b8266208-ee86-4fd6-9d67-f8963246468d"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("bd2a7676-3754-4f79-89dc-fee16d6fb0b9"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d384f80d-1a28-4606-bcea-cf489445f0fe"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("ee7d4f57-c972-4e53-87f0-b0f85d7329ba"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("f2a603fd-7fcd-4ce6-9bff-ac9da733a98a"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("fee4d345-5d15-4163-8133-a4c6cad07ded"));

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "inventories",
                table: "PhysicalAssets");

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                schema: "inventories",
                table: "SemexTransactionLog",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryCode",
                schema: "inventories",
                table: "ReceivingLineItem",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassCode",
                schema: "inventories",
                table: "ReceivingLineItem",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                schema: "inventories",
                table: "ReceivingLineItem",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                schema: "inventories",
                table: "InventoryTransactionLog",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "inventories",
                table: "AssetAssignmentHistories",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssetDisposals",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhysicalAssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetPropertyCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssetDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RequestedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DisposalMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    JustificationReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AssetConditionAtDisposal = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CompletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    SalvageValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    GainOrLoss = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DisposalReferenceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CancelledOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CancellationReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_AssetDisposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetDisposals_Employees_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AssetDisposals_Employees_CancelledBy",
                        column: x => x.CancelledBy,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AssetDisposals_Employees_CompletedBy",
                        column: x => x.CompletedBy,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AssetDisposals_Employees_RequestedBy",
                        column: x => x.RequestedBy,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetDisposals_PhysicalAssets_PhysicalAssetId",
                        column: x => x.PhysicalAssetId,
                        principalSchema: "inventories",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetMaintenances",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhysicalAssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetPropertyCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssetDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletionNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FindingsNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ScheduledBy = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ActualCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CostReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CancelledOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CancellationReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_AssetMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetMaintenances_Employees_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AssetMaintenances_Employees_CancelledBy",
                        column: x => x.CancelledBy,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AssetMaintenances_Employees_PerformedBy",
                        column: x => x.PerformedBy,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AssetMaintenances_Employees_ScheduledBy",
                        column: x => x.ScheduledBy,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetMaintenances_PhysicalAssets_PhysicalAssetId",
                        column: x => x.PhysicalAssetId,
                        principalSchema: "inventories",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NfaOfficeCodes",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OfficeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ParentOfficeCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NfaOfficeCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PPECategoryCodes",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AccountCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    COAReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPECategoryCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PPEItemCodes",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    COAReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPEItemCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyAcknowledgementReceipt",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PARNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Department = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Position = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IssuanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuancePurpose = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IssuanceLocation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReturnRemarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ReceivedByEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReceivedByEmployeeName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IssuedByName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IssuedByDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReceivedByName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ReceivedByDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedByName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ApprovedByDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LineItems = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyAcknowledgementReceipt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyCodeSequences",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    YearKey = table.Column<int>(type: "integer", nullable: false),
                    OfficeCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ClassCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ItemCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    LastSequence = table.Column<int>(type: "integer", nullable: false),
                    ResetAnnually = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyCodeSequences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PPETypeCodes",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    COAReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPETypeCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PPETypeCodes_PPECategoryCodes_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "inventories",
                        principalTable: "PPECategoryCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("35213e43-542b-43df-8ee0-de5a33cd5a20"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("6554a4ec-ff42-4e3f-9773-77f610ac992a"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("b3bda4c6-dd44-4896-9f74-0c0a1abfdbd1"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("cc778f8f-2c0c-445e-8a91-1dbd19f06f2c"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("e260415c-257f-4c08-b4a6-d87a2c8e4443"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("1622aa27-a352-4eb8-a5d1-c7e652419fea"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("3017e188-1951-47ae-b41b-850617fa63d1"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("65e258b6-0ce7-4e9f-8115-00738ee5d464"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("a0be47f9-78a7-4d96-9c63-ac36410be5d8"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("d4b4010d-f317-4448-8597-8f500b828a13"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("06831733-297e-4e5f-9680-14a9fa3639ea"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("07af8399-7d07-4c2d-8873-70dcf9072020"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("07be8273-91ca-4da6-9328-9fc9957fdd28"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("09d0e69d-fe3b-49a0-92cf-c4320904c2c2"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("11a311af-fa21-4240-ac27-377d0e3c10f0"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("1d0ef6ca-5438-4818-b24a-1199a60b1531"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("21379ffa-f630-42be-99ab-fa3d121d0d30"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("47d0993c-9772-4b7a-b0ad-fdd64aa94e8d"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("5261ec5b-9ba2-42de-886b-19abfcca9451"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("8ce127c6-3c91-4dec-b7b3-b2f2fbdcfc7e"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("99f9346b-f4f8-46ba-bda6-139c203f9cb3"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("9d3e1ccc-61dd-4296-9f72-04e8604cc223"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("9de6ffff-102e-440f-a8f3-2d1a01989e9f"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("b253cd17-7c81-4d1b-a341-9b0e165498b7"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("b45be00a-9959-4474-b060-7e72e0c968ca"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("c65eaa05-07e7-48cc-b209-5b915c8725e2"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("d8e19101-0cc7-404b-a690-d8ca077d25c6"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("da1f2c1e-9b87-443d-a97f-2dc7a81cd7c2"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("fe87cc69-ee0e-4414-9f76-074386667413"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposals_ApprovedBy",
                schema: "inventories",
                table: "AssetDisposals",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposals_CancelledBy",
                schema: "inventories",
                table: "AssetDisposals",
                column: "CancelledBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposals_CompletedBy",
                schema: "inventories",
                table: "AssetDisposals",
                column: "CompletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposals_PhysicalAssetId",
                schema: "inventories",
                table: "AssetDisposals",
                column: "PhysicalAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposals_PropertyCode",
                schema: "inventories",
                table: "AssetDisposals",
                column: "AssetPropertyCode");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposals_RequestDate",
                schema: "inventories",
                table: "AssetDisposals",
                column: "RequestDate");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposals_RequestedBy",
                schema: "inventories",
                table: "AssetDisposals",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposals_Status",
                schema: "inventories",
                table: "AssetDisposals",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposals_StatusApprover",
                schema: "inventories",
                table: "AssetDisposals",
                columns: new[] { "Status", "ApprovedBy" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_ApprovedBy",
                schema: "inventories",
                table: "AssetMaintenances",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_CancelledBy",
                schema: "inventories",
                table: "AssetMaintenances",
                column: "CancelledBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_PerformedBy",
                schema: "inventories",
                table: "AssetMaintenances",
                column: "PerformedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_PhysicalAssetId",
                schema: "inventories",
                table: "AssetMaintenances",
                column: "PhysicalAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_PropertyCode",
                schema: "inventories",
                table: "AssetMaintenances",
                column: "AssetPropertyCode");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_ScheduledBy",
                schema: "inventories",
                table: "AssetMaintenances",
                column: "ScheduledBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_ScheduledDate",
                schema: "inventories",
                table: "AssetMaintenances",
                column: "ScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_Status",
                schema: "inventories",
                table: "AssetMaintenances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_StatusScheduled",
                schema: "inventories",
                table: "AssetMaintenances",
                columns: new[] { "Status", "ScheduledDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_StatusTechnician",
                schema: "inventories",
                table: "AssetMaintenances",
                columns: new[] { "Status", "PerformedBy" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenances_Type",
                schema: "inventories",
                table: "AssetMaintenances",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_NfaOfficeCodes_Code",
                schema: "inventories",
                table: "NfaOfficeCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PPECategoryCodes_Code",
                schema: "inventories",
                table: "PPECategoryCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PPEItemCodes_ClassCode_CategoryCode_Code",
                schema: "inventories",
                table: "PPEItemCodes",
                columns: new[] { "ClassCode", "CategoryCode", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PPETypeCodes_CategoryId_Code",
                schema: "inventories",
                table: "PPETypeCodes",
                columns: new[] { "CategoryId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAcknowledgementReceipt_EmployeeId",
                schema: "inventories",
                table: "PropertyAcknowledgementReceipt",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAcknowledgementReceipt_PARNumber",
                schema: "inventories",
                table: "PropertyAcknowledgementReceipt",
                column: "PARNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyCodeSequences_YearKey_OfficeCode_ClassCode_Category~",
                schema: "inventories",
                table: "PropertyCodeSequences",
                columns: new[] { "YearKey", "OfficeCode", "ClassCode", "CategoryCode", "ItemCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetDisposals",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "AssetMaintenances",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "NfaOfficeCodes",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPEItemCodes",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPETypeCodes",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PropertyAcknowledgementReceipt",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PropertyCodeSequences",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPECategoryCodes",
                schema: "inventories");

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("35213e43-542b-43df-8ee0-de5a33cd5a20"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("6554a4ec-ff42-4e3f-9773-77f610ac992a"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("b3bda4c6-dd44-4896-9f74-0c0a1abfdbd1"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("cc778f8f-2c0c-445e-8a91-1dbd19f06f2c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("e260415c-257f-4c08-b4a6-d87a2c8e4443"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("1622aa27-a352-4eb8-a5d1-c7e652419fea"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("3017e188-1951-47ae-b41b-850617fa63d1"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("65e258b6-0ce7-4e9f-8115-00738ee5d464"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("a0be47f9-78a7-4d96-9c63-ac36410be5d8"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("d4b4010d-f317-4448-8597-8f500b828a13"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("06831733-297e-4e5f-9680-14a9fa3639ea"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("07af8399-7d07-4c2d-8873-70dcf9072020"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("07be8273-91ca-4da6-9328-9fc9957fdd28"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("09d0e69d-fe3b-49a0-92cf-c4320904c2c2"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("11a311af-fa21-4240-ac27-377d0e3c10f0"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("1d0ef6ca-5438-4818-b24a-1199a60b1531"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("21379ffa-f630-42be-99ab-fa3d121d0d30"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("47d0993c-9772-4b7a-b0ad-fdd64aa94e8d"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("5261ec5b-9ba2-42de-886b-19abfcca9451"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("8ce127c6-3c91-4dec-b7b3-b2f2fbdcfc7e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("99f9346b-f4f8-46ba-bda6-139c203f9cb3"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9d3e1ccc-61dd-4296-9f72-04e8604cc223"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9de6ffff-102e-440f-a8f3-2d1a01989e9f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b253cd17-7c81-4d1b-a341-9b0e165498b7"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b45be00a-9959-4474-b060-7e72e0c968ca"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("c65eaa05-07e7-48cc-b209-5b915c8725e2"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d8e19101-0cc7-404b-a690-d8ca077d25c6"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("da1f2c1e-9b87-443d-a97f-2dc7a81cd7c2"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("fe87cc69-ee0e-4414-9f76-074386667413"));

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                schema: "inventories",
                table: "SemexTransactionLog");

            migrationBuilder.DropColumn(
                name: "CategoryCode",
                schema: "inventories",
                table: "ReceivingLineItem");

            migrationBuilder.DropColumn(
                name: "ClassCode",
                schema: "inventories",
                table: "ReceivingLineItem");

            migrationBuilder.DropColumn(
                name: "ItemCode",
                schema: "inventories",
                table: "ReceivingLineItem");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                schema: "inventories",
                table: "InventoryTransactionLog");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "inventories",
                table: "AssetAssignmentHistories");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "inventories",
                table: "PhysicalAssets",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("1e5f25e6-2faa-4099-af37-54f44507dd10"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("35c48950-95a4-4ca7-896a-50bc46c5897d"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("4e555dde-8dca-4939-be0d-9e40ad4cfa2b"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("566ede58-5524-48fa-b89d-654332d86d78"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("5aa867ff-9ad6-4a95-9a5a-217289bed4c5"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("1cd66582-b1dc-4582-89b4-272843acdc0b"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("4dce0b12-27c9-441a-9f76-a1d40c63f40b"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("5505b04c-b00f-46c1-b5a1-8ba7bfb0fc0e"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("9ebad020-fed9-4598-aba4-31864f7b767d"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("eb8877f1-4fcc-4473-86c2-e28f81cd2c70"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("199fab06-6944-473e-88f7-f9b9440e7a41"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("19cc3ca7-1ccf-4339-8c55-707056090942"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("1c638566-bbb2-48f2-93d5-518b7d5d4ce7"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("1e41a779-b713-4b05-8b77-d184f8752553"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("20bbf5d1-cae0-4cab-98a3-cf004b78500e"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("223478f0-66a2-4209-ac61-1d3430522685"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("38315ae4-79ba-4cd3-8dd5-29c1a6b3104f"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("3881abcf-4ca8-4fbc-9f06-e55a37b2fe4f"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("433e2dcf-2341-41f5-b7f9-59e9e153b111"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("6f799000-a96b-4e45-94a6-becb63ffa82f"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("8102f8f3-f630-4fbd-b3ef-a78f940cd66b"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("94d07398-91a3-496c-9c28-9e75e96cd5ec"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("952d461b-0d59-4000-9a97-df7d1d905f99"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("b8266208-ee86-4fd6-9d67-f8963246468d"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("bd2a7676-3754-4f79-89dc-fee16d6fb0b9"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("d384f80d-1a28-4606-bcea-cf489445f0fe"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("ee7d4f57-c972-4e53-87f0-b0f85d7329ba"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("f2a603fd-7fcd-4ce6-9bff-ac9da733a98a"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("fee4d345-5d15-4163-8133-a4c6cad07ded"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 }
                });
        }
    }
}
