using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using AMIS.WebApi.Inventories.Infrastructure.Persistence.Data;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence;

internal sealed class InventoriesDbInitializer(
    ILogger<InventoriesDbInitializer> logger,
    InventoriesDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (await TableExistsAsync("inventories", "AnnualProcurementPlans", cancellationToken))
            {
                logger.LogWarning("[{Tenant}] detected existing inventories tables; skipping migrations to avoid duplicate-object errors. Consider baselining the migration history if the schema is already applied.", context.TenantInfo!.Identifier);
                return;
            }

            var pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);

            if (pendingMigrations.Any())
            {
                await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] applied database migrations for inventories module", context.TenantInfo!.Identifier);
            }
        }
        catch (PostgresException ex) when (IsDuplicateObjectError(ex))
        {
            logger.LogWarning("[{Tenant}] skipped inventories migrations because database objects already exist (SQLSTATE {SqlState}). Consider baselining the migration history if the schema is already applied. Error: {Message}", context.TenantInfo!.Identifier, ex.SqlState, ex.Message);
        }
    }

    private static bool IsDuplicateObjectError(PostgresException ex) =>
        ex.SqlState == PostgresErrorCodes.DuplicateTable ||
        ex.SqlState == PostgresErrorCodes.DuplicateObject ||
        ex.SqlState == PostgresErrorCodes.DuplicateSchema;

    private async Task<bool> TableExistsAsync(string schema, string table, CancellationToken cancellationToken)
    {
        var connection = context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = "select to_regclass(@fullName)::text;";

        var fullName = $"{schema}.\"{table}\"";
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@fullName";
        parameter.Value = fullName;
        parameter.DbType = DbType.String;
        command.Parameters.Add(parameter);

        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return result is string value && !string.IsNullOrWhiteSpace(value);
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("[{Tenant}] Starting comprehensive seed data generation", context.TenantInfo!.Identifier);

        await NfaOfficeCodeSeeder.SeedDefaultsAsync(context, logger, cancellationToken);
        await PpeCodeSeeder.SeedDefaultsAsync(context, logger, cancellationToken);

        // 1. Seed Categories (10)
        var categories = await SeedCategoriesAsync(cancellationToken);
        
        // 2. Seed Suppliers (10)
        var suppliers = await SeedSuppliersAsync(cancellationToken);
        
        // 3. Seed Employees (10)
        var employees = await SeedEmployeesAsync(cancellationToken);
        
        // 4. Seed Products (10)
        var products = await SeedProductsAsync(categories, cancellationToken);
        
        // 5. Seed Purchases with Items (10)
        var purchases = await SeedPurchasesAsync(suppliers, products, cancellationToken);
        
        // 6. Seed Inspections (10)
        await SeedInspectionsAsync(purchases, employees, cancellationToken);
        
        // 7. Seed Inventory Registry (sample PPE items)
        await SeedInventoryRegistriesAsync(cancellationToken);

        // 8. Seed PPE Type Account Mappings
        await SeedPPETypeAccountMappingsAsync(cancellationToken);
        
        logger.LogInformation("[{Tenant}] Comprehensive seed data generation completed", context.TenantInfo!.Identifier);
    }

    private async Task<List<Category>> SeedCategoriesAsync(CancellationToken cancellationToken)
    {
        var categoryNames = new[]
        {
            ("Office Equipment", "Desktop computers, printers, scanners, and office machines"),
            ("IT Hardware", "Servers, network equipment, storage devices"),
            ("Furniture & Fixtures", "Office desks, chairs, cabinets, and furnishings"),
            ("Vehicles", "Company cars, trucks, and transportation equipment"),
            ("Heavy Machinery", "Construction and industrial equipment"),
            ("Office Supplies", "Consumable supplies and materials"),
            ("Software Licenses", "Software applications and licenses"),
            ("Medical Equipment", "Healthcare and medical devices"),
            ("Security Systems", "CCTV, access control, and security equipment"),
            ("Building Materials", "Construction and facility maintenance materials")
        };

        var seededCategories = new List<Category>();
        
        foreach (var (name, description) in categoryNames)
        {
            if (await context.Categories.FirstOrDefaultAsync(c => c.Name == name, cancellationToken) is null)
            {
                var category = Category.Create(name, description);
                await context.Categories.AddAsync(category, cancellationToken);
                seededCategories.Add(category);
            }
            else
            {
                var existing = await context.Categories.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
                if (existing != null) seededCategories.Add(existing);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
        
        // Refresh with persisted IDs
        seededCategories = await context.Categories
            .Where(c => categoryNames.Select(cn => cn.Item1).Contains(c.Name))
            .ToListAsync(cancellationToken);
        
        logger.LogInformation("[{Tenant}] Seeded {Count} categories", context.TenantInfo!.Identifier, seededCategories.Count);
        return seededCategories;
    }

    private async Task<List<Supplier>> SeedSuppliersAsync(CancellationToken cancellationToken)
    {
        var supplierData = new[]
        {
            ("ABC Office Solutions Inc.", "123 Business Ave, Makati City", "123-456-789-000", "VAT", "0917-123-4567", "sales@abcoffice.ph"),
            ("TechPro Supplies Corp.", "456 Innovation St, BGC Taguig", "234-567-890-000", "VAT", "0918-234-5678", "info@techpro.ph"),
            ("Metro Furniture & Fixtures", "789 Commerce Rd, Quezon City", "345-678-901-000", "NON-VAT", "0919-345-6789", "sales@metrofurniture.ph"),
            ("Global IT Solutions", "321 Tech Hub, Ortigas Pasig", "456-789-012-000", "VAT", "0920-456-7890", "contact@globalit.ph"),
            ("Prime Hardware Trading", "654 Industrial Ave, Caloocan", "567-890-123-000", "VAT", "0921-567-8901", "orders@primehw.ph"),
            ("Elite Medical Supplies", "987 Health Plaza, Manila", "678-901-234-000", "VAT", "0922-678-9012", "supplies@elitemed.ph"),
            ("AutoParts & Services Co.", "147 Automotive Blvd, Valenzuela", "789-012-345-000", "NON-VAT", "0923-789-0123", "parts@autoservices.ph"),
            ("Smart Security Systems", "258 Safety St, Mandaluyong", "890-123-456-000", "VAT", "0924-890-1234", "security@smartsys.ph"),
            ("BuildPro Construction Supply", "369 Builder Ave, Marikina", "901-234-567-000", "VAT", "0925-901-2345", "supply@buildpro.ph"),
            ("EcoGreen Office Products", "741 Eco Park, Pasay City", "012-345-678-000", "NON-VAT", "0926-012-3456", "green@ecooffice.ph")
        };

        var suppliers = new List<Supplier>();
        
        foreach (var (name, address, tin, taxClass, contact, email) in supplierData)
        {
            if (await context.Suppliers.FirstOrDefaultAsync(s => s.Name == name, cancellationToken) is null)
            {
                var supplier = Supplier.Create(name, address, tin, taxClass, contact, email);
                await context.Suppliers.AddAsync(supplier, cancellationToken);
                suppliers.Add(supplier);
            }
            else
            {
                var existing = await context.Suppliers.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
                if (existing != null) suppliers.Add(existing);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
        
        suppliers = await context.Suppliers
            .Where(s => supplierData.Select(sd => sd.Item1).Contains(s.Name))
            .ToListAsync(cancellationToken);
        
        logger.LogInformation("[{Tenant}] Seeded {Count} suppliers", context.TenantInfo!.Identifier, suppliers.Count);
        return suppliers;
    }

    private async Task<List<Employee>> SeedEmployeesAsync(CancellationToken cancellationToken)
    {
        var employeeData = new[]
        {
            ("Juan Dela Cruz", "Supply Officer", "RESP001"),
            ("Maria Santos", "Inspector", "RESP002"),
            ("Jose Rizal", "Procurement Manager", "RESP003"),
            ("Ana Reyes", "Warehouse Supervisor", "RESP004"),
            ("Pedro Garcia", "Asset Custodian", "RESP005"),
            ("Carmen Lopez", "Senior Inspector", "RESP006"),
            ("Roberto Mendoza", "Receiving Officer", "RESP007"),
            ("Teresa Ramos", "Inventory Clerk", "RESP008"),
            ("Miguel Torres", "Property Officer", "RESP009"),
            ("Sofia Fernandez", "Acceptance Officer", "RESP010")
        };

        var employees = new List<Employee>();
        
        foreach (var (name, designation, respCode) in employeeData)
        {
            if (await context.Employees.FirstOrDefaultAsync(e => e.ResponsibilityCode == respCode, cancellationToken) is null)
            {
                var employee = Employee.Create(name, designation, respCode, null);
                await context.Employees.AddAsync(employee, cancellationToken);
                employees.Add(employee);
            }
            else
            {
                var existing = await context.Employees.FirstOrDefaultAsync(e => e.ResponsibilityCode == respCode, cancellationToken);
                if (existing != null) employees.Add(existing);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
        
        employees = await context.Employees
            .Where(e => employeeData.Select(ed => ed.Item3).Contains(e.ResponsibilityCode))
            .ToListAsync(cancellationToken);
        
        logger.LogInformation("[{Tenant}] Seeded {Count} employees", context.TenantInfo!.Identifier, employees.Count);
        return employees;
    }

    private async Task<List<Product>> SeedProductsAsync(List<Category> categories, CancellationToken cancellationToken)
    {
        var productData = new[]
        {
            ("Dell OptiPlex 7090 Desktop", "Intel i7, 16GB RAM, 512GB SSD", 45000m, "unit", PropertyClassification.SemiExpendable, 36),
            ("HP LaserJet Pro Printer", "Monochrome laser printer with duplex", 15000m, "unit", PropertyClassification.SemiExpendable, 24),
            ("Executive Office Chair", "Ergonomic high-back chair with lumbar support", 8500m, "unit", PropertyClassification.SemiExpendable, 60),
            ("Cisco Network Switch 24-Port", "Managed gigabit ethernet switch", 28000m, "unit", PropertyClassification.SemiExpendable, 60),
            ("Samsung 27-inch Monitor", "4K UHD LED display", 18000m, "unit", PropertyClassification.SemiExpendable, 36),
            ("Office Desk Organizer Set", "Desktop organizer with compartments", 850m, "set", PropertyClassification.Consumable, 12),
            ("Whiteboard Markers Pack", "Pack of 12 assorted colors", 350m, "pack", PropertyClassification.Consumable, 6),
            ("A4 Bond Paper Ream", "500 sheets 80gsm bond paper", 250m, "ream", PropertyClassification.Consumable, 12),
            ("Logitech Wireless Keyboard & Mouse", "Wireless combo set", 2500m, "set", PropertyClassification.SemiExpendable, 24),
            ("APC UPS 1000VA", "Uninterruptible power supply with battery backup", 6500m, "unit", PropertyClassification.SemiExpendable, 36)
        };

        var products = new List<Product>();
        var categoryCount = categories.Count;
        
        for (int i = 0; i < productData.Length; i++)
        {
            var (name, description, price, unit, classification, usefulLife) = productData[i];
            
            if (await context.Products.FirstOrDefaultAsync(p => p.Name == name, cancellationToken) is null)
            {
                var categoryId = categoryCount > 0 ? categories[i % categoryCount].Id : (Guid?)null;
                var product = Product.Create(name, description, price, unit, null, categoryId, classification, usefulLife);
                await context.Products.AddAsync(product, cancellationToken);
                products.Add(product);
            }
            else
            {
                var existing = await context.Products.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
                if (existing != null) products.Add(existing);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
        
        products = await context.Products
            .Where(p => productData.Select(pd => pd.Item1).Contains(p.Name))
            .ToListAsync(cancellationToken);
        
        logger.LogInformation("[{Tenant}] Seeded {Count} products", context.TenantInfo!.Identifier, products.Count);
        return products;
    }

    private async Task<List<Purchase>> SeedPurchasesAsync(List<Supplier> suppliers, List<Product> products, CancellationToken cancellationToken)
    {
        var purchases = new List<Purchase>();
        var random = new Random(42); // Fixed seed for reproducibility
        
        for (int i = 1; i <= 10; i++)
        {
            var refNumber = $"PO-2026-{i:D4}";
            
            if (await context.Purchases.FirstOrDefaultAsync(p => p.ReferenceNumber == refNumber, cancellationToken) is null)
            {
                var supplier = suppliers[random.Next(suppliers.Count)];
                var purchaseDate = DateTime.UtcNow.AddDays(-random.Next(1, 90));
                
                var purchase = Purchase.Create(
                    supplier.Id,
                    purchaseDate,
                    refNumber,
                    $"Purchase order for office supplies and equipment - Batch {i}",
                    "Main Office, Manila");
                
                // Add 2-4 random items to each purchase
                var itemCount = random.Next(2, 5);
                for (int j = 0; j < itemCount; j++)
                {
                    var product = products[random.Next(products.Count)];
                    var quantity = random.Next(1, 10);
                    var unitPrice = product.Sku * (decimal)(0.9 + random.NextDouble() * 0.2); // �10% variance
                    
                    purchase.AddItem(product.Id, quantity, unitPrice, PurchaseStatus.Draft);
                }
                
                await context.Purchases.AddAsync(purchase, cancellationToken);
                purchases.Add(purchase);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
        
        // Reload with items
        purchases = await context.Purchases
            .Include(p => p.Items)
            .Where(p => p.ReferenceNumber!.StartsWith("PO-2026-"))
            .ToListAsync(cancellationToken);
        
        logger.LogInformation("[{Tenant}] Seeded {Count} purchases with items", context.TenantInfo!.Identifier, purchases.Count);
        return purchases;
    }

    private async Task SeedInspectionsAsync(List<Purchase> purchases, List<Employee> employees, CancellationToken cancellationToken)
    {
        var random = new Random(42);
        var inspectionCount = 0;
        
        // Create inspections for first 10 purchases
        foreach (var purchase in purchases.Take(10))
        {
            if (purchase.Items.Count == 0) continue;
            
            // Check if inspection already exists
            var existingInspection = await context.Inspections
                .FirstOrDefaultAsync(i => i.PurchaseId == purchase.Id, cancellationToken);
            
            if (existingInspection is null)
            {
                var inspector = employees[random.Next(employees.Count)];
                var inspectedDate = purchase.PurchaseDate!.Value.AddDays(random.Next(3, 15));
                
                var inspection = Inspection.Create(
                    purchase.Id,
                    inspector.Id,
                    inspectedDate,
                    $"Inspection for PO {purchase.ReferenceNumber}",
                    null);
                
                // Add inspection items for each purchase item
                foreach (var purchaseItem in purchase.Items)
                {
                    var qtyInspected = purchaseItem.Qty;
                    var passRate = random.NextDouble();
                    var qtyPassed = passRate > 0.8 ? qtyInspected : (int)(qtyInspected * passRate);
                    var qtyFailed = qtyInspected - qtyPassed;
                    
                    InspectionItemStatus? status = qtyFailed == 0 
                        ? InspectionItemStatus.Passed 
                        : qtyPassed > 0 
                            ? InspectionItemStatus.AcceptedWithDeviation 
                            : InspectionItemStatus.Failed;
                    
                    var inspectionItem = InspectionItem.Create(
                        inspection.Id,
                        purchaseItem.Id,
                        qtyInspected,
                        qtyPassed,
                        qtyFailed,
                        qtyFailed > 0 ? $"Minor defects found in {qtyFailed} units" : "All units passed inspection",
                        status);
                    
                    inspection.AddItem(inspectionItem);
                }
                
                // Finalize inspection if all passed
                if (inspection.Items.All(i => i.InspectionItemStatus == InspectionItemStatus.Passed))
                {
                    inspection.Approve();
                }
                
                await context.Inspections.AddAsync(inspection, cancellationToken);
                inspectionCount++;
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("[{Tenant}] Seeded {Count} inspections with items", context.TenantInfo!.Identifier, inspectionCount);
    }

    private async Task SeedPPETypeAccountMappingsAsync(CancellationToken cancellationToken)
    {
        if (await context.PPETypeAccountMappings.AnyAsync(cancellationToken))
        {
            logger.LogInformation("[{Tenant}] PPE type account mappings already seeded", context.TenantInfo!.Identifier);
            return;
        }

        var mappings = new[]
        {
            PPETypeAccountMapping.Create("MACHINERY", "1-06-03-010", "Heavy machinery and industrial equipment"),
            PPETypeAccountMapping.Create("EQUIPMENT", "1-06-03-010", "General equipment and tools"),
            PPETypeAccountMapping.Create("TRANSPORTATION", "1-06-04-010", "Vehicles and transportation assets"),
            PPETypeAccountMapping.Create("VEHICLE", "1-06-04-010", "Motor vehicles"),
            PPETypeAccountMapping.Create("AUTOMOTIVE", "1-06-04-010", "Automotive equipment"),
            PPETypeAccountMapping.Create("FURNITURE", "1-06-05-010", "Office furniture and fixtures"),
            PPETypeAccountMapping.Create("FIXTURES", "1-06-05-010", "Office fixtures and fittings"),
            PPETypeAccountMapping.Create("ICT", "1-06-06-010", "Information and communication technology"),
            PPETypeAccountMapping.Create("COMPUTER", "1-06-06-010", "Computer hardware and peripherals"),
            PPETypeAccountMapping.Create("IT", "1-06-06-010", "Information technology equipment"),
            PPETypeAccountMapping.Create("TECHNOLOGY", "1-06-06-010", "Technology equipment")
        };

        await context.PPETypeAccountMappings.AddRangeAsync(mappings, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("[{Tenant}] seeded {Count} PPE type account mappings", context.TenantInfo!.Identifier, mappings.Length);
    }

    private async Task SeedInventoryRegistriesAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("[{Tenant}] Checking inventory registry for existing data...", context.TenantInfo!.Identifier);
            
            var existingCount = await context.InventoryRegistries.CountAsync(cancellationToken);
            logger.LogInformation("[{Tenant}] Found {Count} existing inventory registry items", context.TenantInfo!.Identifier, existingCount);
            
            if (existingCount > 0)
            {
                logger.LogInformation("[{Tenant}] inventory registry already seeded, skipping", context.TenantInfo!.Identifier);
                return;
            }

            logger.LogInformation("[{Tenant}] Starting inventory registry seeding...", context.TenantInfo!.Identifier);
            
            var now = DateTime.UtcNow;
            var baseLocation = "Central Warehouse";

            var seedItems = new[]
            {
                new { Code = "PPE-2026-0001", Desc = "Dell OptiPlex 7090 Desktop", Qty = 5, Location = baseLocation, Report = "PPERR-2026-0001" },
                new { Code = "PPE-2026-0002", Desc = "HP LaserJet Pro Printer", Qty = 3, Location = baseLocation, Report = "PPERR-2026-0001" },
                new { Code = "PPE-2026-0003", Desc = "Executive Office Chair", Qty = 12, Location = "Main Office - Floor 5", Report = "PPERR-2026-0002" },
                new { Code = "PPE-2026-0004", Desc = "Cisco Network Switch 24-Port", Qty = 4, Location = "Data Center", Report = "PPERR-2026-0003" },
                new { Code = "PPE-2026-0005", Desc = "Samsung 27-inch Monitor", Qty = 10, Location = baseLocation, Report = "PPERR-2026-0002" },
                new { Code = "PPE-2026-0006", Desc = "Logitech Wireless Keyboard & Mouse", Qty = 15, Location = baseLocation, Report = "PPERR-2026-0004" },
                new { Code = "PPE-2026-0007", Desc = "APC UPS 1000VA", Qty = 6, Location = "Data Center", Report = "PPERR-2026-0003" },
                new { Code = "PPE-2026-0008", Desc = "Office Desk Organizer Set", Qty = 25, Location = baseLocation, Report = "PPERR-2026-0005" },
                new { Code = "PPE-2026-0009", Desc = "Whiteboard Markers Pack", Qty = 40, Location = "Supply Room", Report = "PPERR-2026-0005" },
                new { Code = "PPE-2026-0010", Desc = "A4 Bond Paper Ream", Qty = 60, Location = "Supply Room", Report = "PPERR-2026-0005" }
            };

            logger.LogInformation("[{Tenant}] Creating {Count} inventory registry items...", context.TenantInfo!.Identifier, seedItems.Length);
            
            var registries = seedItems.Select(item =>
            {
                var registry = InventoryRegistry.CreateFromReceiving(item.Code, item.Desc, item.Qty, item.Location, item.Report);
                registry.LastTransactionDate = now;
                return registry;
            }).ToList();

            await context.InventoryRegistries.AddRangeAsync(registries, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation("[{Tenant}] Successfully seeded {Count} inventory registry items", context.TenantInfo!.Identifier, registries.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{Tenant}] Error seeding inventory registry: {Message}", context.TenantInfo!.Identifier, ex.Message);
            throw;
        }
    }
}


