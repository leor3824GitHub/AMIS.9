namespace AMIS.WebApi.Catalog.Domain;
public class PurchaseItemUpdate
{
    public Guid? Id { get; set; }          // Nullable in case it's a new item
    public Guid ProductId { get; set; }
    public int Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Status { get; set; }

    public PurchaseItemUpdate(Guid? id, Guid productId, int qty, decimal unitPrice, string? status)
    {
        Id = id;
        ProductId = productId;
        Qty = qty;
        UnitPrice = unitPrice;
        Status = status;
    }
}
