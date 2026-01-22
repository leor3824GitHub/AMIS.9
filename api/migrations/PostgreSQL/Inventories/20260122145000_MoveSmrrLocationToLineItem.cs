using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class MoveSmrrLocationToLineItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                schema: "inventories",
                table: "SuppliesAndMaterialsReceivingReports");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "inventories",
                table: "ReceivingLineItem",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                schema: "inventories",
                table: "ReceivingLineItem");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "inventories",
                table: "SuppliesAndMaterialsReceivingReports",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
