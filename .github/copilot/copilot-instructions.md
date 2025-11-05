# GitHub Copilot Instructions

## Priority Guidelines

When generating code for this repository:

1. Version Compatibility: Detect and respect the exact versions of languages, frameworks, and libraries used in this project.
2. Context Files: Prioritize any standards defined in `.github/copilot/` (this file) and, secondarily, `.github/copilot-instructions.md` at the repo root.
3. Codebase Patterns: When context files don’t cover a topic, scan the codebase and follow established patterns in similar files.
4. Architectural Consistency: Maintain the modular Clean Architecture (Layered + Domain-Driven) with strict module boundaries.
5. Code Quality: Prioritize maintainability, performance, security, and testability in all generated code, matching existing patterns.

## Technology Version Detection

Before you generate code, verify and constrain output to these detected versions:

- .NET SDK: 9.0.100 (from `global.json`)
- Target Framework: net9.0 (from `Directory.Build.props` and individual `.csproj` files)
- C# Language Version: Not explicitly set. Use the default for .NET 9 (no preview features). Do not require language features beyond the default compiler for .NET 9.
- Central Package Management: Enabled via `Directory.Packages.props`. Respect exact library versions below.

Key libraries and versions (non-exhaustive; from `Directory.Packages.props`):
- API/versioning/runtime
  - Asp.Versioning.Http 8.1.0
  - Asp.Versioning.Mvc.ApiExplorer 8.1.0
  - Microsoft.AspNetCore.* 9.0.x
  - Carter 9.0.0
  - MediatR 12.4.1
  - FluentValidation.DependencyInjectionExtensions 11.11.0
- Data
  - Microsoft.EntityFrameworkCore 9.0.2
  - Microsoft.EntityFrameworkCore.SqlServer 9.0.2
  - Npgsql.EntityFrameworkCore.PostgreSQL 9.0.3
  - Ardalis.Specification 8.0.0 (+ EFCore package)
- Auth/Multi-tenancy
  - Finbuckle.MultiTenant* 9.0.0
  - Microsoft.AspNetCore.Authentication.JwtBearer 9.0.2
- Observability/Logging
  - Serilog.* 9.0.0 (core Serilog 4.2.0; sinks/enrichers per props)
  - OpenTelemetry.* ~1.11.x
- Blazor client
  - MudBlazor 8.3.0
  - Blazored.FluentValidation (no explicit version in project, centrally managed if present)
- Testing
  - xUnit 2.9.3 (+ runner 3.0.2)
  - Microsoft.NET.Test.Sdk 17.13.0
  - Moq 4.20.72

Never use APIs not available in the detected versions above. If you need a capability that isn’t present, propose an alternative that matches what the codebase already does.

## Context Files

Prioritize this file. Also consult the existing high-level guide:
- `.github/copilot-instructions.md` — architecture overview, layering, CQRS, versioning patterns, DI, Blazor client integration, and development flows.

If other context files are added under `.github/copilot/` (architecture.md, tech-stack.md, coding-standards.md, folder-structure.md, exemplars.md), prioritize them in that order.

## Codebase Scanning Instructions

When guidance isn’t explicitly documented:

1. Locate the most similar file to the one you’re modifying or creating (by module, layer, and feature slice).
2. Analyze and mirror patterns for:
   - Naming conventions (namespaces reflect layer and module; feature folders use `Features/[Operation]/vN/`)
   - Route grouping and Carter modules
   - MediatR request/handler pairing
   - API versioning via `MapToApiVersion(new ApiVersion(1, 0))`
   - Permission checks via `.RequirePermission("Permissions.Resource.Action")`
   - Validation via FluentValidation
   - Repository usage via keyed services and EF DbContexts
   - Logging with `ILogger<T>` using structured logs
   - Testing style (xUnit naming, Moq for doubles)
3. Prefer newer code and code with tests when conflicts arise.
4. Do not introduce patterns that don’t exist in the repo.

## Architectural Guidance (observed)

- Style: Modular Clean Architecture (Layered + Domain-Driven) inside a modular monolith, with microservice-friendly patterns and Aspire for local orchestration.
- Layers:
  - Framework layer: `api/framework/` (Core + Infrastructure) for cross-cutting concerns, DI, logging, versioning, persistence, multi-tenancy, and shared endpoint infrastructure.
  - Modules: `api/modules/[ModuleName]/` with `Domain/`, `Application/`, and `Infrastructure/` where applicable.
  - Host: `api/server/` configures framework and registers modules.
  - Blazor Client: `apps/blazor/` (WASM) with `client/`, `infrastructure/`, `shared/`.
  - Aspire host: `aspire/Host/` orchestrates local services and apps.
- Feature Slices (CQRS): `Features/[Operation]/v[N]/` per module
  - Command: `[Operation]Command.cs` (implements `IRequest<TResponse>`)
  - Handler: `[Operation]Handler.cs` (implements `IRequestHandler<,>`)
  - Endpoint: `[Operation]Endpoint.cs` (maps route, permission, version)
  - Validator: `[Operation]Validator.cs` (FluentValidation)
  - Response: `[Operation]Response.cs` (typed output)

Example endpoint pattern (from Todo module):
```csharp
endpoints.MapPost("/", async (CreateTodoCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.CreatedAtRoute(nameof(CreateTodoEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateTodoEndpoint))
        .WithSummary("Creates a todo item")
        .WithDescription("Creates a todo item")
        .Produces<CreateTodoResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Todos.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
```

