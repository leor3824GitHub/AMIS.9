# 🎨 Visual Implementation Overview

## 🏗️ Domain Model Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    DOMAIN LAYER (NEW)                           │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌──────────────────┐         ┌──────────────────┐            │
│  │ PhysicalAsset    │         │ Employee         │            │
│  │ (ENHANCED)       │         │                  │            │
│  ├──────────────────┤         └──────────────────┘            │
│  │ + QRCodeData     │              ▲                          │
│  │ + PropertyNumber │              │                          │
│  │ + QRGenerated    │              │                          │
│  │ + CurrentCustod. │──────────────┘                          │
│  │ + GenerateQR()   │                                          │
│  │ + AssignToCusto()│                                          │
│  └──────────────────┘                                          │
│           ▲                                                    │
│           │                                                    │
│  ┌────────┴─────────────────────────────────────┐             │
│  │                                              │             │
│  │                                              │             │
│  │        ┌──────────────────────────────────┐  │             │
│  │        │   AssetRequisition (NEW)         │  │             │
│  │        ├──────────────────────────────────┤  │             │
│  │        │ - EmployeeId                    │  │             │
│  │        │ - IssuanceId                    │  │             │
│  │        │ - Status                        │  │             │
│  │        │ - AcceptanceSignature           │  │             │
│  │        │ - ExpirationDate                │  │             │
│  │        │                                 │  │             │
│  │        │ + Accept()                      │  │             │
│  │        │ + Reject()                      │  │             │
│  │        │ + Cancel()                      │  │             │
│  │        │ + IsExpired()                   │  │             │
│  │        └──────────────────────────────────┘  │             │
│  │                                              │             │
│  │        ┌──────────────────────────────────┐  │             │
│  │        │   Issuance (ENHANCED)            │  │             │
│  │        ├──────────────────────────────────┤  │             │
│  │        │ + Type (PAR|ICS)                 │  │             │
│  │        │ + CustodianId                   │  │             │
│  │        │ + Status                        │  │             │
│  │        │ + AcceptanceSignature           │  │             │
│  │        │                                 │  │             │
│  │        │ + Accept()                      │  │             │
│  │        │ + Reject()                      │  │             │
│  │        │ + MarkAsReturned()              │  │             │
│  │        └──────────────────────────────────┘  │             │
│  │                                              │             │
│  │        ┌──────────────────────────────────┐  │             │
│  │        │   Acceptance (ENHANCED)          │  │             │
│  │        ├──────────────────────────────────┤  │             │
│  │        │ + Status (7 values)              │  │             │
│  │        │                                 │  │             │
│  │        │ + MarkAsInspected()              │  │             │
│  │        │ + MarkAsAccepted()               │  │             │
│  │        │ + MarkAsPartiallyAccepted()      │  │             │
│  │        │ + MarkAsRejected()               │  │             │
│  │        └──────────────────────────────────┘  │             │
│  │                                              │             │
│  └──────────────────────────────────────────────┘             │
│                                                                 │
│  ┌────────────────────────────────────────────────────┐        │
│  │     Depreciation Management (NEW)                 │        │
│  ├────────────────────────────────────────────────────┤        │
│  │                                                   │        │
│  │  ┌──────────────────────────────────────────┐    │        │
│  │  │  DepreciationSchedule (AGGREGATE)        │    │        │
│  │  ├──────────────────────────────────────────┤    │        │
│  │  │ - PhysicalAssetId                       │    │        │
│  │  │ - Month/Year                            │    │        │
│  │  │ - MonthlyDepreciation                   │    │        │
│  │  │ - AccumulatedDepreciation               │    │        │
│  │  │ - JournalEntryVoucherId                 │    │        │
│  │  │                                         │    │        │
│  │  │ + Post(jevId)                           │    │        │
│  │  │ + Reverse()                             │    │        │
│  │  └──────────────────────────────────────────┘    │        │
│  │           ▲                                      │        │
│  │           │ owns                                │        │
│  │           │                                      │        │
│  │  ┌────────┴──────────────────────────────────┐   │        │
│  │  │  JournalEntryVoucher (AGGREGATE)          │   │        │
│  │  ├────────────────────────────────────────────┤   │        │
│  │  │ - VoucherNumber                            │   │        │
│  │  │ - Month/Year                               │   │        │
│  │  │ - TotalDebit/Credit                        │   │        │
│  │  │ - Status                                   │   │        │
│  │  │ - ExportFormat                             │   │        │
│  │  │                                            │   │        │
│  │  │ + Post()        [validates balance]        │   │        │
│  │  │ + MarkAsExported()                         │   │        │
│  │  │ + Reverse()                                │   │        │
│  │  │ + IsBalanced    [debit = credit]           │   │        │
│  │  └───────────────┬─────────────────────────────┘   │        │
│  │                 │ contains                        │        │
│  │                 │                                 │        │
│  │  ┌──────────────▼───────────────────────────┐    │        │
│  │  │  JournalEntry (ENTITY)                   │    │        │
│  │  ├────────────────────────────────────────────┤    │        │
│  │  │ - AccountCode                             │    │        │
│  │  │ - DebitAmount | CreditAmount (mutually ex)    │        │
│  │  │ - Description                             │    │        │
│  │  │                                            │    │        │
│  │  │ + Amount   [returns debit OR credit]      │    │        │
│  │  │ + IsDebit                                 │    │        │
│  │  │ + IsCredit                                │    │        │
│  │  └────────────────────────────────────────────┘   │        │
│  │                                                   │        │
│  └────────────────────────────────────────────────────┘        │
│                                                                 │
│  ┌────────────────────────────────────────────────────┐        │
│  │  VALUE OBJECTS                                    │        │
│  ├────────────────────────────────────────────────────┤        │
│  │                                                   │        │
│  │  ┌──────────────────────────────────────────┐    │        │
│  │  │  DigitalSignature (VALUE OBJECT) (NEW)   │    │        │
│  │  ├──────────────────────────────────────────┤    │        │
│  │  │ - SignatureData (base64)                 │    │        │
│  │  │ - SignedOn (timestamp)                   │    │        │
│  │  │ - SignedByEmployeeId                     │    │        │
│  │  │ - IpAddress [audit]                      │    │        │
│  │  │ - UserAgent [audit]                      │    │        │
│  │  │ - DeviceFingerprint [audit]              │    │        │
│  │  │                                          │    │        │
│  │  │ Used in:                                 │    │        │
│  │  │ • AssetRequisition.AcceptanceSignature   │    │        │
│  │  │ • Issuance.AcceptanceSignature           │    │        │
│  │  └──────────────────────────────────────────┘    │        │
│  │                                                   │        │
│  └────────────────────────────────────────────────────┘        │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## 📊 Workflow Sequence Diagrams

