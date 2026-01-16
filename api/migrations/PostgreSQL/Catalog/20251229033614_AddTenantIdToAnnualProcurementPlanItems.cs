using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddTenantIdToAnnualProcurementPlanItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // NOTE: This migration was scaffolded with large seed-data churn.
            // We only want the schema change for AnnualProcurementPlanItems.TenantId.

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "catalog",
                table: "AnnualProcurementPlanItems",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE catalog.\"AnnualProcurementPlanItems\" i SET \"TenantId\" = h.\"TenantId\" FROM catalog.\"AnnualProcurementPlans\" h WHERE i.\"PlanHeaderId\" = h.\"Id\";");

            migrationBuilder.Sql(
                "UPDATE catalog.\"AnnualProcurementPlanItems\" SET \"TenantId\" = '' WHERE \"TenantId\" IS NULL;");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "catalog",
                table: "AnnualProcurementPlanItems",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnnualProcurementPlanItems_TenantId",
                schema: "catalog",
                table: "AnnualProcurementPlanItems",
                column: "TenantId");

#if false
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

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "catalog",
                table: "AnnualProcurementPlanItems",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("202894eb-3c6f-45e9-9df1-c29d304debe3"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("5e8553b7-dddd-4a3d-8a71-a61f31bd77ef"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("78aaa975-a106-4f12-ac49-15f5817f3d58"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("aa3fa500-0f7b-48bc-a1dd-4e4f53d89766"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("d3aa7b0d-3be8-419e-84d0-65334e489460"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("4693d646-34fe-4d0c-a5ce-d0728c8c08ab"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("53f6a1ba-a467-4851-87ed-f1739ac0bb2b"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("6779fa07-477b-4f51-99cf-4337fc700b0b"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("6a333abd-f01b-4457-85e1-3beca3da448d"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("bff147b2-13ef-4520-ade2-736c3488ef8b"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("0742edec-3f14-42b8-98c5-7819ceae864d"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("3c405641-1cc1-4259-9cea-7ca4d0b899d5"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("535cdd2e-80a4-4f81-8b4b-148de8d5b73a"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("566185e2-af42-437e-aa70-8178d71267dc"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("5a525280-8c7b-4295-a3cb-a060b4c46d33"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("69593370-106f-43b5-b297-40fa17cd6098"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("6a793a86-c75c-4798-9628-c2c98623e1a6"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("6bc34cd8-1ee3-4217-953c-bb447ca4570e"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("75d9717d-a831-4d81-aa58-fb73992126f4"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("842a3f67-2002-426e-bf15-fab60785737f"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("91afffd2-e704-4c68-973b-197bf71ddd0a"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("9a944623-10d8-4e0a-a51c-9255b48787c7"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("9fdfe450-d428-4525-8544-3f7c67ede543"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("a2148005-93d4-465f-bff2-ab4fe4d4825d"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("b21c5b01-6bbe-4532-944d-4a9a5ffaf6f6"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("b27925d9-e581-4903-93fe-c231490020f0"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("b907b767-be3a-4af9-92bf-fe87869ac880"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("d45141fc-24aa-489c-8d4c-d6a2b548ed9d"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("ed9ab760-f0f8-4b8a-8e2e-80242e25ca66"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 }
                });
        #endif
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnnualProcurementPlanItems_TenantId",
                schema: "catalog",
                table: "AnnualProcurementPlanItems");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "catalog",
                table: "AnnualProcurementPlanItems");

#if false
            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("202894eb-3c6f-45e9-9df1-c29d304debe3"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("5e8553b7-dddd-4a3d-8a71-a61f31bd77ef"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("78aaa975-a106-4f12-ac49-15f5817f3d58"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("aa3fa500-0f7b-48bc-a1dd-4e4f53d89766"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("d3aa7b0d-3be8-419e-84d0-65334e489460"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("4693d646-34fe-4d0c-a5ce-d0728c8c08ab"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("53f6a1ba-a467-4851-87ed-f1739ac0bb2b"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("6779fa07-477b-4f51-99cf-4337fc700b0b"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("6a333abd-f01b-4457-85e1-3beca3da448d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("bff147b2-13ef-4520-ade2-736c3488ef8b"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("0742edec-3f14-42b8-98c5-7819ceae864d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("3c405641-1cc1-4259-9cea-7ca4d0b899d5"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("535cdd2e-80a4-4f81-8b4b-148de8d5b73a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("566185e2-af42-437e-aa70-8178d71267dc"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("5a525280-8c7b-4295-a3cb-a060b4c46d33"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("69593370-106f-43b5-b297-40fa17cd6098"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("6a793a86-c75c-4798-9628-c2c98623e1a6"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("6bc34cd8-1ee3-4217-953c-bb447ca4570e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("75d9717d-a831-4d81-aa58-fb73992126f4"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("842a3f67-2002-426e-bf15-fab60785737f"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("91afffd2-e704-4c68-973b-197bf71ddd0a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9a944623-10d8-4e0a-a51c-9255b48787c7"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9fdfe450-d428-4525-8544-3f7c67ede543"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a2148005-93d4-465f-bff2-ab4fe4d4825d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b21c5b01-6bbe-4532-944d-4a9a5ffaf6f6"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b27925d9-e581-4903-93fe-c231490020f0"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("b907b767-be3a-4af9-92bf-fe87869ac880"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("d45141fc-24aa-489c-8d4c-d6a2b548ed9d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("ed9ab760-f0f8-4b8a-8e2e-80242e25ca66"));

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "catalog",
                table: "AnnualProcurementPlanItems");

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
#endif
        }
    }
}
