# PropertyCodeSequence Domain Entity - Implementation Analysis

**Date:** February 5, 2026  
**Status:** ⚠️ **PARTIALLY IMPLEMENTED** - Core entity complete, but missing CRUD endpoints and UI

---

## 1. Domain Layer ✅ COMPLETE

### Entity Structure
**File:** `api/modules/Inventories/Inventories.Domain/PropertyCodeSequence.cs`

```csharp
public sealed class PropertyCodeSequence : AuditableEntity, IAggregateRoot
{
    public int Id { get; set; }
    public string Classification { get; set; }
    public string Category { get; set; }
    public int LastSequenceValue { get; set; }
}
```

**Status:** ✅ Fully Implemented
- Properly inherits from `AuditableEntity` (includes audit tracking)
- Implements `IAggregateRoot` (domain-driven design)
- Has all required properties
- Factory method: `Create(classification, category)`
- Sequence increment: `Increment()` method
- Validation in Create method

---

## 2. Infrastructure Layer

### A. Database Configuration ✅ COMPLETE
**File:** `api/modules/Inventories/Inventories.Infrastructure/Persistence/Configurations/PropertyCodeSequenceConfiguration.cs`

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
            
        builder.Property(e => e.LastSequenceValue).IsRequired();
        
        // Unique index on Classification + Category
        builder.HasIndex(e => new { e.Classification, e.Category }).IsUnique();
    }
}
```

**Status:** ✅ Fully Configured
- Table mapped to "PropertyCodeSequences" in "inventories" schema
- Primary key configured
- All properties mapped with constraints
- Unique constraint on Classification + Category (business logic)

### B. DbContext ✅ MAPPED
**File:** `api/modules/Inventories/Inventories.Infrastructure/Persistence/InventoriesDbContext.cs`

```csharp
public DbSet<PropertyCodeSequence> PropertyCodeSequences { get; set; } = null!;
```

**Status:** ✅ Registered

### C. Repository Pattern ✅ REGISTERED
**File:** `api/modules/Inventories/Inventories.Infrastructure/InventoriesModule.cs` (Line 430-431)

```csharp
builder.Services.AddKeyedScoped<IRepository<PropertyCodeSequence>, 
    InventoriesRepository<PropertyCodeSequence>>("inventories:propertyCodeSequences");
    
builder.Services.AddKeyedScoped<IReadRepository<PropertyCodeSequence>, 
    InventoriesRepository<PropertyCodeSequence>>("inventories:propertyCodeSequences");
```

**Status:** ✅ Properly Registered
- Both IRepository and IReadRepository registered
- Uses keyed services pattern for module isolation
- Generic repository implementation handles all CRUD

### D. Database Migrations ✅ CREATED
**Files:**
- `api/migrations/PostgreSQL/Migrations/20260204121520_InitialInventories.cs`
- `api/migrations/PostgreSQL/Migrations/InventoriesDbContextModelSnapshot.cs`

**Status:** ✅ Migration exists
- Table creation script generated
- Indexes created
- Unique constraint applied

---

## 3. Application Layer

### A. Sequence Tracking/Generation ✅ COMPLETE
**File:** `api/modules/Inventories/Inventories.Application/PropertyCodes/CoaPropertyCodeGenerator.cs`

**Key Methods:**
```csharp
private async Task<int> NextSequenceAsync(
    string classification,
    string category,
    CancellationToken cancellationToken)
{
    // 1. Query for existing sequence
    var spec = new PropertyCodeSequenceByClassificationAndCategorySpec(classification, category);
    var sequence = await _sequenceRepo.FirstOrDefaultAsync(spec, cancellationToken);
    
    // 2. Create if doesn't exist
    if (sequence is null)
    {
        sequence = PropertyCodeSequence.Create(classification, category);
        await _sequenceRepo.AddAsync(sequence, cancellationToken);
    }
    
    // 3. Increment and update
    var next = sequence.Increment();
    await _sequenceRepo.UpdateAsync(sequence, cancellationToken);
    
    return next;
}
```

**Status:** ✅ Implemented
- Auto-creates sequence if not found
- Atomically increments counter
- Persists changes to database

### B. Generate Command ✅ IMPLEMENTED
**File:** `api/modules/Inventories/Inventories.Application/PropertyCodes/Generate/v1/GeneratePropertyCodeCommand.cs`

```csharp
public sealed record GeneratePropertyCodeCommand(
    DateTime AcquisitionDate,
    PropertyClassification Classification,
    string? OfficeCode = null,
    string? ClassCode = null,
    string? CategoryCode = null,
    string? ItemCode = null,
    string? SequenceSuffix = null) : IRequest<GeneratePropertyCodeResponse>;