### Asset Issuance & Acceptance Workflow

```
SUPPLY OFFICER                    SYSTEM                    END USER
      │                             │                         │
      │── 1. Create Issuance ──────>│                         │
      │    (Select Asset, Type)     │                         │
      │                             │                         │
      │<─ Issuance Created ────────│                         │
      │  (Status: Pending)          │                         │
      │                             │                         │
      │── 2. Create AssetReq. ─────>│                         │
      │                             │── Notification ────────>│
      │                             │   (Dashboard Alert)     │
      │                             │                         │
      │                             │<─── 3. View Pending ────│
      │                             │   Issuance Details      │
      │                             │                         │
      │                             │<─── 4. Accept ────────│
      │                             │   + Digital Signature   │
      │                             │                         │
      │<─ Signature Captured ──────│                         │
      │   DigitalSignature object   │<─ Success ────────────│
      │                             │                         │
      │── 5. Confirmation ────────>│                         │
      │    Update Custodian         │                         │
      │                             │                         │
      │<─ Issuance Accepted ──────│                         │
      │  (Status: Accepted)         │                         │
      │  Custodian: EndUser         │                         │
      │                             │                         │
```

### Depreciation & JEV Workflow

```
PPE ASSET                    SYSTEM                    ACCOUNTANT
      │                         │                         │
      │<─ Acquisition ─────────│                         │
      │  (AcquisitionCost set) │                         │
      │                         │                         │
      ├─ Month 1 ──────────────>│                         │
      │                         │── Calculate ──────────>│
      │                         │ Depreciation: $100     │
      │                         │                         │
      │<─ Depreciation Record ──│<─ Review Schedule ────│
      │  DepreciationSchedule   │   Month 1: $100       │
      │  (Status: Pending)      │                         │
      │                         │<─ Create JEV ────────│
      │                         │   For Month 1         │
      │                         │   (Status: Draft)     │
      │                         │                         │
      │                         │<─ Add Entries ────────│
      │                         │   Dr. Asset: $100     │
      │                         │   Cr. Depr. Exp: $100│
      │                         │                         │
      │<─ JEV Created ─────────│                         │
      │  VoucherNumber: Generated                       │
      │  IsBalanced: Yes        │<─ Post JEV ──────────│
      │                         │   (Status: Posted)    │
      │                         │                         │
      │<─ Schedule Posted ─────│<─ Export ────────────│
      │  (Status: Posted)       │   Excel/PDF format   │
      │  JournalEntryVoucherId: │                         │
      │                         │<─ Success ────────────│
      │                         │   JEV Exported        │
      │                         │                         │
```

---

## 📈 Status Enums

### AssetRequisitionStatus
```
Pending ─────────> Accepted
    │                 │
    │                 └─────> (workflow complete)
    │
    ├─────────────> Rejected
    │
    ├─────────────> Cancelled
    │
    └─────────────> Expired (auto-check)
```

### IssuanceStatus
```
Pending ────────> Accepted ────────> Returned ──> (complete)
    │                                     ▲
    │                                     │
    └─────> Rejected ─────────────────────┘
    │
    └─────> Cancelled
```

### AcceptanceStatus
```
Pending ─────> Inspected ─────> Accepted ─────> Posted
                    │               │
                    │               └─> PartiallyAccepted
                    │
                    └─> Rejected ──> Cancelled
```

