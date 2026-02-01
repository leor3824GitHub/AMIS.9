# NFA Standards Alignment Analysis - AMIS.9 Domain Structure

## Executive Summary

## Accounting Treatment — Semi-Expendable (NFA Guidance)

Per NFA practice, Semi-Expendable items (AcquisitionCost < 50,000 PHP) are generally treated as expenses rather than capitalized fixed assets. The system should therefore record these items as expense or consumable inventory in the GL while still maintaining a robust logbook/registry for monitoring, custody, and internal control.

Key points to include in policy and implementation:
- **Expense Recognition**: Record acquisition of semi-expendable items to `SuppliesAndMaterialsExpense` or `Consumables Inventory` depending on whether items are stocked or immediately consumed.
- **Logbook / Registry**: Continue to maintain `InventoryRegistry` or `SemexRegistry` entries for tracking property custody, quantities, and location. The registry is for monitoring and control only — it does not convert the item to a fixed asset for accounting purposes.
- **Control Numbers & Forms**: SMRR/SMIR/ICS control numbers must be retained and linked to the registry entries for audit trail. Include `ControlNumber`, `ProcessedByUserId`, and `ApprovedByUserId` on `NfaFormHeader`.
- **VAT/TAX**: Clearly document whether `AcquisitionCost` stored is VAT-inclusive. Track recoverable VAT in a separate VAT receivable ledger when VAT is applicable.
- **Bundled Purchases**: If multiple semi-expendable items are purchased together and the combined cost meets the PPE threshold, treat the combined purchase per capitalization rules (policy to define bundling rules).
- **Repairs vs Capital Improvements**: Repair costs for semi-expendable items are typically expensed. If a repair materially improves an item beyond its original capacity, escalate for capitalization review.

Sample journal entries (Semi-Expendable):

- Receiving semi-expendable (stocked):

```
Dr Consumables Inventory / Supplies and Materials Inventory
Dr Recoverable VAT (if VAT applicable)
    Cr Accounts Payable / Cash
```

- Receiving semi-expendable (immediately expensed):

```
Dr Supplies and Materials Expense
Dr Recoverable VAT (if VAT applicable)
    Cr Accounts Payable / Cash
```

- Issue to employee (ICS) — no GL if purely custodial transfer; if issuance triggers expense recognition when previously capitalized as inventory, then:

```
Dr Supplies Expense
    Cr Consumables Inventory
```

Controls and reconciliations:
- Reconcile `InventoryRegistry` / `SemexRegistry` to the GL at period end; record adjustments with supporting journal entries and `NfaFormHeader` references.
- Maintain signed scans of SMRR/SMIR/ICS forms linked to the `NfaFormHeader` record (persist `DocumentUrl` or `AttachmentId`).
- Approval thresholds: require finance approval for aggregations or exceptions above policy thresholds.

Linking to domain entities:
- `SemexRegistry` / `ConsumableInventory` entries should carry `ControlNumber`, `ReceivingDocumentId` (link to `NfaFormHeader`), and optional `JournalEntryId` for GL references.
- `NfaFormHeader` should include an optional `JournalEntryId` to trace accounting posts back to the document.

---

## Next steps (accounting changes)
- Add `JournalEntryId` nullable FK to `NfaFormHeader` and to `SemexTransactionLog`/`ConsumableInventory` to trace accounting postings.
- Add `IsVatInclusive` flag or document policy text to clarify VAT treatment.
- Update UI forms (SMRR/SMIR/ICS) to capture whether items are stocked or immediately expensed.

## Supply Officer Custody & Retirement of Semi-Expendable

Policy: Semi-Expendable items remain under the custody and control of the Supply Officer until formally retired or disposed. Although accounted as expense (or inventory consumed) for financial reporting, the asset lifecycle must be tracked in the registry until retirement so that custody, physical verification, and disposal are auditable.

