namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.DTOs;

public record SuppliesAndMaterialsIssuanceReportDto
{
    public Guid Id { get; init; }
    public string SmirNumber { get; init; }
    public DateTime TransactionDate { get; init; }
    public RecipientInfoDto Recipient { get; init; }
    public string IssuanceReason { get; init; }
    public IReadOnlyCollection<IssuanceLineItemDto> LineItems { get; init; }
    public IssuanceAuthorizationDto Authorization { get; init; }
    public decimal TotalAmount { get; init; }
    public IReadOnlyDictionary<string, bool> DistributionStatus { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public string? Notes { get; init; }
}
