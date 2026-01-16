# Unified PRD: Asset Management Information System (AMIS)

**System:** PH NFA COA‑Compliant Inventory System (Consumables, Semi‑Expendables, PPE)  
**Document owner:** _TBD_  
**Last updated:** 2025-12-23  
**Status:** Draft

## 1) Purpose
This document consolidates the system’s PRD and PRD-like specifications into a single, unified product requirements document.

Inputs consolidated:
- System and compliance overview (root README and guidance docs)
- Asset procurement & inventory workflow (purchaseworkflow)
- COA-compliant operational flow (Systemsflow)
- Domain workflow constraints and status transitions (Aggregate usage guide)
- Product Inspection workflow before warehouse receiving (product inspection PRD)

## 2) Product Summary
AMIS supports end-to-end inventory and asset operations with an audit-ready workflow and COA/DBM/PPSAS compliance.

At minimum, the product covers:
- Inventory tracking (receipts, issuances, balances)
- Procurement-to-inventory workflows (purchase → delivery/receipt → inspection/acceptance → inventory)
- Item classification (Consumable vs Semi‑Expendable vs PPE) driven by policy thresholds
- Mandatory document generation (RIS, RSMI, ICS, PAR, WMR, JEV)
- Reporting and audit readiness (monthly and year-end reports)

## 3) Goals
- Maintain accurate, real-time inventory balances with complete audit trail.
- Enforce COA/DBM/PPSAS rules (classification, RCA mapping, documentary requirements).
- Ensure inspection and acceptance controls occur before inventory receiving (where applicable).
- Standardize workflow statuses and prevent invalid transitions.
- Support multi-role approvals and accountability (encoded/approved/released/received-by).

## 4) Non-Goals (Out of Scope)
- Full supplier/vendor lifecycle management.
- Advanced Quality Management (CAPA, sampling plans, supplier scoring).
- Warehouse optimization features (bin locations, routing, WMS wave picking).
- Full accounting general ledger system (AMIS produces JEV-ready outputs and references, but does not replace GL).

## 5) Personas & Roles
- **Requisitioner**: files RIS requests for items.
- **Supply Officer / Receiving Clerk**: receives goods, verifies deliveries, manages stock, processes issuances.
- **Inspector / QA**: schedules and performs inspections and submits inspection reports.
- **Approving Authority**: approves transactions and decisions (issuance, acceptance/rejection), depending on policy.
- **Accounting**: validates compliance, generates/posts JEV outputs, produces statutory reports.
- **Warehouse Staff**: performs receiving into inventory after acceptance.
- **Admin**: maintains threshold locks, RCA mapping, and access control.

## 6) Key Compliance Requirements
### 6.1 Regulations
System behavior must align with:
- COA circulars on inventory accounts and documentary requirements
- COA circular on RCA 2019 conversion and correct account usage
- DBM inventory issuance/reporting requirements
- PPSAS 12 inventory recognition and valuation policies

### 6.2 Classification Rules (₱50,000 threshold)
- **Cost > ₱50,000** → classify as **PPE**
- **Cost ≤ ₱50,000** → classify as **Consumable** or **Semi‑Expendable** based on nature/useful life

**Document enforcement:**
- Consumables → **RSMI**
- Semi‑Expendables (≤ ₱50k) → **ICS**
- PPE (> ₱50k) → **PAR**

### 6.3 RCA mapping enforcement
- System must use **RCA 2019** codes and block outdated mappings.
- Transactions must record account codes and supporting document references.

### 6.4 Audit trail
- Every transaction records: Encoded by, Approved by, Released by, Received by (as applicable).
- Every workflow state transition is logged with actor, timestamp, and optional notes.

## 7) System Modules (Product View)
### 7.1 Catalog / Inventory
- Product master data (SKU, category, UoM)
- Inventory balances and average cost
- Inventory transactions (receipts, issuances, adjustments, disposal)

### 7.2 Procurement / Purchases
- Purchase Orders (PO) and line items
- Delivery/Goods receipt capture (GRN/DR references)
- Status progression and controls

### 7.3 Inspection & Acceptance
- Inspection requests, scheduling, execution
- Inspection report submission
- Acceptance/rejection decisioning

### 7.4 Issuance
- RIS filing and approval flow
- Issuance posting with COA form generation
- Custodian accountability rules

### 7.5 Reporting
- Monthly RSMI and inventory summaries
- Year-end inventory report
- Notes to Financial Statements disclosures

### 7.6 Accounting Outputs
- JEV templates linked to transactions and documents
- Export formats: PDF and (optional) XML/JSON for integration

## 8) Core Workflows

### 8.1 Procurement → Delivery → Inspection → Acceptance → Receiving (Before Warehouse Receiving)
This workflow is used when items must be inspected before warehouse receiving into inventory.

**Steps:**
1. Delivery Arrival recorded
2. Initial Verification by Receiving Clerk / Supply Officer
3. Inspection Request created and assigned
4. Inspector notified
5. Inspection scheduled and executed
6. Inspection report submitted
7. Acceptance or rejection decision
8. Warehouse receiving for accepted items only

**Rules:**
- Acceptance/rejection cannot occur without an inspection report (unless policy overrides).
- Rejected items never enter inventory.

