using System.Text.Json.Serialization;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

// JsonConverter applied to the enum for correct string serialization/deserialization
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemOperationType
{
    Add,
    Update,
    Remove
}

public class PurchaseItemUpdate
{
    public Guid? Id { get; set; }          // Nullable in case it's a new item
    public Guid? ProductId { get; set; }
    public int Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public PurchaseStatus? ItemStatus { get; set; }

    // New property to specify the operation type (Add, Update, Remove)
    public ItemOperationType OperationType { get; set; }

    // Constructor with the added OperationType parameter
    public PurchaseItemUpdate(Guid? id, Guid? productId, int qty, decimal unitPrice,
                              PurchaseStatus? itemStatus, ItemOperationType operationType)
    {
        Id = id;
        ProductId = productId;
        Qty = qty;
        UnitPrice = unitPrice;
        ItemStatus = itemStatus;
        OperationType = operationType;  // Assign operation type
    }
}
