using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class addupdate7Catalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InspectionRequestId",
                schema: "catalog",
                table: "Inspections",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_InspectionRequestId",
                schema: "catalog",
                table: "Inspections",
                column: "InspectionRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_InspectionRequests_InspectionRequestId",
                schema: "catalog",
                table: "Inspections",
                column: "InspectionRequestId",
                principalSchema: "catalog",
                principalTable: "InspectionRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_InspectionRequests_InspectionRequestId",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_InspectionRequestId",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "InspectionRequestId",
                schema: "catalog",
                table: "Inspections");
        }
    }
}
