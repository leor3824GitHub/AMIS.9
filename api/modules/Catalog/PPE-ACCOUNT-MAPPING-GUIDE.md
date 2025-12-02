# PPE Account Code Mapping - Configuration Guide

## Problem
The original `PhysicalAsset.GetPPEAccountCode()` method had hardcoded mappings between PPE types and RCA account codes. This created several issues:
- **Inflexibility**: Adding new PPE types required code changes and redeployment
- **Regulatory compliance**: COA circulars can change account codes, requiring quick updates
- **Multi-agency support**: Different agencies might have different categorizations

## Solution Architecture

### 1. Domain Service Pattern (`IPPEAccountCodeMapper`)
Abstracts PPE type-to-account code mapping logic outside the entity.

```csharp
public interface IPPEAccountCodeMapper
{
    string GetAccountCode(string ppeType);
}
```

### 2. Implementation Options

#### Option A: Default In-Memory Mapper (Quick Setup)
**File**: `DefaultPPEAccountCodeMapper.cs`
- Dictionary-based lookup with `StringComparer.OrdinalIgnoreCase`
- No database dependency
- Suitable for agencies with stable PPE type classifications

**Usage**:
```csharp
services.AddScoped<IPPEAccountCodeMapper, DefaultPPEAccountCodeMapper>();
```

#### Option B: Database-Driven Mapper (Recommended for Production)
**Files**: 
- `PPETypeAccountMapping.cs` - Entity for storing mappings
- `DatabasePPEAccountCodeMapper.cs` - Service with caching
- `IPPETypeAccountMappingRepository` - Data access interface

**Benefits**:
- Runtime configuration via admin UI
- Audit trail for mapping changes
- 5-minute cache to reduce database load
- Automatic fallback to default mapper on errors

**Usage**:
```csharp
services.AddScoped<DefaultPPEAccountCodeMapper>();
services.AddScoped<IPPEAccountCodeMapper, DatabasePPEAccountCodeMapper>();
services.AddScoped<IPPETypeAccountMappingRepository, PPETypeAccountMappingRepository>();
```

### 3. Database Schema (for Option B)

```sql
CREATE TABLE catalog.ppe_type_account_mappings (
    id UUID PRIMARY KEY,
    ppe_type VARCHAR(100) NOT NULL,
    rca_account_code VARCHAR(50) NOT NULL,
    description VARCHAR(500) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    created TIMESTAMPTZ NOT NULL,
    created_by UUID NOT NULL,
    last_modified TIMESTAMPTZ NOT NULL,
    last_modified_by UUID,
    deleted TIMESTAMPTZ,
    deleted_by UUID
);

CREATE UNIQUE INDEX idx_ppe_type_active 
ON catalog.ppe_type_account_mappings(ppe_type) 
WHERE is_active = TRUE AND deleted IS NULL;
```

### 4. Seed Data (Initial Mappings)

```csharp
public static class PPETypeAccountMappingSeed
{
    public static List<PPETypeAccountMapping> GetDefaultMappings()
    {
        return new List<PPETypeAccountMapping>
        {
            PPETypeAccountMapping.Create("MACHINERY", RCAAccountCode.MachineryAndEquipment, 
                "Heavy machinery and industrial equipment"),
            PPETypeAccountMapping.Create("EQUIPMENT", RCAAccountCode.MachineryAndEquipment, 
                "General equipment and tools"),
            PPETypeAccountMapping.Create("TRANSPORTATION", RCAAccountCode.TransportationEquipment, 
                "Vehicles and transportation assets"),
            PPETypeAccountMapping.Create("VEHICLE", RCAAccountCode.TransportationEquipment, 
                "Motor vehicles"),
            PPETypeAccountMapping.Create("FURNITURE", RCAAccountCode.FurnitureFixturesAndBooksEquipment, 
                "Office furniture and fixtures"),
            PPETypeAccountMapping.Create("ICT", RCAAccountCode.ICTEquipment, 
                "Information and communication technology"),
            PPETypeAccountMapping.Create("COMPUTER", RCAAccountCode.ICTEquipment, 
                "Computer hardware and peripherals")
        };
    }
}
```

## Migration Path

### Phase 1: Add Service Infrastructure (Current)
✅ Created `IPPEAccountCodeMapper` interface  
✅ Created `DefaultPPEAccountCodeMapper` implementation  
✅ Created `PPETypeAccountMapping` entity  
✅ Created `DatabasePPEAccountCodeMapper` with caching  
✅ Documented the hardcoded issue in `PhysicalAsset`

### Phase 2: Entity Refactoring (TODO)
- Inject `IPPEAccountCodeMapper` into `PhysicalAsset` via domain event or factory
- OR: Move `RCAAccountCode` computation to application layer
- OR: Use specification pattern for account code resolution

### Phase 3: Admin UI (TODO)
- CRUD endpoints for `PPETypeAccountMapping`
- Blazor admin page for managing mappings
- Permission: `Permissions.Catalog.ManagePPEMappings`

## Usage Examples

### Application Layer Usage
```csharp
public class CreatePhysicalAssetHandler
{
    private readonly IPPEAccountCodeMapper _mapper;
    
    public async Task<PhysicalAssetResponse> Handle(CreateCommand request)
    {
        var asset = PhysicalAsset.Create(...);
        
        // Resolve account code at application layer instead of computed property
        var accountCode = _mapper.GetAccountCode(asset.PPEType);
        
        // Use in reporting/JEV generation
        var jev = new JournalEntry(accountCode, asset.AcquisitionCost);
    }
}
```

### Admin Management (Future)
```csharp
// Add new mapping when COA issues new circular
var mapping = PPETypeAccountMapping.Create(
    "MEDICAL_EQUIPMENT",
    "1-06-07-010", // New RCA code from COA Circular 2025-XXX
    "Medical and laboratory equipment"
);
await _repository.AddAsync(mapping);

// Update existing mapping
mapping.Update("1-06-07-020", "Updated per COA Circular 2025-XXX");
```

## Testing

```csharp
[Fact]
public void GetAccountCode_MachineryType_ReturnsMachineryAccountCode()
{
    // Arrange
    var mapper = new DefaultPPEAccountCodeMapper();
    
    // Act
    var accountCode = mapper.GetAccountCode("MACHINERY");
    
    // Assert
    Assert.Equal(RCAAccountCode.MachineryAndEquipment, accountCode);
}

[Fact]
public void GetAccountCode_CaseInsensitive_ReturnsCorrectCode()
{
    var mapper = new DefaultPPEAccountCodeMapper();
    Assert.Equal(
        mapper.GetAccountCode("Machinery"), 
        mapper.GetAccountCode("MACHINERY")
    );
}
```

## Benefits

✅ **Configurability**: Mappings can be changed without code deployment  
✅ **Compliance**: Quick adaptation to COA/DBM regulatory changes  
✅ **Audit Trail**: All mapping changes are tracked with timestamps and users  
✅ **Performance**: 5-minute cache reduces database load  
✅ **Resilience**: Automatic fallback to default mappings on errors  
✅ **Extensibility**: Easy to add new PPE types via admin UI

## Next Steps

1. Implement `IPPETypeAccountMappingRepository` in Infrastructure layer
2. Add EF Core configuration for `PPETypeAccountMapping`
3. Create migration for database table
4. Add seed data in `CatalogDbInitializer`
5. Create CRUD endpoints in Application layer
6. Build admin UI in Blazor
7. Consider moving `RCAAccountCode` computation to application layer to avoid computed property limitations
