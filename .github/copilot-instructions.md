# AMIS.9 Copilot Instructions

Asset Management Information System - A modular Clean Architecture .NET 9 application for tracking procurement, inspections, and asset lifecycle.

## Architecture Overview

Three-layer modular architecture with strict boundaries:
- **Framework** (`api/framework/`): Core domain primitives + Infrastructure cross-cutting concerns
- **Modules** (`api/modules/`): Self-contained business domains (Catalog, Todo) with Domain/Application/Infrastructure
- **Host** (`api/server/`): Bootstrap, DI wiring, module orchestration via `Program.cs` + `Extensions.cs`

## Adding Features (Vertical Slices)

Features follow CQRS in `Features/[Operation]/v[N]/` folders:

```
Features/Create/v1/
├── CreateCommand.cs         # IRequest<Response> - input model
├── CreateHandler.cs         # IRequestHandler - business logic
├── CreateEndpoint.cs        # Carter endpoint mapping
├── CreateValidator.cs       # FluentValidation rules
└── CreateResponse.cs        # Output DTO
```

**Example from Todo module** (`api/modules/Todo/Features/Create/v1/`):
```csharp
// Command
public record CreateTodoCommand(string Title, string Note) : IRequest<CreateTodoResponse>;

// Handler with keyed repository injection
public sealed class CreateTodoHandler(
    [FromKeyedServices("todo")] IRepository<TodoItem> repository)
    : IRequestHandler<CreateTodoCommand, CreateTodoResponse>
{
    public async Task<CreateTodoResponse> Handle(CreateTodoCommand request, CancellationToken ct)
    {
        var item = TodoItem.Create(request.Title, request.Note);
        await repository.AddAsync(item, ct);
        await repository.SaveChangesAsync(ct);
        return new CreateTodoResponse(item.Id);
    }
}

// Endpoint
endpoints.MapPost("/", async (CreateTodoCommand request, ISender mediator) => {
    var response = await mediator.Send(request);
    return Results.CreatedAtRoute(nameof(CreateTodoEndpoint), new { id = response.Id }, response);
})
.WithName(nameof(CreateTodoEndpoint))
.RequirePermission("Permissions.Todos.Create")
.MapToApiVersion(new ApiVersion(1, 0));
```

## Module Registration Pattern

Modules expose three extension methods in `[ModuleName]Module.cs`:

1. **RegisterModuleServices** - DI registration with keyed repositories:
```csharp
builder.Services.BindDbContext<TodoDbContext>();
builder.Services.AddKeyedScoped<IRepository<TodoItem>, TodoRepository<TodoItem>>("todo");
```

2. **Endpoints (Carter)** - Route mapping:
```csharp
public class Endpoints : CarterModule
{
    public Endpoints() : base("catalog") { }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("products").WithTags("products");
        group.MapProductCreationEndpoint();
    }
}
```

3. **UseModule** - Middleware/pipeline configuration (often empty)

Wire modules in `api/server/Extensions.cs`:
```csharp
// RegisterModules() - call RegisterCatalogServices(), RegisterTodoServices()
// UseModules() - configure versioning, call app.UseCatalogModule(), MapCarter()
```

## Database & Migrations

**Critical**: Migrations live in **separate projects** (`api/migrations/PostgreSQL`, `api/migrations/MSSQL`), not in module folders.

From `api/server/` directory:
```powershell
# Add migration
dotnet ef migrations add AddInspectionStatus --project ../migrations/PostgreSQL --context CatalogDbContext -o Catalog

# Apply
dotnet ef database update --project ../migrations/PostgreSQL --context CatalogDbContext
```

Primary database: PostgreSQL. Connection string in `api/server/appsettings.Development.json`.

## Keyed Services for Module Isolation

Repositories use keyed DI to prevent cross-module contamination:
```csharp
// Registration
builder.Services.AddKeyedScoped<IRepository<Product>, CatalogRepository<Product>>("catalog:products");

// Injection
public Handler([FromKeyedServices("catalog:products")] IRepository<Product> repo)
```

## Authorization & Permissions

Permission-based, defined in `Shared/Authorization/FshPermissions.cs`:
```csharp
new("Create Products", FshActions.Create, FshResources.Products),
```

Apply to endpoints:
```csharp
.RequirePermission("Permissions.Products.Create")
```

Three levels: Basic (IsBasic: true), Admin (default), Root (IsRoot: true).

## Business Context: Asset Procurement Workflow

System tracks end-to-end asset lifecycle (see `purchaesworkflow.md`):

1. **Purchase Request** → PR Approval
2. **Procurement** → PO Creation → Goods Delivery
3. **Receiving** → Inspection Request → QA Inspection → Acceptance
4. **Asset Tagging** → Inventory Recording → Assignment → Maintenance

Key status progressions:
- PO: `Draft` → `Issued` → `Received` → `Closed`
- Inspection: `Pending` → `In Progress` → `Complete`
- Inventory: `In-Stock` → `Assigned` → `Under Maintenance`

## Employee Registration Enforcement

**UI-based user onboarding** links Identity users to Employee records:

**Flow** (`Pages/Identity/Account/Profile.razor` & `.razor.cs`):
1. User logs in → navigates to Profile page
2. `LoadEmployeeProfileAsync()` checks for existing Employee record by UserId
3. If **no Employee record found**:
   - UI displays alert with registration prompt
   - Shows benefits: "Create purchase requests, Receive assignments, Access features"
   - "Register as Employee" button navigates to `/catalog/employees`
   