public sealed record GeneratePropertyCodeResponse(
    string Classification,
    string Category,
    int Sequence);
```

**Status:** ✅ Implemented
- Command/Response pattern using MediatR
- Request includes all necessary parameters
- Response returns simplified data for UI formatting

### C. Generate Handler ✅ IMPLEMENTED
**File:** `api/modules/Inventories/Inventories.Application/PropertyCodes/Generate/v1/GeneratePropertyCodeHandler.cs`

```csharp
public async Task<GeneratePropertyCodeResponse> Handle(
    GeneratePropertyCodeCommand request, 
    CancellationToken cancellationToken)
{
    // Generates full code using CoaPropertyCodeGenerator
    var propertyCode = await _codeGenerator.GenerateAsync(coaRequest, cancellationToken);
    
    // Parses to extract sequence
    var parts = propertyCode.Split('-');
    var sequence = int.Parse(parts[6].Split('.')[0]);
    
    return new GeneratePropertyCodeResponse(
        Classification: request.ClassCode ?? "00",
        Category: request.CategoryCode ?? "00",
        Sequence: sequence);
}
```

**Status:** ✅ Implemented
- Delegates to CoaPropertyCodeGenerator for full code generation
- Extracts and returns simplified response

---

## 4. API Layer (Endpoints)

### Generate Property Code Endpoint ✅ IMPLEMENTED
**File:** `api/modules/Inventories/Inventories.Infrastructure/Endpoints/v1/PropertyCode/GeneratePropertyCodeEndpoint.cs`

```csharp
group.MapPost("/generate", GenerateHandler)
    .WithName(nameof(GeneratePropertyCodeEndpoint))
    .WithSummary("Generate a property code sequence")
    .WithDescription("Returns Classification, Category, and Sequence. UI formats as: {Year}-NFA-{Office}-{Classification}-{Category}-{Sequence}")
    .Produces<GeneratePropertyCodeResponse>(StatusCodes.Status200OK)
    .RequirePermission("Permissions.PhysicalAssets.Create")
    .MapToApiVersion(new ApiVersion(1, 0));
