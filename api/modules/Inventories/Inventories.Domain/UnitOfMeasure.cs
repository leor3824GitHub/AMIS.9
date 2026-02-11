using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Database-driven unit of measure definitions
/// Replaces hardcoded 'piece' with flexible configuration
/// </summary>
public class UnitOfMeasure : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!; // PC, SET, KG, L, M, etc.
    public string Name { get; private set; } = default!; // Piece, Set, Kilogram, Liter, Meter
    public string? Abbreviation { get; private set; } // pcs, kg, L, m
    public UnitType UnitType { get; private set; }
    public bool IsActive { get; private set; } = true;
    public int SortOrder { get; private set; }
    public bool IsDefault { get; private set; } // Default UoM for new assets

    // Conversion support (optional)
    public Guid? BaseUnitId { get; private set; } // For unit conversions
    public decimal? ConversionFactor { get; private set; } // Multiply by this to get base unit

    private UnitOfMeasure() { }

    private UnitOfMeasure(
        Guid? id,
        string code,
        string name,
        UnitType unitType,
        int sortOrder,
        string? abbreviation = null,
        bool isDefault = false,
        Guid? baseUnitId = null,
        decimal? conversionFactor = null)
    {
        Id = id ?? Guid.NewGuid();
        Code = code;
        Name = name;
        Abbreviation = abbreviation;
        UnitType = unitType;
        SortOrder = sortOrder;
        IsDefault = isDefault;
        BaseUnitId = baseUnitId;
        ConversionFactor = conversionFactor;
        IsActive = true;
    }

    public static UnitOfMeasure Create(
        string code,
        string name,
        UnitType unitType,
        int sortOrder = 10,
        string? abbreviation = null,
        bool isDefault = false,
        Guid? baseUnitId = null,
        decimal? conversionFactor = null,
        Guid? id = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Unit code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Unit name is required.", nameof(name));
        if (baseUnitId.HasValue && !conversionFactor.HasValue)
            throw new ArgumentException("Conversion factor is required when base unit is specified.", nameof(conversionFactor));
        if (conversionFactor.HasValue && conversionFactor.Value <= 0)
            throw new ArgumentException("Conversion factor must be greater than zero.", nameof(conversionFactor));

        return new UnitOfMeasure(
            id,
            code,
            name,
            unitType,
            sortOrder,
            abbreviation,
            isDefault,
            baseUnitId,
            conversionFactor);
    }

    public void Update(
        string name,
        UnitType unitType,
        int sortOrder,
        string? abbreviation = null,
        Guid? baseUnitId = null,
        decimal? conversionFactor = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Unit name is required.", nameof(name));
        if (baseUnitId.HasValue && !conversionFactor.HasValue)
            throw new ArgumentException("Conversion factor is required when base unit is specified.", nameof(conversionFactor));
        if (conversionFactor.HasValue && conversionFactor.Value <= 0)
            throw new ArgumentException("Conversion factor must be greater than zero.", nameof(conversionFactor));

        Name = name;
        UnitType = unitType;
        SortOrder = sortOrder;
        Abbreviation = abbreviation;
        BaseUnitId = baseUnitId;
        ConversionFactor = conversionFactor;
    }

    public void SetAsDefault() => IsDefault = true;
    public void RemoveDefault() => IsDefault = false;
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    /// <summary>
    /// Seed default units based on common government standards
    /// </summary>
    public static IReadOnlyList<UnitOfMeasure> SeedDefaults()
    {
        return new List<UnitOfMeasure>
        {
            // Count-based units
            Create("PC", "Piece", UnitType.Count, 1, "pc", isDefault: true, id: new Guid("59edba6a-5678-4823-9b32-252b17d9af28")),
            Create("SET", "Set", UnitType.Count, 2, "set", id: new Guid("bd2d2fae-8c18-4b68-84b6-82832160ea5a")),
            Create("UNIT", "Unit", UnitType.Count, 3, "unit", id: new Guid("752476b7-58e3-4f5e-a8e0-29d45fc6f9d3")),
            Create("PAIR", "Pair", UnitType.Count, 4, "pair", id: new Guid("628c07ae-8c1c-45f7-a6d7-1136eb01b9d6")),
            
            // Weight-based units
            Create("KG", "Kilogram", UnitType.Weight, 10, "kg", id: new Guid("87e85a94-c532-4177-9a8a-907040c9b013")),
            Create("G", "Gram", UnitType.Weight, 11, "g", id: new Guid("a105d59a-cef8-4be6-b682-0f702199d9db")),
            Create("MT", "Metric Ton", UnitType.Weight, 12, "MT", id: new Guid("a721c410-0db8-4fbf-af71-e1e4a0038fa2")),
            
            // Volume-based units
            Create("L", "Liter", UnitType.Volume, 20, "L", id: new Guid("1c36ce8b-91d3-47db-b60d-205d34eb524a")),
            Create("ML", "Milliliter", UnitType.Volume, 21, "mL", id: new Guid("b31f7aef-c02c-44d9-9b19-5f7e3b47cbab")),
            Create("GAL", "Gallon", UnitType.Volume, 22, "gal", id: new Guid("91186001-b2c5-4faf-99c3-6b9bea3518e4")),
            
            // Length-based units
            Create("M", "Meter", UnitType.Length, 30, "m", id: new Guid("bb4ca419-3e3a-4d3f-be00-978e0c5f1560")),
            Create("CM", "Centimeter", UnitType.Length, 31, "cm", id: new Guid("585e3d94-c3c4-4ad6-98d9-de2dabc45f4e")),
            Create("MM", "Millimeter", UnitType.Length, 32, "mm", id: new Guid("83a766bf-95fa-4844-9a80-ceeb7f42d72b")),
            Create("FT", "Feet", UnitType.Length, 33, "ft", id: new Guid("9d3f8a2f-1b31-4609-87d8-e30fe7bde26c")),
            
            // Area-based units
            Create("SQM", "Square Meter", UnitType.Area, 40, "m²", id: new Guid("a0568313-645e-4db5-ab2d-1d4e25585849")),
            
            // Other
            Create("BOX", "Box", UnitType.Container, 50, "box", id: new Guid("236f1166-3360-4368-8da7-2cee2b670721")),
            Create("PACK", "Pack", UnitType.Container, 51, "pack", id: new Guid("e824ebde-56d7-4796-b321-1a229daf84dd")),
            Create("BOTTLE", "Bottle", UnitType.Container, 52, "btl", id: new Guid("47b56c02-75f5-44e8-b209-b5f821834886")),
            Create("CAN", "Can", UnitType.Container, 53, "can", id: new Guid("ecf0f606-da73-4fd6-b3bf-ed6353f8a763"))
        };
    }
}

/// <summary>
/// Categories of unit types
/// </summary>
public enum UnitType
{
    Count = 1,
    Weight = 2,
    Volume = 3,
    Length = 4,
    Area = 5,
    Container = 6,
    Other = 99
}

