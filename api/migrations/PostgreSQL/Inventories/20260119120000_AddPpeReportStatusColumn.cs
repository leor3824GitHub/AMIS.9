using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class AddPpeReportStatusColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Status column to PpeReceivingReport
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "inventories",
                table: "PpeReceivingReport",
                type: "integer",
                nullable: false,
                defaultValue: 0); // Default to Draft

            // Add Status column to PpeIssuanceReport
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "inventories",
                table: "PpeIssuanceReport",
                type: "integer",
                nullable: false,
                defaultValue: 0); // Default to Draft
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove Status column from PpeReceivingReport
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "inventories",
                table: "PpeReceivingReport");

            // Remove Status column from PpeIssuanceReport
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "inventories",
                table: "PpeIssuanceReport");
        }
    }
}
