using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class UpdateInventoryRegistrySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("2363fd33-c248-489f-b134-352fa056e8e4"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("7ace8870-d2ce-483a-9a92-d646a017d32f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("bc1b176a-f7be-43be-856f-eb1f33502edd"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("e2358a8d-a717-44aa-bd93-82bf6022fa25"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("f585921c-9e26-4bd5-873a-fc230ee70f2e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("381b58eb-1964-4737-ab43-77a4760b5e88"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("510d1da8-013d-458d-adf6-e1e57ef1fca2"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("91bc29cc-2800-4255-9a3b-2092e0a69663"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("a3c92223-fd85-425c-9370-63e814f3fe57"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("fe045f9c-6ce0-45c9-bb9c-c8628dbe4d2f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("01ec6bdd-99b8-49c8-960d-a725601d016f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("02d81d09-c8a0-48c0-93ae-b509ca233486"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("030eda61-478b-4ac2-ba43-59c5dd773c66"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("05de7ad7-810b-4de4-8b2b-f50fc143a837"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("11658efb-c6c3-4bc8-9531-b84267890d4d"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("139cc5e0-4f81-4344-b253-edcc1d580fad"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("26cf201a-5b49-444b-9f1b-077726797f42"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("5d960e9c-5e46-4a5b-866b-a256f9f75cbb"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("5e915490-b163-46f1-a497-cd2d1adc2666"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("61b9dbdd-3ca4-4a1e-b209-95fcd43d54b3"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9eccbbcb-1b7a-4915-9756-44fd24291263"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a1878b97-ff92-4dd9-a455-f012ebefbc5d"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a3fe11aa-4695-43a3-834b-c185b28e2194"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a50557bd-839f-48bd-bade-7c06c49e8c6e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("cb8e735e-d607-47f9-9bd4-bbc614c233c4"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("cd55e9e2-d999-4f5d-a188-d004d9692adb"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d6b7a2a0-a337-4e19-bc37-7d66369d8768"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("e7d2b153-5ebe-4c44-a816-f4a5e3a0667c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("e9c8acde-76eb-43f1-8ced-87e766a9cd91"));

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("302fd823-5df3-467d-8774-66c11f439d37"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("aed52624-e3ba-4b8b-ae48-e2b782d9df11"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("e241f556-0f59-459f-be16-3ecabb4f704c"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("f3bd0d94-97c4-4b07-a37b-fe5af572815e"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("fe18a2be-a8a5-4caa-8f22-2907e560c2fc"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "InventoryRegistry",
                columns: new[] { "Id", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "IssuedDate", "LastModified", "LastModifiedBy", "LastTransactionDate", "LastTransactionReference", "LastTransactionType", "Location", "PropertyCode", "Quantity", "ReceivedDate", "Status" },
                values: new object[,]
                {
                    { new Guid("2912ac6f-5caa-4003-9f28-b57a458b1e86"), new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Desktop Computer", null, new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), "PPERR-001", "PPERR", "IT Office - Room 101", "234", 10, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), 1 },
                    { new Guid("6804d388-2f4e-4efa-a5c2-d42db7575edb"), new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Office Chair", null, new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), "PPERR-002", "PPERR", "Main Office", "237", 20, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), 1 },
                    { new Guid("81d2979e-3938-41e5-a050-e6b3d9f2354b"), new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Desk Lamp", null, new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), "PPERR-002", "PPERR", "Main Office", "238", 15, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), 1 },
                    { new Guid("85dafa67-f968-40d2-9985-ef9049cae480"), new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Printer", null, new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), "PPERR-001", "PPERR", "IT Office - Room 103", "236", 3, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), 1 },
                    { new Guid("d531c9e5-81c2-4caa-ba82-e8de3ce81d4f"), new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000001"), null, null, "Laptop Computer", null, new DateTimeOffset(new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Unspecified).AddTicks(5040), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), "PPERR-001", "PPERR", "IT Office - Room 102", "235", 5, new DateTime(2026, 1, 22, 6, 52, 44, 136, DateTimeKind.Utc).AddTicks(5040), 1 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("039b0c00-c144-449a-ba27-232ab7b7aa7e"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("0881dbd7-ebf8-4e8b-9bdd-0684ea6ccd8c"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("0dc3aedf-6fa0-45d2-b855-097c2c87622d"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("16983efc-de31-40cd-8148-eaab73ef7ceb"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("dd712e1a-8bbe-4c25-8f20-b692a4c0fe9c"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("005aae90-2fe3-40dc-8f64-70b4ffaf2cee"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("032395a1-e03f-419a-bea7-296fb4cf9f7b"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("033faf73-b51e-49f7-ad4b-f1da2e12243e"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("0db935ba-78f9-4480-b7eb-daae5b22b065"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("1be7e316-025e-4c5b-ae9c-5bdd7e426c61"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("21db3a5e-7e27-4d7d-ae18-a9adb7470d59"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("21ed1943-c69c-45ab-8b63-2cdfeff1b604"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("4cb891d7-ce6f-44ff-8041-31828bc9294a"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("4e312d36-7382-44ba-a2da-87c9d2f14823"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("751197ab-9fd0-4bde-bb40-7ddd6a8ec666"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("79b80d10-9328-44c7-90a1-cc5e200209fc"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("9e9c92ad-7162-4831-a0ed-620ef66ca70e"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("b539e198-85a7-4478-9a48-f30dab7308b5"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("b9745c46-8d7a-4621-b1bc-5f203128c17c"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("bc338fdb-1580-4aae-9a3b-818f3d0ec8ee"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("bdfe6305-15b7-4ba1-aa52-3fe4d34a35ee"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("c2175ab4-730e-4a7f-9450-9d02ee8bcbc4"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("c8aa8c93-3fe6-428d-94d4-b32d910e34f3"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("ffad33cd-80c7-459a-a077-d13eea806315"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("302fd823-5df3-467d-8774-66c11f439d37"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("aed52624-e3ba-4b8b-ae48-e2b782d9df11"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("e241f556-0f59-459f-be16-3ecabb4f704c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("f3bd0d94-97c4-4b07-a37b-fe5af572815e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("fe18a2be-a8a5-4caa-8f22-2907e560c2fc"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("2912ac6f-5caa-4003-9f28-b57a458b1e86"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("6804d388-2f4e-4efa-a5c2-d42db7575edb"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("81d2979e-3938-41e5-a050-e6b3d9f2354b"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("85dafa67-f968-40d2-9985-ef9049cae480"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "InventoryRegistry",
                keyColumn: "Id",
                keyValue: new Guid("d531c9e5-81c2-4caa-ba82-e8de3ce81d4f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("039b0c00-c144-449a-ba27-232ab7b7aa7e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("0881dbd7-ebf8-4e8b-9bdd-0684ea6ccd8c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("0dc3aedf-6fa0-45d2-b855-097c2c87622d"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("16983efc-de31-40cd-8148-eaab73ef7ceb"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("dd712e1a-8bbe-4c25-8f20-b692a4c0fe9c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("005aae90-2fe3-40dc-8f64-70b4ffaf2cee"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("032395a1-e03f-419a-bea7-296fb4cf9f7b"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("033faf73-b51e-49f7-ad4b-f1da2e12243e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("0db935ba-78f9-4480-b7eb-daae5b22b065"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("1be7e316-025e-4c5b-ae9c-5bdd7e426c61"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("21db3a5e-7e27-4d7d-ae18-a9adb7470d59"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("21ed1943-c69c-45ab-8b63-2cdfeff1b604"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("4cb891d7-ce6f-44ff-8041-31828bc9294a"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("4e312d36-7382-44ba-a2da-87c9d2f14823"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("751197ab-9fd0-4bde-bb40-7ddd6a8ec666"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("79b80d10-9328-44c7-90a1-cc5e200209fc"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9e9c92ad-7162-4831-a0ed-620ef66ca70e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b539e198-85a7-4478-9a48-f30dab7308b5"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b9745c46-8d7a-4621-b1bc-5f203128c17c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("bc338fdb-1580-4aae-9a3b-818f3d0ec8ee"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("bdfe6305-15b7-4ba1-aa52-3fe4d34a35ee"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("c2175ab4-730e-4a7f-9450-9d02ee8bcbc4"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("c8aa8c93-3fe6-428d-94d4-b32d910e34f3"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("ffad33cd-80c7-459a-a077-d13eea806315"));

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("2363fd33-c248-489f-b134-352fa056e8e4"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("7ace8870-d2ce-483a-9a92-d646a017d32f"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("bc1b176a-f7be-43be-856f-eb1f33502edd"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("e2358a8d-a717-44aa-bd93-82bf6022fa25"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("f585921c-9e26-4bd5-873a-fc230ee70f2e"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("381b58eb-1964-4737-ab43-77a4760b5e88"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("510d1da8-013d-458d-adf6-e1e57ef1fca2"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("91bc29cc-2800-4255-9a3b-2092e0a69663"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("a3c92223-fd85-425c-9370-63e814f3fe57"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("fe045f9c-6ce0-45c9-bb9c-c8628dbe4d2f"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("01ec6bdd-99b8-49c8-960d-a725601d016f"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("02d81d09-c8a0-48c0-93ae-b509ca233486"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("030eda61-478b-4ac2-ba43-59c5dd773c66"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("05de7ad7-810b-4de4-8b2b-f50fc143a837"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("11658efb-c6c3-4bc8-9531-b84267890d4d"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("139cc5e0-4f81-4344-b253-edcc1d580fad"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("26cf201a-5b49-444b-9f1b-077726797f42"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("5d960e9c-5e46-4a5b-866b-a256f9f75cbb"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("5e915490-b163-46f1-a497-cd2d1adc2666"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("61b9dbdd-3ca4-4a1e-b209-95fcd43d54b3"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("9eccbbcb-1b7a-4915-9756-44fd24291263"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("a1878b97-ff92-4dd9-a455-f012ebefbc5d"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("a3fe11aa-4695-43a3-834b-c185b28e2194"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("a50557bd-839f-48bd-bade-7c06c49e8c6e"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("cb8e735e-d607-47f9-9bd4-bbc614c233c4"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("cd55e9e2-d999-4f5d-a188-d004d9692adb"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("d6b7a2a0-a337-4e19-bc37-7d66369d8768"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("e7d2b153-5ebe-4c44-a816-f4a5e3a0667c"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("e9c8acde-76eb-43f1-8ced-87e766a9cd91"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 }
                });
        }
    }
}
