using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMIS.Blazor.Client.Pages.Catalog.Acceptances;

public sealed class AcceptanceFormModel
{
    private readonly List<AcceptanceItemInput> _items = new();

    public Guid? Id { get; set; }

    [Required]
    public Guid? PurchaseId { get; set; }

    [Required]
    public Guid? SupplyOfficerId { get; set; }

    [Required]
    public DateTime AcceptanceDate { get; set; } = DateTime.Today;

    public string? Remarks { get; set; }

    public IList<AcceptanceItemInput> Items => _items;

    public static AcceptanceFormModel CreateDefault() => new()
    {
        AcceptanceDate = DateTime.Today
    };

    public void ReplaceItems(IEnumerable<AcceptanceItemInput> items)
    {
        _items.Clear();
        _items.AddRange(items);
    }

    public void ClearItems() => _items.Clear();

    public sealed class AcceptanceItemInput
    {
        public Guid? AcceptanceItemId { get; set; }
        public Guid PurchaseItemId { get; set; }
        public int OrderedQty { get; set; }
        public int QtyAccepted { get; set; }
        // Computed client-side for UX guidance
        public int ApprovedQty { get; set; }
        public int AcceptedSoFar { get; set; }
        public int Remaining => Math.Max(ApprovedQty - AcceptedSoFar, 0);
        public bool IsDepleted => Remaining <= 0;
        public string ProductName { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }
}
