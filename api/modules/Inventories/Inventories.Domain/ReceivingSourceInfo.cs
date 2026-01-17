namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents the source/origin of the received materials
/// </summary>
public record ReceivingSourceInfo
{
    public string Name { get; }
    public string Address { get; }
    public DateTime ReceivingDate { get; }

    public ReceivingSourceInfo(string name, string address, DateTime receivingDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Source name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Source address cannot be empty.", nameof(address));

        if (receivingDate > DateTime.UtcNow)
            throw new ArgumentException("Receiving date cannot be in the future.", nameof(receivingDate));

        Name = name;
        Address = address;
        ReceivingDate = receivingDate;
    }
}

