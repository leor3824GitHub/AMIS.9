# PhysicalAsset Domain Entity - Connected Entities Analysis

## Executive Summary
The `PhysicalAsset` entity is a core aggregate root in the Inventories module that manages asset lifecycle from acquisition through disposition. It serves as the primary intersection point between procurement, asset management, and inventory tracking workflows.

---

## PhysicalAsset Core Properties

### Identity & Core Information
- **PropertyCode** (string) - Unique property identifier
- **ProductId** (Guid) - Foreign key to Product
- **Description** (string) - Asset description
- **Id** (Guid) - Primary key (inherited from AuditableEntity)

### Acquisition Details
- **AcquisitionCost** (decimal) - Purchase price
- **AcquisitionDate** (DateTime) - Purchase date
- **EstimatedUsefulLife** (int) - In months

### Physical Characteristics
- **SerialNumber** (string, nullable)
- **ModelNumber** (string, nullable)
- **Location** (string, nullable)
- **Condition** (string) - Asset condition status
- **UnitOfMeasure** (string)

### Classification & Lifecycle
- **CurrentClassification** (PropertyClassification) - Consumable, SemiExpendable, or PropertyPlantEquipment
- **Quantity** (int) - For semi-expendable batch tracking
- **PPEType** (string, nullable) - Type of PPE (Machinery, ICT, Transportation, etc.)
- **DisposalDate** (DateTime, nullable)
- **DisposalReason** (string, nullable)

### Financial
- **AccumulatedDepreciation** (decimal) - For PPE only
- **BookValue** (computed) - AcquisitionCost - AccumulatedDepreciation
- **RCAAccountCode** (computed) - Chart of Accounts mapping

### Tracking & Identification
- **QRCodeData** (string, nullable) - Base64 encoded QR code
- **PropertyNumber** (string, nullable) - Auto-generated identifier
- **QRGeneratedDate** (DateTime, nullable)
- **CurrentCustodianId** (Guid, nullable) - Assigned employee

### Computed Properties
- **IsDisposed** - Whether asset has been disposed
- **IsDepreciable** - Whether asset is PPE classification
- **CurrentAssignment** - Active AssetAssignmentHistory
- **LastReclassification** - Most recent AssetReclassificationHistory
- **HasQRCode** - Whether QR code has been generated

---

## Connected Entities

### 1. **Product** (One-to-One Navigation)
**Location**: `api/modules/Inventories/Inventories.Domain/Product.cs`

**Relationship**: `PhysicalAsset.Product` (Many PhysicalAssets → One Product)

**Purpose**: Defines the product master data that the physical asset represents

**Key Properties**:
- `Id` (Guid) - Primary key
- `Name` (string)
- `Description` (string)
- `Sku` (decimal)
- `Unit` (string)
- `ImagePath` (string, nullable)
- `CategoryId` (Guid, nullable)
- `PropertyClassification` (PropertyClassification)
- `EstimatedUsefulLife` (int) - Default useful life in months
- `Category` (virtual navigation)

**Connection Point**: `PhysicalAsset.ProductId → Product.Id`

**Usage**:
```csharp
public virtual Product Product { get; private set; } = default!;
```

---

### 2. **AssetAssignmentHistory** (One-to-Many Navigation)
**Location**: `api/modules/Inventories/Inventories.Domain/AssetAssignmentHistory.cs`

**Relationship**: One PhysicalAsset → Many AssetAssignmentHistory (ICS/PAR documents)

**Purpose**: Tracks complete custody chain - who has been assigned the asset and when

**Key Properties**:
- `Id` (Guid)
- `AssetId` (Guid) - Foreign key to PhysicalAsset
- `AssetNumber` (string) - PropertyCode denormalized
- `EmployeeId` (Guid) - Who asset was issued to
- `EmployeeName` (string) - Denormalized for reporting
- `DocumentNumber` (string) - ICS/PAR number
- `DocumentType` (DocumentType) - ICS or PAR enum
- `AssignmentDate` (DateTime)
- `ReturnDate` (DateTime, nullable)
- `AssignmentType` (string) - Initial, Transfer, Return
- `Quantity` (int) - For semi-expendable tracking
- `AssetClassification` (PropertyClassification)
- `Status` (string) - Active, Returned, Transferred
- `Condition` (string, nullable) - At return
- `AcceptedBy` (Guid, nullable) - Who accepted return
- `AcceptanceDate` (DateTime, nullable)
- `TransferredToEmployeeId` (Guid, nullable)
- `TransferredToDocumentNumber` (string, nullable)

