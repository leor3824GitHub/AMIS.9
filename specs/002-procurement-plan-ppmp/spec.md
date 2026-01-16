# Feature Specification: Procurement Plan (PPMP) Module

**Feature Branch**: `002-procurement-plan-ppmp`  
**Created**: 2025-12-27  
**Status**: Draft  
**Input**: User description: "Implement Procurement Plan (PPMP) domain, API, and Blazor UI"

## User Scenarios & Testing *(mandatory)*

<!--
  IMPORTANT: User stories should be PRIORITIZED as user journeys ordered by importance.
  Each user story/journey must be INDEPENDENTLY TESTABLE - meaning if you implement just ONE of them,
  you should still have a viable MVP (Minimum Viable Product) that delivers value.
  
  Assign priorities (P1, P2, P3, etc.) to each story, where P1 is the most critical.
  Think of each story as a standalone slice of functionality that can be:
  - Developed independently
  - Tested independently
  - Deployed independently
  - Demonstrated to users independently
-->

### User Story 1 - Create and maintain a Draft PPMP (Priority: P1)

As a procurement/supply officer, I want to create a PPMP header and manage its line items in DRAFT so that the organization has a controlled procurement plan baseline for the fiscal year.

**Why this priority**: This is the foundation; without a draft PPMP, no compliant PR gating can exist.

**Independent Test**: Can be fully tested by creating a PPMP header, adding/editing/removing items, verifying validations and computed totals, and confirming the data is tenant-isolated.

**Acceptance Scenarios**:

1. **Given** a user has permission to manage PPMPs, **When** they create a PPMP header with a unique control number and fiscal year, **Then** it is saved in `DRAFT` status.
2. **Given** a PPMP header in `DRAFT`, **When** the user adds items with quantity and unit cost, **Then** item budget is computed and header total budget reflects the sum of item budgets.
3. **Given** a PPMP header in `DRAFT`, **When** the user edits or removes items, **Then** totals recompute and changes are recorded as auditable events.

---

### User Story 2 - Submit PPMP for approval and approve it (Priority: P2)

As a procurement/supply officer and approver, I want the PPMP to support submission and approval so that only approved plans can be used as the source of truth for downstream purchasing.

**Why this priority**: RA 12009/PPMP governance requires controlled approval state transitions.

**Independent Test**: Can be tested by submitting a draft PPMP, verifying status changes + timestamps, approving it, and verifying that edits are blocked after approval (except via a supplemental workflow).

**Acceptance Scenarios**:

1. **Given** a PPMP header in `DRAFT`, **When** the preparer submits it, **Then** status becomes `PENDING_APPROVAL` and `submissionDate` is set.
2. **Given** a PPMP header in `PENDING_APPROVAL`, **When** an authorized approver approves it, **Then** status becomes `APPROVED`, `approvedByUserId` is set, and `approvalDate` is set.
3. **Given** a PPMP header in `APPROVED`, **When** a user attempts to add/edit/remove items, **Then** the operation is rejected unless the PPMP is explicitly supplemental and the workflow allows it.

---

### User Story 3 - Manage Procurement Projects derived from PPMP (Priority: P3)

As a procurement user, I want to maintain a Procurement Project with schedule and budget details (as one aggregate) so that planned items can be tracked through procurement timelines and budget allocations.

**Why this priority**: Enables operational planning and reporting, without blocking the core PPMP workflow.

**Independent Test**: Can be tested by creating a `ProcurementProject` with schedule and budget, verifying the aggregate persists and updates atomically, and ensuring access is tenant-scoped.

**Acceptance Scenarios**:

1. **Given** a user has procurement permissions, **When** they create a Procurement Project (title, end-user, mode, fund source), **Then** the project is saved and can be retrieved.
2. **Given** an existing project, **When** the user updates schedule milestones and budget splits, **Then** the project reflects the changes and the aggregate remains consistent (budget totals validate).

---

### Edge Cases