- API Versioning: Use `Asp.Versioning` conventions consistently.
  - Define version set in `api/server/Extensions.cs` via `NewApiVersionSet()`.
  - Map endpoints under `api/v{version:apiVersion}`.
  - Apply `MapToApiVersion(new ApiVersion(1, 0))` on endpoints.
- Authorization: Permission-based policy via `.RequirePermission("Permissions.Resource.Action")`.
  - Define permissions in `Shared/Authorization/FshPermissions.cs`.
- Dependency Injection:
  - Register module validators with `AddValidatorsFromAssemblies`.
  - Register MediatR handlers per module with `RegisterServicesFromAssemblies`.
  - Use keyed services to isolate repositories per module: `[FromKeyedServices("todo")] IRepository<TodoItem>`.
- Data Access:
  - EF Core DbContexts per module (see `TodoDbContext`, etc.).
  - Repository pattern via `IRepository<TEntity>` / `IReadRepository<TEntity>` with Ardalis.Specification.
  - Storage/backing providers for SQL Server and PostgreSQL are present; migrations in `api/migrations/`.
- Logging/Observability:
  - Serilog configured via infrastructure; use `ILogger<T>` in handlers and services for app-level logs.
  - OpenTelemetry packages present (respect existing wiring in infrastructure/host before adding new instrumentation).
- Blazor client:
  - `apps/blazor/client/` (WASM), shared models in `apps/blazor/shared/`, API client generation via NSwag target in `apps/blazor/infrastructure/`.
  - UI with MudBlazor and validation with Blazored.FluentValidation.

## Code Quality Standards

### Maintainability
- Favor small, focused handlers and endpoints following established CQRS slices.
- Keep naming aligned with namespaces (module + feature + version).
- Follow existing route grouping and Carter module structure (`CarterModule.AddRoutes`).
- Centralize DI wiring in module registration methods (`Register[Module]Services`) and in `api/server/Extensions.cs`.

### Performance
- Use async/await patterns as in handlers/endpoints; avoid blocking calls.
- Reuse scoped services through DI; avoid creating contexts or clients manually.
- Apply pagination and specification helpers (`Query.PaginateBy`, `Query.OrderBy`) as shown in existing specs.

### Security
- Enforce permission checks on endpoints with `.RequirePermission(...)` matching entries in `FshPermissions.cs`.
- Use built-in authentication mechanisms already wired (JWT Bearer) via infrastructure—don’t roll custom auth.
- Validate inputs with FluentValidation validators, placed next to the command.

### Testability
- Keep handlers injective (logger, repositories, services) to enable mocking in tests.
- Prefer interfaces and abstractions already present (e.g., repositories, services).
- Use xUnit + Moq patterns consistent with `TestProject.XUnit`.

## Documentation Requirements

- XML docs are enabled (`GenerateDocumentationFile` true). Follow the level and style present in current code.
- Document non-obvious behavior in code where existing patterns do so; otherwise prefer clear naming and small methods.

## Testing Approach

### Unit Testing
- Framework: xUnit
- Style: one test class per subject, `[Fact]` methods with descriptive names.
- Use Moq to create doubles for collaborators (e.g., `ILogger<T>`, repositories, client abstractions).
- Keep tests isolated; prefer in-memory or mock dependencies for unit scope.

### Integration Testing
- Use `Microsoft.AspNetCore.Mvc.Testing` when exercising API behaviors.
- For data, use EF InMemory where appropriate in integration-style tests contained in `TestProject.XUnit`.

## Technology-Specific Guidelines (.NET)

- Use only C# features compatible with .NET 9 default language level; avoid preview features unless a project explicitly opts in.
- Match existing async/await usage in endpoints and handlers.
- Keep LINQ/specification usage consistent with Ardalis.Specification helpers.
- Use the same collection and type choices evident in surrounding code.
- Follow existing DI patterns (module-level registration and host-level aggregation).

## General Best Practices

- Namespaces: `AMIS.Framework.*`, `AMIS.WebApi.*`, `AMIS.Blazor.*`, `AMIS.Shared.*` — keep consistent.
- Endpoints: Use Carter modules and route groups; return `Results.*` with typed `Produces<T>` and API versioning.
- Permissions: Always add/align permissions in `Shared/Authorization/FshPermissions.cs` when introducing new resources/actions.
- Configuration: Keep packages centrally versioned; add new dependencies via `Directory.Packages.props` unless exemption is justified.
- Logging: Use structured logging (`ILogger<T>`) with message templates (e.g., `logger.LogInformation("todo item created {TodoItemId}", item.Id);`).

## Project-Specific Guidance

- Respect module boundaries: never reference another module’s internals directly; interact via application-layer contracts.
- Follow the vertical slice structure for new features: `Command` → `Handler` → `Endpoint` → `Validator` → `Response`.
- Wire endpoints into the module’s `CarterModule.AddRoutes()`, and wire module services in `Register[Module]Services()`.
- Register the module (validators, MediatR handlers, Carter endpoints) in `api/server/Extensions.cs`.
- For the Blazor client, regenerate the API client via the NSwag target in `apps/blazor/infrastructure` when API endpoints change.

If a necessary pattern isn’t documented here but is present in the codebase, prefer consistency with the codebase over external best practices.
