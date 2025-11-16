# Asset Management Information System

A modular Clean Architecture .NET 9 application with vertical-slice features, CQRS/MediatR, Blazor WebAssembly client, and local orchestration via .NET Aspire.

## Overview

- Backend API: Minimal APIs with Carter, MediatR, FluentValidation, EF Core, Serilog, Hangfire
- Architecture: Clean Architecture with Framework layer + self-contained business Modules
- Client: Blazor WebAssembly with MudBlazor, generated API client via NSwag
- Data: EF Core with PostgreSQL (primary) and MSSQL (for migrations/testing scenarios)
- Orchestration: .NET Aspire for local multi-service development

## Repository layout

```
AMIS.9.sln
api/
  framework/              # Core + Infrastructure (cross-cutting)
  modules/                # Business modules (Catalog, Todo, ...)
  migrations/             # EF Core migrations projects (PostgreSQL, MSSQL)
  server/                 # API host (startup, DI wiring, endpoints)
apps/
  blazor/                 # Blazor WebAssembly app, infrastructure, shared
aspire/
  Host/                   # Aspire orchestration host
service-defaults/         # Shared service defaults for Aspire
Shared/                   # Shared authorization/resources
```

## Architecture highlights

- Framework layer (`api/framework`)
  - Core: domain primitives, exceptions, specs, paging, storage abstractions
  - Infrastructure: logging, caching, auth, persistence, OpenAPI, security headers, etc.
- Modules (`api/modules/[Module]`)
  - Domain: entities, value objects, domain events
  - Application: commands/queries, DTOs, validators, handlers (CQRS with MediatR)
  - Infrastructure: persistence configs and external integrations
  - Module file: registration and Carter endpoints
- Host (`api/server`)
  - Bootstraps services, registers modules, configures versioning, permissions, OpenAPI
- Client (`apps/blazor`)
  - MudBlazor UI patterns, generated `IApiClient`, JWT auth handler, shared components

### Feature slices

Each feature is organized as vertical slices under `Features/[Operation]/vN` with:
- Command/Query (IRequest<T>)
- Handler
- Validator
- Endpoint (Carter)
- Response DTO

## Prerequisites

- .NET 9 SDK
- Node.js (for some web tooling, if needed)
- PostgreSQL (primary local database)
- Optional: MSSQL (if running alternative migrations)

## Configuration

- API settings: `api/server/appsettings.Development.json`
- Connection strings (PostgreSQL by default). Ensure your Postgres instance is reachable and credentials are correct.
- Centralized package versions in `Directory.Packages.props`

## Quick start

- Build solution: dotnet build AMIS.9.sln
- Run API: dotnet run --project api/server
- Run Blazor client: dotnet run --project apps/blazor/client
- Aspire (optional): dotnet run --project aspire/Host

If you use Aspire, it can wire up dependent services and provide dashboards locally.

## Database and migrations

EF Core migrations are maintained in separate projects under `api/migrations`.

- PostgreSQL migrations (primary): `api/migrations/PostgreSQL`
  - Catalog schema folder: `api/migrations/PostgreSQL/Catalog`
- MSSQL migrations (optional): `api/migrations/MSSQL`

Typical commands for CatalogDbContext with PostgreSQL (run from repo root):

- Create migration (output to Catalog folder):
  - dotnet ef migrations add <Name> --project api/migrations/PostgreSQL --startup-project api/server --context CatalogDbContext -o Catalog
- Apply latest migration:
  - dotnet ef database update --project api/migrations/PostgreSQL --startup-project api/server --context CatalogDbContext

Note: Replace `<Name>` with a descriptive label (e.g., `AddInspectionAndAcceptanceStatuses`).

## Generated API client (NSwag)

- The Blazor app consumes a generated `IApiClient` from the API OpenAPI spec.
- Regenerate client after API changes using the NSwag setup in the Blazor infrastructure project (see `AMIS.nswag` and project targets).
- After regeneration, rebuild the Blazor client to pick up changes.

## Authorization and permissions

- Permission-based: `Shared/Authorization/FshPermissions.cs`
- Use `.RequirePermission("Permissions.Resource.Action")` on endpoints
- Multi-tenant support via Finbuckle.MultiTenant

## UI patterns (Blazor)

- `EntityTable` for CRUD lists with paging
- `PageHeader` for consistent layout
- Fluent validation in forms
- Theme: MudBlazor (light/dark)

## Development workflow

1. Add/modify API endpoints in a module (Domain → Application → Endpoint)
2. Create and apply EF migrations (PostgreSQL)
3. Regenerate the API client (NSwag)
4. Build and run the Blazor client
5. Add tests where appropriate (XUnit)

## Testing

- Test project: `TestProject.XUnit/`
- Run tests via `dotnet test` at the solution level

## Notes for Postgres users

- This repository prioritizes PostgreSQL as the primary database. Ensure your connection string in `api/server/appsettings.Development.json` points to your Postgres instance.
- Existing migrations (e.g., `20251025165044_AddInspectionAndAcceptanceStatuses`) are authored for PostgreSQL and can be applied via the commands above.

## Contributing

- Follow the vertical-slice organization for new features
- Prefer small, focused PRs per feature
- Keep migrations per module and database separate and descriptive
- Update this README when adding major capabilities

---

If you get stuck or see inconsistencies, check `.github/copilot-instructions.md` for project-specific guidance and architecture conventions.

## Employee Registration Enforcement Flow

- User logs in → Identity system authenticates
- User attempts to access any API endpoint
- Middleware checks if `Employee.UserId` matches authenticated user's ID
- If not found → Returns 403 with message to complete registration
- User calls `/self-register` endpoint with employee details
- Employee record created with `UserId` linking to Identity
- Subsequent requests pass through middleware successfully
