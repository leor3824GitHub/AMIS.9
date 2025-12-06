using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class Add_PPETypeAccountMappings_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE catalog.\"PhysicalAssets\" DROP COLUMN IF EXISTS \"LastReclassificationDate\";");
            migrationBuilder.Sql("ALTER TABLE catalog.\"PhysicalAssets\" DROP COLUMN IF EXISTS \"PreviousClassification\";");
            migrationBuilder.Sql("ALTER TABLE catalog.\"PhysicalAssets\" DROP COLUMN IF EXISTS \"ReclassificationReason\";");

            migrationBuilder.CreateTable(
                name: "PPETypeAccountMappings",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PPEType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RCAAccountCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_PPETypeAccountMappings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PPETypeAccountMappings_PPEType",
                schema: "catalog",
                table: "PPETypeAccountMappings",
                column: "PPEType",
                unique: true,
                filter: "\"IsActive\" = true AND \"Deleted\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PPETypeAccountMappings",
                schema: "catalog");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReclassificationDate",
                schema: "catalog",
                table: "PhysicalAssets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClassification",
                schema: "catalog",
                table: "PhysicalAssets",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReclassificationReason",
                schema: "catalog",
                table: "PhysicalAssets",
                type: "text",
                nullable: true);
        }
    }
}