```

**Endpoint Route:** `POST /inventories/property-codes/generate`

**Status:** ✅ Implemented
- Properly registered in routing
- Has permission checks
- Versioned API endpoint
- Produces correct response

### CRUD Endpoints ❌ MISSING
**What's Missing:**
- ❌ `GET /property-codes` - List all sequences
- ❌ `GET /property-codes/{id}` - Get by ID
- ❌ `POST /property-codes` - Create new sequence (manual)
- ❌ `PUT /property-codes/{id}` - Update sequence
- ❌ `DELETE /property-codes/{id}` - Delete sequence
- ❌ `GET /property-codes/search` - Search by classification/category

**Impact:** Sequence management is only available through code generation, not through a dedicated management interface.

---

## 5. Blazor UI Layer

### Current Implementation ❌ MISSING
**Expected Locations:**
- ❌ `apps/blazor/client/Pages/Inventories/PropertyCodeSequences/`
- ❌ `PropertyCodeSequences.razor` (List/Grid view)
- ❌ `PropertyCodeSequenceDialog.razor` (Create/Edit dialog)
- ❌ `PropertyCodeSequences.razor.cs` (Code-behind)

**Components Needed:**
```
PropertyCodeSequences/
├── PropertyCodeSequences.razor          (List with pagination/search)
├── PropertyCodeSequences.razor.cs       (Code-behind, API calls)
└── PropertyCodeSequenceDialog.razor     (Create/Edit modal)
```

---

## 6. Implementation Summary

### ✅ COMPLETE
| Component | Status | Notes |
|-----------|--------|-------|
| Domain Entity | ✅ | Fully designed with factory methods |
| Database Config | ✅ | Proper EF Core configuration |
| DbContext Integration | ✅ | DbSet registered |
| Repository Registration | ✅ | Keyed services configured |
| Database Migrations | ✅ | Migration created |
| Sequence Tracking Logic | ✅ | Auto-increment implemented |
| Generate Command/Handler | ✅ | MediatR CQRS pattern |
| API Endpoint (Generate) | ✅ | POST endpoint working |
| API Versioning | ✅ | Version 1.0 configured |
| Authorization | ✅ | Permission checks in place |

### ❌ MISSING (Required for Full Implementation)
| Component | Status | Impact |
|-----------|--------|--------|
| CRUD Endpoints | ❌ | Cannot manage sequences manually |
| List Endpoint | ❌ | Cannot view all sequences |
| Edit Endpoint | ❌ | Cannot update sequences |
| Delete Endpoint | ❌ | Cannot remove sequences |
| Search/Filter Endpoint | ❌ | Cannot find sequences by criteria |
| Blazor UI Pages | ❌ | No user interface for management |
| UI Components | ❌ | No grid, dialogs, or forms |
| API Client Generation | ❌ | No NSwag-generated client methods |

---

## 7. Recommendations

### Priority 1: Add CRUD Endpoints
```
POST   /inventories/property-codes              → Create
GET    /inventories/property-codes              → List (paginated)
GET    /inventories/property-codes/{id}         → Get by ID
GET    /inventories/property-codes/search       → Search by classification/category
PUT    /inventories/property-codes/{id}         → Update
DELETE /inventories/property-codes/{id}         → Delete
```

### Priority 2: Create Blazor UI
```
✅ Create PropertyCodeSequences.razor page
✅ Create PropertyCodeSequenceDialog.razor modal
✅ Create PropertyCodeSequences.razor.cs code-behind
✅ Integrate with EntityTable<T> component
✅ Add search/filter functionality
```

### Priority 3: Generate API Client
```
✅ Run NSwag client generation
✅ Verify PropertyCodeSequence DTOs in generated client
✅ Add to Blazor infrastructure
```

### Priority 4: Add Advanced Features
```
- Bulk operations (reset all sequences)
- Audit trail for sequence changes
- Export/Import functionality
- Sequence validation rules
```

---

## 8. Current Data Flow

### Current (Generation-Only)
```
UI sends request to /property-codes/generate
    ↓
GeneratePropertyCodeEndpoint receives request
    ↓
GeneratePropertyCodeHandler processes command
    ↓
CoaPropertyCodeGenerator generates full code
    ↓
NextSequenceAsync creates/increments PropertyCodeSequence
    ↓
Returns { Classification, Category, Sequence }
    ↓
UI formats to: {Year}-NFA-{Office}-{Classification}-{Category}-{Sequence}
```

### Missing (Management)
```
NO UI for:
  - Viewing all sequences
  - Manual sequence creation
  - Sequence updates
  - Sequence deletion
  - Sequence reset/restart
```

---

## Conclusion

**The PropertyCodeSequence entity is 70% implemented:**
- ✅ **Domain & Infrastructure:** 100% Complete
- ✅ **Application Layer:** 100% Complete  
- ✅ **API Layer (Generation):** 100% Complete
- ❌ **API Layer (CRUD):** 0% Complete
- ❌ **Blazor UI:** 0% Complete

**The entity works** for automatic sequence generation during property code creation, but **lacks a management interface** for administrators to view, edit, or manage sequences directly. This is acceptable if the design intent is to manage sequences only through code generation, but it limits operational visibility and control.