System requirements and fields:
- `DateRetired` (DateTime?) — when item is formally retired.
- `RetirementFormId` (FK to `NfaFormHeader`) — typically an `RRP` control number.
- `RetiredById` / `RetirementApprovedById` — employee IDs for custody release and accounting approval.
- `RetirementReason` (string) — cause (worn out, cannibalised, lost, sold).
- `RetirementJournalEntryId` (nullable) — link to any GL posting created for disposal/write-off.
- Maintain `IsRetired` flag on `SemexRegistry`/`ConsumableInventory` and retain historical entries (do not purge on retirement).

Accounting treatment at retirement:
- If item was previously expensed (no inventory GL balance):
  - No reversal required; record retirement metadata and retain the signed `RRP` as evidence.
  - Optional administrative posting: none (audit trail via registry + RRP).

- If item remained on inventory (stocked) and is retired/unusable:
```
Dr Loss on Retirement / Disposal
    Cr Consumables Inventory
```

- If item is disposed/sold with proceeds:
```
Dr Cash / Proceeds
Dr Loss on Retirement (if any)
    Cr Consumables Inventory
    Cr Gain on Disposal (if proceeds > carrying amount)
```

Controls & process notes:
- Supply Officer must not mark an item retired without an approved `RRP` and `RetirementApprovedById` (finance authorization where required).
- Keep scanned RRP and supporting documents attached to `NfaFormHeader` and retain per statutory retention policy.
- Include `RetirementJournalEntryId` in the registry to link GL adjustments to the retirement event during reconciliation.

Domain linkage suggestions:
- Add `DateRetired`, `IsRetired`, `RetirementFormId`, `RetirementJournalEntryId` to `SemexRegistry` and `ConsumableInventory`.
- Add `RetirementReason` and `RetiredById` to `SemexTransactionLog` or `SemexRegistry`.




Your AMIS.9 system has a **solid foundation** for NFA compliance but requires specific enhancements to fully distinguish between **PPE** (≥50k PHP) and **Semi-Expendable** (<50k PHP) asset types while maintaining vehicle-specific data tracking. This document maps your current domain structure against NFA requirements and identifies critical gaps.

---

## 1. Current Domain Structure Analysis

### ✅ What You Have (Strong Foundation)

| NFA Requirement | Current Implementation | Status |
|---|---|---|
| **Core Asset Record** | `PhysicalAsset` aggregate | ✅ Exists |
| **Asset Classification** | `PropertyClassification` enum (Consumable, SemiExpendable, PPE) | ✅ Exists |
| **Property Identifier** | `PropertyCode` + `PropertyNumber` | ✅ Exists |
| **Acquisition Tracking** | AcquisitionCost, AcquisitionDate, SupplierID (via Product) | ✅ Exists |
| **Purchase Documentation** | PR_Number, PO_Number, SI_Number fields (need verification) | ⚠️ Partial |
| **Asset Assignment** | `AssetAssignmentHistory` (tracks ICS/PAR) | ✅ Exists |
| **Inventory Sync** | `InventoryRegistry` (event-driven projection) | ✅ Exists |
| **Depreciation** | `DepreciationSchedule` + RecordDepreciation() | ✅ Exists (PPE) |
| **Maintenance Log** | No specific entity found | ❌ Missing |
| **Vehicle Details** | No specialized vehicle entity | ❌ Missing |
| **Form Type Routing** | Not based on cost threshold | ❌ Missing |

---

## 2. Critical Gaps & NFA Alignment Issues

### Gap 1: **Missing `NFA_Form_Headers` Entity** (HIGH PRIORITY)

**Current Problem:**
- No centralized form tracking table
- Document types (ICS, PAR, SMIR, SMRR) scattered across different entities
- Cannot route form generation based on cost threshold

**NFA Requirement:**
```
FormType Enum:
- PPE (≥50k): PPERR, PPEIR, PAR
- Semi (<50k): SMRR, SMIR, ICS
- Both: RRP
```

