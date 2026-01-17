namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Represents the authorization and receipt section of the SMIR
/// </summary>
public record IssuanceAuthorization
{
    public string IssuingOfficerName { get; }
    public string IssuingOfficerSignature { get; }
    public DateTime IssuingDate { get; }

    public string ApprovingOfficerName { get; }
    public string ApprovingOfficerSignature { get; }
    public DateTime ApprovingDate { get; }

    public string RecipientName { get; }
    public string RecipientSignature { get; }
    public DateTime ReceiptDate { get; }

    public string? DriverName { get; }
    public string? DriverSignature { get; }
    public string? BillOfLadingNumber { get; }

    public IssuanceAuthorization(
        string issuingOfficerName,
        string issuingOfficerSignature,
        DateTime issuingDate,
        string approvingOfficerName,
        string approvingOfficerSignature,
        DateTime approvingDate,
        string recipientName,
        string recipientSignature,
        DateTime receiptDate,
        string? driverName = null,
        string? driverSignature = null,
        string? billOfLadingNumber = null)
    {
        if (string.IsNullOrWhiteSpace(issuingOfficerName))
            throw new ArgumentException("Issuing officer name cannot be empty.", nameof(issuingOfficerName));

        if (string.IsNullOrWhiteSpace(issuingOfficerSignature))
            throw new ArgumentException("Issuing officer signature cannot be empty.", nameof(issuingOfficerSignature));

        if (string.IsNullOrWhiteSpace(approvingOfficerName))
            throw new ArgumentException("Approving officer name cannot be empty.", nameof(approvingOfficerName));

        if (string.IsNullOrWhiteSpace(approvingOfficerSignature))
            throw new ArgumentException("Approving officer signature cannot be empty.", nameof(approvingOfficerSignature));

        if (string.IsNullOrWhiteSpace(recipientName))
            throw new ArgumentException("Recipient name cannot be empty.", nameof(recipientName));

        if (string.IsNullOrWhiteSpace(recipientSignature))
            throw new ArgumentException("Recipient signature cannot be empty.", nameof(recipientSignature));

        IssuingOfficerName = issuingOfficerName;
        IssuingOfficerSignature = issuingOfficerSignature;
        IssuingDate = issuingDate;
        ApprovingOfficerName = approvingOfficerName;
        ApprovingOfficerSignature = approvingOfficerSignature;
        ApprovingDate = approvingDate;
        RecipientName = recipientName;
        RecipientSignature = recipientSignature;
        ReceiptDate = receiptDate;
        DriverName = driverName;
        DriverSignature = driverSignature;
        BillOfLadingNumber = billOfLadingNumber;
    }
}
