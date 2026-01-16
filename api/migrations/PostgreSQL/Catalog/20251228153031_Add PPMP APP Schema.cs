using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddPPMPAPPSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { new Guid("092b359d-d89b-4c2d-9f5a-52cfcc4bc8ce"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("142de110-f39c-487e-b374-92f3ce95b662"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("77ff15d6-a963-4621-9570-b8650b01811a"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("b19fa14a-242f-470b-b4fe-635d48e91dc5"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("d3127555-57e3-44d5-96b6-24502707aa38"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("4e1f4c63-0692-4b08-b9a2-9be055f2b656"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("8915bfe6-af8e-419a-ac07-46fa49d70a0f"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("b595fe6d-365d-489f-98fa-51c22ca58fc2"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("b85cf3da-5223-4c68-a54a-e62d831e2088"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("dc14fd17-ad6c-42f0-8c8a-12bf3b58f638"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("072ecd79-d5b8-4230-9e91-c199e6eaa29c"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("14183a13-ed6a-414c-b71c-febf1f4a8892"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("2852c07e-76f9-4b0a-9805-9959e02f2891"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("30186dbf-cdc8-46a0-8cb6-c1dcfebd0aed"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("3035d143-c4b4-40cd-9688-10ccdf5620bf"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("310c382c-4d28-4fb6-afc8-2d8e4843b66a"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("4e1d558b-3b5a-4a1b-84b0-eb6dda853ff3"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("67ecd901-d0e7-4716-a8cf-0b7931362e08"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("68deee69-aa53-4106-94d8-3be43d6cd2dc"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("695d9ed7-1e60-42a8-84bd-0cc182393d69"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("7a6a7ea7-cb5e-4aa2-99c6-30d1c9208040"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("926c0fa2-e546-4043-b1bb-af29e2077142"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("9e679ce3-54fb-417b-892c-e7ad92f702ac"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("a1a326c7-6c3b-4b6f-b830-e6856bceca37"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("b51ecd3b-be80-4f88-9a5f-fafd8852b208"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("ba79d06d-8eab-4f2b-a0b2-0da6ad92969c"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("c3c2c4ad-1426-4d7f-a532-acfef441b37c"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("ea3ca963-7f57-4ddb-b275-aff8fcbc3f12"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("ffcfba60-1317-4ed3-b4dc-2426fb22b358"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("092b359d-d89b-4c2d-9f5a-52cfcc4bc8ce"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("142de110-f39c-487e-b374-92f3ce95b662"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("77ff15d6-a963-4621-9570-b8650b01811a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("b19fa14a-242f-470b-b4fe-635d48e91dc5"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("d3127555-57e3-44d5-96b6-24502707aa38"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("4e1f4c63-0692-4b08-b9a2-9be055f2b656"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("8915bfe6-af8e-419a-ac07-46fa49d70a0f"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("b595fe6d-365d-489f-98fa-51c22ca58fc2"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("b85cf3da-5223-4c68-a54a-e62d831e2088"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("dc14fd17-ad6c-42f0-8c8a-12bf3b58f638"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("072ecd79-d5b8-4230-9e91-c199e6eaa29c"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("14183a13-ed6a-414c-b71c-febf1f4a8892"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("2852c07e-76f9-4b0a-9805-9959e02f2891"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("30186dbf-cdc8-46a0-8cb6-c1dcfebd0aed"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("3035d143-c4b4-40cd-9688-10ccdf5620bf"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("310c382c-4d28-4fb6-afc8-2d8e4843b66a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("4e1d558b-3b5a-4a1b-84b0-eb6dda853ff3"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("67ecd901-d0e7-4716-a8cf-0b7931362e08"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("68deee69-aa53-4106-94d8-3be43d6cd2dc"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("695d9ed7-1e60-42a8-84bd-0cc182393d69"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("7a6a7ea7-cb5e-4aa2-99c6-30d1c9208040"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("926c0fa2-e546-4043-b1bb-af29e2077142"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9e679ce3-54fb-417b-892c-e7ad92f702ac"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a1a326c7-6c3b-4b6f-b830-e6856bceca37"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b51ecd3b-be80-4f88-9a5f-fafd8852b208"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("ba79d06d-8eab-4f2b-a0b2-0da6ad92969c"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("c3c2c4ad-1426-4d7f-a532-acfef441b37c"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("ea3ca963-7f57-4ddb-b275-aff8fcbc3f12"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("ffcfba60-1317-4ed3-b4dc-2426fb22b358"));

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
        }
    }
}
