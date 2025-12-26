using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddPurchaseRequestItemManualProductName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("41d4a251-c8f4-48a5-a4ed-f7fe33cd079f"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("669f921b-76a4-4aed-932a-15afa3c288d2"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("6fe8ff84-7667-4247-86ad-eb0db427242c"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("a568e767-7b46-41f5-a57f-261c4b2b94ae"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("ae49e9fd-a2bd-41aa-ae86-ba964448b63e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("2025d61f-9cb2-4a8c-93a4-d40df637d5f4"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("4dba4769-1cfe-4042-832c-c7fd5409fe76"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("50658792-65c3-4d41-b087-a20aa06329fc"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("a6b2cfdb-3741-4920-8422-c9fe57ccc2be"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("b96aa336-e2ba-471b-b452-4ea434a0ed98"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("14682b4b-614f-475b-8e47-a5305f16a965"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("23a4de7b-0505-473e-bee8-f3cf31026941"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("298e7e87-bd06-436b-a929-ca660810cdf2"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("4019ba90-159e-4e7a-8c31-898fd89fb6b3"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("4fb0b91c-1816-43c9-a6fc-33a934315f86"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("50a8abab-d0b5-4c26-946f-287c7ee1e91c"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("725a5187-13b2-44d4-8dcc-7e3c17be8b61"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("7c4d5910-da2a-44df-9d2b-d21929f198af"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("7fef522a-f64a-4b56-88cc-7ad1806ca845"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("847cf8f5-7034-49e2-a4e1-1e7df481945d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("8d575521-d80e-45cf-af3b-8c35d96156a8"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9256c4da-4bed-45e8-b2f0-c11064d67a84"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("937de7a4-1fe2-42a7-b1bd-4c19ba1d694a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("95f4da69-c36a-4357-af6a-5d9ae771c2b6"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("aadca10b-24d2-4b05-9fb3-f049db139c11"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("af3c2bfc-4268-4fc6-a152-0ef6a819a0ca"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("bbc37a18-bbff-44ad-9883-154a77718365"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("e105e3f4-b17d-4667-8c57-02d72f5f1367"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("f8ae353f-d899-46fa-8dcd-b5eee344ae5c"));

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseRequestId",
                schema: "catalog",
                table: "Purchases",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedCanvassId",
                schema: "catalog",
                table: "Purchases",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManualProductName",
                schema: "catalog",
                table: "PurchaseRequestItems",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GoodsReceiptId",
                schema: "catalog",
                table: "Acceptances",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GoodsReceipt",
                schema: "catalog",
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
                        principalSchema: "catalog",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceipt_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "catalog",
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceipt_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "catalog",
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceiptItem",
                schema: "catalog",
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
                        principalSchema: "catalog",
                        principalTable: "GoodsReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptItem_PurchaseItems_PurchaseItemId",
                        column: x => x.PurchaseItemId,
                        principalSchema: "catalog",
                        principalTable: "PurchaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PurchaseRequestId",
                schema: "catalog",
                table: "Purchases",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SelectedCanvassId",
                schema: "catalog",
                table: "Purchases",
                column: "SelectedCanvassId");

            migrationBuilder.CreateIndex(
                name: "IX_Acceptances_GoodsReceiptId",
                schema: "catalog",
                table: "Acceptances",
                column: "GoodsReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_PurchaseId",
                schema: "catalog",
                table: "GoodsReceipt",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_ReceivedById",
                schema: "catalog",
                table: "GoodsReceipt",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_SupplierId",
                schema: "catalog",
                table: "GoodsReceipt",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptItem_GoodsReceiptId",
                schema: "catalog",
                table: "GoodsReceiptItem",
                column: "GoodsReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptItem_PurchaseItemId",
                schema: "catalog",
                table: "GoodsReceiptItem",
                column: "PurchaseItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Acceptances_GoodsReceipt_GoodsReceiptId",
                schema: "catalog",
                table: "Acceptances",
                column: "GoodsReceiptId",
                principalSchema: "catalog",
                principalTable: "GoodsReceipt",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Canvasses_SelectedCanvassId",
                schema: "catalog",
                table: "Purchases",
                column: "SelectedCanvassId",
                principalSchema: "catalog",
                principalTable: "Canvasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_PurchaseRequests_PurchaseRequestId",
                schema: "catalog",
                table: "Purchases",
                column: "PurchaseRequestId",
                principalSchema: "catalog",
                principalTable: "PurchaseRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acceptances_GoodsReceipt_GoodsReceiptId",
                schema: "catalog",
                table: "Acceptances");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Canvasses_SelectedCanvassId",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_PurchaseRequests_PurchaseRequestId",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.DropTable(
                name: "GoodsReceiptItem",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "GoodsReceipt",
                schema: "catalog");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PurchaseRequestId",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_SelectedCanvassId",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Acceptances_GoodsReceiptId",
                schema: "catalog",
                table: "Acceptances");

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

            migrationBuilder.DropColumn(
                name: "PurchaseRequestId",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "SelectedCanvassId",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ManualProductName",
                schema: "catalog",
                table: "PurchaseRequestItems");

            migrationBuilder.DropColumn(
                name: "GoodsReceiptId",
                schema: "catalog",
                table: "Acceptances");

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("41d4a251-c8f4-48a5-a4ed-f7fe33cd079f"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("669f921b-76a4-4aed-932a-15afa3c288d2"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("6fe8ff84-7667-4247-86ad-eb0db427242c"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("a568e767-7b46-41f5-a57f-261c4b2b94ae"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("ae49e9fd-a2bd-41aa-ae86-ba964448b63e"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("2025d61f-9cb2-4a8c-93a4-d40df637d5f4"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("4dba4769-1cfe-4042-832c-c7fd5409fe76"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("50658792-65c3-4d41-b087-a20aa06329fc"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("a6b2cfdb-3741-4920-8422-c9fe57ccc2be"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("b96aa336-e2ba-471b-b452-4ea434a0ed98"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("14682b4b-614f-475b-8e47-a5305f16a965"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("23a4de7b-0505-473e-bee8-f3cf31026941"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("298e7e87-bd06-436b-a929-ca660810cdf2"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("4019ba90-159e-4e7a-8c31-898fd89fb6b3"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("4fb0b91c-1816-43c9-a6fc-33a934315f86"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("50a8abab-d0b5-4c26-946f-287c7ee1e91c"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("725a5187-13b2-44d4-8dcc-7e3c17be8b61"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("7c4d5910-da2a-44df-9d2b-d21929f198af"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("7fef522a-f64a-4b56-88cc-7ad1806ca845"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("847cf8f5-7034-49e2-a4e1-1e7df481945d"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("8d575521-d80e-45cf-af3b-8c35d96156a8"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("9256c4da-4bed-45e8-b2f0-c11064d67a84"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("937de7a4-1fe2-42a7-b1bd-4c19ba1d694a"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("95f4da69-c36a-4357-af6a-5d9ae771c2b6"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("aadca10b-24d2-4b05-9fb3-f049db139c11"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("af3c2bfc-4268-4fc6-a152-0ef6a819a0ca"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("bbc37a18-bbff-44ad-9883-154a77718365"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("e105e3f4-b17d-4667-8c57-02d72f5f1367"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("f8ae353f-d899-46fa-8dcd-b5eee344ae5c"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 }
                });
        }
    }
}
