namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.DTOs;

public record IssuanceAuthorizationDto
{
    public string IssuingOfficerName { get; init; }
    public string IssuingOfficerSignature { get; init; }
    public DateTime IssuingDate { get; init; }

    public string ApprovingOfficerName { get; init; }
    public string ApprovingOfficerSignature { get; init; }
    public DateTime ApprovingDate { get; init; }

    public string RecipientName { get; init; }
    public string RecipientSignature { get; init; }
    public DateTime ReceiptDate { get; init; }

    public string? DriverName { get; init; }
    public string? DriverSignature { get; init; }
    public string? BillOfLadingNumber { get; init; }
}