**Navigation Properties**:
- `Asset` (PhysicalAsset) - Back reference
- `Employee` (Employee) - Assigned to
- `TransferredToEmployee` (Employee) - Transferred to
- `AcceptedByEmployee` (Employee) - Accepted return

**Domain Methods**:
- `CreateInitialAssignment()` - ICS/PAR issuance
- `MarkAsTransferred()` - Asset transfer to another employee
- `MarkAsReturned()` - Asset return

---

### 3. **AssetReclassificationHistory** (One-to-Many Navigation)
**Location**: `api/modules/Inventories/Inventories.Domain/AssetReclassificationHistory.cs`

**Relationship**: One PhysicalAsset → Many AssetReclassificationHistory

**Purpose**: Audit trail when asset is reclassified due to COA/DBM threshold changes

**Key Properties**:
- `Id` (Guid)
- `AssetId` (Guid) - Foreign key to PhysicalAsset
- `AssetNumber` (string) - PropertyCode denormalized
- `OldClassification` (PropertyClassification) - From classification
- `NewClassification` (PropertyClassification) - To classification
- `EffectiveDate` (DateTime)
- `Reason` (string) - Reclassification reason
- `AcquisitionCostAtReclassification` (decimal)
- `COAReference` (string, nullable) - e.g., "COA Circular 2025-001"
- `Remarks` (string, nullable)

**Navigation Properties**:
- `Asset` (PhysicalAsset) - Back reference

**Domain Methods**:
- `Create()` - Factory method
- `AddRemarks()` - Add reclassification remarks
- `GetReclassificationType()` - Upgrade/Downgrade/Lateral

**Use Cases**:
- Asset originally purchased at ₱8,000 (SemiExpendable) but threshold increases to ₱15,000 → upgrade to PPE
- Asset classified as PPE at ₱25,000 but threshold decreases to ₱10,000 → downgrade to SemiExpendable

---

### 4. **Employee** (Many-to-One Navigation - Multiple References)
**Location**: `api/modules/Inventories/Inventories.Domain/Employee.cs`

**Relationship**: Multiple connections via AssetAssignmentHistory

**Key Properties**:
- `Id` (Guid)
- `Name` (string)
- `Designation` (string)
- `ResponsibilityCode` (string)
- `UserId` (Guid, nullable)

**Connection Points in AssetAssignmentHistory**:
1. **EmployeeId**: The employee currently assigned the asset
2. **TransferredToEmployeeId**: Employee transferred to
3. **AcceptedBy**: Employee who accepted return

**Deletion Behavior**: `DeleteBehavior.Restrict` - Prevents employee deletion if assigned assets exist

---

### 5. **InventoryRegistry** (Related via Events)
**Location**: `api/modules/Inventories/Inventories.Domain/InventoryRegistry.cs`

**Relationship**: Projection/Event-driven synchronization (NOT direct foreign key)

**Purpose**: Real-time inventory availability tracking by PropertyCode

**Key Properties**:
- `Id` (Guid)
- `PropertyCode` (string) - Links to PhysicalAsset.PropertyCode
- `Description` (string)
- `Quantity` (int)
- `Location` (string)
- `Status` (InventoryItemStatus)
- `ReceivedDate` (DateTime)
- `IssuedDate` (DateTime, nullable)
- `LastTransactionDate` (DateTime)
- `LastTransactionType` (string)
- `LastTransactionReference` (string)

**Event Handlers** (`PhysicalAssetInventoryProjectionHandler`):
- `PhysicalAssetCreated` → Creates new InventoryRegistry entry
- `PhysicalAssetIssued` → Deducts quantity from registry
- `PhysicalAssetReturned` → Restores quantity to registry

