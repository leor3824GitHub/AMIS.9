# Property Code Seed Data Examination

**Date:** February 5, 2026  
**Status:** Examination Complete

---

## Overview

The codebase has **NO dedicated seed data for PropertyCodeSequence** entities. This is a critical finding for the property code generation feature.

---

## Current State

### PropertyCodeSequence Domain Entity
**Location:** [api/modules/Inventories/Inventories.Domain/PropertyCodeSequence.cs](api/modules/Inventories/Inventories.Domain/PropertyCodeSequence.cs)

**Structure:**
```csharp
public sealed class PropertyCodeSequence : AuditableEntity, IAggregateRoot
{
    public int Id { get; set; }
    public string Classification { get; set; } = default!;      // e.g., "PPE", "001"
    public string Category { get; set; } = default!;              // e.g., "Furniture", "Equipment"
    public int LastSequenceValue { get; set; }                    // Sequence counter (0, 1, 2, ...)
}
```

**Key Methods:**
- `Create(string classification, string category)` - Factory method (starts with LastSequenceValue = 0)
- `Increment()` - Atomically increments and returns new sequence value

### PropertyCodeSequenceConfiguration
**Location:** [api/modules/Inventories/Inventories.Infrastructure/Persistence/Configurations/PropertyCodeSequenceConfiguration.cs](api/modules/Inventories/Inventories.Infrastructure/Persistence/Configurations/PropertyCodeSequenceConfiguration.cs)

**Configuration:**
```csharp
public void Configure(EntityTypeBuilder<PropertyCodeSequence> builder)
{
    builder.ToTable("PropertyCodeSequences", SchemaNames.Inventories);
    builder.HasKey(e => e.Id);
    
    builder.Property(e => e.Classification)
        .IsRequired()
        .HasMaxLength(100);
    
    builder.Property(e => e.Category)
        .IsRequired()
        .HasMaxLength(100);
    
    builder.Property(e => e.LastSequenceValue)
        .IsRequired();
    
    // ✅ UNIQUE constraint on (Classification, Category) pair
    builder.HasIndex(e => new { e.Classification, e.Category })
        .IsUnique();
    
    // ❌ NO SEED DATA CONFIGURED - HasData() NOT CALLED
}
```

---

## Seed Data Strategy Analysis

### Current Seed Data Flow

**Location:** [api/modules/Inventories/Inventories.Infrastructure/Persistence/InventoriesDbInitializer.cs](api/modules/Inventories/Inventories.Infrastructure/Persistence/InventoriesDbInitializer.cs)

**Seeded Entities:**
1. ✅ Categories (10) - via `SeedCategoriesAsync()`
2. ✅ Suppliers (10) - via `SeedSuppliersAsync()`
3. ✅ Employees (10) - via `SeedEmployeesAsync()`
4. ✅ Products (10) - via `SeedProductsAsync()`
5. ✅ Purchases with Items (10) - via `SeedPurchasesAsync()`
6. ✅ Inspections (10) - via `SeedInspectionsAsync()`
7. ✅ Inventory Registry (5 items) - via `SeedInventoryRegistriesAsync()`
8. ✅ PPE Type Account Mappings - via `SeedPPETypeAccountMappingsAsync()`
9. ❌ **PropertyCodeSequence - NOT SEEDED**

### InventoryRegistry Seed Example

**Pattern Used:**
```csharp
private static void SeedTestData(EntityTypeBuilder<InventoryRegistry> builder)
{
    var seedTimestamp = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    var seedData = new[]
    {
        CreateSeedItem(Guid.Parse("10000000-0000-0000-0000-000000000001"), 
                      "234",  // PropertyCode (simple numbers)
                      "Desktop Computer", 
                      10, 
                      "IT Office - Room 101", 
                      "PPERR-001", 
                      seedTimestamp),
        // ... more items
    };
    builder.HasData(seedData);
}
```

**Key Patterns:**
- Uses static timestamp (2026-01-01) to prevent model changes on rebuilds
- Returns anonymous object with all required properties
- Called in `OnModelCreating()` via `SeedTestData()` method
- No async/await (EF Core seed data is synchronous)

---

## Gap Analysis: Why PropertyCodeSequence Should Be Seeded

### Problem Statement

The PropertyCodeGenerator.razor page expects pre-existing PropertyCodeSequence records for Classification/Category combinations:

```csharp
// PropertyCodeGenerator.razor
var allocateRequest = new AllocatePropertyCodeSequenceRequest
{
    Classification = _selectedClassification.Trim(),  // e.g., "PPE"
    Category = _selectedCategory.Trim()                // e.g., "001", "Furniture"
};

_allocatedSequence = await ApiClient
    .PropertyCodeSequenceEndpointsAllocateAsync("1", allocateRequest);
```

