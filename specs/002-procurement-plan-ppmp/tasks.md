# Tasks: Procurement Plan (PPMP) Module

**Feature**: 002-procurement-plan-ppmp

## Phase 0 — Setup

- [X] T00 Confirm prerequisites script passes (`check-prerequisites.ps1 -RequireTasks -IncludeTasks`)
- [X] T01 Verify/extend ignore files for .NET solution (.gitignore present; no changes unless missing critical patterns)
- [X] T02 Locate module patterns (Todo/Catalog) and permission patterns (Shared/Authorization/FshPermissions.cs)

## Phase 1 — Tests (TDD-first where feasible)

- [ ] T10 Add unit tests for PPMP invariants (status transitions, totals, uniqueness behavior via domain services)
- [ ] T11 Add unit tests for Procurement Project budget invariant (total = mooe + co)

## Phase 2 — Domain (api/server/api/Procurement/Domain)

- [ ] T20 Create `Procurement` module folder structure under `api/server/api/Procurement/`
- [ ] T21 Implement enums: `ProcurementPlanStatus`, `BudgetType`, `ProjectType`
- [ ] T22 Implement aggregate `ProcurementPlanHeader` + owned entity `ProcurementPlanItem` with invariants and total recomputation
- [ ] T23 Implement aggregate `ProcurementProject` + owned `ProcurementSchedule` and `ProjectBudget` with invariants

## Phase 3 — Application (CQRS) (api/server/api/Procurement/Features)

- [ ] T30 Add DTOs/Responses for PPMP header + items and Procurement Project
- [ ] T31 Implement PPMP commands/handlers/validators:
  - Create header
  - Update header
  - Add item
  - Update item
  - Delete item
  - Submit
  - Approve
- [ ] T32 Implement PPMP queries/handlers (Get by id, Search/paged)
- [ ] T33 Implement Procurement Project commands/handlers/validators (Create, Update)
- [ ] T34 Implement Procurement Project queries/handlers (Get by id, Search/paged)

## Phase 4 — Infrastructure + Persistence (api/server/api/Procurement/Persistence)

- [ ] T40 Add EF Core entities/config for aggregates (table design + owned entities) and tenant/audit support
- [ ] T41 Add module DbContext or integrate into existing infrastructure DbContext (follow repo conventions)
- [ ] T42 Add repository registrations and keyed services (module isolation)
- [ ] T43 Add migrations for PostgreSQL (and MSSQL if required by repo conventions)

## Phase 5 — API Endpoints (Carter) (api/server/api/Procurement/Features)

- [ ] T50 Add Carter endpoints for PPMP v1 (routes + versioning + permissions + mediator wiring)
- [ ] T51 Add Carter endpoints for Procurement Projects v1
- [ ] T52 Register module services + endpoints in server startup (api/server/Extensions.cs + module entry file)
- [ ] T53 Add new permissions in Shared/Authorization/FshPermissions.cs

## Phase 6 — Blazor UI (WASM + MudBlazor)

- [ ] T60 Add navigation/menu entry for Procurement (matching existing nav patterns)
- [ ] T61 Add PPMP list page using EntityTable (search/paging) + create/edit actions
- [ ] T62 Add PPMP detail page to edit header + manage items, plus submit/approve actions
- [ ] T63 Add Procurement Projects list/detail pages (schedule + budget)
- [ ] T64 Regenerate/update NSwag client and wire new API calls in Blazor infrastructure

## Phase 7 — Validation + Polish

- [ ] T70 Build solution (Debug)
- [ ] T71 Run unit tests (Debug)
- [ ] T72 Quick manual smoke steps documented in quickstart + ensure no cross-tenant leakage in code paths