- Control number uniqueness conflict (same fiscal year/unit).
- Item validation: quantity <= 0, unit cost < 0, missing schedule month/funding source.
- Budget rounding rules when computing `estimatedBudget` and `totalBudget`.
- Concurrent edits to the same PPMP header/items.
- Status transitions out of order (e.g., approve a DRAFT; resubmit an APPROVED plan).
- Supplemental PPMP behavior (allowed edits, linkage to original, audit trail).
- Tenant isolation: user from another tenant cannot view or mutate PPMPs/projects.

## Requirements *(mandatory)*

<!--
  ACTION REQUIRED: The content in this section represents placeholders.
  Fill them out with the right functional requirements.
-->

### Functional Requirements

- **FR-001**: The system MUST provide CRUD operations for `ProcurementPlanHeader` and its `ProcurementPlanItem` children within a `ProcurementPlan` aggregate.
- **FR-002**: The system MUST enforce status-based write rules: items and header metadata MUST be editable only in `DRAFT` (and in explicitly-allowed supplemental flows).
- **FR-003**: The system MUST compute `ProcurementPlanItem.estimatedBudget = quantity * unitCost` and MUST compute `ProcurementPlanHeader.totalBudget` as the sum of all item budgets.
- **FR-004**: The system MUST support workflow transitions for PPMP: `DRAFT → PENDING_APPROVAL → APPROVED` with audit trail and timestamps (`submissionDate`, `approvalDate`).
- **FR-005**: The system MUST enforce uniqueness of `controlNumber` per tenant and MUST reject duplicates.
- **FR-006**: The system MUST enforce multi-tenant isolation for all PPMP and Procurement Project reads/writes.
- **FR-007**: The system MUST enforce permission-based authorization for PPMP actions (create/update/submit/approve).
- **FR-008**: The system MUST provide CRUD operations for `ProcurementProject` as a single aggregate including `ProcurementSchedule` and `ProjectBudget`.
- **FR-009**: The system MUST validate `ProjectBudget.total_amount = mooe_amount + co_amount` (or else reject the change).
- **FR-010**: The Blazor UI MUST provide list + detail views for PPMPs and allow PPMP editing in `DRAFT`, submission, and approval flows (subject to permissions).
- **FR-011**: The Blazor UI MUST provide list + detail views for Procurement Projects and allow editing schedule/budget fields (subject to permissions).
- **FR-012**: All externally-facing inputs MUST be validated with server-side validators.

**AMIS constraints to apply when writing FRs**:
- Compliance-sensitive workflows MUST include audit trail and documentary outputs where applicable.
- Procurement-related workflows MUST consider RA 12009 compliance and PPMP source-of-truth rules when applicable.
- Multi-tenant data MUST be isolated by tenant context.
- Clean Architecture boundaries MUST be respected (no UI/persistence leakage into Domain).

### Key Entities *(include if feature involves data)*

- **ProcurementPlan (Aggregate)**: Rooted at `ProcurementPlanHeader` with owned `ProcurementPlanItem` children.
- **ProcurementPlanHeader**: PPMP envelope metadata and approval state.
- **ProcurementPlanItem**: PPMP line item; controls what can be requested downstream.
- **ProcurementProject (Aggregate)**: Rooted at `ProcurementProject` with owned `ProcurementSchedule` and `ProjectBudget`.
- **Tenant**: Isolation boundary for all procurement data.

## Success Criteria *(mandatory)*

<!--
  ACTION REQUIRED: Define measurable success criteria.
  These must be technology-agnostic and measurable.
-->

### Measurable Outcomes

- **SC-001**: A procurement user can create a draft PPMP (header + at least 1 item) in under 5 minutes.
- **SC-002**: PPMP totals are consistent (header total equals sum of item budgets) for 100% of persisted PPMPs.
- **SC-003**: An approver can approve a submitted PPMP and the system blocks further edits to approved PPMPs.
- **SC-004**: PPMP and Procurement Project data is inaccessible across tenants (0 cross-tenant data leaks in tests).