### DepreciationScheduleStatus
```
Pending ────────> Posted ────────> (remain in GL)
    │
    └─────────────> Reversed ────────> (removed from GL)
```

### JournalEntryVoucherStatus
```
Draft ───────> Posted ───────> Exported
                    │              │
                    │              └─> Excel/PDF
                    │
                    └──────────────> Reversed
```

---

## 💾 Database Table Structure

```
┌─────────────────────────────┐
│   AssetRequisitions (NEW)   │
├─────────────────────────────┤
│ Id (PK)                     │
│ EmployeeId (FK)             │
│ IssuanceId (FK)             │
│ RequisitionDate             │
│ ResponseDate                │
│ Status                      │
│ RejectionReason             │
│ ExpirationDate              │
│ AcceptanceSignature (OwnedType)
│ ├─ SignatureData            │
│ ├─ SignedOn                 │
│ ├─ SignedByEmployeeId       │
│ ├─ IpAddress                │
│ ├─ UserAgent                │
│ └─ DeviceFingerprint        │
│ CreatedBy, CreatedOn, ...   │
│ TenantId                    │
└─────────────────────────────┘

┌─────────────────────────────┐
│ DepreciationSchedules (NEW) │
├─────────────────────────────┤
│ Id (PK)                     │
│ PhysicalAssetId (FK)        │
│ Month                       │
│ Year                        │
│ MonthlyDepreciationAmount   │
│ AccumulatedDepreciationAmt  │
│ Status                      │
│ JournalEntryVoucherId (FK)  │
│ PostedDate                  │
│ Remarks                     │
│ CreatedBy, CreatedOn, ...   │
│ TenantId                    │
│ UNIQUE INDEX (AssetId, Y, M)
└─────────────────────────────┘

┌──────────────────────────────┐
│ JournalEntryVouchers (NEW)   │
├──────────────────────────────┤
│ Id (PK)                      │
│ VoucherNumber (UNIQUE)       │
│ VoucherDate                  │
│ Month                        │
│ Year (UNIQUE together w/Mo)  │
│ DepreciationMethod           │
│ TotalDebitAmount             │
│ TotalCreditAmount            │
│ Status                       │
│ PostedDate                   │
│ ExportedDate                 │
│ ExportFormat                 │
│ ExportFileName               │
│ Remarks                      │
│ CreatedBy, CreatedOn, ...    │
│ TenantId                     │
└──────────────────────────────┘

┌──────────────────────────────┐
│     JournalEntries (NEW)     │
├──────────────────────────────┤
│ Id (PK)                      │
│ JournalEntryVoucherId (FK)   │
│ LineNumber                   │
│ AccountCode                  │
│ AccountName                  │
│ DebitAmount                  │
│ CreditAmount                 │
│ Description                  │
│ CreatedBy, CreatedOn, ...    │
│ UNIQUE (VoucherId, LineNum)  │
│ INDEX (AccountCode)          │
└──────────────────────────────┘

PhysicalAssets (ENHANCED)
├── + QRCodeData
├── + PropertyNumber (UNIQUE)
├── + QRGeneratedDate
├── + CurrentCustodianId (FK)
└── + Index on PropertyNumber & CurrentCustodianId

Issuances (ENHANCED)
├── + Type (enum: PAR/ICS)
├── + CustodianId (FK)
├── + Status (enum)
├── + AcceptedOn
├── + RejectionReason
├── + AcceptanceSignature (OwnedType)
└── + Index on Status & CustodianId

Acceptances (ENHANCED)
├── Status (Enhanced enum: 7 values)
└── [No new columns, just enum update]
```

---

## 🔗 Entity Relationships

```
    Employee
    ├── AssetRequisition.EmployeeId
    ├── Issuance.CustodianId
    └── PhysicalAsset.CurrentCustodianId

    PhysicalAsset
    ├── DepreciationSchedule.PhysicalAssetId
    └── [from Issuance]

    Issuance
    ├── AssetRequisition.IssuanceId
    └── [from PhysicalAsset items]

    JournalEntryVoucher
    ├── JournalEntry.JournalEntryVoucherId (1 to many)
    └── DepreciationSchedule.JournalEntryVoucherId (0 to many)
```

---

## ✨ Key Features by Phase

### Phase 1 (COMPLETE ✅)
- ✅ Domain entities created
- ✅ Status enums defined
- ✅ Domain events created
- ✅ Value objects created
- ✅ Validations implemented

### Phase 2 (NEXT ⏳)
- ⏳ EF Core configurations
- ⏳ Database migrations
- ⏳ Repositories
- ⏳ Application services

### Phase 3 (LATER 📋)
- 📋 API endpoints
- 📋 Authorization
- 📋 Document generation

### Phase 4 (LATER 📋)
- 📋 Blazor pages
- 📋 Components
- 📋 Digital signature UI

### Phase 5 (LATER 📋)
- 📋 Testing
- 📋 Performance tuning
- 📋 Deployment

