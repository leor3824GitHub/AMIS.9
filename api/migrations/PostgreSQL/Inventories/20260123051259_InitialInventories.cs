using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class InitialInventories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inventories");

            migrationBuilder.CreateTable(
                name: "AnnualProcurementPlans",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ControlNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FiscalYear = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    BudgetType = table.Column<int>(type: "integer", nullable: false),
                    TotalBudget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PreparedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmissionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovalDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_AnnualProcurementPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetClassificationRules",
                schema: "inventories",
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
                name: "AssetConditionConfigurations",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ColorCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    AllowsForUse = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresRepair = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresDisposal = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetConditionConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Designation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ResponsibilityCode = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryRegistry",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastTransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastTransactionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastTransactionReference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryRegistry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactionLog",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ReportNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    QuantityChange = table.Column<int>(type: "integer", nullable: false),
                    InventoryBefore = table.Column<int>(type: "integer", nullable: false),
                    InventoryAfter = table.Column<int>(type: "integer", nullable: false),
                    StatusBefore = table.Column<int>(type: "integer", nullable: false),
                    StatusAfter = table.Column<int>(type: "integer", nullable: false),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    InitiatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactionLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryVouchers",
                schema: "inventories",
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
                    table.PrimaryKey("PK_JournalEntryVouchers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PpeIssuanceReport",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RecipientName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RecipientAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IssuanceType = table.Column<string>(type: "text", nullable: false),
                    IssuanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DistributedToVoucher = table.Column<bool>(type: "boolean", nullable: false),
                    DistributedToPMSDS = table.Column<bool>(type: "boolean", nullable: false),
                    DistributedToAccounting = table.Column<bool>(type: "boolean", nullable: false),
                    DistributedToFile = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_PpeIssuanceReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PpeReceivingReport",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SourceName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SourceAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SourceReceiptDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceiptType = table.Column<string>(type: "text", nullable: false),
                    DistributedToVoucher = table.Column<bool>(type: "boolean", nullable: false),
                    DistributedToPMSDS = table.Column<bool>(type: "boolean", nullable: false),
                    DistributedToAccounting = table.Column<bool>(type: "boolean", nullable: false),
                    DistributedToFile = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_PpeReceivingReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PPETypeAccountMappings",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PPEType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RCAAccountCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_PPETypeAccountMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PPETypeDefinitions",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RCAAccountCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DepreciationAccountCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DefaultDepreciationRate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    DefaultUsefulLifeYears = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    COAReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IconName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPETypeDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementPlans",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ControlNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FiscalYear = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsSupplemental = table.Column<bool>(type: "boolean", nullable: false),
                    BudgetType = table.Column<int>(type: "integer", nullable: false),
                    TotalBudget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PreparedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmissionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovalDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_ProcurementPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementProjects",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PapCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProjectTitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PmoEndUser = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsEpa = table.Column<bool>(type: "boolean", nullable: false),
                    Mode = table.Column<int>(type: "integer", nullable: false),
                    FundSource = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SourcePlanItemId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_ProcurementProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RcaAccountCodes",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    AccountCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RcaAccountCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SemexRegistry",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastTransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastTransactionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastTransactionReference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemexRegistry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SemexTransactionLog",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ReportNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    QuantityChange = table.Column<int>(type: "integer", nullable: false),
                    InventoryBefore = table.Column<int>(type: "integer", nullable: false),
                    InventoryAfter = table.Column<int>(type: "integer", nullable: false),
                    StatusBefore = table.Column<int>(type: "integer", nullable: false),
                    StatusAfter = table.Column<int>(type: "integer", nullable: false),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    InitiatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemexTransactionLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Tin = table.Column<string>(type: "text", nullable: true),
                    TaxClassification = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ContactNo = table.Column<string>(type: "text", nullable: true),
                    Emailadd = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuppliesAndMaterialsIssuanceReports",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SmirNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Recipient_Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Recipient_Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Recipient_ContactNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IssuanceReason = table.Column<string>(type: "text", nullable: false),
                    Authorization_IssuingOfficerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Authorization_IssuingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Authorization_ApprovingOfficerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Authorization_ApprovingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Authorization_RecipientName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Authorization_ReceiptDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Authorization_DriverName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Authorization_BillOfLadingNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DistributedToRecipient = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DistributedToPMSDS = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DistributedToAccounting = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DistributedToAccountingAdvice = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DistributedToFile = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliesAndMaterialsIssuanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuppliesAndMaterialsReceivingReports",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SmrrNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Source_Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Source_Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Source_ReceivingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionType = table.Column<string>(type: "text", nullable: false),
                    Authentication_ReceivedByName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Authentication_ReceivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Authentication_NotedByName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Authentication_NotedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DistributedToVoucher = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DistributedToPMSDS = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DistributedToAccounting = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DistributedToFile = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliesAndMaterialsReceivingReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitsOfMeasure",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    UnitType = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    BaseUnitId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConversionFactor = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsOfMeasure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitsOfMeasure_UnitsOfMeasure_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalSchema: "inventories",
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnnualProcurementPlanItems",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanHeaderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PapCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProjectType = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    EstimatedBudget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Mode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsEarlyProcurement = table.Column<bool>(type: "boolean", nullable: false),
                    ScheduleMonth = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FundingSource = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_AnnualProcurementPlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnualProcurementPlanItems_AnnualProcurementPlans_PlanHeade~",
                        column: x => x.PlanHeaderId,
                        principalSchema: "inventories",
                        principalTable: "AnnualProcurementPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Sku = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    PropertyClassification = table.Column<int>(type: "integer", nullable: false, defaultValue: 1, comment: "1=Consumable, 2=SemiExpendable, 3=PPE"),
                    EstimatedUsefulLife = table.Column<int>(type: "integer", nullable: false, defaultValue: 12, comment: "Estimated useful life in months"),
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
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "inventories",
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Issuances",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CustodianId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AcceptedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AcceptanceSignature_SignatureData = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    AcceptanceSignature_SignedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AcceptanceSignature_SignedByEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    AcceptanceSignature_IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    AcceptanceSignature_UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AcceptanceSignature_DeviceFingerprint = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_Issuances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issuances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequests",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Purpose = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ApprovalRemarks = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Employees_RequestedBy",
                        column: x => x.RequestedBy,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JournalEntryVoucherId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    AccountCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    DebitAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreditAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_JournalEntryVouchers_JournalEntryVoucherId",
                        column: x => x.JournalEntryVoucherId,
                        principalSchema: "inventories",
                        principalTable: "JournalEntryVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementPlanItems",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanHeaderId = table.Column<Guid>(type: "uuid", nullable: false),
                    PapCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProjectType = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    EstimatedBudget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Mode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsEarlyProcurement = table.Column<bool>(type: "boolean", nullable: false),
                    ScheduleMonth = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FundingSource = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_ProcurementPlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcurementPlanItems_ProcurementPlans_PlanHeaderId",
                        column: x => x.PlanHeaderId,
                        principalSchema: "inventories",
                        principalTable: "ProcurementPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementSchedules",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdsPosting = table.Column<DateOnly>(type: "date", nullable: true),
                    PreBidConference = table.Column<DateOnly>(type: "date", nullable: true),
                    BidOpening = table.Column<DateOnly>(type: "date", nullable: true),
                    BidEvaluation = table.Column<DateOnly>(type: "date", nullable: true),
                    PostQualification = table.Column<DateOnly>(type: "date", nullable: true),
                    NoticeOfAward = table.Column<DateOnly>(type: "date", nullable: true),
                    ContractSigning = table.Column<DateOnly>(type: "date", nullable: true),
                    NoticeToProceeed = table.Column<DateOnly>(type: "date", nullable: true),
                    DeliveryCompletion = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcurementSchedules_ProcurementProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "inventories",
                        principalTable: "ProcurementProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBudgets",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MooeAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CoAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBudgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBudgets_ProcurementProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "inventories",
                        principalTable: "ProcurementProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssuanceLineItem",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SuppliesAndMaterialsIssuanceReportId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuanceLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuanceLineItem_SuppliesAndMaterialsIssuanceReports_Suppli~",
                        column: x => x.SuppliesAndMaterialsIssuanceReportId,
                        principalSchema: "inventories",
                        principalTable: "SuppliesAndMaterialsIssuanceReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingLineItem",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AcquisitionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SuppliesAndMaterialsReceivingReportId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivingLineItem_SuppliesAndMaterialsReceivingReports_Supp~",
                        column: x => x.SuppliesAndMaterialsReceivingReportId,
                        principalSchema: "inventories",
                        principalTable: "SuppliesAndMaterialsReceivingReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsumableInventories",
                schema: "inventories",
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
                        principalSchema: "inventories",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    AvePrice = table.Column<decimal>(type: "numeric", nullable: false),
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
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventories",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventories",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalAssets",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AcquisitionCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ModelNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Condition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EstimatedUsefulLife = table.Column<int>(type: "integer", nullable: false),
                    DisposalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DisposalReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CurrentClassification = table.Column<string>(type: "text", nullable: false),
                    PPEType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AccumulatedDepreciation = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    QRCodeData = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    PropertyNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    QRGeneratedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrentCustodianId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_PhysicalAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalAssets_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventories",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetRequisitions",
                schema: "inventories",
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
                    table.PrimaryKey("PK_AssetRequisitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetRequisitions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetRequisitions_Issuances_IssuanceId",
                        column: x => x.IssuanceId,
                        principalSchema: "inventories",
                        principalTable: "Issuances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssuanceItems",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_IssuanceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuanceItems_Issuances_IssuanceId",
                        column: x => x.IssuanceId,
                        principalSchema: "inventories",
                        principalTable: "Issuances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssuanceItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventories",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Canvasses",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemDescription = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    QuotedPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ResponseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSelected = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_Canvasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Canvasses_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "inventories",
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Canvasses_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "inventories",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequestItems",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManualProductName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    Unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
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
                    table.PrimaryKey("PK_PurchaseRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventories",
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequestItems_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "inventories",
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetAssignmentHistories",
                schema: "inventories",
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
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignmentHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignmentHistories_Employees_TransferredToEmployeeId",
                        column: x => x.TransferredToEmployeeId,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignmentHistories_PhysicalAssets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "inventories",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetReclassificationHistories",
                schema: "inventories",
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
                        principalSchema: "inventories",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepreciationSchedules",
                schema: "inventories",
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
                    table.PrimaryKey("PK_DepreciationSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepreciationSchedules_JournalEntryVouchers_JournalEntryVouc~",
                        column: x => x.JournalEntryVoucherId,
                        principalSchema: "inventories",
                        principalTable: "JournalEntryVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepreciationSchedules_PhysicalAssets_PhysicalAssetId",
                        column: x => x.PhysicalAssetId,
                        principalSchema: "inventories",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseRequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    SelectedCanvassId = table.Column<Guid>(type: "uuid", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_Canvasses_SelectedCanvassId",
                        column: x => x.SelectedCanvassId,
                        principalSchema: "inventories",
                        principalTable: "Canvasses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchases_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "inventories",
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchases_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "inventories",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceipt",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReceivedById = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryNoteNumber = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReceipt_Employees_ReceivedById",
                        column: x => x.ReceivedById,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceipt_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "inventories",
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceipt_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "inventories",
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InspectionRequests",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_InspectionRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionRequests_Employees_InspectorId",
                        column: x => x.InspectorId,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InspectionRequests_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "inventories",
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Approved = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IARDocumentPath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspections_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "inventories",
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseItems",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ItemStatus = table.Column<int>(type: "integer", nullable: false),
                    InspectionStatus = table.Column<int>(type: "integer", nullable: false),
                    AcceptanceStatus = table.Column<int>(type: "integer", nullable: false),
                    QtyInspected = table.Column<int>(type: "integer", nullable: false),
                    QtyPassed = table.Column<int>(type: "integer", nullable: false),
                    QtyFailed = table.Column<int>(type: "integer", nullable: false),
                    QtyAccepted = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PurchaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventories",
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseItems_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "inventories",
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Acceptances",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplyOfficerId = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    GoodsReceiptId = table.Column<Guid>(type: "uuid", nullable: true),
                    AcceptanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsPosted = table.Column<bool>(type: "boolean", nullable: false),
                    PostedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PurchaseId1 = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_Acceptances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acceptances_Employees_SupplyOfficerId",
                        column: x => x.SupplyOfficerId,
                        principalSchema: "inventories",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acceptances_GoodsReceipt_GoodsReceiptId",
                        column: x => x.GoodsReceiptId,
                        principalSchema: "inventories",
                        principalTable: "GoodsReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Acceptances_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalSchema: "inventories",
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Acceptances_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "inventories",
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acceptances_Purchases_PurchaseId1",
                        column: x => x.PurchaseId1,
                        principalSchema: "inventories",
                        principalTable: "Purchases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceiptItem",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GoodsReceiptId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    QtyReceived = table.Column<int>(type: "integer", nullable: false),
                    Condition = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_GoodsReceiptItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptItem_GoodsReceipt_GoodsReceiptId",
                        column: x => x.GoodsReceiptId,
                        principalSchema: "inventories",
                        principalTable: "GoodsReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptItem_PurchaseItems_PurchaseItemId",
                        column: x => x.PurchaseItemId,
                        principalSchema: "inventories",
                        principalTable: "PurchaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionItems",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    QtyInspected = table.Column<int>(type: "integer", nullable: false),
                    QtyPassed = table.Column<int>(type: "integer", nullable: false),
                    QtyFailed = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    InspectionItemStatus = table.Column<int>(type: "integer", nullable: false),
                    InspectionId1 = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_InspectionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionItems_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalSchema: "inventories",
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionItems_Inspections_InspectionId1",
                        column: x => x.InspectionId1,
                        principalSchema: "inventories",
                        principalTable: "Inspections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InspectionItems_PurchaseItems_PurchaseItemId",
                        column: x => x.PurchaseItemId,
                        principalSchema: "inventories",
                        principalTable: "PurchaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcceptanceItems",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AcceptanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    QtyAccepted = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_AcceptanceItems", x => x.Id);
                    table.CheckConstraint("CK_AcceptanceItems_QtyAccepted_NonNegative", "\"QtyAccepted\" >= 0");
                    table.ForeignKey(
                        name: "FK_AcceptanceItems_Acceptances_AcceptanceId",
                        column: x => x.AcceptanceId,
                        principalSchema: "inventories",
                        principalTable: "Acceptances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcceptanceItems_PurchaseItems_PurchaseItemId",
                        column: x => x.PurchaseItemId,
                        principalSchema: "inventories",
                        principalTable: "PurchaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("5d9263de-e5fb-4f58-b930-2028dcf2c8a0"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("aa635205-5079-4222-bc27-43c92ed71157"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("acceae57-4620-4249-8a93-3f8e5e72c9c2"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("e6f3b00f-d125-421c-ba38-6214fc67d8d0"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("ee20580b-9ffb-4079-8d7b-e86bcead292b"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "InventoryRegistry",
                columns: new[] { "Id", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "IssuedDate", "LastModified", "LastModifiedBy", "LastTransactionDate", "LastTransactionReference", "LastTransactionType", "Location", "PropertyCode", "Quantity", "ReceivedDate", "Status" },
                values: new object[,]
                {
                    { new Guid("0aba5db9-db62-4686-9560-8af03efafa60"), new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Office Chair", null, new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), "PPERR-002", "PPERR", "Main Office", "237", 20, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), 1 },
                    { new Guid("316f6324-68f6-462d-a6d2-e7b7ab4a01c3"), new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Desktop Computer", null, new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), "PPERR-001", "PPERR", "IT Office - Room 101", "234", 10, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), 1 },
                    { new Guid("3c01b487-d17f-4fa0-8394-d78e6faf6402"), new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Desk Lamp", null, new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), "PPERR-002", "PPERR", "Main Office", "238", 15, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), 1 },
                    { new Guid("c969f975-2567-4138-b02c-fab8f9dd48ff"), new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Printer", null, new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), "PPERR-001", "PPERR", "IT Office - Room 103", "236", 3, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), 1 },
                    { new Guid("ca30dcf2-b1e2-4581-ad06-efcc881fc72d"), new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Laptop Computer", null, new DateTimeOffset(new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Unspecified).AddTicks(3399), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), "PPERR-001", "PPERR", "IT Office - Room 102", "235", 5, new DateTime(2026, 1, 23, 5, 12, 58, 113, DateTimeKind.Utc).AddTicks(3399), 1 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("1771185c-aace-419f-a6cf-104a07f09247"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("81a8575b-34a0-4fe4-af62-1af49d991de9"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("b97868ec-4c69-4ad8-b861-b7f759c3031c"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("ce42ce96-0b92-4cb1-9496-330af1c989fc"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("e4642772-3ad6-4736-a540-ff9c73893f06"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("11087ecd-b56d-4fe7-9f40-de602c73bcbf"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("11c8de38-bc26-4ac7-a3ff-4cc8f87afa3e"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("206de9f8-4146-4a45-b528-1c73f3b1ea42"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("2410eeaa-c2a4-46d7-9320-b4337f141c3f"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("2a1b6921-aaaf-44bb-b96f-041eaa4120a7"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("6b3aa276-0e7f-42d0-a249-8e098d1f8f47"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("6b96c7dd-5e18-4a0c-9c3f-6e958a23ab57"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("8078b956-3266-4b10-a434-473f37093ea8"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("96a5630f-2ac5-4042-b62c-20f019f0a9b9"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("9a3e2cdd-fa25-4d6c-8577-d767afc5ed79"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("a2747561-2a22-476d-aeac-2c6b44096a61"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("a411ca10-9dd2-4bd0-a43e-902dd7e88828"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("aceb9d77-ccf2-4cc6-93aa-9e37f9aa7aed"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("b231b179-2545-4d16-82c8-cf5117e85be9"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("cb2b19ea-ff7c-4b71-89df-9787b83ca65b"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("d8d66d3b-f3e3-45b2-b6af-e6bcbb296662"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("d9c8d74f-6bfb-4651-9e0d-c0a7b04a4754"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("e8f21e33-8691-474c-8734-98550197a35e"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("fd80e628-d666-4736-84bb-10402bfb4621"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_AcceptanceId_PurchaseItemId",
                schema: "inventories",
                table: "AcceptanceItems",
                columns: new[] { "AcceptanceId", "PurchaseItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "inventories",
                table: "AcceptanceItems",
                column: "PurchaseItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Acceptances_GoodsReceiptId",
                schema: "inventories",
                table: "Acceptances",
                column: "GoodsReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_Acceptances_InspectionId",
                schema: "inventories",
                table: "Acceptances",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Acceptances_IsPosted",
                schema: "inventories",
                table: "Acceptances",
                column: "IsPosted");

            migrationBuilder.CreateIndex(
                name: "IX_Acceptances_PurchaseId",
                schema: "inventories",
                table: "Acceptances",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Acceptances_PurchaseId1",
                schema: "inventories",
                table: "Acceptances",
                column: "PurchaseId1");

            migrationBuilder.CreateIndex(
                name: "IX_Acceptances_Status",
                schema: "inventories",
                table: "Acceptances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Acceptances_SupplyOfficerId",
                schema: "inventories",
                table: "Acceptances",
                column: "SupplyOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualProcurementPlanItems_PlanHeaderId",
                schema: "inventories",
                table: "AnnualProcurementPlanItems",
                column: "PlanHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualProcurementPlans_TenantId_ControlNumber",
                schema: "inventories",
                table: "AnnualProcurementPlans",
                columns: new[] { "TenantId", "ControlNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignmentHistories_AcceptedBy",
                schema: "inventories",
                table: "AssetAssignmentHistories",
                column: "AcceptedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignmentHistories_AssetId",
                schema: "inventories",
                table: "AssetAssignmentHistories",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignmentHistories_EmployeeId",
                schema: "inventories",
                table: "AssetAssignmentHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignmentHistories_TransferredToEmployeeId",
                schema: "inventories",
                table: "AssetAssignmentHistories",
                column: "TransferredToEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetConditionConfigurations_Code",
                schema: "inventories",
                table: "AssetConditionConfigurations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetReclassificationHistories_AssetId",
                schema: "inventories",
                table: "AssetReclassificationHistories",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequisitions_EmployeeId",
                schema: "inventories",
                table: "AssetRequisitions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequisitions_ExpirationDate",
                schema: "inventories",
                table: "AssetRequisitions",
                column: "ExpirationDate");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequisitions_IssuanceId",
                schema: "inventories",
                table: "AssetRequisitions",
                column: "IssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequisitions_Status",
                schema: "inventories",
                table: "AssetRequisitions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Canvass_PurchaseRequestId",
                schema: "inventories",
                table: "Canvasses",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Canvass_PurchaseRequestId_SupplierId",
                schema: "inventories",
                table: "Canvasses",
                columns: new[] { "PurchaseRequestId", "SupplierId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Canvass_SupplierId",
                schema: "inventories",
                table: "Canvasses",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumableInventories_ProductId",
                schema: "inventories",
                table: "ConsumableInventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumableInventories_StockNumber",
                schema: "inventories",
                table: "ConsumableInventories",
                column: "StockNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationSchedules_JournalEntryVoucherId",
                schema: "inventories",
                table: "DepreciationSchedules",
                column: "JournalEntryVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationSchedules_PhysicalAssetId_Year_Month",
                schema: "inventories",
                table: "DepreciationSchedules",
                columns: new[] { "PhysicalAssetId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationSchedules_Status",
                schema: "inventories",
                table: "DepreciationSchedules",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_PurchaseId",
                schema: "inventories",
                table: "GoodsReceipt",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_ReceivedById",
                schema: "inventories",
                table: "GoodsReceipt",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_SupplierId",
                schema: "inventories",
                table: "GoodsReceipt",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptItem_GoodsReceiptId",
                schema: "inventories",
                table: "GoodsReceiptItem",
                column: "GoodsReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptItem_PurchaseItemId",
                schema: "inventories",
                table: "GoodsReceiptItem",
                column: "PurchaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_InspectionId",
                schema: "inventories",
                table: "InspectionItems",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_InspectionId1",
                schema: "inventories",
                table: "InspectionItems",
                column: "InspectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "inventories",
                table: "InspectionItems",
                column: "PurchaseItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequests_InspectorId",
                schema: "inventories",
                table: "InspectionRequests",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequests_PurchaseId",
                schema: "inventories",
                table: "InspectionRequests",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_EmployeeId",
                schema: "inventories",
                table: "Inspections",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PurchaseId",
                schema: "inventories",
                table: "Inspections",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                schema: "inventories",
                table: "Inventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryRegistry_PropertyCode",
                schema: "inventories",
                table: "InventoryRegistry",
                column: "PropertyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionLog_PropertyCode_TransactionDate",
                schema: "inventories",
                table: "InventoryTransactionLog",
                columns: new[] { "PropertyCode", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ProductId",
                schema: "inventories",
                table: "InventoryTransactions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceItems_IssuanceId",
                schema: "inventories",
                table: "IssuanceItems",
                column: "IssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceItems_ProductId",
                schema: "inventories",
                table: "IssuanceItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceLineItem_SuppliesAndMaterialsIssuanceReportId",
                schema: "inventories",
                table: "IssuanceLineItem",
                column: "SuppliesAndMaterialsIssuanceReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Issuances_CustodianId",
                schema: "inventories",
                table: "Issuances",
                column: "CustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_Issuances_EmployeeId",
                schema: "inventories",
                table: "Issuances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Issuances_Status",
                schema: "inventories",
                table: "Issuances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_AccountCode",
                schema: "inventories",
                table: "JournalEntries",
                column: "AccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_JournalEntryVoucherId_LineNumber",
                schema: "inventories",
                table: "JournalEntries",
                columns: new[] { "JournalEntryVoucherId", "LineNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryVouchers_Status",
                schema: "inventories",
                table: "JournalEntryVouchers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryVouchers_VoucherNumber",
                schema: "inventories",
                table: "JournalEntryVouchers",
                column: "VoucherNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryVouchers_Year_Month",
                schema: "inventories",
                table: "JournalEntryVouchers",
                columns: new[] { "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_CurrentClassification",
                schema: "inventories",
                table: "PhysicalAssets",
                column: "CurrentClassification");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_CurrentCustodianId",
                schema: "inventories",
                table: "PhysicalAssets",
                column: "CurrentCustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_DisposalDate",
                schema: "inventories",
                table: "PhysicalAssets",
                column: "DisposalDate");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_ProductId",
                schema: "inventories",
                table: "PhysicalAssets",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_PropertyCode",
                schema: "inventories",
                table: "PhysicalAssets",
                column: "PropertyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_PropertyNumber",
                schema: "inventories",
                table: "PhysicalAssets",
                column: "PropertyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PpeIssuanceReport_ReportNumber",
                schema: "inventories",
                table: "PpeIssuanceReport",
                column: "ReportNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PpeReceivingReport_ReportNumber",
                schema: "inventories",
                table: "PpeReceivingReport",
                column: "ReportNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PPETypeAccountMappings_PPEType",
                schema: "inventories",
                table: "PPETypeAccountMappings",
                column: "PPEType",
                unique: true,
                filter: "\"IsActive\" = true AND \"Deleted\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PPETypeDefinitions_Code",
                schema: "inventories",
                table: "PPETypeDefinitions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlanItems_PlanHeaderId",
                schema: "inventories",
                table: "ProcurementPlanItems",
                column: "PlanHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_TenantId_ControlNumber",
                schema: "inventories",
                table: "ProcurementPlans",
                columns: new[] { "TenantId", "ControlNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementSchedules_ProjectId",
                schema: "inventories",
                table: "ProcurementSchedules",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "inventories",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PropertyClassification",
                schema: "inventories",
                table: "Products",
                column: "PropertyClassification");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBudgets_ProjectId",
                schema: "inventories",
                table: "ProjectBudgets",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_ProductId",
                schema: "inventories",
                table: "PurchaseItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_PurchaseId",
                schema: "inventories",
                table: "PurchaseItems",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItems_ProductId",
                schema: "inventories",
                table: "PurchaseRequestItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItems_PurchaseRequestId",
                schema: "inventories",
                table: "PurchaseRequestItems",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_RequestedBy",
                schema: "inventories",
                table: "PurchaseRequests",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PurchaseRequestId",
                schema: "inventories",
                table: "Purchases",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SelectedCanvassId",
                schema: "inventories",
                table: "Purchases",
                column: "SelectedCanvassId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SupplierId",
                schema: "inventories",
                table: "Purchases",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_RcaAccountCodes_Key",
                schema: "inventories",
                table: "RcaAccountCodes",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLineItem_SuppliesAndMaterialsReceivingReportId",
                schema: "inventories",
                table: "ReceivingLineItem",
                column: "SuppliesAndMaterialsReceivingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_SemexRegistry_ItemCode",
                schema: "inventories",
                table: "SemexRegistry",
                column: "ItemCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemexTransactionLog_ItemCode",
                schema: "inventories",
                table: "SemexTransactionLog",
                column: "ItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_SemexTransactionLog_ReportNumber",
                schema: "inventories",
                table: "SemexTransactionLog",
                column: "ReportNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SemexTransactionLog_TransactionDate",
                schema: "inventories",
                table: "SemexTransactionLog",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_SemexTransactionLog_TransactionType",
                schema: "inventories",
                table: "SemexTransactionLog",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliesAndMaterialsIssuanceReports_SmirNumber",
                schema: "inventories",
                table: "SuppliesAndMaterialsIssuanceReports",
                column: "SmirNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuppliesAndMaterialsReceivingReports_SmrrNumber",
                schema: "inventories",
                table: "SuppliesAndMaterialsReceivingReports",
                column: "SmrrNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitsOfMeasure_BaseUnitId",
                schema: "inventories",
                table: "UnitsOfMeasure",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitsOfMeasure_Code",
                schema: "inventories",
                table: "UnitsOfMeasure",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcceptanceItems",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "AnnualProcurementPlanItems",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "AssetAssignmentHistories",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "AssetClassificationRules",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "AssetConditionConfigurations",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "AssetReclassificationHistories",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "AssetRequisitions",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Brands",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "ConsumableInventories",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "DepreciationSchedules",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "GoodsReceiptItem",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "InspectionItems",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "InspectionRequests",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Inventories",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "InventoryRegistry",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "InventoryTransactionLog",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "InventoryTransactions",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "IssuanceItems",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "IssuanceLineItem",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "JournalEntries",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PpeIssuanceReport",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PpeReceivingReport",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPETypeAccountMappings",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPETypeDefinitions",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "ProcurementPlanItems",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "ProcurementSchedules",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "ProjectBudgets",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PurchaseRequestItems",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "RcaAccountCodes",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "ReceivingLineItem",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "SemexRegistry",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "SemexTransactionLog",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "UnitsOfMeasure",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Acceptances",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "AnnualProcurementPlans",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PhysicalAssets",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PurchaseItems",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Issuances",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "SuppliesAndMaterialsIssuanceReports",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "JournalEntryVouchers",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "ProcurementPlans",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "ProcurementProjects",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "SuppliesAndMaterialsReceivingReports",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "GoodsReceipt",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Inspections",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Purchases",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Canvasses",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PurchaseRequests",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "inventories");
        }
    }
}
