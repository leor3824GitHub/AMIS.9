# PropertyCodeSequence CRUD Endpoints - Implementation Complete

**Date:** February 5, 2026  
**Status:** ✅ **FULLY IMPLEMENTED & TESTED**

---

## Implemented Endpoints

### 1. **List Property Code Sequences** ✅
**Endpoint:** `GET /inventories/property-code-sequences`  
**Permissions:** `Permissions.PhysicalAssets.View`  
**Parameters:**
- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10)
- `searchTerm` (string, optional) - Search by Classification or Category

**Response:**
```json
{
  "items": [
    {
      "id": 1,
      "classification": "PPE",
      "category": "001",
      "lastSequenceValue": 42
    }
  ],
  "totalCount": 100,
  "pageNumber": 1,
  "pageSize": 10
}
```

**Features:**
- Paginated results
- Optional search by classification or category
- Sorted by Classification then Category
- Full counts for pagination

---

### 2. **Get Property Code Sequence by ID** ✅
**Endpoint:** `GET /inventories/property-code-sequences/{id}`  
**Permissions:** `Permissions.PhysicalAssets.View`  
**Parameters:**
- `id` (int) - Sequence ID

**Response:**
```json
{
  "id": 1,
  "classification": "PPE",
  "category": "001",
  "lastSequenceValue": 42
}
```

**Error Handling:** Returns 404 if not found

---

### 3. **Create Property Code Sequence** ✅
**Endpoint:** `POST /inventories/property-code-sequences`  
**Permissions:** `Permissions.PhysicalAssets.Create`  
**Request Body:**
```json
{
  "classification": "PPE",
  "category": "001"
}
```

**Response:**
```json
{
  "id": 1,
  "classification": "PPE",
  "category": "001",
  "lastSequenceValue": 0
}
```

**Features:**
- Auto-validates classification and category (not null/whitespace)
- Unique constraint on Classification + Category combination
- Returns 201 Created with Location header
- Initializes LastSequenceValue to 0

---

### 4. **Update Property Code Sequence** ✅
**Endpoint:** `PUT /inventories/property-code-sequences/{id}`  
**Permissions:** `Permissions.PhysicalAssets.Edit`  
**Request Body:**
```json
{
  "classification": "PPE",
  "category": "001",
  "lastSequenceValue": 50
}
```

**Response:**
```json
{
  "id": 1,
  "classification": "PPE",
  "category": "001",
  "lastSequenceValue": 50
}
```

**Features:**
- Allows manual update of classification and category
- Allows resetting/adjusting LastSequenceValue (for administrative purposes)
- Validates LastSequenceValue >= 0
- Returns 404 if sequence not found

---

### 5. **Delete Property Code Sequence** ✅
**Endpoint:** `DELETE /inventories/property-code-sequences/{id}`  
**Permissions:** `Permissions.PhysicalAssets.Delete`  
**Parameters:**
- `id` (int) - Sequence ID

**Response:**
```json
{
  "success": true,
  "message": "PropertyCodeSequence 'PPE'-'001' deleted successfully."
}
```

**Error Handling:** Returns 404 if not found

---

## Implementation Details

### File Structure
```
api/modules/Inventories/
├── Inventories.Application/
│   └── PropertyCodeSequences/
│       ├── List/v1/
│       │   ├── ListPropertyCodeSequencesCommand.cs
│       │   └── ListPropertyCodeSequencesHandler.cs
│       ├── GetById/v1/
│       │   ├── GetPropertyCodeSequenceCommand.cs
│       │   └── GetPropertyCodeSequenceHandler.cs
│       ├── Create/v1/
│       │   ├── CreatePropertyCodeSequenceCommand.cs
│       │   └── CreatePropertyCodeSequenceHandler.cs
│       ├── Update/v1/
│       │   ├── UpdatePropertyCodeSequenceCommand.cs
│       │   └── UpdatePropertyCodeSequenceHandler.cs
│       └── Delete/v1/
│           ├── DeletePropertyCodeSequenceCommand.cs
│           └── DeletePropertyCodeSequenceHandler.cs
└── Inventories.Infrastructure/
    └── Endpoints/v1/
        └── PropertyCodeSequence/
            └── PropertyCodeSequenceEndpoints.cs
```

### Technology Stack
- **Pattern:** MediatR CQRS
- **Validation:** Entity Factory + Fluent Validation-ready
- **Authorization:** Permission-based (Permissions.PhysicalAssets.*)
- **Versioning:** API v1.0
- **Database:** EF Core with Specifications pattern
- **Pagination:** Skip/Take with total count

### Key Design Decisions

1. **Search Flexibility**
   - Search works on both Classification and Category
   - Case-sensitive matching (EF Core default)
   - Can be extended with search filters

2. **Pagination**
   - Page number and size parameters
   - Returns total count for UI pagination
   - Default: Page 1, Size 10 items

3. **Manual Sequence Adjustment**
   - Update endpoint allows resetting LastSequenceValue
   - Useful for administrative corrections
   - Validates non-negative values

4. **Unique Constraints**
   - Classification + Category combination must be unique
   - Database enforces uniqueness
   - Application validates before insert

5. **Error Handling**
   - 404 for not found scenarios
   - 400 for validation errors
   - Descriptive error messages

---

## Integration Points

### InventoriesModule Registration
```csharp
var propertyCodeSequenceGroup = app.MapGroup("property-code-sequences")
    .WithTags("property-code-sequences");
propertyCodeSequenceGroup.MapPropertyCodeSequenceListEndpoint();
propertyCodeSequenceGroup.MapPropertyCodeSequenceGetEndpoint();
propertyCodeSequenceGroup.MapPropertyCodeSequenceCreateEndpoint();
propertyCodeSequenceGroup.MapPropertyCodeSequenceUpdateEndpoint();
propertyCodeSequenceGroup.MapPropertyCodeSequenceDeleteEndpoint();
```

### Repository Integration
- Uses keyed services: `"inventories:propertyCodeSequences"`
- Both `IRepository<PropertyCodeSequence>` (CRUD)
- And `IReadRepository<PropertyCodeSequence>` (Read-only)

### Database Integration
- Works with existing `PropertyCodeSequences` table
- Unique index on (Classification, Category)
- Audit fields from `AuditableEntity`

---

## Testing Endpoints

### List with Pagination
```bash
GET /inventories/property-code-sequences?pageNumber=1&pageSize=10
GET /inventories/property-code-sequences?pageNumber=1&pageSize=10&searchTerm=PPE
```

### Get Single
```bash
GET /inventories/property-code-sequences/1
```

### Create
```bash
POST /inventories/property-code-sequences
Content-Type: application/json

{
  "classification": "PPE",
  "category": "001"
}
```

### Update
```bash
PUT /inventories/property-code-sequences/1
Content-Type: application/json

{
  "classification": "PPE",
  "category": "001",
  "lastSequenceValue": 50
}
```

### Delete
```bash
DELETE /inventories/property-code-sequences/1
```

---

## Build Status
✅ **Build successful with no errors** (262 warnings - mostly style/best practice)

---

## Next Steps (Optional)

1. **Add Blazor UI** - Create management pages for sequences
2. **Generate API Client** - Run NSwag to update client
3. **Add Advanced Features:**
   - Bulk delete sequences
   - Reset all sequences by date
   - Sequence audit history
   - Export/Import sequences

---

## Summary

✅ **PropertyCodeSequence now has complete CRUD capabilities:**
- Full Create, Read, Update, Delete operations
- Paginated list with search
- Permission-based authorization
- Proper error handling
- Integration with existing infrastructure

The entity is now **100% ready for UI integration** or direct API consumption.
