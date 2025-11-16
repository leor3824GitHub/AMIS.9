namespace AMIS.Blazor.Shared.Purchases
{
    public enum ItemCondition
    {
        Good,
        Damaged,
        Incorrect
    }

    public class GoodsReceiptItemDto
    {
        public Guid PurchaseItemId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyPreviouslyReceived { get; set; }
        public int QtyReceived { get; set; }
        public ItemCondition Condition { get; set; } = ItemCondition.Good;
    }
}
