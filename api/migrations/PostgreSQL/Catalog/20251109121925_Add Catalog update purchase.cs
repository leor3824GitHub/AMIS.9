using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddCatalogupdatepurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "catalog",
                table: "Purchases",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "catalog",
                table: "Purchases",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                schema: "catalog",
                table: "Purchases",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "catalog",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                schema: "catalog",
                table: "Purchases");
        }
    }
}