**What You Need:**
```csharp
public class NfaFormHeader : AuditableEntity, IAggregateRoot
{
    public Guid Id { get; private set; }
    public NfaFormType FormType { get; private set; }        // SMIR, SMRR, ICS, PAR, PPEIR, PPERR, RRP
    public string ControlNumber { get; private set; }        // Official Form Number
    public DateTime TransactionDate { get; private set; }
    public Guid OfficeId { get; private set; }               // Physical location
    public Guid ProcessedByUserId { get; private set; }      // Supply Officer
    public Guid? ApprovedByUserId { get; private set; }      // Manager/Chief
    public PropertyClassification ApplicableClassification { get; private set; }
    public List<NfaFormLineItem> LineItems { get; private set; }
}

public enum NfaFormType
{
    // PPE (≥50k)
    PPERR,  // PPE Receiving Report
    PPEIR,  // PPE Issuance Report
    PAR,    // Property Acknowledgment Receipt
    
    // Semi-Expendable (<50k)
    SMRR,   // Semi-Expendable Material Receiving Report
    SMIR,   // Semi-Expendable Material Issuance Report
    ICS,    // Inventory Custodian Slip
    
    // Both Classifications
    RRP     // Receipt for Returned Property
}
```

**Current Workaround:** You have `PpeReceivingReport`, `PpeIssuanceReport`, and `SuppliesAndMaterialsReceivingReport` - these should be unified under NfaFormHeader.

---

### Gap 2: **Missing `Vehicle_Details` Entity** (MEDIUM PRIORITY)

**Current Problem:**
- PPEType field exists but is just a string
- No specialized vehicle tracking (plate, engine, chassis)
- Cannot differentiate vehicle-specific workflows

**NFA Requirement:**
Vehicles require **Exhibit 10** (Vehicle History Card) with:
- License Plate Number
- Engine Number
- Chassis Number
- Vehicle Category
- Mileage History

**What You Need:**
```csharp
public class VehicleDetails : Entity
{
    public Guid AssetId { get; private set; }               // FK to PhysicalAsset
    public string PlateNumber { get; private set; }         // License plate
    public string EngineNumber { get; private set; }        // Engine ID
    public string ChassisNumber { get; private set; }       // Chassis ID
    public string VehicleType { get; private set; }         // Truck, Sedan, Motorcycle, etc.
    public int? Mileage { get; private set; }               // Current mileage
    public DateTime? MileageLastRecorded { get; private set; }
    public string? Color { get; private set; }
    public string? TransmissionType { get; private set; }
    public int? ManufacturingYear { get; private set; }
    
    public virtual PhysicalAsset Asset { get; private set; } = default!;
}
```

**Current Status:** 
- You have `PPEType` as string - should be split into `VehicleDetails` for vehicles and retained for other PPE categories.

---

### Gap 3: **Missing `Maintenance_Log` Entity** (MEDIUM PRIORITY)

**Current Problem:**
- No formal maintenance tracking
- Cannot generate PPE Ledger Card with repair history

**NFA Requirement:**
Track repairs for PPE Ledger Card:
- Reference Number (J.O. / RIV)
- Nature of Repair
- Parts Cost / Labor Cost
- Service Date

**What You Need:**
```csharp
public class MaintenanceLog : AuditableEntity, IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid AssetId { get; private set; }               // FK to PhysicalAsset
    public string ReferenceNumber { get; private set; }     // J.O. or RIV number
    public string NatureOfRepair { get; private set; }
    public decimal? PartsCost { get; private set; }
    public decimal? LaborCost { get; private set; }
    public DateTime ServiceDate { get; private set; }
    public Guid PerformedBy { get; private set; }           // Employee/Technician ID
    public string? Notes { get; private set; }
    
    public virtual PhysicalAsset Asset { get; private set; } = default!;
}
```

**Current Status:** ❌ No equivalent entity exists

