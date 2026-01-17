namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents the nature/type of receiving transaction
/// </summary>
public record ReceivingTransactionType
{
    public static readonly ReceivingTransactionType Purchase = new("Purchase");
    public static readonly ReceivingTransactionType Transfer = new("Transfer");
    public static readonly ReceivingTransactionType Donation = new("Donation");
    public static readonly ReceivingTransactionType Others = new("Others");

    public string Value { get; }

    private ReceivingTransactionType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Transaction type cannot be empty.", nameof(value));

        Value = value;
    }

    public static ReceivingTransactionType FromString(string value) => value switch
    {
        "Purchase" => Purchase,
        "Transfer" => Transfer,
        "Donation" => Donation,
        "Others" => Others,
        _ => throw new ArgumentException($"Invalid transaction type: {value}", nameof(value))
    };

    public override string ToString() => Value;
}

