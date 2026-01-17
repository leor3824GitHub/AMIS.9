namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents the authentication section of the SMRR
/// </summary>
public record ReceivingAuthentication
{
    public string ReceivedByName { get; }
    public string ReceivedBySignature { get; }
    public DateTime ReceivedDate { get; }

    public string NotedByName { get; }
    public string NotedBySignature { get; }
    public DateTime NotedDate { get; }

    public ReceivingAuthentication(
        string receivedByName,
        string receivedBySignature,
        DateTime receivedDate,
        string notedByName,
        string notedBySignature,
        DateTime notedDate)
    {
        if (string.IsNullOrWhiteSpace(receivedByName))
            throw new ArgumentException("Received by name cannot be empty.", nameof(receivedByName));

        if (string.IsNullOrWhiteSpace(receivedBySignature))
            throw new ArgumentException("Received by signature cannot be empty.", nameof(receivedBySignature));

        if (string.IsNullOrWhiteSpace(notedByName))
            throw new ArgumentException("Noted by name cannot be empty.", nameof(notedByName));

        if (string.IsNullOrWhiteSpace(notedBySignature))
            throw new ArgumentException("Noted by signature cannot be empty.", nameof(notedBySignature));

        if (receivedDate > DateTime.UtcNow)
            throw new ArgumentException("Received date cannot be in the future.", nameof(receivedDate));

        if (notedDate > DateTime.UtcNow)
            throw new ArgumentException("Noted date cannot be in the future.", nameof(notedDate));

        ReceivedByName = receivedByName;
        ReceivedBySignature = receivedBySignature;
        ReceivedDate = receivedDate;
        NotedByName = notedByName;
        NotedBySignature = notedBySignature;
        NotedDate = notedDate;
    }
}