**AllocatePropertyCodeSequenceHandler Behavior:**

```csharp
public async Task<int> Handle(AllocatePropertyCodeSequenceCommand request, CancellationToken cancellationToken)
{
    // 1. Query existing sequence for Classification/Category
    var spec = new PropertyCodeSequenceByClassificationAndCategorySpec(
        request.Classification, 
        request.Category);
    var sequence = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

    // 2. CREATE if doesn't exist (auto-creates on first use)
    if (sequence is null)
    {
        sequence = PropertyCodeSequence.Create(request.Classification, request.Category);
        await _repository.AddAsync(sequence, cancellationToken);
    }

    // 3. Increment and return sequence number
    sequence.Increment();
    await _repository.UpdateAsync(sequence, cancellationToken);
    return sequence.LastSequenceValue;
}
```

**Current Behavior:**
- ✅ Auto-creates PropertyCodeSequence on first allocation (idempotent)
- ❌ No pre-populated sequences for UI dropdowns to display
- ⚠️ Users must use the PropertyCodeGenerator page to create sequences on-demand

### Issues Without Seed Data

1. **Empty Dropdown Options** - PropertyCodeSequenceEndpointsListAsync() returns empty list initially
2. **No Pre-configured Classifications** - Users don't know what options are available
3. **Manual Sequence Creation Required** - First-time users must know the format
4. **Testing Overhead** - Developers must create test sequences before testing PAR workflow

---

## Recommended Seed Data Structure

### PPE Property Code Sequences (from NFA standards)

Based on [api/modules/Inventories/Inventories.Infrastructure/Persistence/Data](api/modules/Inventories/Inventories.Infrastructure/Persistence/Data), the system should seed:

```csharp
// Classification codes
private static readonly string[] Classifications = new[]
{
    "PPE",          // Personal Protective Equipment
    "001",          // Office Equipment
    "002",          // IT Hardware
    "003",          // Furniture
    "004",          // Vehicles
    "005",          // Heavy Machinery
};

// Category codes
private static readonly string[] Categories = new[]
{
    "001",          // Common/Generic items
    "002",          // Specialized equipment
    "003",          // Consumables/Furniture
    "004",          // Technology items
    "005",          // Transportation
};
```

### Proposed Seed Data Configuration

**Location:** [api/modules/Inventories/Inventories.Infrastructure/Persistence/Configurations/PropertyCodeSequenceConfiguration.cs](api/modules/Inventories/Inventories.Infrastructure/Persistence/Configurations/PropertyCodeSequenceConfiguration.cs)

```csharp
public class PropertyCodeSequenceConfiguration : IEntityTypeConfiguration<PropertyCodeSequence>
{
    public void Configure(EntityTypeBuilder<PropertyCodeSequence> builder)
    {
        builder.ToTable("PropertyCodeSequences", SchemaNames.Inventories);
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Classification)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Category)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.LastSequenceValue)
            .IsRequired();
        
        builder.HasIndex(e => new { e.Classification, e.Category })
            .IsUnique();
        
        // ✅ ADD SEED DATA HERE
        SeedDefaultPropertyCodeSequences(builder);
    }
    
    private static void SeedDefaultPropertyCodeSequences(EntityTypeBuilder<PropertyCodeSequence> builder)
    {
        var seedData = new object[]
        {
            // PPE Classifications with multiple categories
            new { Id = 1, Classification = "PPE", Category = "001", LastSequenceValue = 0 },
            new { Id = 2, Classification = "PPE", Category = "002", LastSequenceValue = 0 },
            new { Id = 3, Classification = "PPE", Category = "003", LastSequenceValue = 0 },
            
            // Office Equipment
            new { Id = 4, Classification = "001", Category = "001", LastSequenceValue = 0 },
            new { Id = 5, Classification = "001", Category = "002", LastSequenceValue = 0 },
            
            // IT Hardware
            new { Id = 6, Classification = "002", Category = "001", LastSequenceValue = 0 },
            new { Id = 7, Classification = "002", Category = "002", LastSequenceValue = 0 },
            
            // Furniture
            new { Id = 8, Classification = "003", Category = "001", LastSequenceValue = 0 },
            new { Id = 9, Classification = "003", Category = "002", LastSequenceValue = 0 },
            
            // Vehicles
            new { Id = 10, Classification = "004", Category = "001", LastSequenceValue = 0 },
            
            // Heavy Machinery
            new { Id = 11, Classification = "005", Category = "001", LastSequenceValue = 0 },
        };
        
        builder.HasData(seedData);
    }
}
```

