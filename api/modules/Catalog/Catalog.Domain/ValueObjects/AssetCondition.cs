namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Value object representing valid asset physical conditions
/// Provides type safety and consistency across the domain
/// </summary>
public sealed class AssetCondition : IEquatable<AssetCondition>
{
    public string Value { get; }

    // Predefined valid conditions
    public static readonly AssetCondition Good = new("Good");
    public static readonly AssetCondition Fair = new("Fair");
    public static readonly AssetCondition Poor = new("Poor");
    public static readonly AssetCondition Unserviceable = new("Unserviceable");
    public static readonly AssetCondition ForDisposal = new("ForDisposal");

    private static readonly Dictionary<string, AssetCondition> ValidConditions = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Good", Good },
        { "Fair", Fair },
        { "Poor", Poor },
        { "Unserviceable", Unserviceable },
        { "ForDisposal", ForDisposal }
    };

    private AssetCondition(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Parse string to AssetCondition
    /// </summary>
    public static AssetCondition Parse(string condition)
    {
        if (string.IsNullOrWhiteSpace(condition))
            throw new ArgumentException("Condition cannot be null or empty.", nameof(condition));

        if (!ValidConditions.TryGetValue(condition, out var result))
            throw new ArgumentException($"Invalid condition '{condition}'. Valid values are: {string.Join(", ", ValidConditions.Keys)}", nameof(condition));

        return result;
    }

    /// <summary>
    /// Try parse string to AssetCondition
    /// </summary>
    public static bool TryParse(string? condition, out AssetCondition? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(condition))
            return false;

        return ValidConditions.TryGetValue(condition, out result);
    }

    /// <summary>
    /// Get all valid conditions
    /// </summary>
    public static IEnumerable<AssetCondition> GetAll() => ValidConditions.Values;

    /// <summary>
    /// Get all valid condition strings
    /// </summary>
    public static IEnumerable<string> GetAllValues() => ValidConditions.Keys;

    public static implicit operator string(AssetCondition condition) => condition.Value;

    public override string ToString() => Value;

    public bool Equals(AssetCondition? other)
    {
        if (other is null) return false;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => obj is AssetCondition other && Equals(other);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public static bool operator ==(AssetCondition? left, AssetCondition? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(AssetCondition? left, AssetCondition? right) => !(left == right);
}