### 8.2 Purchase Order Lifecycle (Status-driven)
**Minimum status model:**
- Draft → Submitted → (Partially Delivered) → Delivered → Closed
- Cancelled (from allowed states)

**Rules (must enforce):**
- Cannot close without required inspection and acceptance completion.
- Cannot edit a closed/cancelled PO.

### 8.3 Goods Receipt (GRN / Delivery)
**Key requirements:**
- Receipt is linked to a PO (or allowed manual receipt if configured).
- Capture delivery document numbers, dates, and received quantities.
- Receipt creates an inspection queue/task when inspection is required.

### 8.4 Inspection Workflow
**Minimum status model:**
- In Progress → Completed → Approved
- Rejected / Cancelled as alternative outcomes

**Rules (must enforce):**
- Passed + failed must equal inspected.
- No duplicate inspection lines per purchase line.

### 8.5 Acceptance Workflow
**Minimum status model:**
- Pending → Posted
- Cancelled (if allowed)

**Rules (must enforce):**
- Cannot accept more than inspected/passed quantity.
- Cannot modify after posting.

### 8.6 Receipt → Classification → Posting
Upon receipt, system must:
- Determine classification using cost threshold + nature
- Generate required inventory records and documents
- Ensure correct RCA mapping

### 8.7 Issuance Workflow (Outflow)
**Steps:**
1. Requisitioner files RIS
2. Supply Officer checks stock availability
3. Approvals applied per policy
4. System generates the correct issuance document (RSMI/ICS/PAR)
5. Stock and ledger records updated
6. Accounting receives summary and JEV references

**Rules:**
- Block issuance if stock would go negative.
- Block issuance if custodian has unsettled accountability (semi‑expendables/PPE).

### 8.8 Disposal / Waste
**Requirements:**
- Disposal transactions generate Waste Materials Report (WMR)
- Disposal clears accountability where applicable
- Disposal updates stock cards and ledgers

## 9) Functional Requirements (Consolidated)

### 9.1 Product & Inventory
- Maintain product catalog with UoM and classification metadata.
- Maintain inventory with non-negative quantity invariant.
- Maintain average cost updates on receipt.

### 9.2 Purchase Orders
- Create PO with supplier, delivery details, and items.
- Submit/issue/close/cancel according to workflow rules.
- Maintain total amount as derived from items.

### 9.3 Receiving / Delivery Arrival
- Record delivery arrival with supplier, documents, delivered items, attachments.
- Link to PO where applicable.

### 9.4 Inspection
- Create inspection request linked to delivery/PO.
- Assign inspector and due date; notify inspector.
- Capture inspection results per line; submit report.

### 9.5 Acceptance
- Record acceptance quantities per line.
- Validate acceptance quantities against inspection results.
- Post acceptance and lock edits.

### 9.6 Issuance
- Support RIS creation, approvals, and issuance.
- Auto-generate required documents (RSMI/ICS/PAR) based on classification.

### 9.7 Reports
- Produce monthly RSMI and inventory summaries.
- Produce year-end inventory report.
- Produce Notes-to-FS disclosure outputs.

### 9.8 Accounting Outputs
- Generate JEV templates for:
  - receipts
  - issuances
  - disposals
  - (optional) reclassifications
- Support exporting outputs for audit and integration.

## 10) Permissions & Access Control
- Role-based access for each workflow step.
- Only authorized users can:
  - change thresholds
  - finalize acceptance/rejection
  - post accounting outputs
  - override locked records

## 11) Data & Traceability (Conceptual)
Key conceptual records:
- Purchase, PurchaseItem
- DeliveryArrival / GoodsReceipt and line items
- Inspection and inspection item results
- Acceptance and acceptance items
- InventoryTransaction
- Issuance and issuance items
- Document records (RIS, RSMI, ICS, PAR, WMR)
- JEV references linked to transactions

## 12) Non-Functional Requirements
- **Performance:** list screens load within 2 seconds for typical datasets.
- **Reliability:** idempotent submissions (avoid duplicates on retries).
- **Security:** tenant isolation (if enabled), least-privilege access.
- **Auditability:** immutable history of posted/approved records.
- **Usability:** responsive UI, clear statuses, accessible forms.

## 13) Metrics / Success Criteria
- Time from delivery arrival to inspection completion.
- % of transactions with complete documentary attachments.
- Number of invalid workflow transition attempts blocked.
- Issuance SLA compliance (request to release time).

## 14) Known Conflicts / Clarifications Needed
1. **Inventory valuation method:** Some docs mention FIFO; other docs and domain rules specify **Weighted Average**. Confirm which is authoritative for valuation and which (if any) FIFO applies to (issuance ordering vs cost method).
2. **Inspection timing:** Confirm whether inspection is always before warehouse receiving, or only for certain classifications/categories.
3. **Acceptance authority:** Confirm who finalizes acceptance/rejection (Inspector vs Supply Officer vs Approver).
4. **Manual receipts:** Confirm whether receipts without PO are permitted.

## 15) Appendix: Source Documents
- Root system overview: README and compliance guidance docs
- Procurement workflow: purchaseworkflow
- COA process guide: Systemsflow
- Domain workflow reference: aggregate usage guide
- Inspection PRD: product inspection workflow PRD