4. If **Employee record exists**:
   - Displays read-only Employee information card
   - Shows Name, Designation, Responsibility Code
   - "Edit Details" button for business field updates
   - Alert: "Basic contact information (name) is automatically synced from your login profile"

**Profile Update → Auto-Sync**:
```csharp
// When user updates Identity profile (FirstName, LastName, PhoneNumber)
private async Task UpdateProfileAsync()
{
    await PersonalClient.UpdateUserEndpointAsync(_profileModel);
    
    // If employee profile exists, sync basic contact info automatically
    if (_hasEmployeeProfile && _employeeProfile != null)
    {
        await SyncEmployeeBasicInfoAsync();  // Copies name from Identity to Employee
    }
}
```

**Backend Self-Registration** (`Catalog.Application/Employees/SelfRegister/v1/`):
```csharp
var employee = Employee.Create(
    request.Name,
    request.Designation,
    request.ResponsibilityCode,
    currentUser.GetUserId());  // Links Employee.UserId to Identity user
```

**Optional Middleware** (`Catalog.Infrastructure/Middleware/EmployeeRegistrationMiddleware.cs`):
- Available but currently **disabled** in `CatalogModule.UseCatalogModule()`
- Can enforce Employee record requirement by returning 403 for unregistered users
- Excluded paths: `/self-register`, `/tokens`, `/health`, `/identity`

## Blazor Client Integration

**Auto-generated API client** via NSwag:
1. Edit API endpoints
2. Run NSwag target: `dotnet build apps/blazor/infrastructure` (triggers NSwag via MSBuild target)
3. Client regenerated from OpenAPI spec at `apps/blazor/infrastructure/Api/`

UI uses **MudBlazor** with `MudTable` or `MudDataGrid` for data-heavy pages. JWT tokens auto-injected via `JwtAuthenticationHeaderHandler`.

## Development Commands

```powershell
# Build & Run
dotnet build AMIS.9.sln
dotnet run --project api/server                    # API at https://localhost:7000
dotnet run --project apps/blazor/client            # Blazor at https://localhost:7100
dotnet run --project aspire/Host                   # Aspire dashboard

# Migrations (see Migration Command.txt)
dotnet ef migrations add <Name> --project ../migrations/PostgreSQL --startup-project . --context CatalogDbContext -o Catalog
dotnet ef database update --project ../migrations/PostgreSQL --startup-project . --context CatalogDbContext

# Tests
dotnet test TestProject.XUnit/TestProject.XUnit.csproj
```

## Key Configuration

- **Packages**: Centralized in `Directory.Packages.props` (CPM enabled)
- **Settings**: `api/server/appsettings.Development.json` for connection strings, JWT, mail, CORS
- **Framework Bootstrap**: `Program.cs` calls `ConfigureFshFramework()` and `UseFshFramework()` from `api/framework/Infrastructure/Extensions.cs`

## Common Patterns to Follow

- **Endpoints**: Return `Results.Ok()`, `Results.CreatedAtRoute()`, `Results.NotFound()`
- **Versioning**: Always add `.MapToApiVersion(new ApiVersion(1, 0))`
- **Validation**: Create `[Operation]Validator.cs` with FluentValidation rules
- **Logging**: Inject `ILogger<THandler>` and log business events
- **Domain Events**: Use `DomainEvent` base class from Framework.Core
- **Specifications**: Use `Specification<T>` pattern from Framework.Core for complex queries

## Specifications Pattern (Ardalis.Specification)

Encapsulate query logic in reusable, testable spec classes:

```csharp
// Simple filter spec
public sealed class EmployeeByUserIdSpec : Specification<Employee>
{
    public EmployeeByUserIdSpec(Guid userId)
    {
        Query.Where(e => e.UserId == userId);
    }
}

// Complex spec with includes and filters
public sealed class GetPurchaseItemWithAcceptancesSpec : Specification<PurchaseItem>
{
    public GetPurchaseItemWithAcceptancesSpec(Guid purchaseItemId)
    {
        Query.Where(pi => pi.Id == purchaseItemId)
             .Include(pi => pi.AcceptanceItems)
                .ThenInclude(ai => ai.Acceptance)
             .Include(pi => pi.InspectionItems);
    }
}

// Usage in handlers
var employee = await repository.FirstOrDefaultAsync(new EmployeeByUserIdSpec(userId), ct);
```

## Domain Events

Events inherit from `DomainEvent` base class and use MediatR `INotification`:

```csharp
// Define event
public sealed record InspectionApproved : DomainEvent
{
    public Guid InspectionId { get; init; }
    public Guid ApprovedBy { get; init; }
}

// Raise event (typically in aggregate methods)
public void Approve(Guid approvedBy)
{
    Status = InspectionStatus.Approved;
    RaiseDomainEvent(new InspectionApproved 
    { 
        InspectionId = Id, 
        ApprovedBy = approvedBy 
    });
}

// Handle event (separate handler class)
public class InspectionApprovedHandler : INotificationHandler<InspectionApproved>
{
    public async Task Handle(InspectionApproved notification, CancellationToken ct)
    {
        // Side effects: create inventory transaction, send notification, etc.
    }
}
```

Examples in codebase: `InspectionApproved`, `InspectionRequestCreated`, `TodoItemCreated`

