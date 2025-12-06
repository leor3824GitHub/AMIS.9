using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class Catalog_Sync_20251205 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("4239944c-882f-43f1-afd7-17b4c6b56d6f"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("511f235e-e035-4b28-8bd6-46fa054f0b1e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("57a0ecd3-fd68-4c7b-983d-8c6cd79581a8"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("e7f77f42-c42f-4b33-b09f-8b4845c0a719"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("ed25eb3b-6fee-4a7c-b172-c9007705008e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("041b7c63-e03c-41f5-add3-f4e323d53ee6"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("44fedf07-fea1-43ea-b7f8-565974dc0eab"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("494994da-b409-44e9-883f-66c98b5a0529"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("c12ed9fc-bc69-4e4d-9c39-86fbdd6c2b03"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("d014a20c-cd2a-4ac0-8cfa-84b15c746a37"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("09abf822-9db6-4440-8d8d-a0e537ec8ea3"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("109665f3-4df4-4e6f-b168-9edba669992c"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("1e4ff1ce-4104-412b-a87e-747c76f8b429"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("1e8a8cb0-db7e-48ba-8cca-e4fcdf52a5da"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("225ddb42-c233-4ce9-8281-df2fa7c36e76"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("2294e1aa-5cf0-4685-b47d-c640df43664a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("26f79bcd-5f11-4849-af98-6a8055af1efa"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("2cb7ae7d-9010-438a-8f40-d2063556b8b3"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("3a8dddb5-1266-4958-b6a4-853085f5e293"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("54bf818e-4f70-49e9-b923-2a23b37eac9d"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("5a8149ef-f0b5-41a1-9437-92e0367e76f3"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("69d893ff-4e14-4e84-a82c-d48a85412783"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("6e6f2a4e-9337-4e1f-a55c-03fd8fda75e2"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("7ca453aa-69b7-4535-8e55-a1be482fe740"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("85bfbe0f-8202-4148-a48c-4450b2ce1fc5"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9d9f8a04-6bbe-4fc4-8095-2cd91de1388b"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("ae8696ab-aea0-45e4-b49b-f1d67da97e56"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("aeb599fc-e327-48bd-bb16-32bbec2142c2"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("c933d108-5541-41f2-8e41-ca922737e34f"));

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("12ff7ceb-92e5-4973-9ef6-a71493423f95"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 },
                    { new Guid("248a4009-6a39-42e6-9456-976b1df08861"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("69ff74df-1d82-4275-b0bf-37cd48ab9009"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("6cd6e0e7-34c1-4b20-86eb-545955bde2b5"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("e9d8d631-cef0-49f6-a9a3-db2b3b458030"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("24332030-c1ba-4c95-9ece-b6dfe1111120"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("437be530-5f7b-449a-9ca5-d95daa163d5a"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("719a09d0-099f-4940-9581-95b9b584ac57"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("d115c817-69fb-469a-a22b-0ee709631ec6"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 },
                    { new Guid("fe4b2ff4-a97b-492b-8cc1-977828afcf6c"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("00ac4b62-a781-439f-8429-355d96665aa6"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("0eb54460-2da5-4498-907d-02e90d6148e7"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("10d83bee-aa9b-44b8-9a92-9dea6effef6a"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("1b105170-8bd7-440b-9732-4d0fcc855167"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("33e5be73-2331-4080-bd2e-40289977d224"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("3733e1b0-18cd-46c7-ae5b-ce01c95a1dcf"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("4c435d4c-291f-498c-8b19-034fde5d481e"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("63ff8c8a-84a3-458d-b5c2-73c3555f8717"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("67a1dbc2-f86e-41e9-87fb-3c34be00112b"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("864d4013-b495-4ab5-8b26-d5a7f132ae3e"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("8e3f7357-b953-4ae0-8929-0c1cdb1268f1"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("9d21330e-2f4c-432d-a158-36a5d1e84222"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("a4fa17f6-e0ee-431a-8a7e-e8ddbf8971ca"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("a7cedd28-673f-4e78-8f16-96be06513c65"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("aa47d8d9-c347-4ccb-8e83-c4e69cbf427f"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("affd0d5a-6da2-4899-bd62-4d1c50729e97"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("ba0bc5d5-4c59-4891-a236-4115ff2f8261"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("da5bfd0d-e509-4dee-923b-d5fdb6dd999b"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 },
                    { new Guid("e8bd1d90-b211-496a-b959-e99319c728b8"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("12ff7ceb-92e5-4973-9ef6-a71493423f95"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("248a4009-6a39-42e6-9456-976b1df08861"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("69ff74df-1d82-4275-b0bf-37cd48ab9009"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("6cd6e0e7-34c1-4b20-86eb-545955bde2b5"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("e9d8d631-cef0-49f6-a9a3-db2b3b458030"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("24332030-c1ba-4c95-9ece-b6dfe1111120"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("437be530-5f7b-449a-9ca5-d95daa163d5a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("719a09d0-099f-4940-9581-95b9b584ac57"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("d115c817-69fb-469a-a22b-0ee709631ec6"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("fe4b2ff4-a97b-492b-8cc1-977828afcf6c"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("00ac4b62-a781-439f-8429-355d96665aa6"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("0eb54460-2da5-4498-907d-02e90d6148e7"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("10d83bee-aa9b-44b8-9a92-9dea6effef6a"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("1b105170-8bd7-440b-9732-4d0fcc855167"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("33e5be73-2331-4080-bd2e-40289977d224"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("3733e1b0-18cd-46c7-ae5b-ce01c95a1dcf"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("4c435d4c-291f-498c-8b19-034fde5d481e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("63ff8c8a-84a3-458d-b5c2-73c3555f8717"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("67a1dbc2-f86e-41e9-87fb-3c34be00112b"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("864d4013-b495-4ab5-8b26-d5a7f132ae3e"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("8e3f7357-b953-4ae0-8929-0c1cdb1268f1"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("9d21330e-2f4c-432d-a158-36a5d1e84222"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a4fa17f6-e0ee-431a-8a7e-e8ddbf8971ca"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("a7cedd28-673f-4e78-8f16-96be06513c65"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("aa47d8d9-c347-4ccb-8e83-c4e69cbf427f"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("affd0d5a-6da2-4899-bd62-4d1c50729e97"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("ba0bc5d5-4c59-4891-a236-4115ff2f8261"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("da5bfd0d-e509-4dee-923b-d5fdb6dd999b"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                keyColumn: "Id",
                keyValue: new Guid("e8bd1d90-b211-496a-b959-e99319c728b8"));

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "AssetConditionConfigurations",
                columns: new[] { "Id", "AllowsForUse", "Code", "ColorCode", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "DisplayName", "IsActive", "LastModified", "LastModifiedBy", "RequiresDisposal", "RequiresRepair", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("4239944c-882f-43f1-afd7-17b4c6b56d6f"), true, "Poor", "#fd7e14", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has significant wear, may need repair", "Poor", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 3 },
                    { new Guid("511f235e-e035-4b28-8bd6-46fa054f0b1e"), true, "Fair", "#ffc107", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset has minor wear but still functional", "Fair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 2 },
                    { new Guid("57a0ecd3-fd68-4c7b-983d-8c6cd79581a8"), true, "Good", "#28a745", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is in excellent working condition", "Good", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, false, 1 },
                    { new Guid("e7f77f42-c42f-4b33-b09f-8b4845c0a719"), false, "Unserviceable", "#dc3545", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is not functional, requires major repair", "Unserviceable", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, true, 4 },
                    { new Guid("ed25eb3b-6fee-4a7c-b172-c9007705008e"), false, "ForDisposal", "#6c757d", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, "Asset is beyond repair and should be disposed", "For Disposal", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, false, 5 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "PPETypeDefinitions",
                columns: new[] { "Id", "COAReference", "Category", "Code", "Created", "CreatedBy", "DefaultDepreciationRate", "DefaultUsefulLifeYears", "Deleted", "DeletedBy", "DepreciationAccountCode", "Description", "IconName", "IsActive", "LastModified", "LastModifiedBy", "Name", "RCAAccountCode", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("041b7c63-e03c-41f5-add3-f4e323d53ee6"), "COA Circular 2022-002", "Office", "FURNITURE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Office furniture, fixtures, and reference books", "chair", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Furniture, Fixtures and Books", "10606010", 3 },
                    { new Guid("44fedf07-fea1-43ea-b7f8-565974dc0eab"), "COA Circular 2022-002", "Transportation", "TRANSPORTATION", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 20m, 5, null, null, "10699010", "Vehicles, motorcycles, boats", "car", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transportation Equipment", "10605010", 2 },
                    { new Guid("494994da-b409-44e9-883f-66c98b5a0529"), "COA Circular 2022-002", "Production", "MACHINERY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Industrial machinery, tools, and equipment", "gear", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Machinery and Equipment", "10604010", 1 },
                    { new Guid("c12ed9fc-bc69-4e4d-9c39-86fbdd6c2b03"), "COA Circular 2022-002", "Technology", "ICT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 33.33m, 3, null, null, "10699010", "Computers, servers, network equipment", "desktop", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ICT Equipment", "10607010", 4 },
                    { new Guid("d014a20c-cd2a-4ac0-8cfa-84b15c746a37"), "COA Circular 2022-002", "General", "OTHER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), 10m, 10, null, null, "10699010", "Other property, plant and equipment", "box", true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Other PPE", "10699990", 5 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "UnitsOfMeasure",
                columns: new[] { "Id", "Abbreviation", "BaseUnitId", "Code", "ConversionFactor", "Created", "CreatedBy", "Deleted", "DeletedBy", "IsActive", "IsDefault", "LastModified", "LastModifiedBy", "Name", "SortOrder", "UnitType" },
                values: new object[,]
                {
                    { new Guid("09abf822-9db6-4440-8d8d-a0e537ec8ea3"), "set", null, "SET", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Set", 2, 1 },
                    { new Guid("109665f3-4df4-4e6f-b168-9edba669992c"), "pair", null, "PAIR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pair", 4, 1 },
                    { new Guid("1e4ff1ce-4104-412b-a87e-747c76f8b429"), "can", null, "CAN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Can", 53, 6 },
                    { new Guid("1e8a8cb0-db7e-48ba-8cca-e4fcdf52a5da"), "pc", null, "PC", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Piece", 1, 1 },
                    { new Guid("225ddb42-c233-4ce9-8281-df2fa7c36e76"), "pack", null, "PACK", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pack", 51, 6 },
                    { new Guid("2294e1aa-5cf0-4685-b47d-c640df43664a"), "kg", null, "KG", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kilogram", 10, 2 },
                    { new Guid("26f79bcd-5f11-4849-af98-6a8055af1efa"), "mL", null, "ML", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Milliliter", 21, 3 },
                    { new Guid("2cb7ae7d-9010-438a-8f40-d2063556b8b3"), "btl", null, "BOTTLE", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bottle", 52, 6 },
                    { new Guid("3a8dddb5-1266-4958-b6a4-853085f5e293"), "MT", null, "MT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metric Ton", 12, 2 },
                    { new Guid("54bf818e-4f70-49e9-b923-2a23b37eac9d"), "unit", null, "UNIT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Unit", 3, 1 },
                    { new Guid("5a8149ef-f0b5-41a1-9437-92e0367e76f3"), "box", null, "BOX", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Box", 50, 6 },
                    { new Guid("69d893ff-4e14-4e84-a82c-d48a85412783"), "m²", null, "SQM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Square Meter", 40, 5 },
                    { new Guid("6e6f2a4e-9337-4e1f-a55c-03fd8fda75e2"), "m", null, "M", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Meter", 30, 4 },
                    { new Guid("7ca453aa-69b7-4535-8e55-a1be482fe740"), "g", null, "G", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gram", 11, 2 },
                    { new Guid("85bfbe0f-8202-4148-a48c-4450b2ce1fc5"), "L", null, "L", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Liter", 20, 3 },
                    { new Guid("9d9f8a04-6bbe-4fc4-8095-2cd91de1388b"), "ft", null, "FT", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Feet", 33, 4 },
                    { new Guid("ae8696ab-aea0-45e4-b49b-f1d67da97e56"), "cm", null, "CM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Centimeter", 31, 4 },
                    { new Guid("aeb599fc-e327-48bd-bb16-32bbec2142c2"), "gal", null, "GAL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Gallon", 22, 3 },
                    { new Guid("c933d108-5541-41f2-8e41-ca922737e34f"), "mm", null, "MM", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Millimeter", 32, 4 }
                });
        }
    }
}
