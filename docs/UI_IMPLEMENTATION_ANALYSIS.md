# UI Implementation Analysis: Asset Management System Reports

## Executive Summary

Based on the `copilot_instruction_for UI_design.md` requirements, this document outlines the UI implementation strategy for the AMIS system, including necessary domain entity modifications and architectural changes.

---

## Current State Analysis

### Existing Domain Entities

#### 1. **Acceptance** (Catalog.Domain)
- **Current State**: Handles IAR (Inspection & Acceptance Report)
- **Properties**: AcceptanceDate, SupplyOfficerId, Status, Items
- **Relationships**: Purchase, Inspection, GoodsReceipt, AcceptanceItems
- **Status**: Has basic status tracking but lacks workflow states

#### 2. **PhysicalAsset** (Catalog.Domain)
- **Current State**: Unified entity for PPE and Semi-Expendable items
- **Properties**: PropertyCode, AcquisitionCost, CurrentClassification, AssignmentHistory
- **Key Logic**: Automatic classification based on 50k PHP threshold
- **Relationships**: Product, AssignmentHistory, ReclassificationHistory
- **Missing**: QR Code data, Digital signature tracking

#### 3. **Issuance** (Catalog.Domain)
- **Current State**: Tracks asset issuance to employees
- **Properties**: EmployeeId, IssuanceDate, Items
- **Status**: Basic IsClosed boolean
- **Missing**: Custodian acceptance workflow, PAR/ICS document generation

#### 4. **Inventory** (Catalog.Domain)
- **Current State**: Tracks available stock
- **Missing**: Direct connection to issuance workflow

---

## UI Navigation Structure (From Requirements)

### Role-Based Dashboards
1. **Supply Officer Dashboard** - Acquisition, Issuance, Disposal management
2. **Accountant Dashboard** - Depreciation calculations, JEV exports
3. **End User Dashboard** - View pending issuances, accept/reject assets

### Key Workflows

#### 1. **Acquisition/Receiving Module**
- IAR Entry Form
- Unit Cost Input
- Auto-classification logic (≥50k PHP → PPE, <50k → Semi-Expendable)
- QR Code generation
- Property sticker printing

#### 2. **Issuance Module**
- Asset selection from inventory
- Custodian/Employee selection
- Document generation (PAR/ICS based on asset type)
- User notification

#### 3. **User Acceptance Module**
- View pending issuances
- Digital signature capability
- Accept/Reject with reasons
- Database update for custodian assignment

#### 4. **Depreciation Module**
- Monthly schedule view
- JEV calculation (Straight Line Method)
- Export to Excel/PDF

---

## Required Domain Entity Modifications

### 1. **PhysicalAsset** - Add QR & Digital Signature Support
```csharp
// Add properties
public string? QRCodeData { get; private set; }
public string? PropertyNumber { get; private set; }
public DateTime? QRGeneratedDate { get; private set; }
public Guid? CurrentCustodianId { get; private set; }

// Add methods
public void GenerateQRCode() { /* Generate QR */ }
public void AssignToCustodian(Guid employeeId) { /* Track assignment */ }
```

### 2. **Issuance** - Add Acceptance Workflow
```csharp
// Add properties
public IssuanceType Type { get; private set; } // PAR or ICS
public Guid? CustodianId { get; private set; }
public IssuanceStatus Status { get; private set; } // Pending, Accepted, Rejected
public DateTime? AcceptedOn { get; private set; }
public string? RejectionReason { get; private set; }
public string? DigitalSignature { get; private set; }

// Add methods
public void Accept(string digitalSignature) { /* Mark as accepted */ }
public void Reject(string reason) { /* Mark as rejected */ }
public bool CanAcceptAsset() { /* Validation */ }
```

### 3. **New: DigitalSignature** Value Object
```csharp
public class DigitalSignature : ValueObject
{
    public string SignatureData { get; private set; }
    public DateTime SignedOn { get; private set; }
    public Guid SignedByEmployeeId { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
}
```

### 4. **New: AssetRequisition** Entity (For Issuance Request Tracking)
```csharp
public class AssetRequisition : AuditableEntity, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public Guid IssuanceId { get; private set; }
    public RequisitionStatus Status { get; private set; } // Pending, Accepted, Rejected
    public DateTime RequisitionDate { get; private set; }
    public DateTime? ResponseDate { get; private set; }
    public string? RejectionReason { get; private set; }
    public DigitalSignature? AcceptanceSignature { get; private set; }
}
```