**Connection Pattern**:
```csharp
// When physical asset is created
PhysicalAsset → PhysicalAssetCreated event 
  → PhysicalAssetInventoryProjectionHandler 
  → Creates/Updates InventoryRegistry by PropertyCode
```

---

## Domain Events Emitted by PhysicalAsset

### 1. **PhysicalAssetCreated**
- When asset is created via `Create()` factory method
- Triggers InventoryRegistry projection

### 2. **PhysicalAssetIssued**
- When asset is issued via `Issue()` method
- Contains: `EmployeeId`, `DocumentNumber`, `DocumentType`, `Quantity`
- Triggers InventoryRegistry deduction

### 3. **PhysicalAssetReturned**
- When asset is returned via `Return()` method
- Contains: `Reason`, `Condition`, `QuantityReturned`
- Triggers InventoryRegistry restoration

### 4. **PhysicalAssetReclassified**
- When asset classification changes via `Reclassify()` method
- Contains: `OldClassification`, `NewClassification`, `Reason`, `EffectiveDate`
- Updates RCA account code mapping

### 5. **PhysicalAssetDepreciated**
- When depreciation is recorded via `RecordDepreciation()` method (PPE only)
- Contains: `Amount`, `AccumulatedDepreciation`, `BookValue`

### 6. **PhysicalAssetConditionUpdated**
- When condition is updated via `UpdateCondition()` method
- Contains: `Condition`, `Remarks`

### 7. **PhysicalAssetQRCodeGenerated**
- When QR code is generated via `GenerateQRCode()` method
- Contains: `PropertyNumber`, `GeneratedDate`

### 8. **PhysicalAssetAssignedToCustodian**
- When assigned to custodian via `AssignToCustodian()` method
- Contains: `CustodianId`, `AssignmentDate`

### 9. **PhysicalAssetCustodianCleared**
- When custodian is cleared via `ClearCustodian()` method
- Contains: `ClearedDate`

---

## Data Flow & Relationships Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    PhysicalAsset                            │
│                  (Aggregate Root)                           │
├─────────────────────────────────────────────────────────────┤
│ Core: PropertyCode, ProductId, Description                 │
│ Acquisition: Cost, Date, UsefulLife                        │
│ Physical: Serial, Model, Location, Condition               │
│ Classification: Type (Consumable/Semi/PPE)                 │
│ Financial: Depreciation, BookValue, RCACode                │
│ Tracking: QRCode, PropertyNumber, CurrentCustodian         │
└─────────────────────────────────────────────────────────────┘
        ↓                  ↓                  ↓
        │                  │                  │
    1:1 │              1:* │              1:* │
        │                  │                  │
    NAVIGATION         NAVIGATION        NAVIGATION
    Product      AssetAssignmentHistory  AssetReclassification
    ├─ Name          ├─ EmployeeId        ├─ OldClass
    ├─ Category      ├─ Document#         ├─ NewClass
    ├─ SKU           ├─ AssignDate        ├─ EffectiveDate
    └─ Usefullife    ├─ ReturnDate        ├─ Reason
                     ├─ Condition         └─ COARef
                     └─ Status
                            ↓
                        FOREIGN KEY
                        Employee
                        ├─ Name
                        ├─ Designation
                        └─ UserId

EVENT-DRIVEN PROJECTION:
┌─────────────────────────────┐
│  Physical Asset Events      │
├─────────────────────────────┤
│ • Created                   │
│ • Issued                    │
│ • Returned                  │
│ • Reclassified             │
│ • Deprecated               │
│ • ConditionUpdated         │
│ • QRCodeGenerated          │
│ • CustodianAssigned        │
│ • CustodianCleared         │
└─────────────────────────────┘
        ↓
PhysicalAssetInventory
ProjectionHandler
        ↓
