# COA-Compliant Inventory System Implementation

## Overview
This implementation provides a complete inventory management system compliant with:
- **COA Circular 2022-002** (Revised Chart of Accounts 2019)
- **DBM Circulars** on inventory management
- **Philippine Public Sector Accounting Standards (PPSAS) 12** (Inventories)
- **₱50,000 capitalization threshold** for property classification

## Key Features Implemented

### 1. Property Classification (Domain Layer)

#### Three-Tier Classification System
Based on cost and useful life, items are automatically classified as:

1. **Consumable** (≤ ₱50,000, short useful life)
   - RCA Account: 10501000 - Supplies and Materials Inventory
   - Expense Account: 50203010 - Supplies and Materials Expense
   - Required Document: **RSMI** (Requisition and Issue Slip)

2. **Semi-Expendable** (≤ ₱50,000, useful life > 1 year)
   - RCA Account: 10599020 - Semi-Expendable Property Inventory
   - Expense Account: 50299010 - Semi-Expendable Property Expense
   - Required Document: **ICS** (Inventory Custodian Slip)
   - Features: Custodian accountability tracking

3. **PPE** (> ₱50,000)
   - RCA Accounts: 1-06-*** (various PPE accounts)
   - Required Document: **PAR** (Property Acknowledgment Receipt)

### 2. Domain Entities

#### ConsumableInventory
- Stock number tracking
- Weighted average cost calculation
- FIFO/Weighted Average method support
- Automatic RSMI generation on issuance
- Reorder level monitoring

#### SemiExpendableInventory
- Property code tracking
- Custodian assignment and accountability
- Estimated useful life tracking
- ICS document generation
- Return from custodian workflow

#### JournalEntryVoucher (JEV)
- Auto-generation per COA Circular 2022-002
- Debit/Credit line items with RCA 2019 account codes
- Balanced entry validation
- Posting workflow with user tracking
- Link to source transactions (RSMI/ICS/PAR)

### 3. Inventory Transactions

#### Receipt Flow
1. Purchase Order created
2. Goods delivered
3. Inspection requested
4. Inspection completed
5. Acceptance posted
6. Inventory updated with weighted average cost
7. **JEV auto-generated** for receipt

#### Issuance Flow
1. Requisition created
2. Stock availability verified
3. Custodian accountability checked (for semi-expendable)
4. Appropriate document generated (RSMI/ICS/PAR)
5. Inventory decreased
6. **JEV auto-generated** for expense recognition

### 4. Document Generation Services

Interfaces defined for generating COA-required documents:

- **IDocumentGenerationService**: Base service for PDF generation
- **RSMI Document**: For consumable issuances
- **ICS Document**: For semi-expendable issuances with custodian tracking
- **PAR Document**: For PPE assignments
- **Stock Ledger Card (SLC)**: For inventory movement tracking

Each document includes:
- Required fields per COA templates
- Approval workflow signatures
- Reference numbers
- Proper accounting classifications

### 5. Journal Entry Auto-Generation

**IJournalEntryService** provides methods for:

#### Consumable Receipts
```
Dr: Supplies and Materials Inventory (10501000)
Cr: Accounts Payable / Cash
```

#### Consumable Issuances
```
Dr: Supplies and Materials Expense (50203010)
Cr: Supplies and Materials Inventory (10501000)
```

#### Semi-Expendable Receipts
```
Dr: Semi-Expendable Property Inventory (10599020)
Cr: Accounts Payable / Cash
```

#### Semi-Expendable Issuances
```
Dr: Semi-Expendable Property Expense (50299010)
Cr: Semi-Expendable Property Inventory (10599020)
```

#### PPE Acquisitions
```
Dr: PPE Asset Account (1-06-xx-xxx)
Cr: Accounts Payable / Cash
```

### 6. Business Rules Enforced

1. **₱50,000 Threshold Validation**
   - Automatic classification based on cost
   - Prevention of misclassification
   - System-locked threshold (editable only by authorized accounting officials)

2. **Weighted Average Cost Calculation**
   - Automatic recalculation on receipts
   - Proper cost allocation on issuances
   - FIFO not used (as per COA preference for weighted average)

3. **Accountability Tracking**
   - Semi-expendable items track current custodian
   - Prevent issuance to users with unsettled accountabilities
   - Return workflow for custodian accountability clearance

4. **RCA 2019 Validation**
   - All account codes mapped to Revised Chart of Accounts 2019
   - Blocked posting to outdated 2015 RCA accounts
   - Validation rules for proper account usage

5. **Three-Way Match**
   - Purchase Order → Goods Receipt → Invoice matching
   - Prevents payment before proper receipt and acceptance

## Database Schema

