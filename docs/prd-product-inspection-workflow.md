# PRD: Product Inspection Workflow (Before Warehouse Receiving)

**Document owner:** _TBD_  
**Last updated:** 2025-12-23  
**Status:** Draft

## 1) Summary
This PRD defines the **Product Inspection Workflow (Before Warehouse Receiving)**. It covers the process from **delivery arrival** to **acceptance or rejection** of goods, and the handoff to **warehouse receiving** for accepted items.

The workflow represented in the source diagram is:
1. Delivery Arrival
2. Initial Verification (Receiving Clerk / Supply Officer)
3. Inspector Notification
4. Inspection Request Creation
5. Inspection Scheduling & Execution
6. Inspection Report
7. Acceptance or Rejection
8. Warehouse Receiving (accepted items only)

## 2) Goals
- Ensure all delivered items are **verified and inspected** before being received into inventory.
- Provide **traceability** from purchase order / goods receipt to inspection outcomes.
- Enable clear **assignment**, **notification**, **scheduling**, and **decisioning**.
- Reduce delays by standardizing the flow and statuses.

## 3) Non-Goals (Out of Scope)
- Full procurement / purchasing creation (handled elsewhere).
- Warehouse put-away, bin locationing, stock adjustments beyond “received into inventory”.
- Quality management system (QMS) features like CAPA, supplier scoring, and advanced sampling plans.
- Automated supplier integrations.

## 4) Personas & Roles
- **Receiving Clerk / Supply Officer**
  - Records delivery arrival.
  - Performs initial verification.
  - Creates/initiates inspection request and assigns inspector.
- **Inspector / Employee**
  - Receives notification.
  - Schedules and performs inspection.
  - Submits inspection report.
  - Recommends accept/reject (if policy allows).
- **Warehouse Staff**
  - Receives accepted items into inventory after approval.
- **Supervisor / Admin (optional)**
  - Overrides decisions (if enabled).
  - Audits records.

## 5) Primary User Stories
1. As a Receiving Clerk, I can record a delivery arrival against a PO so items are queued for inspection.
2. As a Receiving Clerk, I can verify basic delivery details and then create an inspection request.
3. As an Inspector, I am notified when an inspection is assigned to me.
4. As an Inspector, I can schedule the inspection and capture inspection results.
5. As an Inspector, I can submit an inspection report.
6. As a Receiving Clerk (or approver), I can accept or reject delivered goods based on the report.
7. As Warehouse Staff, I can receive accepted goods into inventory (and rejected goods do not enter inventory).

## 6) Workflow States
### 6.1 Inspection Request Status
- **Draft**: Created but not submitted/assigned.
- **Assigned**: Assigned to an inspector.
- **Notified**: Notification delivered (or attempted).
- **Scheduled**: Inspection appointment/time set.
- **In Progress**: Inspection being conducted.
- **Reported**: Report submitted.
- **Accepted**: Accepted for warehouse receiving.
- **Rejected**: Rejected (not eligible for receiving).
- **Cancelled**: Workflow stopped.

### 6.2 Item-Level Inspection Status (for each delivered line)
- **Pending**
- **Inspected**
- **Accepted**
- **Rejected**

## 7) Functional Requirements

### 7.1 Delivery Arrival
**Purpose:** Start the workflow when goods physically arrive.

**Requirements:**
- Allow Receiving Clerk to create a **Delivery Arrival record** with:
  - Reference number (auto-generated)
  - Supplier
  - Related PO / Purchase reference (if applicable)
  - Delivery note / DR number (optional)
  - Delivery date/time
  - Delivered by / driver info (optional)
  - List of delivered items (quantity delivered, product, unit)
  - Attachments (photo, delivery note scan)
- A delivery arrival must be **linked** to either:
  - a Purchase Order / Goods Receipt context, or
  - a manual delivery entry (if no PO exists) _(optional, if allowed)_

**Acceptance criteria:**
- Delivery can be saved as Draft and later submitted for verification.
- System generates a unique reference.

### 7.2 Initial Verification (Receiving Clerk / Supply Officer)
**Purpose:** Confirm basic completeness before inspection.

**Requirements:**
- Provide a checklist (simple boolean fields):
  - Supplier & document match (yes/no)
  - Item count matches delivery note (yes/no)
  - Packaging intact (yes/no)
- Allow marking discrepancies with notes and attachments.
- Verification step must support **assigning** an inspector (user selection) and due date.

**Business rules:**
- Verification must be completed before an inspection request can be created/activated.

**Acceptance criteria:**
- If verification is incomplete, inspection request cannot be moved to Assigned.

### 7.3 Inspection Request Creation
**Purpose:** Formalize inspection work for newly arrived items.

**Requirements:**
- Create an **Inspection Request** linked to Delivery Arrival and containing:
  - Inspector assigned
  - Items to inspect (line items)
  - Priority (Low/Normal/High)
  - Target inspection date (optional)
  - Notes
- Support “Create inspection request” action from a Delivery Arrival.

**Acceptance criteria:**
- System creates request and moves status to Assigned.

### 7.4 Inspector Notification
**Purpose:** Ensure inspector knows they have work.

**Requirements:**
- Upon assignment, notify inspector via:
  - In-app notification (required)
  - Email (optional, if configured)
- Notification payload includes: request reference, supplier, location, due date.
- If notification fails, show a warning and allow retry.

