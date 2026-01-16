# Implementation Plan: Procurement Plan (PPMP) Module

**Branch**: `002-procurement-plan-ppmp` | **Date**: 2025-12-27 | **Spec**: ../002-procurement-plan-ppmp/spec.md
**Input**: Feature specification from `/specs/002-procurement-plan-ppmp/spec.md`

## Summary

Implement a Procurement Plan (PPMP) module compliant with RA 12009, including:
- PPMP domain aggregates (`ProcurementPlan` and `ProcurementProject`) per constitution
- Versioned Web API endpoints (Carter + MediatR + FluentValidation), tenant-isolated
- Blazor WebAssembly UI using MudBlazor patterns (EntityTable + dialogs) consuming generated API client

## Technical Context

<!--
  ACTION REQUIRED: Replace the content in this section with the technical details
  for the project. The structure here is presented in advisory capacity to guide
  the iteration process.
-->

**Language/Version**: .NET 9 / C#  
**Primary Dependencies**: Carter (Minimal APIs), MediatR (CQRS), FluentValidation, EF Core, Finbuckle.MultiTenant, MudBlazor (Blazor WASM)  
**Storage**: PostgreSQL primary (EF Core); MSSQL migrations/testing scenarios exist  
**Testing**: xUnit (TestProject.XUnit) + module-level unit/integration tests as needed  
**Target Platform**: ASP.NET Core Web API + Blazor WebAssembly (browser)
**Project Type**: Modular Clean Architecture (framework + modules)  
**Performance Goals**: Responsive CRUD and search flows for PPMPs and projects (interactive UI; no heavy batch workloads)  
**Constraints**: RA 12009 / PPMP governance, auditability, strict tenant isolation, permission-based authorization, backward-compatible APIs for NSwag clients  
**Scale/Scope**: Department-scale usage; expected hundreds–thousands of PPMP line items per tenant per fiscal year

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- [x] Compliance impact assessed (COA/DBM/PPSAS/RCA, prescribed forms)
- [x] Procurement planning impact assessed when applicable (RA 12009, PPMP alignment, PR must map to PPMP item)
- [x] Clean Architecture boundaries respected (Domain/Application/Infrastructure)
- [x] Multi-tenant isolation preserved (tenant context + permission checks)
- [x] Cloud readiness considered (config, logging; no secrets)
- [x] Test plan included for behavioral changes (or justification documented)

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)
```text
AMIS.9.sln
api/
  framework/
    Core/
    Infrastructure/
  modules/
    [Module]/
      Domain/
      Application/
      Infrastructure/
      Features/
  server/
apps/
  blazor/
    client/
    infrastructure/
    shared/
aspire/
  Host/
Shared/
TestProject.XUnit/
```

**Structure Decision**: [Document the selected structure and reference the real
directories captured above]

**Structure Decision**: Implement as a new module under `api/server/api/Procurement/` following existing module patterns:
- `Domain/` for aggregates and invariants
- `Application/` for commands/queries and DTOs
- `Infrastructure/` for persistence (DbContext, repos) and endpoint wiring
- `Features/[Operation]/v1/` vertical slices for PPMP and Procurement Project endpoints

Blazor pages live under `apps/blazor/client/Pages/Procurement/` and consume the generated client in `apps/blazor/infrastructure/`.

## Implementation Phases (High Level)

### Phase 0: Research & Decisions

- Confirm module naming, schema naming, and permission naming strategy
- Confirm whether IDs are standardized to UUID/Guid across procurement entities
- Confirm API route structure and client generation strategy

### Phase 1: Design & Contracts

- Define entity models and invariants for `ProcurementPlan` and `ProcurementProject` aggregates
- Define API contracts (OpenAPI) for CRUD + workflow actions (submit/approve)
- Define Blazor UI routes/pages aligned with existing EntityTable patterns

### Phase 2: Implementation Planning

- Outline vertical-slice implementation tasks and test coverage strategy (generated later by `/speckit.tasks`)

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., 4th project] | [current need] | [why 3 projects insufficient] |
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |
