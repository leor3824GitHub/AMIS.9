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
