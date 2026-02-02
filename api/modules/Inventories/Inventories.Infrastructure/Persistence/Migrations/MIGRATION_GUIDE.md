# Asset Disposal & Maintenance - Database Migration Guide

## Overview
This guide provides instructions for creating and applying EF Core database migrations for the new AssetDisposal and AssetMaintenance features.

## Prerequisites
- Ensure the solution builds successfully
- EF Core CLI tools installed: `dotnet tool install -g dotnet-ef`
- Both PostgreSQL and MSSQL migration projects are configured

## Step 1: Generate PostgreSQL Migration

Navigate to the PostgreSQL migration project directory:

```powershell
cd api/migrations/PostgreSQL
```

Create migration for the Inventories module:

```powershell
dotnet ef migrations add "AddAssetDisposalAndMaintenance" `
  --project .\PostgreSQL.csproj `
  --startup-project ..\..\server\Server.csproj `
  --context InventoriesDbContext `
  --output-dir Inventories
```

This command will:
- Create a migration file in `Inventories/20250131XXXXX_AddAssetDisposalAndMaintenance.cs`
- Include AssetDisposal and AssetMaintenance table definitions
- Create foreign key relationships to PhysicalAsset and Employee
- Create indexes for query performance

## Step 2: Review PostgreSQL Migration

Examine the generated migration file to verify:

```csharp
// Check the Up() method includes:
- CreateTable for "AssetDisposals"
- CreateTable for "AssetMaintenances"
- AddColumn for PhysicalAsset.MaintenanceHistory navigation
- CreateIndex for Status, PropertyCode, Date combinations
```

## Step 3: Apply PostgreSQL Migration

Apply the migration to your PostgreSQL database:

```powershell
dotnet ef database update `
  --project .\PostgreSQL.csproj `
  --startup-project ..\..\server\Server.csproj `
  --context InventoriesDbContext
```

## Step 4: Generate MSSQL Migration

Navigate to the MSSQL migration project directory:

```powershell
cd api/migrations/MSSQL
```

Create migration for MSSQL:

```powershell
dotnet ef migrations add "AddAssetDisposalAndMaintenance" `
  --project .\MSSQL.csproj `
  --startup-project ..\..\server\Server.csproj `
  --context InventoriesDbContext `
  --output-dir Inventories
```

## Step 5: Review & Apply MSSQL Migration

Review the MSSQL migration and apply it:

```powershell
dotnet ef database update `
  --project .\MSSQL.csproj `
  --startup-project ..\..\server\Server.csproj `
  --context InventoriesDbContext
```

## Step 6: Verify Tables in Database

### For PostgreSQL:
```sql
-- List new tables
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' 
AND table_name IN ('AssetDisposals', 'AssetMaintenances');

-- Check AssetDisposals structure
\d+ "AssetDisposals"

-- Check AssetMaintenances structure
\d+ "AssetMaintenances"

-- Verify indexes
SELECT indexname 
FROM pg_indexes 
WHERE tablename IN ('AssetDisposals', 'AssetMaintenances');
```

### For MSSQL:
```sql
-- List new tables
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN ('AssetDisposals', 'AssetMaintenances');

-- Check structure
EXEC sp_help 'AssetDisposals';
EXEC sp_help 'AssetMaintenances';

-- Verify indexes
SELECT name, object_name(object_id) as TableName
FROM sys.indexes
WHERE object_name(object_id) IN ('AssetDisposals', 'AssetMaintenances');
```

## Tables Created

### AssetDisposals Table
**Purpose**: Tracks asset retirement/disposal workflow

