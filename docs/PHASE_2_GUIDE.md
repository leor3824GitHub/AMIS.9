# Next Steps: Phase 2 Implementation Guide

## Overview

Phase 1 (Domain Entities) is complete. Phase 2 focuses on database infrastructure and application layer to support the domain entities.

**Estimated Duration**: 2-3 days  
**Prerequisites**: Phase 1 complete

---

## Tasks for Phase 2

### 1. EF Core DbContext Configuration

**Location**: `api/framework/Infrastructure/Persistence/`

#### Files to Create/Modify

**1a. Add Configurations for New Entities**

```csharp
// In Catalog DbContext

modelBuilder.Entity<AssetRequisition>(entity =>
{
    entity.HasKey(x => x.Id);
    entity.Property(x => x.EmployeeId).IsRequired();
    entity.Property(x => x.IssuanceId).IsRequired();
    entity.Property(x => x.Status).IsRequired();
    entity.Property(x => x.RequisitionDate).IsRequired();
    entity.Property(x => x.ExpirationDate);
    entity.Property(x => x.ResponseDate);
    entity.Property(x => x.RejectionReason);
    
    // Complex type for DigitalSignature
    entity.OwnsOne(x => x.AcceptanceSignature, nav =>
    {
        nav.Property(x => x.SignatureData).HasMaxLength(5000);
        nav.Property(x => x.SignedOn);
        nav.Property(x => x.SignedByEmployeeId);
        nav.Property(x => x.IpAddress).HasMaxLength(45);
        nav.Property(x => x.UserAgent).HasMaxLength(500);
        nav.Property(x => x.DeviceFingerprint).HasMaxLength(256);
    });
    
    entity.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    entity.HasOne(x => x.Issuance).WithMany().HasForeignKey(x => x.IssuanceId).OnDelete(DeleteBehavior.Restrict);
    
    entity.HasIndex(x => x.Status);
    entity.HasIndex(x => x.ExpirationDate);
});

modelBuilder.Entity<DepreciationSchedule>(entity =>
{
    entity.HasKey(x => x.Id);
    entity.Property(x => x.PhysicalAssetId).IsRequired();
    entity.Property(x => x.Month).IsRequired();
    entity.Property(x => x.Year).IsRequired();
    entity.Property(x => x.MonthlyDepreciationAmount).HasPrecision(18, 2);
    entity.Property(x => x.AccumulatedDepreciationAmount).HasPrecision(18, 2);
    entity.Property(x => x.Status).IsRequired();
    entity.Property(x => x.PostedDate);
    entity.Property(x => x.Remarks).HasMaxLength(500);
    
    entity.HasOne(x => x.PhysicalAsset).WithMany().HasForeignKey(x => x.PhysicalAssetId).OnDelete(DeleteBehavior.Restrict);
    entity.HasOne(x => x.JournalEntryVoucher).WithMany().HasForeignKey(x => x.JournalEntryVoucherId).OnDelete(DeleteBehavior.SetNull);
    
    entity.HasIndex(x => new { x.PhysicalAssetId, x.Year, x.Month }).IsUnique();
    entity.HasIndex(x => x.Status);
});

modelBuilder.Entity<JournalEntryVoucher>(entity =>
{
    entity.HasKey(x => x.Id);
    entity.Property(x => x.VoucherNumber).IsRequired().HasMaxLength(25);
    entity.Property(x => x.VoucherDate).IsRequired();
    entity.Property(x => x.Month).IsRequired();
    entity.Property(x => x.Year).IsRequired();
    entity.Property(x => x.DepreciationMethod).HasMaxLength(50);
    entity.Property(x => x.TotalDebitAmount).HasPrecision(18, 2);
    entity.Property(x => x.TotalCreditAmount).HasPrecision(18, 2);
    entity.Property(x => x.Status).IsRequired();
    entity.Property(x => x.PostedDate);
    entity.Property(x => x.ExportedDate);
    entity.Property(x => x.ExportFormat).HasMaxLength(20);
    entity.Property(x => x.ExportFileName).HasMaxLength(256);
    entity.Property(x => x.Remarks).HasMaxLength(500);
    
    entity.HasMany(x => x.Entries).WithOne(x => x.JournalEntryVoucher).HasForeignKey(x => x.JournalEntryVoucherId).OnDelete(DeleteBehavior.Cascade);
    
    entity.HasIndex(x => x.VoucherNumber).IsUnique();
    entity.HasIndex(x => new { x.Year, x.Month }).IsUnique();
    entity.HasIndex(x => x.Status);
});

modelBuilder.Entity<JournalEntry>(entity =>
{
    entity.HasKey(x => x.Id);
    entity.Property(x => x.JournalEntryVoucherId).IsRequired();
    entity.Property(x => x.LineNumber).IsRequired();
    entity.Property(x => x.AccountCode).IsRequired().HasMaxLength(20);
    entity.Property(x => x.AccountName).HasMaxLength(150);
    entity.Property(x => x.DebitAmount).HasPrecision(18, 2);
    entity.Property(x => x.CreditAmount).HasPrecision(18, 2);
    entity.Property(x => x.Description).HasMaxLength(500);
    
    entity.HasOne(x => x.JournalEntryVoucher).WithMany(x => x.Entries).HasForeignKey(x => x.JournalEntryVoucherId).OnDelete(DeleteBehavior.Cascade);
    
    entity.HasIndex(x => new { x.JournalEntryVoucherId, x.LineNumber }).IsUnique();
    entity.HasIndex(x => x.AccountCode);
});
```

