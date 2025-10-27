using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddCatalog6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionRequests_Employees_EmployeeId",
                schema: "catalog",
                table: "InspectionRequests");

            migrationBuilder.DropIndex(
                name: "IX_InspectionRequests_EmployeeId",
                schema: "catalog",
                table: "InspectionRequests");

            migrationBuilder.DropColumn(
                name: "AssignedInspectorId",
                schema: "catalog",
                table: "InspectionRequests");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "catalog",
                table: "InspectionRequests");

            migrationBuilder.RenameColumn(
                name: "RequestedById",
                schema: "catalog",
                table: "InspectionRequests",
                newName: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequests_InspectorId",
                schema: "catalog",
                table: "InspectionRequests",
                column: "InspectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionRequests_Employees_InspectorId",
                schema: "catalog",
                table: "InspectionRequests",
                column: "InspectorId",
                principalSchema: "catalog",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionRequests_Employees_InspectorId",
                schema: "catalog",
                table: "InspectionRequests");

            migrationBuilder.DropIndex(
                name: "IX_InspectionRequests_InspectorId",
                schema: "catalog",
                table: "InspectionRequests");

            migrationBuilder.RenameColumn(
                name: "InspectorId",
                schema: "catalog",
                table: "InspectionRequests",
                newName: "RequestedById");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedInspectorId",
                schema: "catalog",
                table: "InspectionRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "catalog",
                table: "InspectionRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequests_EmployeeId",
                schema: "catalog",
                table: "InspectionRequests",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionRequests_Employees_EmployeeId",
                schema: "catalog",
                table: "InspectionRequests",
                column: "EmployeeId",
                principalSchema: "catalog",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
