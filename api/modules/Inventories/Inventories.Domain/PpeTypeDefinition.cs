using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Database-driven PPE type definitions with account code mappings
/// Replaces hardcoded PPE type logic with flexible configuration
/// </summary>
public class PPETypeDefinition : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!; // MACHINERY, ICT, FURNITURE, etc.
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string RCAAccountCode { get; private set; } = default!;
    public string DepreciationAccountCode { get; private set; } = default!;
    public decimal DefaultDepreciationRate { get; private set; } // Annual %
    public int DefaultUsefulLifeYears { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? COAReference { get; private set; } // e.g., "COA Circular 2022-002"

    // Optional categorization
    public string? Category { get; private set; } // Office, Field, Medical, etc.
    public string? IconName { get; private set; } // For UI display

    private PPETypeDefinition() { }

    private PPETypeDefinition(
        Guid? id,
        string code,
        string name,
        string rcaAccountCode,
        string depreciationAccountCode,
        decimal defaultDepreciationRate,
        int defaultUsefulLifeYears,
        int sortOrder,
        string? description = null,
        string? category = null,
        string? iconName = null,
        string? coaReference = null)
    {
        Id = id ?? Guid.NewGuid();
        Code = code;
        Name = name;
        Description = description;
        RCAAccountCode = rcaAccountCode;
        DepreciationAccountCode = depreciationAccountCode;
        DefaultDepreciationRate = defaultDepreciationRate;
        DefaultUsefulLifeYears = defaultUsefulLifeYears;
        SortOrder = sortOrder;
        Category = category;
        IconName = iconName;
        COAReference = coaReference;
        IsActive = true;
    }

    public static PPETypeDefinition Create(
        string code,
        string name,
        string rcaAccountCode,
        string depreciationAccountCode,
        decimal defaultDepreciationRate,
        int defaultUsefulLifeYears,
        int sortOrder = 10,
        string? description = null,
        string? category = null,
        string? iconName = null,
        string? coaReference = null,
        Guid? id = null)
    {
        ValidateCreate(code, name, rcaAccountCode, depreciationAccountCode, defaultDepreciationRate, defaultUsefulLifeYears);

        return new PPETypeDefinition(
            id,
            code,
            name,
            rcaAccountCode,
            depreciationAccountCode,
            defaultDepreciationRate,
            defaultUsefulLifeYears,
            sortOrder,
            description,
            category,
            iconName,
            coaReference);
    }

    public void Update(
        string name,
        string rcaAccountCode,
        string depreciationAccountCode,
        decimal defaultDepreciationRate,
        int defaultUsefulLifeYears,
        int sortOrder,
        string? description = null,
        string? category = null,
        string? iconName = null,
        string? coaReference = null)
    {
        ValidateUpdate(name, rcaAccountCode, depreciationAccountCode, defaultDepreciationRate, defaultUsefulLifeYears);

        Name = name;
        RCAAccountCode = rcaAccountCode;
        DepreciationAccountCode = depreciationAccountCode;
        DefaultDepreciationRate = defaultDepreciationRate;
        DefaultUsefulLifeYears = defaultUsefulLifeYears;
        SortOrder = sortOrder;
        Description = description;
        Category = category;
        IconName = iconName;
        COAReference = coaReference;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private static void ValidateCreate(
        string code,
        string name,
        string rcaAccountCode,
        string depreciationAccountCode,
        decimal defaultDepreciationRate,
        int defaultUsefulLifeYears)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("PPE type code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("PPE type name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(rcaAccountCode))
            throw new ArgumentException("RCA account code is required.", nameof(rcaAccountCode));
        if (string.IsNullOrWhiteSpace(depreciationAccountCode))
            throw new ArgumentException("Depreciation account code is required.", nameof(depreciationAccountCode));
        if (defaultDepreciationRate <= 0 || defaultDepreciationRate > 100)
            throw new ArgumentException("Depreciation rate must be between 0 and 100.", nameof(defaultDepreciationRate));
        if (defaultUsefulLifeYears <= 0)
            throw new ArgumentException("Useful life must be greater than zero.", nameof(defaultUsefulLifeYears));
    }

    private static void ValidateUpdate(
        string name,
        string rcaAccountCode,
        string depreciationAccountCode,
        decimal defaultDepreciationRate,
        int defaultUsefulLifeYears)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("PPE type name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(rcaAccountCode))
            throw new ArgumentException("RCA account code is required.", nameof(rcaAccountCode));
        if (string.IsNullOrWhiteSpace(depreciationAccountCode))
            throw new ArgumentException("Depreciation account code is required.", nameof(depreciationAccountCode));
        if (defaultDepreciationRate <= 0 || defaultDepreciationRate > 100)
            throw new ArgumentException("Depreciation rate must be between 0 and 100.", nameof(defaultDepreciationRate));
        if (defaultUsefulLifeYears <= 0)
            throw new ArgumentException("Useful life must be greater than zero.", nameof(defaultUsefulLifeYears));
    }

    /// <summary>
    /// Seed default PPE types based on COA 2022-002
    /// </summary>
    public static List<PPETypeDefinition> SeedDefaults()
    {
        return new List<PPETypeDefinition>
        {
            Create("MACHINERY", "Machinery and Equipment", "10604010", "10699010", 10m, 10, 1,
                "Industrial machinery, tools, and equipment", "Production", "gear", "COA Circular 2022-002", new Guid("8cfb4254-c41d-4217-a176-998a332e4a68")),
            Create("TRANSPORTATION", "Transportation Equipment", "10605010", "10699010", 20m, 5, 2,
                "Vehicles, motorcycles, boats", "Transportation", "car", "COA Circular 2022-002", new Guid("4a2d8afe-1374-4cc0-b1f8-d86dd9bd9526")),
            Create("FURNITURE", "Furniture, Fixtures and Books", "10606010", "10699010", 10m, 10, 3,
                "Office furniture, fixtures, and reference books", "Office", "chair", "COA Circular 2022-002", new Guid("1b062724-34c0-40fc-aaa3-618afe3146d4")),
            Create("ICT", "ICT Equipment", "10607010", "10699010", 33.33m, 3, 4,
                "Computers, servers, network equipment", "Technology", "desktop", "COA Circular 2022-002", new Guid("114e8090-cab3-43d6-9485-f5d28642e9b8")),
            Create("OTHER", "Other PPE", "10699990", "10699010", 10m, 10, 5,
                "Other property, plant and equipment", "General", "box", "COA Circular 2022-002", new Guid("211945cc-dde6-4b91-a3f7-0da8e5fa742d"))
        };
    }
}

