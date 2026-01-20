using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class AddPPERRListSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("0441315f-d76f-40d5-952f-a1f85e1cf62a"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("1b76dcbc-90d3-442d-a696-c21d35e5bdb6"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("b0589b6d-e8ac-4c41-a436-a2460dddd6ad"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("cea157fb-a51b-4479-be8d-9770c42f7ba4"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("e5997004-66d9-4386-8396-a2dd5bf690bb"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("394e2ad2-645e-4c26-9364-ee5906f76155"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("5083b9f4-07e6-4dc2-a86e-3cb82ce229e5"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("a252df7c-3ab5-4754-bb76-6fe946e24c67"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("c1e9d25a-f367-4e0b-8178-e6f2d3e0be73"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("f6a1d714-c817-4c8b-b659-feacf1bb5c0f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("036d1a0e-d480-49ac-864c-05360a7d0fab"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("15b57ce7-4a14-4625-b778-7062bbcdcc3f"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("256c4553-eaeb-4431-a31a-42e44913b4d0"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("25cf6692-a20d-4131-9711-c0ea013117c5"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("26c67ecb-d862-47e4-ac4c-e0a07525d09a"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("47e2ff9a-3148-402f-82ff-9b345f30491a"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("5b0d8d06-1418-464b-95ea-539b7fbc655c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("5cdcbf71-7999-4872-bee8-0daafba0a515"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("6c6657a0-a1d3-4327-bf27-0f3a97f8fd5c"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("7060ca5b-81ea-4d25-aea2-9dde4aa021ac"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9be2df4c-9f9c-4147-942f-f68a18f9e322"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b0683b00-cc63-4c28-80bd-bb8dc3e2a6cf"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b4056f42-89bf-4ea4-842e-62feaa15cee5"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("c061ae28-d980-4e51-812d-2f07f30e1fff"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d57c3b61-c515-4fce-8f5c-5415222957e2"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("df69f2c8-17d0-4363-81b3-227364023321"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("e05810d5-da2b-455b-994d-7d0e1cfd5220"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("ef711ad5-320e-42c5-87f7-e551e193ad13"));

            migrationBuilder.DeleteData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("f2b20719-5856-4b2c-a080-6ebe868b24e2"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { new Guid("0441315f-d76f-40d5-952f-a1f85e1cf62a"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("1b76dcbc-90d3-442d-a696-c21d35e5bdb6"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("b0589b6d-e8ac-4c41-a436-a2460dddd6ad"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("cea157fb-a51b-4479-be8d-9770c42f7ba4"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("e5997004-66d9-4386-8396-a2dd5bf690bb"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("394e2ad2-645e-4c26-9364-ee5906f76155"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("5083b9f4-07e6-4dc2-a86e-3cb82ce229e5"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("a252df7c-3ab5-4754-bb76-6fe946e24c67"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("c1e9d25a-f367-4e0b-8178-e6f2d3e0be73"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("f6a1d714-c817-4c8b-b659-feacf1bb5c0f"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 }
                });

            migrationBuilder.InsertData(
                schema: "inventories",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("036d1a0e-d480-49ac-864c-05360a7d0fab"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("15b57ce7-4a14-4625-b778-7062bbcdcc3f"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("256c4553-eaeb-4431-a31a-42e44913b4d0"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("25cf6692-a20d-4131-9711-c0ea013117c5"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("26c67ecb-d862-47e4-ac4c-e0a07525d09a"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("47e2ff9a-3148-402f-82ff-9b345f30491a"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("5b0d8d06-1418-464b-95ea-539b7fbc655c"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("5cdcbf71-7999-4872-bee8-0daafba0a515"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("6c6657a0-a1d3-4327-bf27-0f3a97f8fd5c"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("7060ca5b-81ea-4d25-aea2-9dde4aa021ac"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("9be2df4c-9f9c-4147-942f-f68a18f9e322"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("b0683b00-cc63-4c28-80bd-bb8dc3e2a6cf"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("b4056f42-89bf-4ea4-842e-62feaa15cee5"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("c061ae28-d980-4e51-812d-2f07f30e1fff"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("d57c3b61-c515-4fce-8f5c-5415222957e2"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("df69f2c8-17d0-4363-81b3-227364023321"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("e05810d5-da2b-455b-994d-7d0e1cfd5220"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("ef711ad5-320e-42c5-87f7-e551e193ad13"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("f2b20719-5856-4b2c-a080-6ebe868b24e2"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 }
                });
        }
    }
}
