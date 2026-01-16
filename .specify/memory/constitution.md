<!--
SYNC IMPACT REPORT

- Version change: 1.2.1 -> 1.2.2
- Modified principles: N/A
- Added sections: N/A
- Expanded sections: Development Workflow & Quality Gates (added EF migration steps)
- Removed sections: N/A
- Templates requiring updates:
	- .specify/templates/plan-template.md (no change)
	- .specify/templates/spec-template.md (no change)
	- .specify/templates/tasks-template.md (no change)
	- .specify/templates/commands/*.md: N/A (folder not present)
- Deferred items: None
-->

# AMIS.9 (Asset Management Information System) Constitution

## Core Principles

### Regulatory Compliance & Auditability (NON-NEGOTIABLE)
AMIS MUST remain COA/DBM/PPSAS-aligned and audit-ready.

Non-negotiable rules:
- All inventory and asset workflows MUST preserve an audit trail (who/when/what changed).
- COA/DBM documentary requirements (e.g., RIS/RSMI/ICS/PAR/WMR, inspection and acceptance controls)
	MUST be treated as product requirements, not optional UX.
- RCA mapping (2019) MUST be enforced where required; the system MUST prevent use of outdated mappings
	when policy requires blocking them.
- Any change affecting accounting classification, valuation method, or prescribed forms MUST include a
	compliance impact note in the PR description and be reviewed as a compliance-sensitive change.

Rationale: This system exists to produce defensible, statutory records and reports.

### Procurement Planning Compliance (RA 12009) (NON-NEGOTIABLE)
AMIS MUST treat procurement planning (PPMP) as a controlled, auditable source of truth for downstream
Purchase Requests and procurement actions.

Non-negotiable rules:
- For procurement-related features, workflows and records MUST be compliant with RA 12009 (The New
	Government Procurement Act) and its implementing rules as adopted by the organization.
- PPMP itemization MUST be modeled explicitly; Purchase Requests MUST NOT be created for goods/services
	not present in an applicable approved PPMP (or a documented and authorized exception flow).
- PPMP approval status transitions MUST be auditable (who/when/what) and enforceable (e.g., prevent
	editing of approved plans unless the workflow explicitly allows supplemental/addendum behavior).
- The canonical data structure for the PPMP envelope and line items MUST follow the definitions in the
	“Domain Entities: Procurement Plan (PPMP)” section of this constitution; deviations MUST be justified
	in the PR and reviewed as compliance-sensitive changes.

Rationale: Procurement planning is a statutory governance artifact and controls downstream procurement.

### Clean/Modular Architecture & Boundary Discipline
AMIS MUST preserve Clean Architecture layering and strict module boundaries.

Non-negotiable rules:
- Domain layer MUST be persistence-agnostic and UI-agnostic.
- Application layer MUST define use cases (CQRS) and contracts; Infrastructure provides implementations.
- Cross-module coupling MUST be explicit via shared contracts; avoid leaking EF entities across modules.
- Prefer vertical slice organization for features (Command/Handler/Validator/Endpoint/Response) to keep
	changes localized and reviewable.

Rationale: Modularity enables safe evolution, multi-team contribution, and prevents tight coupling.

### Multi-Tenancy & Authorization by Default
AMIS MUST treat tenant isolation and permissions as first-class invariants.

Non-negotiable rules:
- Every request that touches tenant-scoped data MUST resolve tenant context and enforce isolation.
- Authorization MUST be permission-based for protected operations (create/update/approve/post).
- All tenant-affecting configuration MUST be explicit, validated, and testable.
- Never introduce global/shared data writes without an intentional, documented policy decision.

Rationale: Multi-tenancy is a security and data integrity boundary, not a feature toggle.

### Production-Grade Cloud Readiness
AMIS MUST be operable in production and cloud environments (API + Blazor client).

Non-negotiable rules:
- Configuration MUST be environment-driven (no secrets in code; no environment-specific assumptions).
- Logging MUST be structured and meaningful for incident response (include correlation identifiers when
	available, avoid sensitive data in logs).
- Deployments MUST be reproducible (container-friendly) and support local orchestration (Aspire).
- Backward compatibility MUST be considered for public APIs and generated clients; breaking changes
	require explicit versioning and migration guidance.

Rationale: “Works on my machine” is not acceptable for a compliance system.

### Quality Gates & Safe Change Management
AMIS MUST prefer correctness and traceability over speed.

Non-negotiable rules:
- Changes to business rules, workflows, or state transitions MUST be covered by tests at the
	appropriate level (unit/integration) unless explicitly justified.
- Database migrations MUST be reviewed for safety (idempotence where applicable, data backfill plan
	when needed) and never silently change meaning of historical data.
- Validators MUST be present for externally-facing inputs.
- Avoid unnecessary complexity; choose the simplest design that satisfies compliance and invariants.

Rationale: Quality gates reduce regressions and preserve audit defensibility.

## Compliance & Architecture Constraints

- **Product scope**: PH NFA COA-compliant inventory system covering consumables, semi-expendables, and
	PPE, aligned to COA circulars, DBM issuances, PPSAS/PGAS, and RCA 2019 requirements.
- **Architecture**: Clean Architecture with a framework layer plus self-contained modules.
- **Client/server**: Web API (Minimal APIs with Carter/MediatR/FluentValidation) + Blazor WebAssembly
	client consuming a generated API client (NSwag).
- **Multi-tenancy**: Finbuckle.MultiTenant-based isolation; permissions-based authorization.
- **Data**: EF Core with PostgreSQL primary; MSSQL may exist for migrations/testing scenarios.

## Domain Entities: Procurement Plan (PPMP)

This document outlines the data structure for the Project Procurement Management Plan (PPMP) module
within the Purchase Request and Inventory System. It is designed to be compliant with **RA 12009
(The New Government Procurement Act)**.

---

### 1. Aggregate: `ProcurementPlan`

**Description:** Represents a single PPMP document as one aggregate. `ProcurementPlanHeader` is the
aggregate root; `ProcurementPlanItem` entries are owned children within the same lifecycle.

#### 1.1 Root: `ProcurementPlanHeader`

**Description:** Represents the metadata and approval state of a single PPMP document. This creates
the "envelope" for the individual items.

| Field Name | Data Type | Constraint | Description |
| :--- | :--- | :--- | :--- |
| **`id`** | `UUID` | Primary Key | Unique internal identifier. |
| **`controlNumber`** | `String` | Unique, Not Null | The official reference number (e.g., "PPMP-2024-ENG-001"). |
| **`fiscalYear`** | `Integer` | Not Null | The budget year (e.g., `2025`). |
| **`departmentId`** | `UUID` | Foreign Key | Reference to the Requesting Unit / Department. |
| **`departmentName`** | `String` | Read-Only | Denormalized name of the unit (e.g., "Engineering Office"). |
| **`status`** | `Enum` | Not Null | Workflow status (e.g., `DRAFT`, `PENDING_APPROVAL`, `APPROVED`). |
| **`isSupplemental`** | `Boolean` | Default: `False` | `True` if this is a Supplemental PPMP (addendum to original). |
| **`budgetType`** | `Enum` | Default: `INDICATIVE`| `INDICATIVE` (Appraisal) or `FINAL` (Approved Budget). |
| **`totalBudget`** | `Decimal` | Read-Only | Calculated sum of all item budgets. |
| **`preparedByUserId`**| `UUID` | Foreign Key | User who created the plan. |
| **`submissionDate`** | `DateTime` | Nullable | Timestamp when status changed to `PENDING_APPROVAL`. |
| **`approvedByUserId`**| `UUID` | Nullable | Head of Procuring Entity (HoPE) or authorized approver. |
| **`approvalDate`** | `DateTime` | Nullable | Timestamp when status changed to `APPROVED`. |

---

#### 1.2 Owned Entity: `ProcurementPlanItem`

**Description:** Represents a specific line item or lot within the plan. This acts as the
"Checklist" for future Purchase Requests; users cannot request items that do not exist here.

| Field Name | Data Type | Constraint | Description |
| :--- | :--- | :--- | :--- |
| **`id`** | `UUID` | Primary Key | Unique internal identifier. |
| **`planHeaderId`** | `UUID` | Foreign Key | Links to `ProcurementPlanHeader`. |
| **`papCode`** | `String` | Optional | Program/Activity/Project Code (internal budget code). |
| **`description`** | `Text` | Not Null | General description (e.g., "Procurement of Office Laptops"). |
| **`projectType`** | `Enum` | Not Null | `GOODS`, `INFRASTRUCTURE`, or `CONSULTING_SERVICES`. |
| **`quantity`** | `Integer` | Not Null | The total quantity approved. |
| **`unitOfMeasure`** | `String` | Not Null | e.g., "Units", "Lots", "Sets". |
| **`unitCost`** | `Decimal` | Not Null | Estimated cost per unit. |
| **`estimatedBudget`** | `Decimal` | Not Null | `quantity` * `unitCost` (Total Allocation). |
| **`mode`** | `Enum` | Not Null | See `ModeOfProcurement` Enum below. |
| **`isEarlyProcurement`**| `Boolean` | Default: `False` | If `True`, procurement starts before the fiscal year begins. |
| **`scheduleMonth`** | `String` | Not Null | Target month for PR submission (e.g., "MARCH"). |
| **`fundingSource`** | `String` | Not Null | e.g., "GAA", "Trust Fund", "Income". |
| **`remarks`** | `Text` | Optional | Specific requirements or notes. |

---

### 2. Aggregate: `ProcurementProject`

**Description:** Represents a procurement project derived from/linked to PPMP planning, modeled as a
single aggregate. `ProcurementProject` is the aggregate root; `ProcurementSchedule` and
`ProjectBudget` are owned parts of the aggregate (no independent lifecycle outside the project).

#### 2.1 Root: `ProcurementProject`

| Attribute | Data Type | Description |
| :--- | :--- | :--- |
| `project_id` | `UUID/Int` | Primary Key (Unique Identifier) |
| `pap_code` | `String` | Program/Activity/Project Code |
| `project_title` | `String` | Name of the procurement project |
| `pmo_end_user` | `String` | Department or Office responsible |
| `is_epa` | `Boolean` | Early Procurement Activity flag (Yes/No) |
| `mode_id` | `Integer` | Foreign Key to a "Modes of Procurement" lookup table |
| `fund_source` | `String` | Source of Funds (e.g., GAA, Trust Fund) |
| `remarks` | `Text` | Brief description or project notes |

#### 2.2 Owned Entity: `ProcurementSchedule`

| Attribute | Data Type | Description |
| :--- | :--- | :--- |
| `schedule_id` | `UUID/Int` | Primary Key |
| `project_id` | `UUID/Int` | Foreign Key (Links to ProcurementProject) |
| `ads_posting` | `Date` | Advertisement/Posting of IB/REI |
| `bid_opening` | `Date` | Submission/Opening of Bids |
| `notice_of_award` | `Date` | Date the NOA is issued |
| `contract_signing` | `Date` | Date the contract is signed |

#### 2.3 Owned Entity: `ProjectBudget`

| Attribute | Data Type | Description |
| :--- | :--- | :--- |
| `budget_id` | `UUID/Int` | Primary Key |
| `project_id` | `UUID/Int` | Foreign Key (Links to ProcurementProject) |
| `total_amount` | `Decimal(15,2)` | Sum of MOOE and CO |
| `mooe_amount` | `Decimal(15,2)` | Maintenance and Other Operating Expenses |
| `co_amount` | `Decimal(15,2)` | Capital Outlay |

## Development Workflow & Quality Gates

- **Feature work** MUST follow vertical slices (Command → Handler → Validator → Endpoint → Response)
	inside the relevant module.
- **API changes** that affect the Blazor client MUST include a regeneration step for the NSwag client.
- **EF Core migrations** MUST be created using the standard workflow:
	1. Change directory to `./api/server`:
		- `cd ./api/server`
	2. Create the migration (replace the quoted migration name every time):
		- `dotnet ef migrations add "Add Catalog Schema" --project .././migrations/postgresql/ --context CatalogDbContext -o Catalog`
- **Compliance-sensitive changes** (forms, RCA mapping, thresholds, valuation policy) MUST include:
	(1) a short impact note, (2) test updates, and (3) review by a domain/compliance-aware reviewer.
- **Release readiness** requires: build green, tests green (as applicable), migrations reviewed, and
	no unresolved TODOs related to compliance or data integrity.

## Governance

This constitution is the highest-level engineering governance for AMIS.

Amendment rules:
- Amendments MUST be made via PR, with an explicit rationale and a summary of behavioral impact.
- Versioning MUST follow semantic versioning (MAJOR.MINOR.PATCH):
	- MAJOR: backward-incompatible governance changes, principle removals, or redefinitions
	- MINOR: new principle/section or materially expanded guidance
	- PATCH: clarifications, wording, typo fixes, non-semantic refinements
- PR reviewers MUST verify the “Constitution Check” gates in feature plans are consistent with this
	document.
- When constitution changes, dependent templates under `.specify/templates/` MUST be reviewed and
	updated to match.

**Version**: 1.2.2 | **Ratified**: 2025-12-24 | **Last Amended**: 2025-12-28