---

### Gap 4: **Form Routing Logic Not Cost-Threshold Driven** (HIGH PRIORITY)

**Current Problem:**
- System doesn't automatically route to correct NFA form based on AcquisitionCost
- Manual workflow tracking instead of rules-based routing

**NFA Requirements - Form Selection by Cost:**

| Workflow Stage | Cost ≥ 50k | Cost < 50k |
|---|---|---|
| **Receiving** | PPERR | SMRR |
| **Assignment** | PAR | ICS |
| **Transfer/Issuance** | PPEIR | SMIR |
| **Return** | RRP | RRP |

**What You Need:**
```csharp
public class NfaFormRoutingService
{
    public NfaFormType DetermineFormType(decimal acquisitionCost, WorkflowStage stage)
    {
        bool isPpe = acquisitionCost >= 50000m; // NFA Threshold
        
        return (isPpe, stage) switch
        {
            (true, WorkflowStage.Receiving) => NfaFormType.PPERR,
            (true, WorkflowStage.Assignment) => NfaFormType.PAR,
            (true, WorkflowStage.Issuance) => NfaFormType.PPEIR,
            (false, WorkflowStage.Receiving) => NfaFormType.SMRR,
            (false, WorkflowStage.Assignment) => NfaFormType.ICS,
            (false, WorkflowStage.Issuance) => NfaFormType.SMIR,
            (_, WorkflowStage.Return) => NfaFormType.RRP,
        };
    }
}

public enum WorkflowStage
{
    Receiving,    // Incoming purchase
    Assignment,   // Assign to employee
    Issuance,     // Transfer/Issue to another location
    Return        // Return/Unserviceable
}
```

---

## 3. Mapping: Current Entities → NFA Schema

### Physical Asset Management Layer

#### Current: `PhysicalAsset`
**Corresponds to NFA:** `Assets` table (Birth Certificate)

**Missing Fields:**
- `SupplierID` (can be derived from Purchase relationship if available)
- `PR_Number`, `PO_Number`, `SI_Number` (need to check if in Purchase entity)
- `AssetStatus` enum (Serviceable, Unserviceable, In Transit)

**Action Required:**
```csharp
// Extend PhysicalAsset aggregate to include:
public string? PR_Number { get; private set; }
public string? PO_Number { get; private set; }
public string? SI_Number { get; private set; }
public Guid? SupplierId { get; private set; }  // Track supplier
public AssetStatus Status { get; private set; } // Serviceable, Unserviceable, In Transit

public enum AssetStatus
{
    Serviceable,
    Unserviceable,
    InTransit,
    Disposed
}
```

---

#### Current: `AssetAssignmentHistory`
**Corresponds to NFA:** `Accountability_Log`

**Status:** ✅ Good, but needs explicit form linking

**Enhancement Needed:**
```csharp
// Add explicit form document tracking:
public Guid? IssuanceFormId { get; private set; }  // Link to NfaFormHeader (PAR/ICS)
public Guid? ReturnFormId { get; private set; }    // Link to NfaFormHeader (RRP)
public DateTime? DateCleared { get; private set; } // When RRP approved
```

---

#### Current: `InventoryRegistry`
**Corresponds to NFA:** Inventory projection layer (real-time availability)

**Status:** ✅ Exists and working well

---

#### Current: `ConsumableInventory` & `SemexRegistry`
**Corresponds to NFA:** Semi-Expendable tracking

**Status:** ⚠️ Needs integration with NFA form headers

---

### Document Control Layer

#### Current: Fragmented across entities
- `PpeReceivingReport`
- `PpeIssuanceReport`
- `SuppliesAndMaterialsReceivingReport`
- `SuppliesAndMaterialsIssuanceReport`
- `PropertyAcknowledgementReceipt` (likely PAR)

**What Should Exist:**
Unified `NfaFormHeader` + specialized line items for each form type

