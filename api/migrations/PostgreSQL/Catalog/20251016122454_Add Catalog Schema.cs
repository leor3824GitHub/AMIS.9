using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddCatalogSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Employees_InspectorId",
                schema: "catalog",
                table: "Inspections");

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

            migrationBuilder.RenameColumn(
                name: "InspectorId",
                schema: "catalog",
                table: "Inspections",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "InspectionDate",
                schema: "catalog",
                table: "Inspections",
                newName: "InspectedOn");

            migrationBuilder.RenameIndex(
                name: "IX_Inspections_InspectorId",
                schema: "catalog",
                table: "Inspections",
                newName: "IX_Inspections_EmployeeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseId",
                schema: "catalog",
                table: "Inspections",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                schema: "catalog",
                table: "Inspections",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IARDocumentPath",
                schema: "catalog",
                table: "Inspections",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InspectionItemStatus",
                schema: "catalog",
                table: "InspectionItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Employees_EmployeeId",
                schema: "catalog",
                table: "Inspections",
                column: "EmployeeId",
                principalSchema: "catalog",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Employees_EmployeeId",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "Approved",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "IARDocumentPath",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "InspectionItemStatus",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.RenameColumn(
                name: "InspectedOn",
                schema: "catalog",
                table: "Inspections",
                newName: "InspectionDate");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                schema: "catalog",
                table: "Inspections",
                newName: "InspectorId");

            migrationBuilder.RenameIndex(
                name: "IX_Inspections_EmployeeId",
                schema: "catalog",
                table: "Inspections",
                newName: "IX_Inspections_InspectorId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseId",
                schema: "catalog",
                table: "Inspections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
                name: "FK_Inspections_Employees_InspectorId",
                schema: "catalog",
                table: "Inspections",
                column: "InspectorId",
                principalSchema: "catalog",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_InspectionRequests_InspectionRequestId",
                schema: "catalog",
                table: "Inspections",
                column: "InspectionRequestId",
                principalSchema: "catalog",
                principalTable: "InspectionRequests",
                principalColumn: "Id");
        }
    }
}
