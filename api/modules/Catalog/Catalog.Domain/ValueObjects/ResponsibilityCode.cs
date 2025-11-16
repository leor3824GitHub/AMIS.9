using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

public sealed record ResponsibilityCode
{
    public string Value { get; }

    private ResponsibilityCode(string value)
    {
        Value = value;
    }

    public static ResponsibilityCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new FshException("Responsibility code cannot be empty");

        if (code.Length > 10)
            throw new FshException("Responsibility code cannot exceed 10 characters");

        // Convert to uppercase for consistency
        var normalizedCode = code.Trim().ToUpperInvariant();

        return new ResponsibilityCode(normalizedCode);
    }

    public static implicit operator string(ResponsibilityCode code) => code.Value;
    
    public static implicit operator ResponsibilityCode(string code) => Create(code);

    public override string ToString() => Value;
}
