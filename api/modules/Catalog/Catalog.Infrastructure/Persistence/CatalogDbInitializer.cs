using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
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
        logger.LogInformation("[{Tenant}] Starting comprehensive seed data generation", context.TenantInfo!.Identifier);

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
        
        // 7. Seed PPE Type Account Mappings
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
                    var unitPrice = product.Sku * (decimal)(0.9 + random.NextDouble() * 0.2); // ±10% variance
                    
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
}
