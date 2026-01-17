namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Represents recipient information for the issuance
/// </summary>
public record RecipientInfo
{
    public string Name { get; }
    public string Address { get; }
    public string? ContactNumber { get; }

    public RecipientInfo(string name, string address, string? contactNumber = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Recipient name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Recipient address cannot be empty.", nameof(address));

        Name = name;
        Address = address;
        ContactNumber = contactNumber;
    }
}