**1b. Modify PhysicalAsset Configuration**

Add to existing PhysicalAsset configuration:

```csharp
entity.Property(x => x.QRCodeData).HasMaxLength(5000); // Base64 encoded
entity.Property(x => x.PropertyNumber).HasMaxLength(50);
entity.Property(x => x.QRGeneratedDate);
entity.Property(x => x.CurrentCustodianId);
entity.HasIndex(x => x.PropertyNumber).IsUnique();
entity.HasIndex(x => x.CurrentCustodianId);
```

**1c. Modify Issuance Configuration**

Add to existing Issuance configuration:

```csharp
entity.Property(x => x.Type).IsRequired();
entity.Property(x => x.CustodianId);
entity.Property(x => x.Status).IsRequired();
entity.Property(x => x.AcceptedOn);
entity.Property(x => x.RejectionReason).HasMaxLength(500);

entity.OwnsOne(x => x.AcceptanceSignature, nav =>
{
    nav.Property(x => x.SignatureData).HasMaxLength(5000);
    nav.Property(x => x.SignedOn);
    nav.Property(x => x.SignedByEmployeeId);
    nav.Property(x => x.IpAddress).HasMaxLength(45);
    nav.Property(x => x.UserAgent).HasMaxLength(500);
    nav.Property(x => x.DeviceFingerprint).HasMaxLength(256);
});

entity.HasIndex(x => x.Status);
entity.HasIndex(x => x.CustodianId);
```

**1d. Modify Acceptance Configuration**

Update Status handling:

```csharp
entity.Property(x => x.Status).IsRequired().HasConversion<int>();
```

---

### 2. Create Database Migrations

#### PostgreSQL Migration

**File**: `api/migrations/PostgreSQL/[timestamp]_AddUIWorkflowEntities.cs`

```csharp
public partial class AddUIWorkflowEntities : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Create AssetRequisitions table
        migrationBuilder.CreateTable(
            name: "AssetRequisitions",
            schema: "catalog",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                IssuanceId = table.Column<Guid>(type: "uuid", nullable: false),
                RequisitionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ResponseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                AcceptanceSignature_SignatureData = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                AcceptanceSignature_SignedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                AcceptanceSignature_SignedByEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                AcceptanceSignature_IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                AcceptanceSignature_UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                AcceptanceSignature_DeviceFingerprint = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                CreatedBy = table.Column<string>(type: "text", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedBy = table.Column<string>(type: "text", nullable: true),
                UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<string>(type: "text", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AssetRequisitions", x => x.Id);
                table.ForeignKey(
                    name: "FK_AssetRequisitions_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalSchema: "catalog",
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AssetRequisitions_Issuances_IssuanceId",
                    column: x => x.IssuanceId,
                    principalSchema: "catalog",
                    principalTable: "Issuances",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        // Create indexes
        migrationBuilder.CreateIndex(
            name: "IX_AssetRequisitions_Status",
            schema: "catalog",
            table: "AssetRequisitions",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_AssetRequisitions_ExpirationDate",
            schema: "catalog",
            table: "AssetRequisitions",
            column: "ExpirationDate");

        // Similar for DepreciationSchedules, JournalEntryVouchers, JournalEntries
        // ... (full migration code in actual file)
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Drop tables in reverse order
    }
}
```

#### MSSQL Migration

**File**: `api/migrations/MSSQL/[timestamp]_AddUIWorkflowEntities.cs`

(Similar to PostgreSQL but using SQL Server specific syntax)

---

### 3. Create Repository Interfaces

**Location**: `api/modules/Catalog/Catalog.Application/`

