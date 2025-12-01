# Copilot Instruction: Database Creation in Docker

## Summary
In the AMIS.9 project, database creation and schema migration in Docker are handled automatically by a combination of Aspire orchestration and application startup logic. No manual Docker entrypoint or migration script is required.

### How It Works
1. **Aspire Orchestration**
   - The Aspire host (`aspire/Host/Program.cs`) defines a Postgres container and ensures the logical database exists:
     ```csharp
     var database = builder.AddPostgres("db", username, password, port: 5432)
         .WithDataVolume()
         .AddDatabase("AMIS1120");
     ```

2. **Application Startup**
   - On startup, the application calls `app.UseMultitenancy()` (see `api/framework/Infrastructure/Extensions.cs`).
   - This triggers tenant initialization and migration logic.

3. **Tenant Initialization and Migration**
   - For each tenant, the system resolves all `IDbInitializer` implementations and calls their `MigrateAsync` and `SeedAsync` methods:
     ```csharp
     var initializers = tenantScope.ServiceProvider.GetServices<IDbInitializer>();
     foreach (var initializer in initializers)
     {
         initializer.MigrateAsync(CancellationToken.None).Wait();
         initializer.SeedAsync(CancellationToken.None).Wait();
     }
     ```
   - For the master tenant schema:
     ```csharp
     if (tenantDbContext.Database.GetPendingMigrations().Any())
     {
         tenantDbContext.Database.Migrate();
         Log.Information("applied database migrations for tenant module");
     }
     ```

4. **Module-Specific Migration**
   - Each module (e.g., Catalog) implements its own migration logic, e.g.:
     ```csharp
     public async Task MigrateAsync(CancellationToken cancellationToken)
     {
         if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
         {
             await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
         }
     }
     ```

### Result
- When the application starts in Docker, the Postgres server and logical database are created by Aspire.
- The application automatically creates the schema and applies migrations if they do not exist, using EF Core's migration APIs.
- No manual intervention is needed for database creation or migration in Docker environments.
