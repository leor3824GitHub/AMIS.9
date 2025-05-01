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
        const string BName = "Epson";
        const string BDescription = "Epson";
        if (await context.Brands.FirstOrDefaultAsync(t => t.Name == BName, cancellationToken).ConfigureAwait(false) is null)
        {
            var brand = Brand.Create(BName, BDescription);
            await context.Brands.AddAsync(brand, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default catalog data", context.TenantInfo!.Identifier);
        }

        // Add more seed data here
        const string CName = "Folder";
        const string CDescription = "Folders";
        if (await context.Categories.FirstOrDefaultAsync(t => t.Name == CName, cancellationToken).ConfigureAwait(false) is null)
        {
            var category = Category.Create(CName, CDescription);
            await context.Categories.AddAsync(category, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default catalog data", context.TenantInfo!.Identifier);
        }
    }
}
