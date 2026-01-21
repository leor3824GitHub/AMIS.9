using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class AddLocationToLineItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("376f33a9-08ab-4b79-8949-ceb133aa3734"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("460c4bbd-dc67-463d-a610-8c3d77548099"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("63616fa9-7d5d-4b3f-8102-3d946c1ebd01"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("6cd6dadf-6588-421a-ae1a-1cce06144296"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("d5914143-68a0-43b2-8745-f9c484d443f8"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("1dc24d46-5aab-446f-aa03-1155c9b3fa56"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("26d99862-7c01-4474-9694-b69e52d939c6"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("519fb925-efbc-432e-9875-9cc709214f1b"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("7830ae43-83f5-46ec-8f3d-bea60b1bd609"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("8a6b03bc-249a-4f9a-97a8-01b191913ced"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("10c2a637-9474-4d92-b850-20a652b9d44e"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("2bfcedc7-4845-4f68-9ae6-961fb61055cc"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("3b3353dd-f62f-412d-b443-96d6dce3acbe"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("480992de-5db8-428a-8385-7e070e979f5d"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("49f654b1-4f36-4532-9045-b2f6c85962c8"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("4f2de37b-e816-434a-89e1-a01beb6dba09"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("4f5105bc-592f-4aa2-ac9b-a30e212625fd"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("68bf424c-ed5c-4f22-a409-e8f9ea6e4152"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("75bdc8a8-e5fb-4d23-b813-47e972957508"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("769f2bfc-5cb3-4805-afc6-433f8bb78431"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("826b3bcc-1a11-47d6-b4bc-3f29aa9e971c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("82fa9caf-c352-46a9-9cfc-55a6c95b1408"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("8cb08a89-0fcd-446e-b18e-9d5c86e8f8aa"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9e08ace6-6d6b-493b-abd2-205e0a69ece4"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a26d31e3-64ce-406a-a562-12745372bedd"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d34b6410-af2d-4b18-805b-1cfda7b1e4a0"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("e050be20-e6e7-4b24-b9c2-f71b0b710dd3"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("edc61dea-6f42-455d-923a-4c87030a22ce"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("f3cb45d0-a763-494d-9ce8-5bb14f469b2d"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { new Guid("376f33a9-08ab-4b79-8949-ceb133aa3734"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("460c4bbd-dc67-463d-a610-8c3d77548099"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("63616fa9-7d5d-4b3f-8102-3d946c1ebd01"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("6cd6dadf-6588-421a-ae1a-1cce06144296"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("d5914143-68a0-43b2-8745-f9c484d443f8"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("1dc24d46-5aab-446f-aa03-1155c9b3fa56"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("26d99862-7c01-4474-9694-b69e52d939c6"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("519fb925-efbc-432e-9875-9cc709214f1b"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("7830ae43-83f5-46ec-8f3d-bea60b1bd609"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("8a6b03bc-249a-4f9a-97a8-01b191913ced"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("10c2a637-9474-4d92-b850-20a652b9d44e"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("2bfcedc7-4845-4f68-9ae6-961fb61055cc"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("3b3353dd-f62f-412d-b443-96d6dce3acbe"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("480992de-5db8-428a-8385-7e070e979f5d"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("49f654b1-4f36-4532-9045-b2f6c85962c8"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("4f2de37b-e816-434a-89e1-a01beb6dba09"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("4f5105bc-592f-4aa2-ac9b-a30e212625fd"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("68bf424c-ed5c-4f22-a409-e8f9ea6e4152"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("75bdc8a8-e5fb-4d23-b813-47e972957508"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("769f2bfc-5cb3-4805-afc6-433f8bb78431"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("826b3bcc-1a11-47d6-b4bc-3f29aa9e971c"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("82fa9caf-c352-46a9-9cfc-55a6c95b1408"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("8cb08a89-0fcd-446e-b18e-9d5c86e8f8aa"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("9e08ace6-6d6b-493b-abd2-205e0a69ece4"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("a26d31e3-64ce-406a-a562-12745372bedd"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("d34b6410-af2d-4b18-805b-1cfda7b1e4a0"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("e050be20-e6e7-4b24-b9c2-f71b0b710dd3"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("edc61dea-6f42-455d-923a-4c87030a22ce"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("f3cb45d0-a763-494d-9ce8-5bb14f469b2d"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 }
                });
        }
    }
}
