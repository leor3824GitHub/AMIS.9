namespace AMIS.Blazor.Shared.Canvasses;

public class CanvassResponse
{
    public Guid Id { get; set; }
    public string? CanvassNumber { get; set; }
    public Guid PurchaseRequestId { get; set; }
    public string? PurchaseRequestNumber { get; set; }
    public DateTime CanvassDate { get; set; }
    public string? Status { get; set; }
    public string? Remarks { get; set; }
}
