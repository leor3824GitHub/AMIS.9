using AMIS.Framework.Core.Domain;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents the reason for issuing supplies and materials
/// </summary>
public record IssuanceReason
{
    public static readonly IssuanceReason Sale = new("Sale");
    public static readonly IssuanceReason Transfer = new("Transfer");
    public static readonly IssuanceReason Donation = new("Donation");

    public string Value { get; }

    private IssuanceReason(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Issuance reason cannot be empty.", nameof(value));

        Value = value;
    }

    public static IssuanceReason FromString(string value) => value switch
    {
        "Sale" => Sale,
        "Transfer" => Transfer,
        "Donation" => Donation,
        _ => throw new ArgumentException($"Invalid issuance reason: {value}", nameof(value))
    };

    public override string ToString() => Value;
}

