using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Database-driven configuration for asset conditions
/// Allows administrators to define custom conditions without code changes
/// </summary>
public class AssetConditionConfiguration : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!; // Good, Fair, Poor, etc.
    public string DisplayName { get; private set; } = default!;
    public string? Description { get; private set; }
    public string ColorCode { get; private set; } = "#6c757d"; // Bootstrap default
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool AllowsForUse { get; private set; } = true; // Can asset in this condition be issued?
    public bool RequiresRepair { get; private set; }
    public bool RequiresDisposal { get; private set; }

    private AssetConditionConfiguration() { }

    private AssetConditionConfiguration(
        Guid? id,
        string code,
        string displayName,
        string? description,
        string colorCode,
        int sortOrder,
        bool allowsForUse,
        bool requiresRepair,
        bool requiresDisposal)
    {
        Id = id ?? Guid.NewGuid();
        Code = code;
        DisplayName = displayName;
        Description = description;
        ColorCode = colorCode;
        SortOrder = sortOrder;
        AllowsForUse = allowsForUse;
        RequiresRepair = requiresRepair;
        RequiresDisposal = requiresDisposal;
        IsActive = true;
    }

    public static AssetConditionConfiguration Create(
        string code,
        string displayName,
        string? description = null,
        string colorCode = "#6c757d",
        int sortOrder = 10,
        bool allowsForUse = true,
        bool requiresRepair = false,
        bool requiresDisposal = false,
        Guid? id = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Condition code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name is required.", nameof(displayName));
        if (string.IsNullOrWhiteSpace(colorCode))
            throw new ArgumentException("Color code is required.", nameof(colorCode));

        return new AssetConditionConfiguration(
            id,
            code,
            displayName,
            description,
            colorCode,
            sortOrder,
            allowsForUse,
            requiresRepair,
            requiresDisposal);
    }

    public void Update(
        string displayName,
        string? description,
        string colorCode,
        int sortOrder,
        bool allowsForUse,
        bool requiresRepair,
        bool requiresDisposal)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name is required.", nameof(displayName));
        if (string.IsNullOrWhiteSpace(colorCode))
            throw new ArgumentException("Color code is required.", nameof(colorCode));

        DisplayName = displayName;
        Description = description;
        ColorCode = colorCode;
        SortOrder = sortOrder;
        AllowsForUse = allowsForUse;
        RequiresRepair = requiresRepair;
        RequiresDisposal = requiresDisposal;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    /// <summary>
    /// Seed default conditions based on common government standards
    /// </summary>
    public static List<AssetConditionConfiguration> SeedDefaults()
    {
        return new List<AssetConditionConfiguration>
        {
            Create("Good", "Good", "Asset is in excellent working condition", "#28a745", 1, allowsForUse: true, id: new Guid("948eeb63-a4c1-4f14-93db-0fa72489a82a")),
            Create("Fair", "Fair", "Asset has minor wear but still functional", "#ffc107", 2, allowsForUse: true, id: new Guid("faac3215-d242-4963-855a-426efb3e5410")),
            Create("Poor", "Poor", "Asset has significant wear, may need repair", "#fd7e14", 3, allowsForUse: true, requiresRepair: true, id: new Guid("0d302547-c9eb-4d6a-a6a7-5102b8a5cd0c")),
            Create("Unserviceable", "Unserviceable", "Asset is not functional, requires major repair", "#dc3545", 4, allowsForUse: false, requiresRepair: true, id: new Guid("69157626-1bea-4571-aeb6-4db0363c01e1")),
            Create("ForDisposal", "For Disposal", "Asset is beyond repair and should be disposed", "#6c757d", 5, allowsForUse: false, requiresDisposal: true, id: new Guid("38aa5cfc-5eec-4865-9b28-978b54c8e4c6"))
        };
    }
}

