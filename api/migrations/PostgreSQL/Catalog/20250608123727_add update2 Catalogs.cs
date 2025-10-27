using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class addupdate2Catalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Employees_InspectorId",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Employees_InspectorId",
                schema: "catalog",
                table: "Inspections",
                column: "InspectorId",
                principalSchema: "catalog",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Employees_InspectorId",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Employees_InspectorId",
                schema: "catalog",
                table: "Inspections",
                column: "InspectorId",
                principalSchema: "catalog",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