**Mapping:**
```
NfaFormHeader (unified form control)
├── NfaFormLineItem (generic line items)
└── Form-specific data via discriminator or related tables
```

---

#### Current: Missing
- **Receiving Document** (PPERR/SMRR) - formal goods receipt + inspection
- **Assignment Document** (PAR/ICS) - employee acknowledgment
- **Issuance Document** (PPEIR/SMIR) - outgoing document
- **Return Document** (RRP) - unserviceable/return receipt

---

### Specialized Tracking Layer

#### Current: Missing
- **Vehicle History Card** (Exhibit 10) - currently no `VehicleDetails` entity
- **Maintenance History** - currently no `MaintenanceLog` entity
- **Depreciation Schedule** - exists for PPE but not linked to form workflow

---

## 4. Entity Relationship Diagram - Target NFA Structure

```
PhysicalAsset (Core Asset Record)
├── ProductId → Product
├── SupplierID → Supplier
├── CurrentCustodianId → Employee
├── VehicleDetails (if vehicle type)
├── MaintenanceLog (repair history)
├── DepreciationSchedule (PPE only)
├── AssetAssignmentHistory
│   ├── IssuanceFormId → NfaFormHeader (PAR/ICS)
│   └── ReturnFormId → NfaFormHeader (RRP)
└── AssetReclassificationHistory

NfaFormHeader (Form Control & Routing)
├── FormType → Enum (PPERR, SMRR, ICS, PAR, PPEIR, SMIR, RRP)
├── ControlNumber → Official form number
├── ApplicableClassification → PPE / Semi-Expendable
├── ProcessedByUserId → Supply Officer
├── ApprovedByUserId → Manager
├── OfficeId → Physical location
└── NfaFormLineItem (1:Many)
    ├── AssetId → PhysicalAsset
    └── Quantity, UnitPrice, Remarks
```

---

## 5. Workflow Routing Rules - Implementation Guide

### Rule 1: Receiving Workflow
```csharp
public async Task CreateReceivingDocument(PhysicalAsset asset, GoodsReceipt receipt)
{
    var formType = asset.AcquisitionCost >= 50000m 
        ? NfaFormType.PPERR 
        : NfaFormType.SMRR;
    
    var form = NfaFormHeader.Create(
        formType: formType,
        transactionDate: DateTime.Now,
        officeId: receipt.OfficeId,
        processedBy: receipt.SupplyOfficerId
    );
    
    await _formRepository.AddAsync(form);
}
```

### Rule 2: Assignment Workflow
```csharp
public async Task CreateAssignmentDocument(
    PhysicalAsset asset, 
    Employee employee)
{
    var formType = asset.AcquisitionCost >= 50000m 
        ? NfaFormType.PAR 
        : NfaFormType.ICS;
    
    var form = NfaFormHeader.Create(
        formType: formType,
        transactionDate: DateTime.Now,
        officeId: employee.OfficeId
    );
    
    var assignment = asset.AssignTo(employee, form);
    await _assetRepository.UpdateAsync(asset);
}
```

### Rule 3: Issuance/Transfer Workflow
```csharp
public async Task CreateIssuanceDocument(
    PhysicalAsset asset,
    Employee toEmployee)
{
    var formType = asset.AcquisitionCost >= 50000m 
        ? NfaFormType.PPEIR 
        : NfaFormType.SMIR;
    
    var form = NfaFormHeader.Create(formType);
    // Create transfer record
}
```

### Rule 4: Return/Unserviceable Workflow
```csharp
public async Task CreateReturnDocument(
    PhysicalAsset asset,
    string reason)
{
    // RRP used for both classifications
    var form = NfaFormHeader.Create(
        formType: NfaFormType.RRP,
        applicableClassification: asset.CurrentClassification
    );
    
    asset.MarkAsUnserviceable(reason, form);
}
```

---

## 6. Implementation Roadmap