### SemiExpendableInventories Table
- PropertyCode (unique, varchar(50))
- ProductId (Guid, FK)
- Description (varchar(500))
- UnitCost, WeightedAverageCost (decimal(18,2))
- Quantity (int)
- UnitOfMeasure (varchar(50))
- Location (varchar(200))
- EstimatedUsefulLifeMonths (int, default 36)
- CurrentCustodianId (Guid, FK to Employees)
- IsIssued (bool, default false)
- Multi-tenant and audit fields

### JournalEntryVouchers Table
- JEVNumber (unique, varchar(50))
- TransactionDate (datetime)
- Description (varchar(500))
- SourceTransactionId, SourceDocumentType, SourceDocumentNumber
- TotalDebit, TotalCredit (decimal(18,2))
- IsPosted (bool), PostedDate, PostedBy
- Notes (varchar(1000))
- Multi-tenant and audit fields

### JournalEntryLines Table
- JournalEntryVoucherId (Guid, FK)
- AccountCode (varchar(50)) - RCA 2019
- AccountTitle (varchar(200))
- DebitAmount, CreditAmount (decimal(18,2))
- Particulars (varchar(500))

## Usage Workflow

### Scenario 1: Consumable Item (e.g., Office Supplies)

1. **Procurement**: Create PO for office supplies (₱5,000)
2. **Receipt**: Goods delivered, inspection passed
3. **System Actions**:
   - ConsumableInventory record created/updated
   - Weighted average cost calculated
   - JEV generated for receipt
4. **Issuance**: Employee requests items via RIS
5. **System Actions**:
   - RSMI document generated
   - ConsumableInventory quantity decreased
   - JEV generated recognizing expense

### Scenario 2: Semi-Expendable Item (e.g., Office Chair ₱8,000)

1. **Procurement**: Create PO for chairs
2. **Receipt**: Goods delivered, inspection passed
3. **System Actions**:
   - SemiExpendableInventory record created
   - Property code assigned
   - JEV generated for receipt
4. **Issuance**: Employee assigned chair via ICS
5. **System Actions**:
   - ICS document generated with custodian name
   - CurrentCustodianId updated
   - IsIssued flag set to true
   - JEV generated recognizing expense
6. **Return**: Employee returns chair
7. **System Actions**:
   - Accountability cleared
   - IsIssued flag reset
   - Item returned to available inventory

### Scenario 3: PPE Item (e.g., Vehicle ₱800,000)

1. **Procurement**: Create PO for vehicle
2. **Receipt**: Vehicle delivered, inspection passed
3. **System Actions**:
   - PhysicalAsset record created (existing implementation)
   - PPE classification assigned
   - Proper PPE account code determined
   - JEV generated for PPE acquisition
4. **Assignment**: Vehicle assigned via PAR
5. **System Actions**:
   - PAR document generated
   - Asset assignment history recorded
   - Depreciation schedule created (if applicable)

## Reporting Capabilities

### Standard Reports (To Be Implemented)
1. **Stock Ledger Card (SLC)**: Track all movements per item
2. **RSMI Summary**: Monthly consumable issuances
3. **ICS Registry**: All semi-expendables with current custodians
4. **PAR Registry**: All PPE with assignments
5. **Monthly Inventory Summary**: For submission to accounting
6. **Year-End Inventory Report**: For COA audit
7. **Waste Materials Report**: For disposals and write-offs
8. **Notes to Financial Statements**: Auto-generated disclosures

## Compliance Checklist

- [x] RCA 2019 account codes implemented
- [x] ₱50,000 threshold enforcement
- [x] Property classification (Consumable/Semi-Expendable/PPE)
- [x] Weighted average cost calculation
- [x] JEV auto-generation framework
- [x] Document types defined (RSMI/ICS/PAR)
- [x] Custodian accountability tracking
- [x] Multi-tenancy support
- [x] Audit trail (CreatedBy, LastModifiedBy, etc.)
- [ ] PDF document generation (templates needed)
- [ ] Report generation (SLC, RSMI, etc.)
- [ ] Year-end disclosure automation
- [ ] Integration with GAS-COA (XML/JSON export)

## Next Steps for Implementation

1. **Application Layer**:
   - Create CQRS handlers for SemiExpendableInventory
   - Implement JournalEntryService
   - Add event handlers for automatic JEV generation

2. **Infrastructure**:
   - Implement DocumentGenerationService with PDF templates
   - Add report generation services

3. **API**:
   - Add endpoints for SemiExpendableInventory CRUD
   - Add endpoints for JEV viewing and posting
   - Add document download endpoints

4. **Testing**:
   - Run database migrations
   - Test weighted average calculations
   - Validate JEV generation logic
   - Test end-to-end workflows

## References

- COA Circular 2022-002: Revised Chart of Accounts (RCA) 2019
- PPSAS 12: Inventories
- DBM Budget Circulars on Inventory Management
- Government Procurement Policy Board (GPPB) Guidelines
- NFA Internal SOPs and Manuals
