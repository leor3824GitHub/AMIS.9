# Research: Procurement Plan (PPMP) Module

This document consolidates the key technical decisions for implementing the Procurement Plan (PPMP) domain, API, and Blazor UI.

## Decisions

### Decision: Create a new `Procurement` module
- **Chosen**: Add a new module under `api/server/api/Procurement/`.
- **Rationale**: Matches existing modular boundaries (Catalog, Todo). Keeps procurement planning logic isolated and testable.
- **Alternatives considered**:
  - Add PPMP to `Catalog` (rejected: mixes master-data with transactional procurement planning and approval workflows).

### Decision: Model PPMP as a single aggregate (`ProcurementPlan`)
- **Chosen**: `ProcurementPlanHeader` (root) + owned `ProcurementPlanItem` children.
- **Rationale**: Matches constitution and enforces lifecycle rules (status transitions, total recalculation) at the aggregate boundary.
- **Alternatives considered**:
  - Separate header and items as independent entities (rejected: weakens invariants; increases risk of orphan/partial updates).

### Decision: Model Procurement Project as a single aggregate (`ProcurementProject`)
- **Chosen**: `ProcurementProject` (root) with owned `ProcurementSchedule` and `ProjectBudget`.
- **Rationale**: Ensures schedule/budget updates remain consistent and auditable.
- **Alternatives considered**:
  - Separate schedule/budget tables with independent lifecycle (rejected: encourages partial updates and complex coordination).

### Decision: Use existing stack patterns for endpoints and validation
- **Chosen**: Carter endpoints + MediatR request handlers + FluentValidation.
- **Rationale**: Consistent with existing modules and supports versioning/permissions.
- **Alternatives considered**:
  - Controllers (rejected: repo standard uses Carter minimal API endpoints).

### Decision: UI implementation uses MudBlazor + existing `EntityTable`
- **Chosen**: PPMP list/detail pages and Procurement Project list/detail pages in Blazor WASM, using `EntityTable` where suitable.
- **Rationale**: Reduces custom UI, leverages existing table CRUD and permission handling.
- **Alternatives considered**:
  - Fully custom components for PPMP item grids (deferred: only introduce if `EntityTable` is insufficient for nested items).

## Open Questions (NEEDS CLARIFICATION)

1. **Permissions naming**: Should PPMP permissions be `Permissions.ProcurementPlans.*` or `Permissions.Procurement.*`?
2. **ID types**: Constitution includes `UUID/Int` for some fields; implementation should likely standardize to `Guid` for new entities. Confirm.
3. **Mode of Procurement lookup**: Is there an existing lookup entity/table for modes, or should the module define it?
4. **Supplemental PPMP**: How should linking to an "original" PPMP be modeled (reference field, versioning, or separate workflow)?
