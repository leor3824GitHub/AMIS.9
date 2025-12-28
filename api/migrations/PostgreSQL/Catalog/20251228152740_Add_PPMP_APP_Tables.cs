using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class Add_PPMP_APP_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("0789331e-58e7-4f66-a652-635a0551a46b"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("5eabab5c-70dc-4f4c-8590-1a72549e72df"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("78718dd2-5c13-4858-8782-6d1e97e3a7b4"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("a56cf2f9-f4c4-4e5c-9f95-29b52555f48b"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("fd0b9627-8e6a-4f7d-89de-349ff1c74e7a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("1e19ea5a-5a21-4d03-921a-fbc5f7172c20"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("9b2f600a-699d-4e4d-865f-e5e1ceaeffd5"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("a6c90687-944a-4c91-a901-812598526dd7"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("b16bc2fe-1ad3-473a-aa85-f0859bb97939"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("c02ee30e-32f4-46c5-ad4c-6fff5f001145"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("012dd80f-5bd9-441a-b1b9-ef096498e28d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("14a2a3e2-a759-4ff9-84f4-d7ebb6bf941f"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("211853aa-2dc9-440e-ab4a-0e291dd91bda"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("333f8997-8bf2-4234-b2d9-9d93a75baa8f"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("34eb97b3-e1a4-4937-96e3-63bc1e490e17"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("6eccc2fe-69bc-4f58-b002-5e3babcd2226"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("85588636-bdfd-497d-81be-f20f0081c47a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("86c4e8eb-2b47-47e1-80ee-4c05b84d0d35"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("8c973db3-8299-405e-8f9f-04f68d93a7de"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("8de58e5f-f8b3-4d7b-8906-8256114febaf"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9ea755ef-8073-4591-aa11-dd401984fa44"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a011807a-46be-491c-b598-62068e92fd1d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b369748f-9a58-48cd-8207-5e289d204110"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("bd68beba-2569-4751-9ede-905e521a27ce"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("c34af435-9e81-4a1f-9f81-82f7ce77061c"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("c3d4f102-3787-4a37-8b37-ff043860d35a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d16ae88a-5423-418b-a841-ab4db11a7790"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d7ff2374-0bb8-41a2-bddb-54600a45af4e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("e19bac2a-4650-46f6-bad4-51ab92c83ee1"));

            migrationBuilder.CreateTable(
                name: "AnnualProcurementPlans",
                schema: "catalog",
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
                name: "ProcurementPlans",
                schema: "catalog",
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
                schema: "catalog",
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
                name: "AnnualProcurementPlanItems",
                schema: "catalog",
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
                        principalSchema: "catalog",
                        principalTable: "AnnualProcurementPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementPlanItems",
                schema: "catalog",
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
                        principalSchema: "catalog",
                        principalTable: "ProcurementPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementSchedules",
                schema: "catalog",
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
                        principalSchema: "catalog",
                        principalTable: "ProcurementProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBudgets",
                schema: "catalog",
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
                        principalSchema: "catalog",
                        principalTable: "ProcurementProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("25592d53-cec4-4cfc-9068-bc6471394106"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("4477d735-71f8-4e27-ac32-7946a1a1f71e"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("543456e1-dde6-49d9-a28f-2935b853a969"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("62176187-bb98-49fa-a364-9f43e9fa76ce"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("ef774f30-68a1-4564-839e-1ba3dbccdb64"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("46edc254-2b6b-4a73-a2ab-cdb9f09af385"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("47f0f821-50f8-4a79-9cd5-bb257702e01d"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("96a26e87-ab9a-44eb-b99b-c0c1ee8e979d"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("c655483a-ec6c-47b2-8d15-c6c5b95576eb"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("e4c5c7d9-14c9-47be-bfe5-de49c4367b9d"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("0040d758-8b76-4d53-8224-aae8066b0b8e"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("1f5d4b7e-ccd7-4e7f-b17f-3ef85dfe0a7f"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("36dc2842-6b29-4cb5-82f3-949ac206db2a"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("48ee3261-210a-4893-bd6f-dfc8d77b2b4e"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("56e9e4bc-99c9-4ea4-b22a-19a2939f5bfb"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("582b81a8-c6e4-4f98-8f7d-78af040cb952"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("5b6bba9e-4588-48ce-ab18-7d9ac6607d96"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("633c82a4-183b-423e-a0a3-058be1cb8777"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("6448c18a-d68d-41a8-9b3d-e40ca9320c16"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("77504f90-a0aa-4af4-b946-cc3543f3e3b4"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("85a7b756-1deb-4b46-9426-9dc13d188d47"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("9197e46e-6f57-4fc6-848d-05a6c00010a2"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("9bd88bb9-dba8-4d1c-8a78-7c0b6cfe48ae"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("aedfe8dd-8f2c-44ed-bf33-776598c75ffc"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("b023174d-9561-495a-b9d0-be872b55f4e6"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("b382764f-d935-43c2-8c58-ef32f5f1065e"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("c7431400-6fe4-4461-a727-b06c44fcbcf6"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("cb0e873c-2f3a-42b6-965d-fd73d3c65f48"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("f975927a-45c1-4aaa-8980-02b21b054064"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnualProcurementPlanItems_PlanHeaderId",
                schema: "catalog",
                table: "AnnualProcurementPlanItems",
                column: "PlanHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualProcurementPlans_TenantId_ControlNumber",
                schema: "catalog",
                table: "AnnualProcurementPlans",
                columns: new[] { "TenantId", "ControlNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlanItems_PlanHeaderId",
                schema: "catalog",
                table: "ProcurementPlanItems",
                column: "PlanHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_TenantId_ControlNumber",
                schema: "catalog",
                table: "ProcurementPlans",
                columns: new[] { "TenantId", "ControlNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementSchedules_ProjectId",
                schema: "catalog",
                table: "ProcurementSchedules",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBudgets_ProjectId",
                schema: "catalog",
                table: "ProjectBudgets",
                column: "ProjectId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualProcurementPlanItems",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "ProcurementPlanItems",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "ProcurementSchedules",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "ProjectBudgets",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "AnnualProcurementPlans",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "ProcurementPlans",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "ProcurementProjects",
                schema: "catalog");

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("25592d53-cec4-4cfc-9068-bc6471394106"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("4477d735-71f8-4e27-ac32-7946a1a1f71e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("543456e1-dde6-49d9-a28f-2935b853a969"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("62176187-bb98-49fa-a364-9f43e9fa76ce"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("ef774f30-68a1-4564-839e-1ba3dbccdb64"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("46edc254-2b6b-4a73-a2ab-cdb9f09af385"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("47f0f821-50f8-4a79-9cd5-bb257702e01d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("96a26e87-ab9a-44eb-b99b-c0c1ee8e979d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("c655483a-ec6c-47b2-8d15-c6c5b95576eb"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("e4c5c7d9-14c9-47be-bfe5-de49c4367b9d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("0040d758-8b76-4d53-8224-aae8066b0b8e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("1f5d4b7e-ccd7-4e7f-b17f-3ef85dfe0a7f"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("36dc2842-6b29-4cb5-82f3-949ac206db2a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("48ee3261-210a-4893-bd6f-dfc8d77b2b4e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("56e9e4bc-99c9-4ea4-b22a-19a2939f5bfb"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("582b81a8-c6e4-4f98-8f7d-78af040cb952"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("5b6bba9e-4588-48ce-ab18-7d9ac6607d96"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("633c82a4-183b-423e-a0a3-058be1cb8777"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("6448c18a-d68d-41a8-9b3d-e40ca9320c16"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("77504f90-a0aa-4af4-b946-cc3543f3e3b4"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("85a7b756-1deb-4b46-9426-9dc13d188d47"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9197e46e-6f57-4fc6-848d-05a6c00010a2"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9bd88bb9-dba8-4d1c-8a78-7c0b6cfe48ae"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("aedfe8dd-8f2c-44ed-bf33-776598c75ffc"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b023174d-9561-495a-b9d0-be872b55f4e6"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b382764f-d935-43c2-8c58-ef32f5f1065e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("c7431400-6fe4-4461-a727-b06c44fcbcf6"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("cb0e873c-2f3a-42b6-965d-fd73d3c65f48"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("f975927a-45c1-4aaa-8980-02b21b054064"));

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("0789331e-58e7-4f66-a652-635a0551a46b"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("5eabab5c-70dc-4f4c-8590-1a72549e72df"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("78718dd2-5c13-4858-8782-6d1e97e3a7b4"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("a56cf2f9-f4c4-4e5c-9f95-29b52555f48b"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("fd0b9627-8e6a-4f7d-89de-349ff1c74e7a"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("1e19ea5a-5a21-4d03-921a-fbc5f7172c20"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("9b2f600a-699d-4e4d-865f-e5e1ceaeffd5"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("a6c90687-944a-4c91-a901-812598526dd7"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("b16bc2fe-1ad3-473a-aa85-f0859bb97939"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("c02ee30e-32f4-46c5-ad4c-6fff5f001145"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("012dd80f-5bd9-441a-b1b9-ef096498e28d"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("14a2a3e2-a759-4ff9-84f4-d7ebb6bf941f"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("211853aa-2dc9-440e-ab4a-0e291dd91bda"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("333f8997-8bf2-4234-b2d9-9d93a75baa8f"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("34eb97b3-e1a4-4937-96e3-63bc1e490e17"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("6eccc2fe-69bc-4f58-b002-5e3babcd2226"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("85588636-bdfd-497d-81be-f20f0081c47a"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("86c4e8eb-2b47-47e1-80ee-4c05b84d0d35"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("8c973db3-8299-405e-8f9f-04f68d93a7de"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("8de58e5f-f8b3-4d7b-8906-8256114febaf"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("9ea755ef-8073-4591-aa11-dd401984fa44"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("a011807a-46be-491c-b598-62068e92fd1d"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("b369748f-9a58-48cd-8207-5e289d204110"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("bd68beba-2569-4751-9ede-905e521a27ce"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("c34af435-9e81-4a1f-9f81-82f7ce77061c"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("c3d4f102-3787-4a37-8b37-ff043860d35a"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("d16ae88a-5423-418b-a841-ab4db11a7790"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("d7ff2374-0bb8-41a2-bddb-54600a45af4e"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("e19bac2a-4650-46f6-bad4-51ab92c83ee1"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 }
                });
        }
    }
}