---

## Implementation Options

### Option 1: Configure in PropertyCodeSequenceConfiguration (Recommended)
**Pros:**
- Follows EF Core best practices
- Configuration in one place with other entity settings
- Syncs with database migrations automatically
- Cleaner architecture

**Cons:**
- Requires new migration

**Code Location:**
[api/modules/Inventories/Inventories.Infrastructure/Persistence/Configurations/PropertyCodeSequenceConfiguration.cs](api/modules/Inventories/Inventories.Infrastructure/Persistence/Configurations/PropertyCodeSequenceConfiguration.cs)

### Option 2: Add to InventoriesDbInitializer
**Pros:**
- Aligns with existing seeding pattern
- No migration needed
- Can use async operations
- Easier to add complex logic

**Cons:**
- Mixing HasData() and async seed patterns
- Less clean separation

**Code Location:**
[api/modules/Inventories/Inventories.Infrastructure/Persistence/InventoriesDbInitializer.cs](api/modules/Inventories/Inventories.Infrastructure/Persistence/InventoriesDbInitializer.cs)

**Pattern:**
```csharp
public async Task SeedAsync(CancellationToken cancellationToken)
{
    // ... existing code ...
    
    // 9. Seed Property Code Sequences
    await SeedPropertyCodeSequencesAsync(cancellationToken);
    
    logger.LogInformation("[{Tenant}] Comprehensive seed data generation completed", 
        context.TenantInfo!.Identifier);
}

private async Task SeedPropertyCodeSequencesAsync(CancellationToken cancellationToken)
{
    if (await context.PropertyCodeSequences.AnyAsync(cancellationToken))
    {
        logger.LogInformation("[{Tenant}] PropertyCodeSequences already seeded", 
            context.TenantInfo!.Identifier);
        return;
    }
    
    var sequences = new[]
    {
        PropertyCodeSequence.Create("PPE", "001"),
        PropertyCodeSequence.Create("PPE", "002"),
        // ... more sequences
    };
    
    foreach (var sequence in sequences)
    {
        await context.PropertyCodeSequences.AddAsync(sequence, cancellationToken);
    }
    
    await context.SaveChangesAsync(cancellationToken);
    logger.LogInformation("[{Tenant}] Seeded {Count} PropertyCodeSequences", 
        context.TenantInfo!.Identifier, sequences.Length);
}
```

---

## Impact Analysis

### Current State (No Seed)
- ✅ PropertyCodeSequence auto-creates on first allocation (idempotent)
- ❌ PropertyCodeGenerator dropdown shows empty list initially
- ⚠️ Requires manual creation before usage
- ⚠️ Testing requires pre-setup

### With Seed Data
- ✅ PropertyCodeGenerator dropdown populated immediately
- ✅ Known Classification/Category combinations available
- ✅ Better user experience
- ✅ Easier testing and demos
- ✅ Clear system expectations

---

## Summary Table

| Component | Current Status | Has Seed Data | Notes |
|-----------|----------------|---------------|-------|
| Categories | ✅ Domain Entity | ✅ YES (10 records) | Seeded in InventoriesDbInitializer |
| Suppliers | ✅ Domain Entity | ✅ YES (10 records) | Seeded in InventoriesDbInitializer |
| Employees | ✅ Domain Entity | ✅ YES (10 records) | Seeded in InventoriesDbInitializer |
| Products | ✅ Domain Entity | ✅ YES (10 records) | Seeded in InventoriesDbInitializer |
| Purchases | ✅ Domain Entity | ✅ YES (10 records) | Seeded in InventoriesDbInitializer |
| Inspections | ✅ Domain Entity | ✅ YES (10 records) | Seeded in InventoriesDbInitializer |
| InventoryRegistry | ✅ Domain Entity | ✅ YES (5 records) | Seeded in Configuration |
| **PropertyCodeSequence** | ✅ Domain Entity | ❌ **NO** | **Needs seed data** |
| PPETypeAccountMapping | ✅ Domain Entity | ✅ YES | Seeded in InventoriesDbInitializer |

---

## Conclusion

The PropertyCodeSequence table has **NO seed data** configured. While the system is designed to auto-create sequences on first use (idempotent), seed data should be added to:

1. **Populate the PropertyCodeGenerator dropdown** with known Classification/Category options
2. **Improve user experience** by showing available property code sequences upfront
3. **Support testing workflows** without manual sequence creation
4. **Document expected property code formats** in seed data

**Recommendation:** Implement seed data using **Option 1** (PropertyCodeSequenceConfiguration) to follow EF Core best practices and maintain consistency with the codebase architecture.
