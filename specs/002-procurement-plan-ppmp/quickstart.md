# Quickstart: Procurement Plan (PPMP) Module

## Prerequisites

- .NET SDK version pinned by `global.json`
- Database configured via `api/server/appsettings*.json` (PostgreSQL primary)

## Run (API)

- Start API host:
  - `dotnet run --project api/server`

## Run (Blazor WASM)

- Start Blazor client:
  - `dotnet run --project apps/blazor/client`

## Development workflow

1. Implement API changes in a new `api/server/api/Procurement/` module (Domain/Features/Persistence).
2. Add module registration:
   - Add module assembly to `api/server/Extensions.cs` validators + MediatR scanning.
   - Register module services on builder (pattern: `RegisterTodoServices`).
   - Register Carter endpoints (pattern: `config.WithModule<TodoModule.Endpoints>()`).
3. Create migrations in the appropriate migrations project:
   - `api/migrations/PostgreSQL/Procurement/`
   - `api/migrations/MSSQL/Procurement/` (if required by repo conventions)
4. Regenerate NSwag client after API endpoints are stable:
   - Use repo’s NSwag workflow (see `AMIS.nswag`).
5. Implement Blazor pages:
   - Add `Pages/Procurement/ProcurementPlans.razor` and `Pages/Procurement/ProcurementProjects.razor` using MudBlazor + `EntityTable`.

## Manual verification checklist

- Create draft PPMP, add items, verify totals.
- Submit and approve PPMP, verify status timestamps and edit restrictions.
- Create Procurement Project and edit schedule/budget, validate totals.
- Verify tenant isolation (cannot access another tenant’s PPMP/project).