### 5. **New: DepreciationSchedule** Entity
```csharp
public class DepreciationSchedule : AuditableEntity, IAggregateRoot
{
    public Guid PhysicalAssetId { get; private set; }
    public int Month { get; private set; } // 1-12
    public int Year { get; private set; }
    public decimal DepreciationAmount { get; private set; }
    public decimal AccumulatedAmount { get; private set; }
    public bool IsPosted { get; private set; }
    public string? JournalVoucherNumber { get; private set; }
}
```

### 6. **New: JournalEntryVoucher** Entity
```csharp
public class JournalEntryVoucher : AuditableEntity, IAggregateRoot
{
    public string VoucherNumber { get; private set; } // Auto-generated
    public DateTime VoucherDate { get; private set; }
    public ICollection<JournalEntry> Entries { get; private set; }
    public JevStatus Status { get; private set; } // Draft, Posted, Exported
    public DateTime? ExportedOn { get; private set; }
    public string? ExportFormat { get; private set; } // Excel, PDF
}
```

### 7. **Acceptance** - Enhanced Status Tracking
```csharp
// Add enum for detailed workflow
public enum AcceptanceStatus
{
    Pending,      // Items received, awaiting inspection
    Inspected,    // Inspector completed verification
    Accepted,     // All items accepted
    PartiallyAccepted,
    Rejected,
    Posted        // Recorded in asset register
}
```

---

## Blazor UI Components Required

### 1. **IAR Entry Page** (`/catalog/acceptances/create`)
- Form inputs: PurchaseId, SupplyOfficerId, AcceptanceDate, Remarks
- Item-level inputs: Product, Quantity, Unit Cost
- Auto-calculation: PropertyClassification based on cost
- Display: Auto-generated PropertyCode preview

### 2. **Asset List Page with QR** (`/catalog/assets`)
- MudDataGrid with asset listing
- QR Code column (preview/print option)
- Property Number column
- Condition status column
- Current Custodian column
- Actions: View, Edit, Print Sticker

### 3. **Issuance Page** (`/catalog/issuances/create`)
- Step 1: Select Asset from Inventory
- Step 2: Select Custodian/Employee
- Step 3: Document Preview (PAR/ICS based on type)
- Step 4: Confirm Issuance

### 4. **Pending Issuances for End User** (`/my-accountability/pending`)
- List of pending issuances
- Document viewer (PAR/ICS)
- Acceptance controls:
  - Digital Signature pad
  - Reason input (if rejecting)
  - Accept/Reject buttons

### 5. **Depreciation Schedule** (`/accounting/depreciation`)
- Monthly schedule view
- Filter by month/year
- Depreciation amount column
- Accumulated column
- JEV Preview column
- Export buttons (Excel/PDF)

### 6. **JEV Export** (`/accounting/jev-export`)
- Date range picker
- JEV list with calculation method (Straight Line)
- Export format selector
- Download buttons

### 7. **Asset Print Sticker Dialog**
- QR Code preview
- Property Code preview
- Asset details
- Print layout options

---

## Database Schema Changes

### New Tables Required
1. `DigitalSignatures` - Store signature data
2. `AssetRequisitions` - Track end-user issuance requests
3. `DepreciationSchedules` - Monthly depreciation tracking
4. `JournalEntryVouchers` - JEV headers
5. `JournalEntries` - JEV line items

### Modified Tables
1. `PhysicalAssets` - Add QRCodeData, PropertyNumber, CurrentCustodianId
2. `Issuances` - Add Type, Status, CustodianId, AcceptedOn, RejectionReason, DigitalSignature
3. `Acceptances` - Expand Status enum values

---

## API Endpoints Required

### Acceptance/IAR
- `POST /api/acceptances` - Create IAR
- `GET /api/acceptances` - List with pagination
- `GET /api/acceptances/{id}` - Detail view
- `PUT /api/acceptances/{id}` - Update IAR
- `POST /api/acceptances/{id}/post` - Post to asset register

### Physical Assets
- `POST /api/assets/{id}/generate-qr` - Generate QR code
- `GET /api/assets/{id}/qr` - Retrieve QR data
- `POST /api/assets/{id}/print-sticker` - Trigger print dialog data

