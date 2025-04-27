using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class update1Catalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVAT",
                schema: "catalog",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "TaxClassification",
                schema: "catalog",
                table: "Suppliers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxClassification",
                schema: "catalog",
                table: "Suppliers");

            migrationBuilder.AddColumn<bool>(
                name: "IsVAT",
                schema: "catalog",
                table: "Suppliers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
