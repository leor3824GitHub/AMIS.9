using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Catalog.Domain;

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
        string code,
        string name,
        UnitType unitType,
        int sortOrder,
        string? abbreviation = null,
        bool isDefault = false,
        Guid? baseUnitId = null,
        decimal? conversionFactor = null)
    {
        Id = Guid.NewGuid();
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
        decimal? conversionFactor = null)
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
            Create("PC", "Piece", UnitType.Count, 1, "pc", isDefault: true),
            Create("SET", "Set", UnitType.Count, 2, "set"),
            Create("UNIT", "Unit", UnitType.Count, 3, "unit"),
            Create("PAIR", "Pair", UnitType.Count, 4, "pair"),
            
            // Weight-based units
            Create("KG", "Kilogram", UnitType.Weight, 10, "kg"),
            Create("G", "Gram", UnitType.Weight, 11, "g"),
            Create("MT", "Metric Ton", UnitType.Weight, 12, "MT"),
            
            // Volume-based units
            Create("L", "Liter", UnitType.Volume, 20, "L"),
            Create("ML", "Milliliter", UnitType.Volume, 21, "mL"),
            Create("GAL", "Gallon", UnitType.Volume, 22, "gal"),
            
            // Length-based units
            Create("M", "Meter", UnitType.Length, 30, "m"),
            Create("CM", "Centimeter", UnitType.Length, 31, "cm"),
            Create("MM", "Millimeter", UnitType.Length, 32, "mm"),
            Create("FT", "Feet", UnitType.Length, 33, "ft"),
            
            // Area-based units
            Create("SQM", "Square Meter", UnitType.Area, 40, "m²"),
            
            // Other
            Create("BOX", "Box", UnitType.Container, 50, "box"),
            Create("PACK", "Pack", UnitType.Container, 51, "pack"),
            Create("BOTTLE", "Bottle", UnitType.Container, 52, "btl"),
            Create("CAN", "Can", UnitType.Container, 53, "can")
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
