using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    Name = table.Column<string>(type: "text", nullable: false),
                    Classification = table.Column<int>(type: "integer", nullable: false),
                    MinimumCost = table.Column<decimal>(type: "numeric", nullable: false),
                    MaximumCost = table.Column<decimal>(type: "numeric", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RCAAccountCode = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
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
                    CorrelationId = table.Column<string>(type: "text", nullable: true),
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
                name: "PPEIR",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IRNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IssuedTo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_PPEIR", x => x.Id);
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
                name: "PPERR",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RRNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReceivedFrom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_PPERR", x => x.Id);
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Classification = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ItemCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ItemDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    GLAccount = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    LastSequenceValue = table.Column<int>(type: "integer", nullable: false),
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
                    CorrelationId = table.Column<string>(type: "text", nullable: true),
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
                    UnitOfMeasure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "piece"),
                    EstimatedUsefulLife = table.Column<int>(type: "integer", nullable: false, defaultValue: 12),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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

            migrationBuilder.CreateTable(
                name: "PPEIRLineItem",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IRNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PPEIRId = table.Column<Guid>(type: "uuid", nullable: false),
                    PPEIRId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPEIRLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PPEIRLineItem_PPEIR_PPEIRId",
                        column: x => x.PPEIRId,
                        principalSchema: "inventories",
                        principalTable: "PPEIR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PPEIRLineItem_PPEIR_PPEIRId1",
                        column: x => x.PPEIRId1,
                        principalSchema: "inventories",
                        principalTable: "PPEIR",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PPERRLineItem",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RRNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PPERRId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPERRLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PPERRLineItem_PPERR_PPERRId",
                        column: x => x.PPERRId,
                        principalSchema: "inventories",
                        principalTable: "PPERR",
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
                    PropertyCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                name: "SuppliesAndMaterialsReceivingLineItem",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AcquisitionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ClassCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    CategoryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    ItemCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    SuppliesAndMaterialsReceivingReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliesAndMaterialsReceivingLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuppliesAndMaterialsReceivingLineItem_SuppliesAndMaterialsR~",
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
                    AcquisitionCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ModelNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Condition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    DisposalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DisposalReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AccumulatedDepreciation = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    QRCodeData = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    QRGeneratedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ImagePaths = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ParentAssetId = table.Column<Guid>(type: "uuid", nullable: true),
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
                        name: "FK_PhysicalAssets_PhysicalAssets_ParentAssetId",
                        column: x => x.ParentAssetId,
                        principalSchema: "inventories",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    TransferredToEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransferredToDocumentNumber = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Condition = table.Column<string>(type: "text", nullable: true),
                    AcceptedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IssuedByName = table.Column<string>(type: "text", nullable: true),
                    ReceivedByName = table.Column<string>(type: "text", nullable: true),
                    ApprovedByName = table.Column<string>(type: "text", nullable: true),
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
                name: "Inspections",
                schema: "inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PurchaseId = table.Column<Guid>(type: "uuid", nullable: true),
                    PhysicalAssetId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Approved = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IARDocumentPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                        name: "FK_Inspections_PhysicalAssets_PhysicalAssetId",
                        column: x => x.PhysicalAssetId,
                        principalSchema: "inventories",
                        principalTable: "PhysicalAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspections_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "inventories",
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    { new Guid("0d302547-c9eb-4d6a-a6a7-5102b8a5cd0c"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("38aa5cfc-5eec-4865-9b28-978b54c8e4c6"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("69157626-1bea-4571-aeb6-4db0363c01e1"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("948eeb63-a4c1-4f14-93db-0fa72489a82a"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("faac3215-d242-4963-855a-426efb3e5410"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "InventoryRegistry",
                columns: new[] { "Id", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "IssuedDate", "LastModified", "LastModifiedBy", "LastTransactionDate", "LastTransactionReference", "LastTransactionType", "Location", "PropertyCode", "Quantity", "ReceivedDate", "Status" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Desktop Computer", null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PPERR-001", "PPERR", "IT Office - Room 101", "234", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Laptop Computer", null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PPERR-001", "PPERR", "IT Office - Room 102", "235", 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Printer", null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PPERR-001", "PPERR", "IT Office - Room 103", "236", 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Office Chair", null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PPERR-002", "PPERR", "Main Office", "237", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Desk Lamp", null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PPERR-002", "PPERR", "Main Office", "238", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("114e8090-cab3-43d6-9485-f5d28642e9b8"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("1b062724-34c0-40fc-aaa3-618afe3146d4"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("211945cc-dde6-4b91-a3f7-0da8e5fa742d"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("4a2d8afe-1374-4cc0-b1f8-d86dd9bd9526"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("8cfb4254-c41d-4217-a176-998a332e4a68"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "PropertyCodeSequences",
                columns: new[] { "Id", "CategoryCode", "ClassCode", "Classification", "Created", "CreatedBy", "Deleted", "DeletedBy", "GLAccount", "ItemCode", "ItemDescription", "LastModified", "LastModifiedBy", "LastSequenceValue" },
                values: new object[,]
                {
                    { 1, "LL", "LL", "LAND", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10601010", "01", "LAND", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 2, "LI", "LI", "LAND IMPROVEMENTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10602990", "01", "OTHER LAND IMPROVEMENTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 3, "BS", "BS", "BUILDING and Other STRUCTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604010", "01", "BUILDINGS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 4, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "01", "OTHER STRUCTURES - WAREHOUSES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 5, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "02", "OTHER STRUCTURES - STAFF HOUSE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 6, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "03", "OTHER STRUCTURES - MOTORPOOL", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 7, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "04", "OTHER STRUCTURES - TRUCK SCALE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 8, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "05", "OTHER STRUCTURES - POWER HOUSE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 9, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "06", "OTHER STRUCTURES - CANTEEN", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 10, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "07", "OTHER STRUCTURES - LABORATORY", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 11, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "08", "OTHER STRUCTURES - GUARD HOUSE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 12, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "09", "OTHER STRUCTURES - TRAINING CENTER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 13, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "10", "OTHER STRUCTURES - STORAGE/STOCK ROOM", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 14, "OS", "OS", "Other Structures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10604990", "11", "OTHER STRUCTURES - COMFORT ROOM", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 15, "FM", "FM", "MACHINERY AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605010", "01", "FARM MACHINERY EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 16, "WE", "FM", "MACHINERY AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605010", "02", "WAREHOUSE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 17, "PE", "FM", "MACHINERY AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605010", "03", "PLANT AND MACHINERY EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 18, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "01", "ADDING MACHINE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 19, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "02", "CALCULATORS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 20, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "03", "AIRCONDITIONING EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 21, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "04", "BUNDY CLOCK/ BIOMETRIC MACHINE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 22, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "05", "SHREDDING MACHINE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 23, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "06", "CHECKWRITER/CURRENCY COUNTING/DETECTOR", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 24, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "07", "ELECTRIC FAN", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 25, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "08", "MIMEOGRAPHING MACHINE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 26, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "09", "PHOTOCOPYING MACHINE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 27, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "10", "STEEL FILING-CABINET", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 28, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "11", "STEEL CABINETS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 29, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "12", "STEEL SAFETY VAULT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 30, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "13", "TYPEWRITER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 31, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "14", "BINDING MACHINE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 32, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "15", "PHOTO STENCILING MACHINE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 33, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "16", "LETTER SCALE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 34, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "17", "HEAVY DUTY PUNCHER- 3 HOLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 35, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "18", "HAND TRUCK/ PUSH CART", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 36, "OE", "OE", "OFFICE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605020", "19", "KITCHEN EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 37, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "01", "PERSONAL COMPUTERS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 38, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "02", "PRINTER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 39, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "03", "COMPUTER PERIPHERAL", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 40, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "04", "SOFTWARE APPLICATION", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 41, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "05", "MONITOR", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 42, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "06", "PERSONAL COMPUTER ACCESSORIES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 43, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "07", "SCANNER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 44, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "08", "POWER SUPPLY FOR COMPUTER(UPS)", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 45, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "09", "Auto Voltage Regulator (AVR)", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 46, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "10", "ACCESS POINT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 47, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "11", "DVD/LCD WRITER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 48, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "12", "SPEAKER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 49, "DP", "DP", "INFORMATION & COMMUNICATION TECHNOLOGY EQPT.", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605030", "13", "SERVERS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 50, "CM", "CM", "COMMUNICATION EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605070", "01", "RADIO EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 51, "CM", "CM", "COMMUNICATION EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605070", "02", "TELEPHONE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 52, "CM", "CM", "COMMUNICATION EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605070", "03", "AUDIO-VISUAL EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 53, "CM", "CM", "COMMUNICATION EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605070", "04", "PROJECTOR", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 54, "CM", "CM", "COMMUNICATION EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605070", "05", "PROJECTOR SCREEN", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 55, "CM", "CM", "COMMUNICATION EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605070", "06", "CAMERA", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 56, "CM", "CM", "COMMUNICATION EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605070", "07", "PORTABLE POWER BANK", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 57, "CM", "CM", "COMMUNICATION EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605070", "08", "PAGING SYSTEM", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 58, "CM", "CM", "COMMUNICATION EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605070", "09", "PHONES/FAX MACHINE/MOBILE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 59, "FR", "FR", "DISASTER RESPONSE AND RESCUE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605090", "01", "Rescue Team Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 60, "FR", "FR", "DISASTER RESPONSE AND RESCUE EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605090", "02", "Firefighting Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 61, "MD", "MD", "MEDICAL /DENTAL /LABORATORY EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605110", "01", "MEDICAL EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 62, "MD", "MD", "MEDICAL /DENTAL /LABORATORY EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605110", "02", "DENTAL EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 63, "MD", "MD", "MEDICAL /DENTAL /LABORATORY EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605110", "03", "LABORATORY EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 64, "SP", "SP", "SPORTS EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605130", "01", "SPORTS EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 65, "SP", "SP", "SPORTS EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605130", "02", "TABLE/BOARD EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 66, "SP", "SP", "SPORTS EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605130", "03", "WEIGHT PLATES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 67, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "01", "DRAFTING EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 68, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "02", "TENSILE/BURSTING STRENGTH TESTER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 69, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "03", "MOISTURE METERS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 70, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "04", "LABORATORY APPARATUS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 71, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "05", "HAND SHELLER -for laboratory use", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 72, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "06", "GAS MASK/GLOVES/FACE MASK", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 73, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "07", "BINOCULARS/SAFETY GOGGLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 74, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "08", "DIAL THERMOMETER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 75, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "09", "BIN PROBE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 76, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "10", "PEST CONTROL PROTECTIVE SUIT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 77, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "11", "ENGRAVER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 78, "TS", "TS", "TECHNICAL AND SCIENTIFIC EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605140", "12", "PALLET TRUCK", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 79, "OEO", "OEO", "OTHER EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605990", "01", "Kitchen Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 80, "OEO", "OEO", "OTHER EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10605990", "02", "Electric equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 81, "LT", "LT", "MOTOR VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606010", "01", "BUS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 82, "LT", "LT", "MOTOR VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606010", "02", "TRUCK", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 83, "LT", "LT", "MOTOR VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606010", "03", "ASIAN UTILITY VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 84, "LT", "LT", "MOTOR VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606010", "04", "JEEP", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 85, "LT", "LT", "MOTOR VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606010", "05", "TRAILERS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 86, "LT", "LT", "MOTOR VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606010", "06", "CARS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 87, "LT", "LT", "MOTOR VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606010", "07", "MOTORCYCLE/BIKES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 88, "LT", "LT", "MOTOR VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606010", "08", "CUSTOMBUILT VEHICLES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 89, "AC", "AC", "AIRCRAFTS AND AIRCRAFTS GROUND EQUIPMENTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606030", "01", "AIRPLANES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 90, "AC", "AC", "AIRCRAFTS AND AIRCRAFTS GROUND EQUIPMENTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606030", "02", "HELICOPTERS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 91, "AC", "AC", "AIRCRAFTS AND AIRCRAFTS GROUND EQUIPMENTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606030", "03", "LIFE VEST/LIFE RING & ACCESSORIES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 92, "WC", "WC", "WATERCRAFTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606040", "01", "MOTORBOAT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 93, "WC", "WC", "WATERCRAFTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606040", "02", "ENGINE FOR MOTORBOAT/RUBBERBOAT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 94, "WC", "WC", "WATERCRAFTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606040", "03", "BARGE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 95, "WC", "WC", "WATERCRAFTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606040", "04", "AMPHIBIAN", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 96, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "01", "HYDRAULIC JACK", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 97, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "02", "AIR COMPRESSOR", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 98, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "03", "WELDING MACHINE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 99, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "04", "ACETYLYNE TORCH", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 100, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "05", "BATTERY CHARGER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 101, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "06", "CHAIN BLOCK", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 102, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "07", "ENGINE ANALYZER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 103, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "08", "TIRE CHANGER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 104, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "09", "CAR/TRUCK/WASHING EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 105, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "10", "VALVE REFACER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 106, "OT", "OT", "Other Transportation Equipment", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10606990", "11", "WHEEL BALANCING EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 107, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "01", "BED", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 108, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "02", "TABLE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 109, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "03", "BOOKSHELVES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 110, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "04", "PAINTING/FRAMES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 111, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "05", "ALL TYPES OF VERTICAL AND VENITIAN BLINDS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 112, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "06", "CART", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 113, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "07", "RACK", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 114, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "08", "PLANT BOX/ DIVIDER", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 115, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "09", "CABINET", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 116, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "10", "CHAIR", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 117, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "11", "SALA SET/SOFA/BENCH", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 118, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "12", "BOARD/SIGNAGES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 119, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "13", "FOLDING STAGE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 120, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "14", "ROSTRUM/DICTIONARY/BIBLE STAND", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 121, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "15", "MIRROR WITH FRAME /STAND", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 122, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "16", "ELEVATOR GUIDE", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 123, "FF", "FF", "FURNITURES AND FIXTURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607010", "17", "ASTRAY/WASTEBASKET/TRASHCAN", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 124, "LB", "LB", "BOOKS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10607020", "01", "BOOKS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 125, "LA", "LA", "LEASED ASSETS IMPROVEMENTS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10609010", "01", "Leased Assets Improvements - Land", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 126, "LA", "LA", "LEASED ASSETS IMPROVEMENTS BUILDING", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10609020", "01", "Leased Assets Improvements Building", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 127, "CIP", "CIP", "CONSTRUCTION IN PROGRESS", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10610010", "01", "Construction in progress - Land Improvements", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 128, "CIP", "CIP", "CONSTRUCTION IN PROGRESS- BUILDING", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10610030", "01", "Construction in progress -Building", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 129, "CIP", "CIP", "CONSTRUCTION IN PROGRESS- BUILDING", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10610030", "02", "Construction in progress -Warehouse", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 130, "CIP", "CIP", "CONSTRUCTION IN PROGRESS- EQUIPMENT FURNITURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10610060", "01", "Construction in Progress - Equipments", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 131, "CIP", "CIP", "CONSTRUCTION IN PROGRESS- EQUIPMENT FURNITURES", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10610060", "02", "Construction in Progress - Furnitures & Fixtures", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 132, "HT", "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10698990", "01", "HANDTOOLS (HT)", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 133, "SE", "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10698990", "02", "Ordnance Non-Expendable Supplies & Equipment (SE)", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 134, "LF", "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10698990", "03", "LIGHTNING FACILITIES (LF)", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 135, "KF", "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10698990", "04", "STORE EQUIPMENT (KF)", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 136, "TN", "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10698990", "05", "TENT/Fumigating Sheet (TN)", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 137, "PD", "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10698990", "06", "PETROLEUM DISPENSER W/DIESEL TANK (PD)", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { 138, "OP", "OP", "OTHER PROPERTY PLANT AND EQUIPMENT", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "10698990", "07", "COLD STORAGE ROOM", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), 0 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("1c36ce8b-91d3-47db-b60d-205d34eb524a"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("236f1166-3360-4368-8da7-2cee2b670721"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("47b56c02-75f5-44e8-b209-b5f821834886"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("585e3d94-c3c4-4ad6-98d9-de2dabc45f4e"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("59edba6a-5678-4823-9b32-252b17d9af28"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("628c07ae-8c1c-45f7-a6d7-1136eb01b9d6"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("752476b7-58e3-4f5e-a8e0-29d45fc6f9d3"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("83a766bf-95fa-4844-9a80-ceeb7f42d72b"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("87e85a94-c532-4177-9a8a-907040c9b013"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("91186001-b2c5-4faf-99c3-6b9bea3518e4"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("9d3f8a2f-1b31-4609-87d8-e30fe7bde26c"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("a0568313-645e-4db5-ab2d-1d4e25585849"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("a105d59a-cef8-4be6-b682-0f702199d9db"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("a721c410-0db8-4fbf-af71-e1e4a0038fa2"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("b31f7aef-c02c-44d9-9b19-5f7e3b47cbab"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("bb4ca419-3e3a-4d3f-be00-978e0c5f1560"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("bd2d2fae-8c18-4b68-84b6-82832160ea5a"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("e824ebde-56d7-4796-b321-1a229daf84dd"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("ecf0f606-da73-4fd6-b3bf-ed6353f8a763"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 }
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
                name: "IX_Inspections_EmployeeId",
                schema: "inventories",
                table: "Inspections",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PhysicalAssetId",
                schema: "inventories",
                table: "Inspections",
                column: "PhysicalAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PurchaseId",
                schema: "inventories",
                table: "Inspections",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_Status",
                schema: "inventories",
                table: "Inspections",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_Type",
                schema: "inventories",
                table: "Inspections",
                column: "Type");

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
                name: "IX_NfaOfficeCodes_Code",
                schema: "inventories",
                table: "NfaOfficeCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_DisposalDate",
                schema: "inventories",
                table: "PhysicalAssets",
                column: "DisposalDate");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalAssets_ParentAssetId",
                schema: "inventories",
                table: "PhysicalAssets",
                column: "ParentAssetId");

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
                name: "IX_PPECategoryCodes_Code",
                schema: "inventories",
                table: "PPECategoryCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PPEIR_IRNumber",
                schema: "inventories",
                table: "PPEIR",
                column: "IRNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PPEIRLineItem_PPEIRId",
                schema: "inventories",
                table: "PPEIRLineItem",
                column: "PPEIRId");

            migrationBuilder.CreateIndex(
                name: "IX_PPEIRLineItem_PPEIRId1",
                schema: "inventories",
                table: "PPEIRLineItem",
                column: "PPEIRId1");

            migrationBuilder.CreateIndex(
                name: "IX_PPEItemCodes_ClassCode_CategoryCode_Code",
                schema: "inventories",
                table: "PPEItemCodes",
                columns: new[] { "ClassCode", "CategoryCode", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PPERR_RRNumber",
                schema: "inventories",
                table: "PPERR",
                column: "RRNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PPERRLineItem_PPERRId",
                schema: "inventories",
                table: "PPERRLineItem",
                column: "PPERRId");

            migrationBuilder.CreateIndex(
                name: "IX_PPETypeAccountMappings_PPEType",
                schema: "inventories",
                table: "PPETypeAccountMappings",
                column: "PPEType",
                unique: true,
                filter: "\"IsActive\" = true AND \"Deleted\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PPETypeCodes_CategoryId_Code",
                schema: "inventories",
                table: "PPETypeCodes",
                columns: new[] { "CategoryId", "Code" },
                unique: true);

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
                name: "IX_ProjectBudgets_ProjectId",
                schema: "inventories",
                table: "ProjectBudgets",
                column: "ProjectId",
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
                name: "IX_PropertyCodeSequences_ClassCode_CategoryCode_ItemCode",
                schema: "inventories",
                table: "PropertyCodeSequences",
                columns: new[] { "ClassCode", "CategoryCode", "ItemCode" });

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
                name: "IX_SuppliesAndMaterialsReceivingLineItem_SuppliesAndMaterialsR~",
                schema: "inventories",
                table: "SuppliesAndMaterialsReceivingLineItem",
                column: "SuppliesAndMaterialsReceivingReportId");

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
                name: "AssetDisposals",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "AssetMaintenances",
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
                name: "NfaOfficeCodes",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPEIRLineItem",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPEItemCodes",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPERRLineItem",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPETypeAccountMappings",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPETypeCodes",
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
                name: "PropertyAcknowledgementReceipt",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PropertyCodeSequences",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PurchaseRequestItems",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "RcaAccountCodes",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "SemexRegistry",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "SemexTransactionLog",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "SuppliesAndMaterialsReceivingLineItem",
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
                name: "PPEIR",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPERR",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "PPECategoryCodes",
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
                name: "PhysicalAssets",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Purchases",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Canvasses",
                schema: "inventories");

            migrationBuilder.DropTable(
                name: "Categories",
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
