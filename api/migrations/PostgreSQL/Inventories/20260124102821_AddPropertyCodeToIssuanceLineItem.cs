using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class AddPropertyCodeToIssuanceLineItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("5d9263de-e5fb-4f58-b930-2028dcf2c8a0"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("aa635205-5079-4222-bc27-43c92ed71157"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("acceae57-4620-4249-8a93-3f8e5e72c9c2"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("e6f3b00f-d125-421c-ba38-6214fc67d8d0"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("ee20580b-9ffb-4079-8d7b-e86bcead292b"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("0aba5db9-db62-4686-9560-8af03efafa60"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("316f6324-68f6-462d-a6d2-e7b7ab4a01c3"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("3c01b487-d17f-4fa0-8394-d78e6faf6402"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("c969f975-2567-4138-b02c-fab8f9dd48ff"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("ca30dcf2-b1e2-4581-ad06-efcc881fc72d"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("1771185c-aace-419f-a6cf-104a07f09247"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("81a8575b-34a0-4fe4-af62-1af49d991de9"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("b97868ec-4c69-4ad8-b861-b7f759c3031c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("ce42ce96-0b92-4cb1-9496-330af1c989fc"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("e4642772-3ad6-4736-a540-ff9c73893f06"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("11087ecd-b56d-4fe7-9f40-de602c73bcbf"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("11c8de38-bc26-4ac7-a3ff-4cc8f87afa3e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("206de9f8-4146-4a45-b528-1c73f3b1ea42"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("2410eeaa-c2a4-46d7-9320-b4337f141c3f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("2a1b6921-aaaf-44bb-b96f-041eaa4120a7"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("6b3aa276-0e7f-42d0-a249-8e098d1f8f47"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("6b96c7dd-5e18-4a0c-9c3f-6e958a23ab57"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("8078b956-3266-4b10-a434-473f37093ea8"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("96a5630f-2ac5-4042-b62c-20f019f0a9b9"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9a3e2cdd-fa25-4d6c-8577-d767afc5ed79"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a2747561-2a22-476d-aeac-2c6b44096a61"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a411ca10-9dd2-4bd0-a43e-902dd7e88828"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("aceb9d77-ccf2-4cc6-93aa-9e37f9aa7aed"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b231b179-2545-4d16-82c8-cf5117e85be9"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("cb2b19ea-ff7c-4b71-89df-9787b83ca65b"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d8d66d3b-f3e3-45b2-b6af-e6bcbb296662"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d9c8d74f-6bfb-4651-9e0d-c0a7b04a4754"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("e8f21e33-8691-474c-8734-98550197a35e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("fd80e628-d666-4736-84bb-10402bfb4621"));

            migrationBuilder.AddColumn<string>(
                name: "PropertyCode",
                schema: "inventories",
                table: "IssuanceLineItem",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

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
                name: "PropertyCode",
                schema: "inventories",
                table: "IssuanceLineItem");

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
        }
    }
}
