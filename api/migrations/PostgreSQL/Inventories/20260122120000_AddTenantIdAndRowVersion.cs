using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Inventories
{
    /// <inheritdoc />
    public partial class AddTenantIdAndRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add RowVersion to InventoryRegistry for optimistic concurrency control
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "inventories",
                table: "InventoryRegistry",
                type: "bytea",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove RowVersion column
            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "inventories",
                table: "InventoryRegistry");
        }
    }
}
