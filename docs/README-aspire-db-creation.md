# Aspire + EF Core: Automatic Database Creation and Migrations

This README explains how AMIS.9 creates and updates databases automatically when running under Aspire (local orchestration with Postgres), and how you can apply the same pattern to Blazor Server apps.

## How It Works
- Orchestration: `aspire/Host/Program.cs` provisions a Postgres container and a logical database (e.g., `AMIS1120`) via `AddPostgres(...).AddDatabase(...)`.
- App Startup: The API calls `UseMultitenancy()` which invokes per-tenant initializers.
- Migrations: Each initializer calls `context.Database.Migrate()`/`MigrateAsync()`. EF Core creates the schema if it doesn't exist and applies any pending migrations.

Key code locations:
- Postgres and database: `aspire/Host/Program.cs`
- Startup pipeline: `api/framework/Infrastructure/Extensions.cs`
- Tenant migration flow: `api/framework/Infrastructure/Tenant/Extensions.cs`
- Module initializers: `api/modules/**/Persistence/*DbInitializer.cs`, `api/framework/Infrastructure/Identity/Persistence/IdentityDbInitializer.cs`

## Run Locally (Windows PowerShell)
```powershell
# From repo root
dotnet build d:\VB\AMIS.9\AMIS.9.sln --configuration Debug
# Start Aspire host (provisions DB, starts projects)
dotnet run --project d:\VB\AMIS.9\aspire\Host
```

Troubleshooting: If build fails with a locked `AMIS.Aspire.exe`, stop the running process in Task Manager and rerun the build/run.

## Customizing
- Change DB name/credentials in `aspire/Host/Program.cs` (e.g., `.AddDatabase("YourDbName")`).
- Add more infra (e.g., Redis, Grafana) via additional `builder.AddContainer(...)` lines.
- Control seeding/migration per module by editing the corresponding `*DbInitializer` class.

## Multi-Tenancy Notes
- The app sets current tenant and runs all `IDbInitializer` implementations per tenant.
- The master tenant store schema is also migrated on startup.
- For strategy options (single DB vs. DB-per-tenant), see `databaseimplementation.md`.

## Applying the Pattern to Blazor Server
Yes—this pattern applies to Blazor Server apps as well.

Minimal Blazor Server startup snippet to ensure migrations on app start:
```csharp
var builder = WebApplication.CreateBuilder(args);
// ... register DbContext, services, etc.
var app = builder.Build();

// Apply EF Core migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<YourDbContext>();
    db.Database.Migrate(); // creates DB/schema if missing, applies pending migrations
}

app.Run();
```

If hosting Blazor Server under Aspire:
- In your Aspire Host, add the Blazor Server project with `.WaitFor(database)` to ensure Postgres is ready before the server starts.
- Use the same `Migrate()` call in `Program.cs` to auto-create/update schema.

## See Also
- `multitenantimplementation.md` — architecture and multi-tenant workflow
- `databaseimplementation.md` — DB configuration, access patterns, single DB vs per-tenant
- `copilotinstruction.md` — brief Copilot-focused summary of DB creation under Docker/Aspire
