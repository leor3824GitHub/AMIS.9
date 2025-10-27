# AMIS.9 Copilot Instructions

## Architecture Overview

This is a modular Clean Architecture .NET 9 application with microservices patterns:

- **Framework Layer**: `api/framework/` contains Core and Infrastructure with cross-cutting concerns
- **Modules**: Self-contained business domains in `api/modules/` (Catalog, Todo) with Domain/Application/Infrastructure layers
- **Host**: `api/server/` bootstraps and configures the application
- **Blazor Client**: Full-stack web app in `apps/blazor/`
- **Aspire Orchestration**: Local development orchestration in `aspire/`

## Module Structure Pattern

Each module follows strict layering:
```
api/modules/[ModuleName]/
├── Domain/           # Entities, value objects, domain events
├── Application/      # Use cases, DTOs, interfaces
├── Infrastructure/   # Data access, external services
└── [ModuleName]Module.cs  # Registration and endpoints
```

## Feature Organization (Vertical Slices)

Features use CQRS with MediatR in `Features/[Operation]/v[N]/` structure:
- `[Operation]Command.cs` - Input model with IRequest<Response>
- `[Operation]Handler.cs` - Business logic with IRequestHandler
- `[Operation]Endpoint.cs` - HTTP endpoint mapping with Carter
- `[Operation]Validator.cs` - FluentValidation rules
- `[Operation]Response.cs` - Output model

Example: `Todo/Features/Create/v1/CreateTodoCommand.cs`

## Development Workflows

### Adding New Features
1. Create feature folder: `Features/[Operation]/v1/`
2. Implement Command → Handler → Endpoint → Validator
3. Register endpoint in Module's `CarterModule.AddRoutes()`
4. Add permissions to `Shared/Authorization/FshPermissions.cs`

### Module Registration
- Services: `RegisterModuleServices()` in module file
- Endpoints: Add to `Host/Extensions.cs` RegisterModules()
- Carter endpoints: Configure in module's Endpoints class

### Database Patterns
- Entity Framework with Repository pattern
- Keyed services for module isolation: `[FromKeyedServices("moduleName")]`
- Migrations in separate projects: `api/migrations/PostgreSQL|MSSQL`

## Key Conventions

### Authorization
- Permission-based with `RequirePermission("Permissions.Resource.Action")`
- Permissions defined in `FshPermissions.cs` with Basic/Admin/Root levels
- Multi-tenant with Finbuckle.MultiTenant

### API Versioning
- Use `MapToApiVersion(new ApiVersion(1, 0))` on endpoints
- Version sets configured in `Extensions.UseModules()`

### Dependency Injection
- Framework services in `FshInfrastructure.cs`
- Module services in each `RegisterModuleServices()`
- Keyed services for module boundaries

### Configuration
- Centralized package management in `Directory.Packages.props`
- Settings in `appsettings.json` with typed options
- Connection strings for PostgreSQL primary, MSSQL migrations

## API Architecture Details

### Endpoint Patterns
- **Carter** for minimal API endpoint mapping with fluent routing
- Endpoints return structured responses: `Results.CreatedAtRoute()`, `Results.Ok()`
- API versioning with `MapToApiVersion(new ApiVersion(1, 0))`
- Permission-based authorization: `RequirePermission("Permissions.Resource.Action")`

### Request/Response Flow
```csharp
// Example endpoint structure
endpoints.MapPost("/", async (CreateCommand request, ISender mediator) => {
    var response = await mediator.Send(request);
    return Results.CreatedAtRoute(nameof(Endpoint), new { id = response.Id }, response);
})
.WithName(nameof(Endpoint))
.RequirePermission("Permissions.Resource.Create")
.MapToApiVersion(new ApiVersion(1, 0));
```

### Database Context Patterns
- Multiple DbContexts per module with keyed services
- Repository pattern with `IRepository<TEntity>` and `IReadRepository<TEntity>`
- Migrations managed separately in `api/migrations/PostgreSQL|MSSQL`

## Blazor Client Architecture

### Project Structure
```
apps/blazor/
├── client/          # Blazor WebAssembly main app
├── infrastructure/ # API client, auth, services
├── shared/         # Shared components and models
└── nginx.conf      # Production deployment config
```

### Key Components
- **MudBlazor** for Material Design UI components
- **Auto-generated API Client** via NSwag from OpenAPI spec
- **JWT Authentication** with automatic token handling
- **Local Storage** for user preferences and caching

### API Integration
- Generated `IApiClient` interface from API OpenAPI spec
- Automatic JWT token injection via `JwtAuthenticationHeaderHandler`
- Centralized HTTP client configuration in `Extensions.cs`
- Type-safe API calls with full IntelliSense support

### UI Patterns
- **EntityTable** component for CRUD operations with pagination
- **PageHeader** for consistent page layouts
- **Form validation** with Blazored.FluentValidation
- **Theme management** with dark/light mode toggle

### Development Workflow
1. Update API endpoints → regenerate client via NSwag
2. Use `EntityTable<TEntity, TId, TRequest>` for standard CRUD pages
3. Implement page-specific logic in `.razor.cs` code-behind files

## Integration Points

- **Carter** for minimal API endpoints
- **MediatR** for CQRS command/query handling
- **FluentValidation** for input validation
- **Serilog** for structured logging
- **Hangfire** for background jobs
- **Entity Framework** with multi-database support
- **Aspire** for local service orchestration
- **NSwag** for API client code generation
- **MudBlazor** for Blazor UI components

## Testing Strategy
- XUnit with Moq for unit tests
- Blazor component testing with mock dependencies
- Test project structure matches feature organization

## Development Commands
- **Build**: `dotnet build AMIS.9.sln`
- **Run API**: `dotnet run --project api/server`
- **Run Blazor**: `dotnet run --project apps/blazor/client`
- **Run Aspire**: `dotnet run --project aspire/Host`
- **Migrations**: Use commands in `Migration Command.txt`
- **Regenerate API Client**: Use NSwag target in Blazor Infrastructure project