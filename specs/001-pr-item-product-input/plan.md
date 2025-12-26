# Implementation Plan: Purchase Request Item Product Input

**Branch**: `001-pr-item-product-input` | **Date**: 2025-12-24 | **Spec**: `specs/001-pr-item-product-input/spec.md`
**Input**: Feature specification for supporting PR item product input via either Product library selection or manual entry.

## Summary

Enable purchase request items to be encoded using either:

1) a linked product (`ProductId`) from the product library, or
2) a manually entered product name (`ManualProductName`) when the library does not yet contain the needed item.

Approach:
- Add `ManualProductName` as an additive, nullable field.
- Enforce `ProductId XOR ManualProductName` (exactly one) at validation and domain invariant levels.
- Update Blazor UI to let users switch between “Select from product list” and “Manual entry”.
- Regenerate NSwag client after API contract changes.

Design artifacts:
- `research.md` (decisions)
- `data-model.md` (entity/DTO updates)
- `contracts/openapi.yaml` (contract excerpt)
- `quickstart.md` (verification)

## Technical Context

**Language/Version**: .NET 9 / C#  
**Primary Dependencies**: Carter (Minimal APIs), MediatR (CQRS), FluentValidation, EF Core, Finbuckle.MultiTenant  
**Storage**: PostgreSQL primary (EF Core); MSSQL migrations/testing scenarios exist  
**Testing**: xUnit (TestProject.XUnit)  
**Target Platform**: Server (ASP.NET Core) + Blazor WebAssembly client  
**Project Type**: Modular Clean Architecture (framework + modules)  
**Performance Goals**: N/A (bounded CRUD change)  
**Constraints**: Multi-tenant isolation; permission-based authorization; backward compatible API changes; auditability  
**Scale/Scope**: Localized change to Purchase Request items pipeline + UI component

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- [x] Compliance impact assessed (no prescribed form changes; auditability preserved)
- [x] Clean Architecture boundaries respected (Domain/Application/Infrastructure; UI remains client-only)
- [x] Multi-tenant isolation preserved (no cross-tenant reads/writes introduced)
- [x] Cloud readiness considered (additive API contract; no secrets; stable DTO evolution)
- [x] Test plan included for behavioral changes (XOR validation + mode switching)

## Project Structure

### Documentation (this feature)

```text
specs/001-pr-item-product-input/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── openapi.yaml
└── checklists/
    └── requirements.md
```

### Source Code (repository root)

```text
AMIS.9.sln
api/
  framework/
  modules/
    Catalog/
      Catalog.Domain/
      Catalog.Application/
      Catalog.Infrastructure/
  server/
apps/
  blazor/
    client/
    infrastructure/
Shared/
TestProject.XUnit/
```

**Structure Decision**: Implement changes inside the Catalog module (Domain + Application + Infrastructure) and update the Blazor client component that edits PR items. Avoid introducing new modules or cross-module dependencies.

## Complexity Tracking

No constitution violations introduced.
