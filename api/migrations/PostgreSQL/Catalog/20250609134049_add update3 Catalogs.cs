using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class addupdate3Catalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acceptances_Employees_AccountableOfficerId",
                schema: "catalog",
                table: "Acceptances");

            migrationBuilder.DropColumn(
                name: "AcceptedBy",
                schema: "catalog",
                table: "Acceptances");

            migrationBuilder.RenameColumn(
                name: "AccountableOfficerId",
                schema: "catalog",
                table: "Acceptances",
                newName: "SupplyOfficerId");

            migrationBuilder.RenameIndex(
                name: "IX_Acceptances_AccountableOfficerId",
                schema: "catalog",
                table: "Acceptances",
                newName: "IX_Acceptances_SupplyOfficerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Acceptances_Employees_SupplyOfficerId",
                schema: "catalog",
                table: "Acceptances",
                column: "SupplyOfficerId",
                principalSchema: "catalog",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acceptances_Employees_SupplyOfficerId",
                schema: "catalog",
                table: "Acceptances");

            migrationBuilder.RenameColumn(
                name: "SupplyOfficerId",
                schema: "catalog",
                table: "Acceptances",
                newName: "AccountableOfficerId");

            migrationBuilder.RenameIndex(
                name: "IX_Acceptances_SupplyOfficerId",
                schema: "catalog",
                table: "Acceptances",
                newName: "IX_Acceptances_AccountableOfficerId");

            migrationBuilder.AddColumn<Guid>(
                name: "AcceptedBy",
                schema: "catalog",
                table: "Acceptances",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Acceptances_Employees_AccountableOfficerId",
                schema: "catalog",
                table: "Acceptances",
                column: "AccountableOfficerId",
                principalSchema: "catalog",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