PROJECT TO InventoryRegistry
├─ PropertyCode (Match)
├─ Quantity (Deduct/Add)
├─ Status (Update)
└─ LastTransaction (Track)
```

---

## Key Relationships Summary Table

| Entity | Relationship | Cardinality | Type | Delete Behavior | Purpose |
|--------|-------------|-------------|------|-----------------|---------|
| Product | Navigation | 1:1 | Direct FK | NO ACTION | Product master definition |
| AssetAssignmentHistory | Collection | 1:* | Direct FK | CASCADE | Custody chain tracking |
| AssetReclassificationHistory | Collection | 1:* | Direct FK | CASCADE | Reclassification audit trail |
| Employee | Via AssetAssignment | *:1 | Direct FK | RESTRICT | Assignment accountability |
| InventoryRegistry | Event-driven | N/A | PropertyCode match | N/A | Real-time inventory sync |
| Category | Transitive via Product | 1:1 | Via Product | N/A | Product categorization |

---

## Workflow Interactions

### Asset Lifecycle Workflow
```
1. CREATE PhysicalAsset
   ↓
2. PhysicalAssetCreated event
   ↓
3. InventoryRegistry.Create() projection
   ↓
4. ISSUE to Employee (creates AssetAssignmentHistory)
   ↓
5. PhysicalAssetIssued event
   ↓
6. InventoryRegistry.Deduct() projection
   ↓
7. RECLASSIFY if COA threshold changes
   ↓
8. PhysicalAssetReclassified event
   ↓
9. RETURN from Employee
   ↓
10. PhysicalAssetReturned event
    ↓
11. InventoryRegistry.Restore() projection
    ↓
12. DISPOSE/RECORD DEPRECIATION
    ↓
13. PhysicalAssetDepreciated event (for PPE)
```

### Assignment Workflow (ICS/PAR)
```
Issue Asset → Create AssetAssignmentHistory (Initial)
  ↓
[Employee has asset]
  ↓
Optional: Transfer to another employee → Create AssetAssignmentHistory (Transfer)
  ↓
Return Asset → Update AssetAssignmentHistory (MarkAsReturned)
  ↓
[Condition recorded, accepted by supervisor]
```

---

## Audit Trail & Traceability

PhysicalAsset maintains complete audit trail through:

1. **Direct Entities**:
   - `AssetAssignmentHistory` - Who had it, when, via what document
   - `AssetReclassificationHistory` - Classification changes and reasons

2. **Event Trail**:
   - Domain events provide point-in-time snapshots
   - Events trigger projections for consistency

3. **AuditableEntity Base**:
   - CreatedBy, CreatedDate
   - LastModifiedBy, LastModifiedDate
   - TenantId (multi-tenant support)

---

## Key Business Rules Enforced

1. **Classification Consistency**
   - PPE type required for PPE classification
   - RCA account code automatically determined from classification

2. **Assignment Integrity**
   - Only one active assignment at a time
   - Cannot issue disposed assets
   - Quantity validation for semi-expendable

3. **Depreciation Rules**
   - Only PPE can be depreciated
   - Accumulated depreciation cannot exceed acquisition cost
   - Cannot depreciate disposed assets

4. **Reclassification Validation**
   - Cannot reclassify disposed assets
   - Reason is required
   - Effective date must be set

5. **Custody Tracking**
   - Condition must be valid enumeration
   - Quantity returned for semi-expendable must be tracked
   - Custodian assignment tracked separately

---

## Implementation Notes

### Repository Pattern
- `IRepository<PhysicalAsset>` - Keyed service: `"inventories:physicalassets"`
- `IReadRepository<PhysicalAsset>` - For queries
- Specifications pattern for filtering/searching

### Database Configuration
- **Migrations**: `api/migrations/PostgreSQL|MSSQL/Inventories/`
- **Ownership**: Collections owned by PhysicalAsset aggregate
- **Indexing**: PropertyCode indexed (unique)

### API Endpoints
- `POST /physical-assets` - Create
- `GET /physical-assets/{id}` - Retrieve
- `PUT /physical-assets/{id}` - Update
- `PUT /physical-assets/{id}/issue` - Issue asset
- `PUT /physical-assets/{id}/return` - Return asset
- `DELETE /physical-assets/{id}` - Delete

---

## Related Documentation
- See: `docs/aggregate-refinements-summary.md` - Complete aggregate patterns
- See: `docs/BLAZOR_UI_IMPLEMENTATION.md` - UI for asset management
- See: `api/modules/Inventories/Inventories.Domain/ValueObjects/` - PropertyClassification enum