### Phase 1: Foundation (Week 1-2)
- [ ] Create `NfaFormHeader` and `NfaFormLineItem` entities
- [ ] Create `NfaFormType` enum
- [ ] Create `VehicleDetails` entity
- [ ] Create `MaintenanceLog` entity

### Phase 2: Integration (Week 3)
- [ ] Extend `PhysicalAsset` with missing purchase fields
- [ ] Update `AssetAssignmentHistory` with form linking
- [ ] Create `NfaFormRoutingService`
- [ ] Add domain events for form creation

### Phase 3: Workflows (Week 4-5)
- [ ] Implement receiving document creation
- [ ] Implement assignment document creation
- [ ] Implement issuance document creation
- [ ] Implement return document creation

### Phase 4: Reporting (Week 6)
- [ ] Update Blazor pages for NFA form display
- [ ] Create form printing/export functionality
- [ ] Add cost-threshold routing UI
- [ ] Create vehicle history card reports

---

## 7. Cost Threshold Configuration

**Critical Business Rule:**
```
Assets ≥ 50,000 PHP → PPE Classification → PPERR/PPEIR/PAR/Depreciation
Assets < 50,000 PHP → Semi-Expendable → SMRR/SMIR/ICS/Straight-line write-off
```

**Configuration Storage:**
```csharp
public class NfaConfiguration
{
    public const decimal PPE_THRESHOLD = 50000m; // PHP
    public const int PPE_USEFUL_LIFE_MONTHS = 60;
    public const int SEMI_EXPENDABLE_USEFUL_LIFE_MONTHS = 24;
}
```

**Validation in Creation:**
```csharp
public PhysicalAsset Create(/* params */)
{
    var classification = acquisitionCost >= NfaConfiguration.PPE_THRESHOLD
        ? PropertyClassification.PropertyPlantEquipment
        : PropertyClassification.SemiExpendable;
    
    // Auto-classify based on cost
}
```

---

## 8. Database Migration Strategy

**New Tables Required:**
```sql
-- 1. NFA Form Headers
CREATE TABLE [dbo].[NfaFormHeaders] (
    [Id] uniqueidentifier PRIMARY KEY,
    [FormType] nvarchar(50) NOT NULL,  -- PPERR, SMRR, PAR, ICS, PPEIR, SMIR, RRP
    [ControlNumber] nvarchar(100) NOT NULL,
    [TransactionDate] datetime2 NOT NULL,
    [OfficeId] uniqueidentifier NOT NULL,
    [ProcessedByUserId] uniqueidentifier NOT NULL,
    [ApprovedByUserId] uniqueidentifier,
    [ApplicableClassification] nvarchar(50),
    [CreatedDate] datetime2,
    [CreatedBy] nvarchar(256)
);

-- 2. Vehicle Details
CREATE TABLE [dbo].[VehicleDetails] (
    [AssetId] uniqueidentifier PRIMARY KEY,
    [PlateNumber] nvarchar(20) NOT NULL,
    [EngineNumber] nvarchar(50),
    [ChassisNumber] nvarchar(50),
    [VehicleType] nvarchar(100),
    [Mileage] int,
    [MileageLastRecorded] datetime2
);

-- 3. Maintenance Log
CREATE TABLE [dbo].[MaintenanceLogs] (
    [Id] uniqueidentifier PRIMARY KEY,
    [AssetId] uniqueidentifier NOT NULL,
    [ReferenceNumber] nvarchar(100),
    [NatureOfRepair] nvarchar(max),
    [PartsCost] decimal(18,2),
    [LaborCost] decimal(18,2),
    [ServiceDate] datetime2 NOT NULL
);
```

---

## 9. Key Takeaways & Next Steps

### ✅ Strengths
1. **PhysicalAsset** is well-structured as core aggregate
2. **AssetAssignmentHistory** provides custody chain
3. **Event-driven projections** (InventoryRegistry) are modern and maintainable
4. **PropertyClassification** enum foundation exists

