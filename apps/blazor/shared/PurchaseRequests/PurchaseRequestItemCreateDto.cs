namespace AMIS.Blazor.Shared.PurchaseRequests;

public class PurchaseRequestItemCreateDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal EstimatedUnitPrice { get; set; }
    public string? Purpose { get; set; }
    public string? Specifications { get; set; }
}