**Acceptance criteria:**
- Inspector can see assigned inspections in their “My Tasks/Inspections” list.

### 7.5 Inspection Scheduling & Execution
**Purpose:** Track planned inspection time and capture execution.

**Requirements:**
- Inspector can set schedule:
  - Scheduled date/time
  - Location
- Inspector can start inspection (status -> In Progress).
- Item-level capture (minimum fields):
  - Quantity inspected
  - Findings/remarks
  - Pass/Fail outcome per item
  - Attachments (photos, documents)

**Business rules:**
- Quantity inspected cannot exceed quantity delivered.

**Acceptance criteria:**
- Inspector can save progress and resume later.

### 7.6 Inspection Report
**Purpose:** Submit final results.

**Requirements:**
- Inspector can submit an **Inspection Report** including:
  - Overall summary
  - Per-line results
  - Recommendation (Accept / Reject) per line
  - Final decision field (if inspector is authorized) or “Recommendation only” (if not)
  - Submitted timestamp

**Acceptance criteria:**
- Submission locks report editing (except via admin override).

### 7.7 Acceptance or Rejection
**Purpose:** Decide disposition of goods.

**Requirements:**
- The system supports decisioning after a report is submitted:
  - Accept all
  - Reject all
  - Mixed accept/reject per item line
- Capture decision metadata:
  - Decided by
  - Decision date/time
  - Notes

**Business rules:**
- Only authorized roles can finalize acceptance/rejection.
- Accepted quantities become eligible for **warehouse receiving**.
- Rejected quantities must not enter inventory.

**Acceptance criteria:**
- Once decision is finalized, status becomes Accepted/Rejected.

### 7.8 Warehouse Receiving (Accepted Items)
**Purpose:** Handoff accepted goods to inventory receiving.

**Requirements:**
- When inspection is Accepted (or partially accepted), allow “Create Warehouse Receiving” action that:
  - Creates receiving transaction for accepted lines/quantities
  - References the delivery and inspection report
- Rejected items are excluded.

**Acceptance criteria:**
- Warehouse Staff can view and receive only the accepted quantities.

## 8) UI / Screens (Minimal Set)
1. **Delivery Arrivals – List**
   - Status, supplier, date, reference
   - Actions: View, Verify, Create Inspection Request
2. **Delivery Arrival – Detail**
   - Verification checklist
   - Items delivered
   - Attachments
3. **Inspection Requests – List**
   - Filters: status, assigned-to-me
   - Actions: View, Schedule, Start
4. **Inspection Request – Detail / Execution**
   - Scheduling
   - Item inspection capture
   - Submit report
5. **Inspection Report – View**
   - Per-line results, attachments
   - Accept/Reject action (if authorized)
6. **Warehouse Receiving – Create/View**
   - Pre-filled with accepted items

## 9) Permissions & Access Control
- Receiving Clerk / Supply Officer:
  - Create delivery arrival, perform verification, create inspection request
- Inspector:
  - View assigned requests, schedule, execute, submit report
- Approver (if separated):
  - Accept/Reject decision
- Warehouse Staff:
  - Receive accepted items into inventory

## 10) Data Model (Conceptual)
- **DeliveryArrival**
  - Id, ReferenceNo, SupplierId, PurchaseId(optional), DeliveryNoteNo, DeliveredAt, CreatedBy, Attachments
- **DeliveryArrivalLine**
  - Id, DeliveryArrivalId, ProductId, QtyDelivered, Uom, Notes
- **InspectionRequest**
  - Id, ReferenceNo, DeliveryArrivalId, AssignedInspectorId, Status, Priority, DueDate, ScheduledAt(optional)
- **InspectionLineResult**
  - Id, InspectionRequestId, DeliveryArrivalLineId, QtyInspected, Outcome(Pass/Fail), Notes, Attachments
- **InspectionReport**
  - Id, InspectionRequestId, SubmittedAt, Summary, SubmittedBy
- **AcceptanceDecision**
  - Id, InspectionRequestId, DecidedAt, DecidedBy, DecisionNotes
- **WarehouseReceiving**
  - Id, ReferenceNo, InspectionRequestId, ReceivedAt, Lines

## 11) Notifications
- Trigger: On assignment to inspector.
- Target: Inspector user.
- Channels: In-app required; email optional.
- Retry: Manual retry by Receiving Clerk/Admin.

## 12) Auditing & Compliance
- Log all state transitions with:
  - from_status, to_status
  - actor
  - timestamp
  - comment (optional)
- Store attachments securely with links in audit.

## 13) Non-Functional Requirements
- **Performance:** List pages load within 2 seconds for typical datasets.
- **Reliability:** Submissions are idempotent (avoid duplicate reports/receivings).
- **Security:** Role-based access, tenant isolation (if multi-tenant is enabled).
- **Usability:** Mobile-friendly lists; keyboard-friendly forms.

## 14) Metrics / Success Criteria
- Median time from Delivery Arrival → Inspection Report submitted.
- % of deliveries inspected within SLA (if SLA exists).
- Rejection rate by supplier/product category.

## 15) Open Questions
1. Should acceptance decision be made by Inspector or Receiving Clerk (or separate approver)?
2. Do we allow inspections without an associated Purchase Order?
3. Do we need partial acceptance per line item quantity (e.g., accept 8, reject 2)?
4. Should Warehouse Receiving be auto-created on acceptance, or always manual?
5. What notifications are required (email/SMS), if any?
