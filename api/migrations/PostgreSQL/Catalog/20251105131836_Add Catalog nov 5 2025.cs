using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMIS.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddCatalognov52025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Purchases_PurchaseId",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.DropIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems");

            // Safe conversion of Unit column from string to integer using temporary column
            // Step 1: Add temporary column for new enum values
            migrationBuilder.AddColumn<int>(
                name: "Unit_New",
                schema: "catalog",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Step 2: Migrate existing data using CASE statement to map string values to enum integers
            migrationBuilder.Sql(@"
                UPDATE catalog.""Products""
                SET ""Unit_New"" = CASE 
                    -- Quantity Units (0-9)
                    WHEN LOWER(""Unit"") IN ('pc', 'piece', 'pcs', 'pieces') THEN 0
                    WHEN LOWER(""Unit"") IN ('ea', 'each') THEN 1
                    WHEN LOWER(""Unit"") IN ('set', 'sets') THEN 2
                    WHEN LOWER(""Unit"") IN ('pair', 'pairs', 'pr') THEN 3
                    WHEN LOWER(""Unit"") IN ('dozen', 'dz') THEN 4
                    
                    -- Weight Units (10-19)
                    WHEN LOWER(""Unit"") IN ('kg', 'kilogram', 'kilograms') THEN 10
                    WHEN LOWER(""Unit"") IN ('g', 'gram', 'grams') THEN 11
                    WHEN LOWER(""Unit"") IN ('mg', 'milligram', 'milligrams') THEN 12
                    WHEN LOWER(""Unit"") IN ('t', 'ton', 'tons', 'tonne', 'tonnes', 'mt') THEN 13
                    WHEN LOWER(""Unit"") IN ('lb', 'pound', 'pounds', 'lbs') THEN 14
                    WHEN LOWER(""Unit"") IN ('oz', 'ounce', 'ounces') THEN 15
                    
                    -- Volume Units (20-29)
                    WHEN LOWER(""Unit"") IN ('l', 'liter', 'liters', 'litre', 'litres') THEN 20
                    WHEN LOWER(""Unit"") IN ('ml', 'milliliter', 'milliliters', 'millilitre', 'millilitres') THEN 21
                    WHEN LOWER(""Unit"") IN ('gal', 'gallon', 'gallons') THEN 22
                    WHEN LOWER(""Unit"") IN ('qt', 'quart', 'quarts') THEN 23
                    WHEN LOWER(""Unit"") IN ('pt', 'pint', 'pints') THEN 24
                    WHEN LOWER(""Unit"") IN ('m3', 'cubic meter', 'cubic meters', 'cbm') THEN 25
                    WHEN LOWER(""Unit"") IN ('fl oz', 'fluid ounce', 'fluid ounces', 'floz') THEN 26
                    
                    -- Length Units (30-39)
                    WHEN LOWER(""Unit"") IN ('m', 'meter', 'meters', 'metre', 'metres') THEN 30
                    WHEN LOWER(""Unit"") IN ('cm', 'centimeter', 'centimeters', 'centimetre', 'centimetres') THEN 31
                    WHEN LOWER(""Unit"") IN ('mm', 'millimeter', 'millimeters', 'millimetre', 'millimetres') THEN 32
                    WHEN LOWER(""Unit"") IN ('km', 'kilometer', 'kilometers', 'kilometre', 'kilometres') THEN 33
                    WHEN LOWER(""Unit"") IN ('in', 'inch', 'inches') THEN 34
                    WHEN LOWER(""Unit"") IN ('ft', 'foot', 'feet') THEN 35
                    WHEN LOWER(""Unit"") IN ('yd', 'yard', 'yards') THEN 36
                    WHEN LOWER(""Unit"") IN ('mi', 'mile', 'miles') THEN 37
                    
                    -- Area Units (40-49)
                    WHEN LOWER(""Unit"") IN ('m2', 'sqm', 'square meter', 'square meters', 'sq m') THEN 40
                    WHEN LOWER(""Unit"") IN ('cm2', 'square centimeter', 'square centimeters', 'sq cm') THEN 41
                    WHEN LOWER(""Unit"") IN ('ft2', 'sqft', 'square foot', 'square feet', 'sq ft') THEN 42
                    WHEN LOWER(""Unit"") IN ('ha', 'hectare', 'hectares') THEN 43
                    WHEN LOWER(""Unit"") IN ('acre', 'acres', 'ac') THEN 44
                    
                    -- Packaging Units (50-59)
                    WHEN LOWER(""Unit"") IN ('box', 'boxes', 'bx') THEN 50
                    WHEN LOWER(""Unit"") IN ('case', 'cases', 'cs') THEN 51
                    WHEN LOWER(""Unit"") IN ('carton', 'cartons', 'ctn') THEN 52
                    WHEN LOWER(""Unit"") IN ('pallet', 'pallets', 'plt') THEN 53
                    WHEN LOWER(""Unit"") IN ('bag', 'bags', 'bg') THEN 54
                    WHEN LOWER(""Unit"") IN ('bottle', 'bottles', 'btl') THEN 55
                    WHEN LOWER(""Unit"") IN ('can', 'cans') THEN 56
                    WHEN LOWER(""Unit"") IN ('drum', 'drums', 'dr') THEN 57
                    WHEN LOWER(""Unit"") IN ('bundle', 'bundles', 'bdl') THEN 58
                    WHEN LOWER(""Unit"") IN ('roll', 'rolls', 'rl') THEN 59
                    
                    -- Time Units (60-69)
                    WHEN LOWER(""Unit"") IN ('hr', 'hour', 'hours', 'h') THEN 60
                    WHEN LOWER(""Unit"") IN ('min', 'minute', 'minutes') THEN 61
                    WHEN LOWER(""Unit"") IN ('sec', 'second', 'seconds', 's') THEN 62
                    WHEN LOWER(""Unit"") IN ('day', 'days', 'd') THEN 63
                    WHEN LOWER(""Unit"") IN ('week', 'weeks', 'wk') THEN 64
                    WHEN LOWER(""Unit"") IN ('month', 'months', 'mo') THEN 65
                    WHEN LOWER(""Unit"") IN ('year', 'years', 'yr') THEN 66
                    
                    -- Default to Piece if unknown
                    ELSE 0
                END;
            ");

            // Step 3: Drop old column
            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "catalog",
                table: "Products");

            // Step 4: Rename new column to replace old one
            migrationBuilder.RenameColumn(
                name: "Unit_New",
                schema: "catalog",
                table: "Products",
                newName: "Unit");

            migrationBuilder.AddColumn<int>(
                name: "CostingMethod",
                schema: "catalog",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCountedDate",
                schema: "catalog",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "catalog",
                table: "Inventories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservedQty",
                schema: "catalog",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StockStatus",
                schema: "catalog",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InspectedOn",
                schema: "catalog",
                table: "Inspections",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<Guid>(
                name: "InspectionRequestId",
                schema: "catalog",
                table: "Inspections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            // Create placeholder InspectionRequests for existing Inspections to satisfy foreign key
            // This handles legacy data that existed before the InspectionRequest relationship was added
            migrationBuilder.Sql(@"
                INSERT INTO catalog.""InspectionRequests"" (""Id"", ""PurchaseId"", ""InspectorId"", ""RequestedOn"", ""Status"", ""TenantId"", ""Created"", ""CreatedBy"", ""LastModified"", ""LastModifiedBy"")
                SELECT 
                    gen_random_uuid() as ""Id"",
                    i.""PurchaseId"",
                    i.""InspectorId"",
                    i.""InspectionDate"" as ""RequestedOn"",
                    'Completed' as ""Status"",
                    i.""TenantId"",
                    i.""Created"",
                    i.""CreatedBy"",
                    i.""LastModified"",
                    COALESCE(i.""LastModifiedBy"", i.""CreatedBy"") as ""LastModifiedBy""
                FROM catalog.""Inspections"" i
                WHERE NOT EXISTS (
                    SELECT 1 FROM catalog.""InspectionRequests"" ir 
                    WHERE ir.""PurchaseId"" = i.""PurchaseId"" 
                    AND ir.""InspectorId"" = i.""InspectorId""
                    AND ir.""RequestedOn""::date = i.""InspectionDate""::date
                );
            ");

            // Update Inspections to link to the created InspectionRequests
            migrationBuilder.Sql(@"
                UPDATE catalog.""Inspections"" i
                SET ""InspectionRequestId"" = (
                    SELECT ir.""Id""
                    FROM catalog.""InspectionRequests"" ir
                    WHERE ir.""PurchaseId"" = i.""PurchaseId""
                    AND ir.""InspectorId"" = i.""InspectorId""
                    AND ir.""RequestedOn""::date = i.""InspectionDate""::date
                    LIMIT 1
                )
                WHERE ""InspectionRequestId"" = '00000000-0000-0000-0000-000000000000';
            ");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceItems_IssuanceId",
                schema: "catalog",
                table: "IssuanceItems",
                column: "IssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_InspectionRequestId",
                schema: "catalog",
                table: "Inspections",
                column: "InspectionRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems",
                column: "PurchaseItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems",
                column: "PurchaseItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_InspectionRequests_InspectionRequestId",
                schema: "catalog",
                table: "Inspections",
                column: "InspectionRequestId",
                principalSchema: "catalog",
                principalTable: "InspectionRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Purchases_PurchaseId",
                schema: "catalog",
                table: "Inspections",
                column: "PurchaseId",
                principalSchema: "catalog",
                principalTable: "Purchases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuanceItems_Issuances_IssuanceId",
                schema: "catalog",
                table: "IssuanceItems",
                column: "IssuanceId",
                principalSchema: "catalog",
                principalTable: "Issuances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_InspectionRequests_InspectionRequestId",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Purchases_PurchaseId",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuanceItems_Issuances_IssuanceId",
                schema: "catalog",
                table: "IssuanceItems");

            migrationBuilder.DropIndex(
                name: "IX_IssuanceItems_IssuanceId",
                schema: "catalog",
                table: "IssuanceItems");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_InspectionRequestId",
                schema: "catalog",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems");

            migrationBuilder.DropIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems");

            migrationBuilder.DropColumn(
                name: "CostingMethod",
                schema: "catalog",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "LastCountedDate",
                schema: "catalog",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "catalog",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "ReservedQty",
                schema: "catalog",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "StockStatus",
                schema: "catalog",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "InspectionRequestId",
                schema: "catalog",
                table: "Inspections");

            // Reverse conversion from integer back to string
            // Step 1: Add temporary text column
            migrationBuilder.AddColumn<string>(
                name: "Unit_Old",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "pc");

            // Step 2: Convert enum values back to text
            migrationBuilder.Sql(@"
                UPDATE catalog.""Products""
                SET ""Unit_Old"" = CASE ""Unit""
                    WHEN 0 THEN 'pc'
                    WHEN 1 THEN 'ea'
                    WHEN 2 THEN 'set'
                    WHEN 3 THEN 'pair'
                    WHEN 4 THEN 'dozen'
                    WHEN 10 THEN 'kg'
                    WHEN 11 THEN 'g'
                    WHEN 12 THEN 'mg'
                    WHEN 13 THEN 't'
                    WHEN 14 THEN 'lb'
                    WHEN 15 THEN 'oz'
                    WHEN 20 THEN 'l'
                    WHEN 21 THEN 'ml'
                    WHEN 22 THEN 'gal'
                    WHEN 23 THEN 'qt'
                    WHEN 24 THEN 'pt'
                    WHEN 25 THEN 'm3'
                    WHEN 26 THEN 'fl oz'
                    WHEN 30 THEN 'm'
                    WHEN 31 THEN 'cm'
                    WHEN 32 THEN 'mm'
                    WHEN 33 THEN 'km'
                    WHEN 34 THEN 'in'
                    WHEN 35 THEN 'ft'
                    WHEN 36 THEN 'yd'
                    WHEN 37 THEN 'mi'
                    WHEN 40 THEN 'm2'
                    WHEN 41 THEN 'cm2'
                    WHEN 42 THEN 'ft2'
                    WHEN 43 THEN 'ha'
                    WHEN 44 THEN 'acre'
                    WHEN 50 THEN 'box'
                    WHEN 51 THEN 'case'
                    WHEN 52 THEN 'carton'
                    WHEN 53 THEN 'pallet'
                    WHEN 54 THEN 'bag'
                    WHEN 55 THEN 'bottle'
                    WHEN 56 THEN 'can'
                    WHEN 57 THEN 'drum'
                    WHEN 58 THEN 'bundle'
                    WHEN 59 THEN 'roll'
                    WHEN 60 THEN 'hr'
                    WHEN 61 THEN 'min'
                    WHEN 62 THEN 'sec'
                    WHEN 63 THEN 'day'
                    WHEN 64 THEN 'week'
                    WHEN 65 THEN 'month'
                    WHEN 66 THEN 'year'
                    ELSE 'pc'
                END;
            ");

            // Step 3: Drop integer column
            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "catalog",
                table: "Products");

            // Step 4: Rename text column back
            migrationBuilder.RenameColumn(
                name: "Unit_Old",
                schema: "catalog",
                table: "Products",
                newName: "Unit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InspectedOn",
                schema: "catalog",
                table: "Inspections",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_PurchaseItemId",
                schema: "catalog",
                table: "InspectionItems",
                column: "PurchaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceItems_PurchaseItemId",
                schema: "catalog",
                table: "AcceptanceItems",
                column: "PurchaseItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Purchases_PurchaseId",
                schema: "catalog",
                table: "Inspections",
                column: "PurchaseId",
                principalSchema: "catalog",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
