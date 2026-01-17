namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.DTOs;

public record IssuanceLineItemDto
{
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime AcquisitionDate { get; init; }
    public decimal Quantity { get; init; }
    public string Unit { get; init; }
    public decimal UnitCost { get; init; }
    public decimal Amount { get; init; }
}