```csharp
// IAssetRequisitionRepository.cs
namespace AMIS.WebApi.Catalog.Application.Repositories;

public interface IAssetRequisitionRepository : IRepository<AssetRequisition, Guid>
{
    Task<AssetRequisition?> GetByIssuanceIdAsync(Guid issuanceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssetRequisition>> GetPendingByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssetRequisition>> GetExpiredAsync(CancellationToken cancellationToken = default);
}

// IDepreciationScheduleRepository.cs
public interface IDepreciationScheduleRepository : IRepository<DepreciationSchedule, Guid>
{
    Task<IEnumerable<DepreciationSchedule>> GetByAssetIdAsync(Guid assetId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DepreciationSchedule>> GetByMonthYearAsync(int month, int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<DepreciationSchedule>> GetPendingAsync(CancellationToken cancellationToken = default);
}

// IJournalEntryVoucherRepository.cs
public interface IJournalEntryVoucherRepository : IRepository<JournalEntryVoucher, Guid>
{
    Task<JournalEntryVoucher?> GetByVoucherNumberAsync(string voucherNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<JournalEntryVoucher>> GetByMonthYearAsync(int month, int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<JournalEntryVoucher>> GetDraftAsync(CancellationToken cancellationToken = default);
}
```

---

### 4. Create Repository Implementations

**Location**: `api/modules/Catalog/Catalog.Infrastructure/Persistence/Repositories/`

Similar to existing repositories, implement:
- `AssetRequisitionRepository`
- `DepreciationScheduleRepository`
- `JournalEntryVoucherRepository`

---

### 5. Update Module Registration

**File**: `api/modules/Catalog/CatalogModule.cs`

```csharp
public static void RegisterModuleServices(this IServiceCollection services, IConfiguration config)
{
    // Existing services...
    
    // New repository registrations
    services.AddScoped<IAssetRequisitionRepository, AssetRequisitionRepository>();
    services.AddScoped<IDepreciationScheduleRepository, DepreciationScheduleRepository>();
    services.AddScoped<IJournalEntryVoucherRepository, JournalEntryVoucherRepository>();
}
```

---

### 6. Update Permissions

**File**: `Shared/Authorization/FshPermissions.cs`

```csharp
public static class Permissions
{
    // Existing permissions...
    
    public static class AssetRequisitions
    {
        public const string View = "Permissions.AssetRequisitions.View";
        public const string Accept = "Permissions.AssetRequisitions.Accept";
        public const string Reject = "Permissions.AssetRequisitions.Reject";
    }

    public static class Depreciation
    {
        public const string View = "Permissions.Depreciation.View";
        public const string Calculate = "Permissions.Depreciation.Calculate";
    }

    public static class JEV
    {
        public const string View = "Permissions.JEV.View";
        public const string Create = "Permissions.JEV.Create";
        public const string Post = "Permissions.JEV.Post";
        public const string Export = "Permissions.JEV.Export";
    }
}
```

---

## Checklist

- [ ] Add DbContext configurations for all 4 new entities
- [ ] Modify PhysicalAsset DbContext configuration
- [ ] Modify Issuance DbContext configuration
- [ ] Modify Acceptance DbContext configuration
- [ ] Create PostgreSQL migration
- [ ] Create MSSQL migration
- [ ] Test migrations locally
- [ ] Create repository interfaces
- [ ] Create repository implementations
- [ ] Update module registration
- [ ] Update permissions
- [ ] Run build to verify no compilation errors
- [ ] Create simple unit tests for repositories

---

## Verification Steps

1. **Build Project**
   ```
   dotnet build AMIS.9.sln
   ```

2. **Apply Migrations** (if running locally)
   ```
   dotnet ef database update --project api/migrations/PostgreSQL
   ```

3. **Verify Tables Created**
   - AssetRequisitions
   - DepreciationSchedules
   - JournalEntryVouchers
   - JournalEntries

4. **Check Column Additions**
   - PhysicalAssets: QRCodeData, PropertyNumber, CurrentCustodianId
   - Issuances: Type, CustodianId, Status, AcceptanceSignature
   - Acceptances: Updated Status enum

---

## Notes

- All new repositories should inherit from `IRepository<T, TId>` from framework
- Use async/await pattern throughout
- Include `CancellationToken` in all async methods
- Add TenantId shadow property handling in repositories
- Follow existing pagination patterns
- Include proper exception handling

---

## What's Next

After Phase 2 completion:
- Phase 3: Create Application Layer (Commands, Queries, Handlers)
- Phase 4: Create Carter Endpoints
- Phase 5: Create Blazor UI Components

