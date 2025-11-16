namespace AMIS.Blazor.Shared.Purchases
{
    public class GoodsReceiptCommand
    {
        public Guid PurchaseId { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public List<GoodsReceiptItemDto> Items { get; set; } = new();
    }
}