**Columns**:
- `Id` (GUID, PK)
- `PhysicalAssetId` (GUID, FK)
- `AssetPropertyCode` (nvarchar(50))
- `AssetDescription` (nvarchar(500))
- `RequestedBy` (GUID, FK to Employee)
- `RequestDate` (datetime)
- `DisposalMethod` (nvarchar(50)) - Sale|Scrap|Donation|Transfer|Condemnation
- `JustificationReason` (nvarchar(1000))
- `AssetConditionAtDisposal` (nvarchar(50))
- `Status` (int) - 0=Pending, 1=Approved, 2=Completed, 3=Cancelled
- `ApprovedBy` (GUID, FK)
- `ApprovedOn` (datetime)
- `ApprovalNotes` (nvarchar(500))
- `CompletedOn` (datetime)
- `CompletedBy` (GUID, FK)
- `SalvageValue` (decimal(18,2))
- `GainOrLoss` (decimal(18,2)) - Book Value minus Salvage Value
- `DisposalReferenceNumber` (nvarchar(100))
- `CancelledOn` (datetime)
- `CancelledBy` (GUID, FK)
- `CancellationReason` (nvarchar(500))
- Audit columns (CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
- `TenantId` (string, multi-tenancy)

**Indexes**:
- PK: Id
- FK: PhysicalAssetId, RequestedBy, ApprovedBy, CompletedBy, CancelledBy
- IX_AssetDisposals_PhysicalAssetId
- IX_AssetDisposals_Status
- IX_AssetDisposals_RequestDate
- IX_AssetDisposals_PropertyCode
- IX_AssetDisposals_StatusApprover

### AssetMaintenances Table
**Purpose**: Tracks maintenance scheduling and execution

**Columns**:
- `Id` (GUID, PK)
- `PhysicalAssetId` (GUID, FK)
- `AssetPropertyCode` (nvarchar(50))
- `AssetDescription` (nvarchar(500))
- `Type` (int) - 0=Preventive, 1=Corrective, 2=Emergency, 3=Inspection
- `Description` (nvarchar(1000))
- `ScheduledDate` (datetime)
- `Status` (int) - 0=Scheduled, 1=InProgress, 2=Completed, 3=Cancelled
- `StartedOn` (datetime)
- `CompletedOn` (datetime)
- `CompletionNotes` (nvarchar(1000))
- `FindingsNotes` (nvarchar(1000))
- `ScheduledBy` (GUID, FK to Employee)
- `PerformedBy` (GUID, FK to Employee)
- `ApprovedBy` (GUID, FK to Employee)
- `EstimatedCost` (decimal(18,2))
- `ActualCost` (decimal(18,2))
- `CostReference` (nvarchar(100))
- `CancelledOn` (datetime)
- `CancelledBy` (GUID, FK to Employee)
- `CancellationReason` (nvarchar(500))
- Audit columns (CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
- `TenantId` (string, multi-tenancy)

**Indexes**:
- PK: Id
- FK: PhysicalAssetId, ScheduledBy, PerformedBy, ApprovedBy, CancelledBy
- IX_AssetMaintenances_PhysicalAssetId
- IX_AssetMaintenances_Status
- IX_AssetMaintenances_ScheduledDate
- IX_AssetMaintenances_Type
- IX_AssetMaintenances_PropertyCode
- IX_AssetMaintenances_StatusScheduled
- IX_AssetMaintenances_StatusTechnician

## Rollback Instructions

If you need to rollback migrations:

### PostgreSQL
```powershell
cd api/migrations/PostgreSQL

# Rollback last migration
dotnet ef migrations remove `
  --project .\PostgreSQL.csproj `
  --startup-project ..\..\server\Server.csproj

# Or revert database
dotnet ef database update <previous-migration-name> `
  --project .\PostgreSQL.csproj `
  --startup-project ..\..\server\Server.csproj
```

### MSSQL
```powershell
cd api/migrations/MSSQL

# Rollback last migration
dotnet ef migrations remove `
  --project .\MSSQL.csproj `
  --startup-project ..\..\server\Server.csproj

# Or revert database
dotnet ef database update <previous-migration-name> `
  --project .\MSSQL.csproj `
  --startup-project ..\..\server\Server.csproj
```

## Key Design Decisions

### Foreign Key Cascade Rules
- **PhysicalAsset**: `RESTRICT` - Cannot delete asset with active disposals/maintenance
- **Employee FK**: `SET NULL` - Allows employee records to be deleted (approval/completion)
- **Child Items**: `CASCADE` - Not applicable (no child tables yet)

### Value Object Storage
- `DisposalMethod` stored as string (50 chars max)
- `AssetConditionAtDisposal` stored as string (50 chars max)
- `MaintenanceType` and `MaintenanceStatus` stored as int enums

### Multi-Tenancy
- Both tables are multi-tenant (`IsMultiTenant()` in EF config)
- `TenantId` column automatically managed by Finbuckle

### Performance Considerations
- Composite indexes on (Status, Date) for reporting queries
- Composite indexes on (Status, AssignedPerson) for dashboard queries
- Property code indexed for asset lookup by code

## Testing the Migration

After applying migrations, verify with a simple query:

```csharp
// In a test or application startup
var dbContext = serviceProvider.GetRequiredService<InventoriesDbContext>();

// Test AssetDisposal queries
var disposals = await dbContext.Set<AssetDisposal>().ToListAsync();

// Test AssetMaintenance queries
var maintenances = await dbContext.Set<AssetMaintenance>().ToListAsync();

// Test navigation
var assetWithDisposals = await dbContext.PhysicalAssets
    .Include(a => a.Disposals)
    .Include(a => a.MaintenanceHistory)
    .FirstOrDefaultAsync();
```

## Next Steps

1. ✅ Migrations created and applied
2. ⏭️ Register repositories (Step 6)
3. ⏭️ Create API endpoints (Step 6-7)
4. ⏭️ Add authorization (Step 8)
5. ⏭️ Build Blazor UI (Steps 9-10)
6. ⏭️ Add tests (Step 11)

## Troubleshooting

### Migration conflicts
If migrations conflict, manually review and merge in the migration files before applying.

### Foreign key violations
Ensure Employee records exist before creating disposals/maintenance with valid EmployeeIds.

### Multi-tenant issues
Verify that `TenantId` is properly set in your DbContext before creating records.

### Indexes not created
Some database providers may not create all indexes. You can manually create missing indexes with:

```sql
-- Example for PostgreSQL
CREATE INDEX ix_asset_disposals_status_request_date 
ON "AssetDisposals" ("Status", "RequestDate");
```