### Issuances
- `POST /api/issuances` - Create issuance
- `GET /api/issuances/pending` - List pending for current user
- `POST /api/issuances/{id}/accept` - Accept with signature
- `POST /api/issuances/{id}/reject` - Reject with reason
- `GET /api/issuances/{id}/document` - Get PAR/ICS document

### Depreciation
- `GET /api/depreciation/schedule` - List monthly schedules
- `POST /api/depreciation/calculate` - Calculate depreciation
- `GET /api/depreciation/jev` - List JEVs
- `POST /api/depreciation/jev/export` - Export to Excel/PDF

---

## Implementation Sequence

### Phase 1: Domain Entities & Database (Weeks 1-2)
1. Create new domain entities (AssetRequisition, DepreciationSchedule, JEV)
2. Add value objects (DigitalSignature)
3. Enhance existing entities (PhysicalAsset, Issuance)
4. Create EF Core migrations
5. Update repository interfaces

### Phase 2: Application Layer (Weeks 2-3)
1. Create command handlers for new operations
2. Create query handlers for reporting
3. Implement validators
4. Add business logic services

### Phase 3: API Endpoints (Week 3)
1. Create Carter endpoints
2. Add authorization/permissions
3. Implement document generation (PAR/ICS)
4. Add QR code generation

### Phase 4: Blazor UI (Weeks 4-6)
1. Create pages and components
2. Integrate with API client
3. Add digital signature component
4. Add print functionality
5. Implement document viewers

### Phase 5: Testing & Polish (Week 6)
1. Unit tests for domain logic
2. Integration tests for workflows
3. UI testing
4. Performance optimization

---

## Key Technical Considerations

### 1. **QR Code Generation**
- Use QRCoder or similar library
- Store both raw data and generated image
- Support both static and dynamic content

### 2. **Digital Signatures**
- Consider Web Cryptography API for client-side signing
- Store signature metadata (IP, user agent, timestamp)
- Audit trail for compliance

### 3. **Document Generation**
- Use iText or similar for PDF generation
- PAR template: Property Acknowledgment Receipt
- ICS template: Internal Control Slip
- Support Excel export via OfficeOpenXml

### 4. **Depreciation Calculations**
- Straight Line Method: (Cost - Salvage) / Useful Life
- Store calculated values (not just formulas)
- Support retroactive adjustments

### 5. **Multi-Tenancy**
- All new entities must support TenantId
- Ensure query filters in repositories

### 6. **Permissions**
- Add to FshPermissions.cs:
  - `Permissions.Acceptances.PostToAssetRegister`
  - `Permissions.Assets.GenerateQR`
  - `Permissions.Issuances.AcceptAsset`
  - `Permissions.Depreciation.Calculate`
  - `Permissions.Jev.Export`

---

## UI Mockup Structure Example

```
/catalog
  /acceptances
    - List (existing)
    - Create (enhanced with IAR form)
    - Detail (with posting capability)
    
/catalog
  /assets
    - List (add QR column, custodian column)
    - Detail (enhanced)
    - PrintSticker (new dialog)

/catalog
  /issuances
    - Create (multi-step wizard)
    - List (existing)

/my-accountability
  /pending-issuances (NEW)
    - List of pending items
    - Accept/Reject actions
    - Digital signature pad

/accounting
  /depreciation (NEW)
    - Monthly schedule view
    - Filter controls
    - JEV preview

/accounting
  /jev-export (NEW)
    - Date range selection
    - JEV list
    - Export controls
```

---

## Success Metrics

- ✓ All UI workflows match flowchart requirements
- ✓ Domain entities support compliance requirements
- ✓ Digital signatures captured and auditable
- ✓ Auto-classification logic working correctly
- ✓ QR codes generate and print correctly
- ✓ Depreciation calculations accurate
- ✓ JEV exports in correct format
- ✓ Multi-tenant isolation maintained
- ✓ All permissions properly configured

---

## Notes for Development Team

1. **Entity Framework Migrations**: Each domain change requires a migration in both PostgreSQL and MSSQL migration projects
2. **API Versioning**: New endpoints should use ApiVersion(2, 0) if breaking changes, else (1, 1)
3. **Authorization**: Coordinate with FshPermissions for new permission definitions
4. **Component Reusability**: Consider extracting digital signature and document viewers into shared components
5. **Testing**: Implement comprehensive unit tests for depreciation calculations and classification logic

