using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class Catalog_Employee_ContactInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResponsibilityCode",
                schema: "catalog",
                table: "Employees",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo_Email",
                schema: "catalog",
                table: "Employees",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo_PhoneNumber",
                schema: "catalog",
                table: "Employees",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                schema: "catalog",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                schema: "catalog",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "catalog",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorId",
                schema: "catalog",
                table: "Employees",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TerminationDate",
                schema: "catalog",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_RequestedBy",
                schema: "catalog",
                table: "PurchaseRequests",
                column: "RequestedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_Employees_RequestedBy",
                schema: "catalog",
                table: "PurchaseRequests",
                column: "RequestedBy",
                principalSchema: "catalog",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Employees_RequestedBy",
                schema: "catalog",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_RequestedBy",
                schema: "catalog",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "ContactInfo_Email",
                schema: "catalog",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ContactInfo_PhoneNumber",
                schema: "catalog",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Department",
                schema: "catalog",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "HireDate",
                schema: "catalog",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "catalog",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                schema: "catalog",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TerminationDate",
                schema: "catalog",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "ResponsibilityCode",
                schema: "catalog",
                table: "Employees",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);
        }
    }
}
