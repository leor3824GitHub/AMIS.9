using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddRcaAccountCodesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RcaAccountCodes",
                schema: "catalog",
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

            migrationBuilder.CreateIndex(
                name: "IX_RcaAccountCodes_Key",
                schema: "catalog",
                table: "RcaAccountCodes",
                column: "Key",
                unique: true);

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "RcaAccountCodes",
                columns: new[]
                {
                    "Id",
                    "Key",
                    "AccountCode",
                    "Description",
                    "IsActive",
                    "Created",
                    "CreatedBy",
                    "LastModified",
                    "LastModifiedBy"
                },
                values: new object[,]
                {
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0001"), "SuppliesAndMaterialsInventory", "10501000", "Supplies and materials inventory", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0002"), "SuppliesAndMaterialsExpense", "50203010", "Supplies and materials expense", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0003"), "SemiExpendablePropertyInventory", "10599020", "Semi-expendable property inventory", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0004"), "SemiExpendablePropertyExpense", "50299010", "Semi-expendable property expense", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0005"), "MachineryAndEquipment", "10604010", "Machinery and equipment", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0006"), "TransportationEquipment", "10605010", "Transportation equipment", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0007"), "FurnitureFixturesAndBooksEquipment", "10606010", "Furniture, fixtures, and books equipment", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0008"), "ICTEquipment", "10607010", "ICT equipment", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0009"), "OtherPropertyPlantAndEquipment", "10699990", "Other property, plant, and equipment", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0010"), "AccumulatedDepreciation", "10699010", "Accumulated depreciation", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty },
                    { new Guid("0a5f2beb-2d1d-4c36-9f56-11d9ad1c0011"), "AccountsPayable", "20101010", "Accounts payable", true, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty, new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, DateTimeKind.Utc)), Guid.Empty }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RcaAccountCodes",
                schema: "catalog");
        }
    }
}
