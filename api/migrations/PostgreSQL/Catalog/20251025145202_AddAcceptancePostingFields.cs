using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddAcceptancePostingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InspectionId",
                schema: "catalog",
                table: "Acceptances",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                schema: "catalog",
                table: "Acceptances",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostedOn",
                schema: "catalog",
                table: "Acceptances",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InspectionId",
                schema: "catalog",
                table: "Acceptances");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                schema: "catalog",
                table: "Acceptances");

            migrationBuilder.DropColumn(
                name: "PostedOn",
                schema: "catalog",
                table: "Acceptances");
        }
    }
}
