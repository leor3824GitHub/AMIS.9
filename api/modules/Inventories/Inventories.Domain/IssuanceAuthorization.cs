namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents the authorization and receipt section of the SMIR
/// </summary>
public record IssuanceAuthorization
{
    public string IssuingOfficerName { get; }
    public DateTime IssuingDate { get; }

    public string ApprovingOfficerName { get; }
    public DateTime ApprovingDate { get; }

    public string RecipientName { get; }
    public DateTime ReceiptDate { get; }

    public string? DriverName { get; }
    public string? BillOfLadingNumber { get; }

    public IssuanceAuthorization(
        string issuingOfficerName,
        DateTime issuingDate,
        string approvingOfficerName,
        DateTime approvingDate,
        string recipientName,
        DateTime receiptDate,
        string? driverName = null,
        string? billOfLadingNumber = null)
    {
        if (string.IsNullOrWhiteSpace(issuingOfficerName))
            throw new ArgumentException("Issuing officer name cannot be empty.", nameof(issuingOfficerName));

        if (string.IsNullOrWhiteSpace(approvingOfficerName))
            throw new ArgumentException("Approving officer name cannot be empty.", nameof(approvingOfficerName));

        if (string.IsNullOrWhiteSpace(recipientName))
            throw new ArgumentException("Recipient name cannot be empty.", nameof(recipientName));

        IssuingOfficerName = issuingOfficerName;
        IssuingDate = issuingDate;
        ApprovingOfficerName = approvingOfficerName;
        ApprovingDate = approvingDate;
        RecipientName = recipientName;
        ReceiptDate = receiptDate;
        DriverName = driverName;
        BillOfLadingNumber = billOfLadingNumber;
    }
}

