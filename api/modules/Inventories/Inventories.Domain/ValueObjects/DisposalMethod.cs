namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Value object representing valid asset disposal methods
/// Aligned with COA and DBM guidelines for asset retirement
/// </summary>
public sealed class DisposalMethod : IEquatable<DisposalMethod>
{
    public string Value { get; }

    // Predefined valid disposal methods
    public static readonly DisposalMethod Sale = new("Sale");
    public static readonly DisposalMethod Scrap = new("Scrap");
    public static readonly DisposalMethod Donation = new("Donation");
    public static readonly DisposalMethod Transfer = new("Transfer");
    public static readonly DisposalMethod Condemnation = new("Condemnation");

    private static readonly Dictionary<string, DisposalMethod> ValidMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Sale", Sale },
        { "Scrap", Scrap },
        { "Donation", Donation },
        { "Transfer", Transfer },
        { "Condemnation", Condemnation }
    };

    private DisposalMethod(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Parse string to DisposalMethod
    /// </summary>
    public static DisposalMethod Parse(string method)
    {
        if (string.IsNullOrWhiteSpace(method))
            throw new ArgumentException("Disposal method cannot be null or empty.", nameof(method));

        if (!ValidMethods.TryGetValue(method, out var result))
            throw new ArgumentException($"Invalid disposal method '{method}'. Valid values are: {string.Join(", ", ValidMethods.Keys)}", nameof(method));

        return result;
    }

    /// <summary>
    /// Try parse string to DisposalMethod
    /// </summary>
    public static bool TryParse(string? method, out DisposalMethod? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(method))
            return false;

        return ValidMethods.TryGetValue(method, out result);
    }

    /// <summary>
    /// Get all valid disposal methods
    /// </summary>
    public static IEnumerable<string> GetValidMethods() => ValidMethods.Keys;

    public override string ToString() => Value;
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => Equals(obj as DisposalMethod);

    public bool Equals(DisposalMethod? other)
        => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
}