### ⚠️ Critical Gaps
1. **No unified NFA form header** - documents scattered
2. **No vehicle specialization** - vehicles treated as generic PPE
3. **No maintenance tracking** - cannot generate full asset history
4. **No cost-threshold routing** - form selection manual, not rules-based

### 🎯 Immediate Actions
1. Create `NfaFormHeader` entity as central form registry
2. Implement cost-based form routing logic
3. Add `VehicleDetails` for specialized vehicle tracking
4. Link `AssetAssignmentHistory` to form documents

---

## 10. Code Example: Enhanced PhysicalAsset

```csharp
public class PhysicalAsset : AuditableEntity, IAggregateRoot
{
    // Existing properties...
    public string PropertyCode { get; private set; } = default!;
    public Guid ProductId { get; private set; }
    public decimal AcquisitionCost { get; private set; }
    public DateTime AcquisitionDate { get; private set; }
    public PropertyClassification CurrentClassification { get; private set; }
    
    // ========== NFA COMPLIANCE ADDITIONS ==========
    
    // Purchase Documentation (Map to NFA Asset Birth Certificate)
    public string? PR_Number { get; private set; }              // Purchase Request
    public string? PO_Number { get; private set; }              // Purchase Order
    public string? SI_Number { get; private set; }              // Sales Invoice
    public Guid? SupplierId { get; private set; }               // Supplier
    
    // Status Tracking (Serviceable/Unserviceable/In Transit)
    public AssetStatus Status { get; private set; } = AssetStatus.Serviceable;
    
    // Vehicle-Specific (Optional)
    public VehicleDetails? VehicleDetails { get; private set; }
    
    // Collections
    public List<MaintenanceLog> MaintenanceHistory { get; private set; } = [];
    public List<AssetAssignmentHistory> AssignmentHistory { get; private set; } = [];
    
    // Factory: Auto-classify based on cost
    public static PhysicalAsset Create(
        string propertyCode,
        Guid productId,
        decimal acquisitionCost,
        DateTime acquisitionDate,
        string? pprNumber = null,
        string? poNumber = null,
        string? siNumber = null,
        Guid? supplierId = null)
    {
        var classification = acquisitionCost >= 50000m
            ? PropertyClassification.PropertyPlantEquipment
            : PropertyClassification.SemiExpendable;
        
        return new PhysicalAsset
        {
            PropertyCode = propertyCode,
            ProductId = productId,
            AcquisitionCost = acquisitionCost,
            AcquisitionDate = acquisitionDate,
            CurrentClassification = classification,
            PR_Number = pprNumber,
            PO_Number = poNumber,
            SI_Number = siNumber,
            SupplierId = supplierId,
            Status = AssetStatus.Serviceable
        };
    }
}

public enum AssetStatus
{
    Serviceable,
    Unserviceable,
    InTransit,
    Disposed
}
```

---

## Summary Table: NFA Alignment Status

| Requirement | Current Status | Gap | Priority |
|---|---|---|---|
| Core Asset Record | ✅ PhysicalAsset exists | Minor additions needed | Medium |
| Cost-Based Classification | ✅ PropertyClassification | Auto-routing missing | High |
| Form Type Routing | ❌ Not implemented | Need NfaFormHeader + routing | High |
| Purchase Documentation | ⚠️ Partial | Need PR/PO/SI fields | High |
| Vehicle Details | ❌ Missing | Need VehicleDetails entity | Medium |
| Maintenance History | ❌ Missing | Need MaintenanceLog entity | Medium |
| Form Documents | ⚠️ Fragmented | Need unified NfaFormHeader | High |
| Accountability Log | ✅ AssetAssignmentHistory | Link to forms needed | Medium |
| Inventory Sync | ✅ InventoryRegistry | Working well | Low |
| Depreciation | ✅ DepreciationSchedule | Integrate with forms | Low |

