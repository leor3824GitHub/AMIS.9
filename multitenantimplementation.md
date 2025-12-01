

# Multi-Tenant Implementation in AMIS.9

## Overview
This document focuses on the architectural and workflow details of multi-tenancy in AMIS.9, including tenant discovery, context setting, and per-tenant migration/seeding. For database configuration and multi-tenancy strategy options, see [databaseimplementation.md](databaseimplementation.md).

## Key Components

- **Tenant Context**: Managed via `IMultiTenantContextSetter` and `FshTenantInfo`.
- **Tenant Database Context**: Each tenant uses a separate `TenantDbContext` with its own connection string.
- **IDbInitializer**: Interface for database migration and seeding logic, implemented per module (e.g., Catalog, Todo, Identity).
- **Tenant Initialization**: On application startup, the system iterates through tenants, sets the context, and applies migrations/seeding.

## How It Works

1. **Tenant Discovery**
   - Tenants are loaded and iterated during application startup.

2. **Context Setting**
   - For each tenant, the current context is set using `IMultiTenantContextSetter` and a new `MultiTenantContext<FshTenantInfo>`.

3. **Database Migration & Seeding**
   - For each tenant, all registered `IDbInitializer` services are resolved and their `MigrateAsync` and `SeedAsync` methods are called.
   - This ensures each tenant's database is created (if missing) and up-to-date with the latest schema and seed data.
   - The master tenant schema is also checked for pending migrations and updated.

4. **Module Isolation**
   - Each module (e.g., Catalog, Todo) registers its own `IDbInitializer` implementation, ensuring migrations and seeding are modular and isolated per domain.

## Full Code Samples

