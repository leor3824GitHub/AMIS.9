namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents the authentication section of the SMRR
/// </summary>
public record ReceivingAuthentication
{
    public string ReceivedByName { get; }
    public DateTime ReceivedDate { get; }

    public string NotedByName { get; }
    public DateTime NotedDate { get; }

    public ReceivingAuthentication(
        string receivedByName,
        DateTime receivedDate,
        string notedByName,
        DateTime notedDate)
    {
        if (string.IsNullOrWhiteSpace(receivedByName))
            throw new ArgumentException("Received by name cannot be empty.", nameof(receivedByName));

        if (string.IsNullOrWhiteSpace(notedByName))
            throw new ArgumentException("Noted by name cannot be empty.", nameof(notedByName));

        if (receivedDate > DateTime.UtcNow)
            throw new ArgumentException("Received date cannot be in the future.", nameof(receivedDate));

        if (notedDate > DateTime.UtcNow)
            throw new ArgumentException("Noted date cannot be in the future.", nameof(notedDate));

        ReceivedByName = receivedByName;
        ReceivedDate = receivedDate;
        NotedByName = notedByName;
        NotedDate = notedDate;
    }
}