See the following code samples for implementation details. For database configuration and strategy options, refer to [databaseimplementation.md](databaseimplementation.md#multi-tenancy-database-access-options).

### 1. Tenant Extensions (api/framework/Infrastructure/Tenant/Extensions.cs)
```csharp
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.Stores.DistributedCacheStore;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Tenant.Abstractions;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.Framework.Infrastructure.Persistence.Services;
using AMIS.Framework.Infrastructure.Tenant.Persistence;
using AMIS.Framework.Infrastructure.Tenant.Services;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AMIS.Framework.Infrastructure.Tenant;
internal static class Extensions
{
   public static IServiceCollection ConfigureMultitenancy(this IServiceCollection services)
   {
      ArgumentNullException.ThrowIfNull(services);
      services.AddTransient<IConnectionStringValidator, ConnectionStringValidator>();
      services.BindDbContext<TenantDbContext>();
      services
         .AddMultiTenant<FshTenantInfo>(config =>
         {
            // to save database calls to resolve tenant
            // this was happening for every request earlier, leading to ineffeciency
            config.Events.OnTenantResolveCompleted = async (context) =>
            {
               if (context.MultiTenantContext.StoreInfo is null) return;
               if (context.MultiTenantContext.StoreInfo.StoreType != typeof(DistributedCacheStore<FshTenantInfo>))
               {
                  var sp = ((HttpContext)context.Context!).RequestServices;
                  var distributedCacheStore = sp
                     .GetService<IEnumerable<IMultiTenantStore<FshTenantInfo>>>()!
                     .FirstOrDefault(s => s.GetType() == typeof(DistributedCacheStore<FshTenantInfo>));

                  await distributedCacheStore!.TryAddAsync(context.MultiTenantContext.TenantInfo!);
               }
               await Task.FromResult(0);
            };
         })
         .WithClaimStrategy(FshClaims.Tenant)
         .WithHeaderStrategy(TenantConstants.Identifier)
         .WithDelegateStrategy(async context =>
         {
            if (context is not HttpContext httpContext)
               return null;
            if (!httpContext.Request.Query.TryGetValue("tenant", out var tenantIdentifier) || string.IsNullOrEmpty(tenantIdentifier))
               return null;
            return await Task.FromResult(tenantIdentifier.ToString());
         })
         .WithDistributedCacheStore(TimeSpan.FromMinutes(60))
         .WithEFCoreStore<TenantDbContext, FshTenantInfo>();
      services.AddScoped<ITenantService, TenantService>();
      return services;
   }

   public static WebApplication UseMultitenancy(this WebApplication app)
   {
      ArgumentNullException.ThrowIfNull(app);
      app.UseMultiTenant();

      // set up tenant store
      var tenants = TenantStoreSetup(app);

      // set up tenant databases
      app.SetupTenantDatabases(tenants);

      return app;
   }

   private static IApplicationBuilder SetupTenantDatabases(this IApplicationBuilder app, IEnumerable<FshTenantInfo> tenants)
   {
      foreach (var tenant in tenants)
      {
         // create a scope for tenant
         using var tenantScope = app.ApplicationServices.CreateScope();

         //set current tenant so that the right connection string is used
         tenantScope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
            .MultiTenantContext = new MultiTenantContext<FshTenantInfo>()
            {
               TenantInfo = tenant
            };

         // using the scope, perform migrations / seeding
         var initializers = tenantScope.ServiceProvider.GetServices<IDbInitializer>();
         foreach (var initializer in initializers)
         {
            initializer.MigrateAsync(CancellationToken.None).Wait();
            initializer.SeedAsync(CancellationToken.None).Wait();
         }
      }
      return app;
   }

   private static IEnumerable<FshTenantInfo> TenantStoreSetup(IApplicationBuilder app)
   {
      var scope = app.ApplicationServices.CreateScope();

      // tenant master schema migration
      var tenantDbContext = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
      if (tenantDbContext.Database.GetPendingMigrations().Any())
      {
         tenantDbContext.Database.Migrate();
         Log.Information("applied database migrations for tenant module");
      }

      // default tenant seeding
      if (tenantDbContext.TenantInfo.Find(TenantConstants.Root.Id) is null)
      {
         var rootTenant = new FshTenantInfo(
            TenantConstants.Root.Id,
            TenantConstants.Root.Name,
            string.Empty,
            TenantConstants.Root.EmailAddress);

         rootTenant.SetValidity(DateTime.UtcNow.AddYears(1));
         tenantDbContext.TenantInfo.Add(rootTenant);
         tenantDbContext.SaveChanges();
         Log.Information("configured default tenant data");
      }

      // get all tenants from store
      var tenantStore = scope.ServiceProvider.GetRequiredService<IMultiTenantStore<FshTenantInfo>>();
      var tenants = tenantStore.GetAllAsync().Result;

      //dispose scope
      scope.Dispose();

      return tenants;
   }
}
```

### 2. Tenant Service (api/framework/Infrastructure/Tenant/Services/TenantService.cs)
```csharp
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Tenant.Abstractions;
using AMIS.Framework.Core.Tenant.Dtos;
using AMIS.Framework.Core.Tenant.Features.CreateTenant;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AMIS.Framework.Infrastructure.Tenant.Services;

public sealed class TenantService : ITenantService
{
   private readonly IMultiTenantStore<FshTenantInfo> _tenantStore;
   private readonly DatabaseOptions _config;
   private readonly IServiceProvider _serviceProvider;

   public TenantService(IMultiTenantStore<FshTenantInfo> tenantStore, IOptions<DatabaseOptions> config, IServiceProvider serviceProvider)
   {
      _tenantStore = tenantStore;
      _config = config.Value;
      _serviceProvider = serviceProvider;
   }

   public async Task<string> ActivateAsync(string id, CancellationToken cancellationToken)
   {
      var tenant = await GetTenantInfoAsync(id).ConfigureAwait(false);

      if (tenant.IsActive)
      {
         throw new FshException($"tenant {id} is already activated");
      }

      tenant.Activate();

      await _tenantStore.TryUpdateAsync(tenant).ConfigureAwait(false);

      return $"tenant {id} is now activated";
   }

   public async Task<string> CreateAsync(CreateTenantCommand request, CancellationToken cancellationToken)
   {
      var connectionString = request.ConnectionString;
      if (request.ConnectionString?.Trim() == _config.ConnectionString.Trim())
      {
         connectionString = string.Empty;
      }

      FshTenantInfo tenant = new(request.Id, request.Name, connectionString, request.AdminEmail, request.Issuer);
      await _tenantStore.TryAddAsync(tenant).ConfigureAwait(false);

      await InitializeDatabase(tenant).ConfigureAwait(false);

      return tenant.Id;
   }

   private async Task InitializeDatabase(FshTenantInfo tenant)
   {
      // First create a new scope
      using var scope = _serviceProvider.CreateScope();

      // Then set current tenant so the right connection string is used
      scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
         .MultiTenantContext = new MultiTenantContext<FshTenantInfo>()
         {
            TenantInfo = tenant
         };

      // using the scope, perform migrations / seeding
      var initializers = scope.ServiceProvider.GetServices<IDbInitializer>();
      foreach (var initializer in initializers)
      {
         await initializer.MigrateAsync(CancellationToken.None).ConfigureAwait(false);
         await initializer.SeedAsync(CancellationToken.None).ConfigureAwait(false);
      }
   }

   public async Task<string> DeactivateAsync(string id)
   {
      var tenant = await GetTenantInfoAsync(id).ConfigureAwait(false);
      if (!tenant.IsActive)
      {
         throw new FshException($"tenant {id} is already deactivated");
      }

      tenant.Deactivate();
      await _tenantStore.TryUpdateAsync(tenant).ConfigureAwait(false);
      return $"tenant {id} is now deactivated";
   }

   public async Task<bool> ExistsWithIdAsync(string id) =>
      await _tenantStore.TryGetAsync(id).ConfigureAwait(false) is not null;

   public async Task<bool> ExistsWithNameAsync(string name) =>
      (await _tenantStore.GetAllAsync().ConfigureAwait(false)).Any(t => t.Name == name);

   public async Task<List<TenantDetail>> GetAllAsync()
   {
      var tenants = (await _tenantStore.GetAllAsync().ConfigureAwait(false)).Adapt<List<TenantDetail>>();
      return tenants;
   }

   public async Task<TenantDetail> GetByIdAsync(string id) =>
      (await GetTenantInfoAsync(id).ConfigureAwait(false))
         .Adapt<TenantDetail>();

   public async Task<DateTime> UpgradeSubscription(string id, DateTime extendedExpiryDate)
   {
      var tenant = await GetTenantInfoAsync(id).ConfigureAwait(false);
      tenant.SetValidity(extendedExpiryDate);
      await _tenantStore.TryUpdateAsync(tenant).ConfigureAwait(false);
      return tenant.ValidUpto;
   }

   private async Task<FshTenantInfo> GetTenantInfoAsync(string id) =>
   await _tenantStore.TryGetAsync(id).ConfigureAwait(false)
      ?? throw new NotFoundException($"{typeof(FshTenantInfo).Name} {id} Not Found.");
}
```

### 3. IDbInitializer Interface (api/framework/Core/Persistence/IDbInitializer.cs)
```csharp
namespace AMIS.Framework.Core.Persistence;
public interface IDbInitializer
{
   Task MigrateAsync(CancellationToken cancellationToken);
   Task SeedAsync(CancellationToken cancellationToken);
}
```

### 4. CatalogDbInitializer (api/modules/Catalog/Catalog.Infrastructure/Persistence/CatalogDbInitializer.cs)
```csharp
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence;

internal sealed class CatalogDbInitializer(
   ILogger<CatalogDbInitializer> logger,
   CatalogDbContext context) : IDbInitializer
{
   public async Task MigrateAsync(CancellationToken cancellationToken)
   {
      if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
      {
         await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
         logger.LogInformation("[{Tenant}] applied database migrations for catalog module", context.TenantInfo!.Identifier);
      }
   }

   public async Task SeedAsync(CancellationToken cancellationToken)
   {
      const string Name = "Keychron V6 QMK Custom Wired Mechanical Keyboard";
      const string Description = "A full-size layout QMK/VIA custom mechanical keyboard";
      const decimal Price = 79;
      Guid? CategoryId = null;
      const string Unit = "pc";
      if (await context.Products.FirstOrDefaultAsync(t => t.Name == Name, cancellationToken).ConfigureAwait(false) is null)
      {
         var product = Product.Create(Name, Description, Price, Unit, null, CategoryId);
         await context.Products.AddAsync(product, cancellationToken);
         await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
         logger.LogInformation("[{Tenant}] seeding default catalog data", context.TenantInfo!.Identifier);
      }

      // Add more seed data here
      // Seed suppliers (5)
      for (var i = 1; i <= 5; i++)
      {
         var sName = $"Supplier {i}";
         if (await context.Suppliers.FirstOrDefaultAsync(t => t.Name == sName, cancellationToken).ConfigureAwait(false) is null)
         {
            var supplier = Supplier.Create(sName, $"Address {i}", $"TIN{i:000}", "VAT", $"0917-000-00{i}", $"supplier{i}@example.com");
            await context.Suppliers.AddAsync(supplier, cancellationToken).ConfigureAwait(false);
         }
      }
      await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      logger.LogInformation("[{Tenant}] seeded suppliers", context.TenantInfo!.Identifier);

      // Seed categories (5)
      var seededCategories = new List<Category>();
      for (var i = 1; i <= 5; i++)
      {
         var cName = $"Category {i}";
         if (await context.Categories.FirstOrDefaultAsync(t => t.Name == cName, cancellationToken).ConfigureAwait(false) is null)
         {
            var category = Category.Create(cName, $"Default category {i}");
            await context.Categories.AddAsync(category, cancellationToken).ConfigureAwait(false);
            seededCategories.Add(category);
         }
         else
         {
            var existing = await context.Categories.FirstOrDefaultAsync(t => t.Name == cName, cancellationToken).ConfigureAwait(false);
            if (existing is not null) seededCategories.Add(existing);
         }
      }
      await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      // refresh seededCategories with persisted IDs
      seededCategories = await context.Categories.Where(c => seededCategories.Select(sc => sc.Name).Contains(c.Name)).ToListAsync(cancellationToken).ConfigureAwait(false);
      logger.LogInformation("[{Tenant}] seeded categories", context.TenantInfo!.Identifier);

      // Seed products (5) and associate them with categories in round-robin
      for (var i = 1; i <= 5; i++)
      {
         var pName = $"Seed Product {i}";
         if (await context.Products.FirstOrDefaultAsync(t => t.Name == pName, cancellationToken).ConfigureAwait(false) is null)
         {
            var categoryId = seededCategories.Count > 0 ? seededCategories[(i - 1) % seededCategories.Count].Id : (Guid?)null;
            var product = Product.Create(pName, $"Description for {pName}", 10m + i, "pcs", null, categoryId);
            await context.Products.AddAsync(product, cancellationToken).ConfigureAwait(false);
         }
      }
      await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      logger.LogInformation("[{Tenant}] seeded products", context.TenantInfo!.Identifier);

      // Seed employees (5)
      for (var i = 1; i <= 5; i++)
      {
         var eName = $"Employee {i}";
         if (await context.Employees.FirstOrDefaultAsync(t => t.Name == eName, cancellationToken).ConfigureAwait(false) is null)
         {
            var emp = Employee.Create(
               eName,
               "Staff",
               $"RESP{i:000}",
               null);
            await context.Employees.AddAsync(emp, cancellationToken).ConfigureAwait(false);
         }
      }
      await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      logger.LogInformation("[{Tenant}] seeded employees", context.TenantInfo!.Identifier);
   }
}
```

### 5. TodoDbInitializer (api/modules/Todo/Persistence/TodoDbInitializer.cs)
```csharp
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Todo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Todo.Persistence;
internal sealed class TodoDbInitializer(
   ILogger<TodoDbInitializer> logger,
   TodoDbContext context) : IDbInitializer
{
   public async Task MigrateAsync(CancellationToken cancellationToken)
   {
      if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
      {
         await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
         logger.LogInformation("[{Tenant}] applied database migrations for todo module", context.TenantInfo!.Identifier);
      }
   }

   public async Task SeedAsync(CancellationToken cancellationToken)
   {
      const string title = "Hello World!";
      const string note = "This is your first task";
      if (await context.Todos.FirstOrDefaultAsync(t => t.Title == title, cancellationToken).ConfigureAwait(false) is null)
      {
         var todo = TodoItem.Create(title, note);
         await context.Todos.AddAsync(todo, cancellationToken);
         await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
         logger.LogInformation("[{Tenant}] seeding default todo data", context.TenantInfo!.Identifier);
      }
   }
}
```

### 6. IdentityDbInitializer (api/framework/Infrastructure/Identity/Persistence/IdentityDbInitializer.cs)
```csharp
using Finbuckle.MultiTenant.Abstractions;
using AMIS.Framework.Core.Origin;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Infrastructure.Identity.RoleClaims;
using AMIS.Framework.Infrastructure.Identity.Roles;
using AMIS.Framework.Infrastructure.Identity.Users;
using AMIS.Framework.Infrastructure.Tenant;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityConstants = AMIS.Shared.Authorization.IdentityConstants;

namespace AMIS.Framework.Infrastructure.Identity.Persistence;
internal sealed class IdentityDbInitializer(
   ILogger<IdentityDbInitializer> logger,
   IdentityDbContext context,
   RoleManager<FshRole> roleManager,
   UserManager<FshUser> userManager,
   TimeProvider timeProvider,
   IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
   IOptions<OriginOptions> originSettings) : IDbInitializer
{
   public async Task MigrateAsync(CancellationToken cancellationToken)
   {
      if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
      {
         await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
         logger.LogInformation("[{Tenant}] applied database migrations for identity module", context.TenantInfo?.Identifier);
      }
   }

   public async Task SeedAsync(CancellationToken cancellationToken)
   {
      await SeedRolesAsync();
      await SeedAdminUserAsync();
   }

   private async Task SeedRolesAsync()
   {
      foreach (string roleName in FshRoles.DefaultRoles)
      {
         if (await roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
            is not FshRole role)
         {
            // create role
            role = new FshRole(roleName, $"{roleName} Role for {multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id} Tenant");
            await roleManager.CreateAsync(role);
         }

         // Assign permissions
         if (roleName == FshRoles.Basic)
         {
            await AssignPermissionsToRoleAsync(context, FshPermissions.Basic, role);
         }
         else if (roleName == FshRoles.Admin)
         {
            await AssignPermissionsToRoleAsync(context, FshPermissions.Admin, role);

            if (multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id == TenantConstants.Root.Id)
            {
               await AssignPermissionsToRoleAsync(context, FshPermissions.Root, role);
            }
         }
      }
   }

   private async Task AssignPermissionsToRoleAsync(IdentityDbContext dbContext, IReadOnlyList<FshPermission> permissions, FshRole role)
   {
      var currentClaims = await roleManager.GetClaimsAsync(role);
      var newClaims = permissions
         .Where(permission => !currentClaims.Any(c => c.Type == FshClaims.Permission && c.Value == permission.Name))
         .Select(permission => new FshRoleClaim
         {
            RoleId = role.Id,
            ClaimType = FshClaims.Permission,
            ClaimValue = permission.Name,
            CreatedBy = "application",
            CreatedOn = timeProvider.GetUtcNow()
         })
         .ToList();

      foreach (var claim in newClaims)
      {
         logger.LogInformation("Seeding {Role} Permission '{Permission}' for '{TenantId}' Tenant.", role.Name, claim.ClaimValue, multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
         await dbContext.RoleClaims.AddAsync(claim);
      }

      // Save changes to the database context
      if (newClaims.Count != 0)
      {
         await dbContext.SaveChangesAsync();
      }

   }

   private async Task SeedAdminUserAsync()
   {
      if (string.IsNullOrWhiteSpace(multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id) || string.IsNullOrWhiteSpace(multiTenantContextAccessor.MultiTenantContext.TenantInfo?.AdminEmail))
      {
         return;
      }

      if (await userManager.Users.FirstOrDefaultAsync(u => u.Email == multiTenantContextAccessor.MultiTenantContext.TenantInfo!.AdminEmail)
         is not FshUser adminUser)
      {
         string adminUserName = $"{multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id.Trim()}.{FshRoles.Admin}".ToUpperInvariant();
         adminUser = new FshUser
         {
            FirstName = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id.Trim().ToUpperInvariant(),
            LastName = FshRoles.Admin,
            Email = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.AdminEmail,
            UserName = adminUserName,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            NormalizedEmail = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.AdminEmail!.ToUpperInvariant(),
            NormalizedUserName = adminUserName.ToUpperInvariant(),
            ImageUrl = new Uri(originSettings.Value.OriginUrl! + TenantConstants.Root.DefaultProfilePicture),
            IsActive = true
         };

         logger.LogInformation("Seeding Default Admin User for '{TenantId}' Tenant.", multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
         var password = new PasswordHasher<FshUser>();
         adminUser.PasswordHash = password.HashPassword(adminUser, TenantConstants.DefaultPassword);
         await userManager.CreateAsync(adminUser);
      }

      // Assign role to user
      if (!await userManager.IsInRoleAsync(adminUser, FshRoles.Admin))
      {
         logger.LogInformation("Assigning Admin Role to Admin User for '{TenantId}' Tenant.", multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
         await userManager.AddToRoleAsync(adminUser, FshRoles.Admin);
      }
   }
}
```

---
This document describes the multi-tenant implementation as of November 2025 for the AMIS.9 project.