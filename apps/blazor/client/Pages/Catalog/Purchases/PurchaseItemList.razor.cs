using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Purchases;
public partial class PurchaseItemList
{
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Blazor parameter requires setter for binding mutable collection.")]
    [Parameter]
    public ICollection<PurchaseItemDto> Items { get; set; } = new List<PurchaseItemDto>();
    [Parameter] public IReadOnlyList<ProductResponse> Products { get; set; } = Array.Empty<ProductResponse>();
    [Parameter] public IReadOnlyList<SupplierResponse> Suppliers { get; set; } = Array.Empty<SupplierResponse>();
    [Parameter] public PurchaseStatus? Status { get; set; }
    [Parameter] public Guid? PurchaseId { get; set; }
    [Parameter] public Action<double>? OnTotalAmountChanged { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Parameter] public Action<Guid>? OnItemRemoved { get; set; }
    [Parameter] public EventCallback<ICollection<PurchaseItemDto>> OnItemsChanged { get; set; }

    private Guid? Productid { get; set; }
    private int Qty { get; set; }
    private double Unitprice { get; set; }
    private PurchaseItemDto? EditingItem { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Initialization logic handled in OnParametersSet
        await Task.CompletedTask;
    }

    protected override void OnParametersSet()
    {
        Items ??= new List<PurchaseItemDto>();
    }
   
    private void EditItem(PurchaseItemDto item)
    {
        EditingItem = item;
    }

    private async Task SaveEdit()
    {
        if (EditingItem == null || EditingItem.Qty <= 0 || EditingItem.UnitPrice <= 0)
            return;
        // Local-only update; persist when the purchase form is saved
        Snackbar?.Add("Item updated (pending save)", Severity.Info);
        EditingItem = null;
        UpdateTotalAmount();
        if (OnItemsChanged.HasDelegate)
            await OnItemsChanged.InvokeAsync(Items);

    }

    private void CancelEdit()
    {
        EditingItem = null;
    }

    private async Task AddNewItem()
    {
        if (Productid == null || Qty <= 0 || Unitprice <= 0)
            return;

        var newItem = new PurchaseItemDto
        {
            Id = Guid.NewGuid(),
            ProductId = Productid.Value,
            Qty = Qty,
            UnitPrice = Unitprice,
            ItemStatus = Status ?? PurchaseStatus.Pending
        };
        Items.Add(newItem);

        // Local-only add; persist when the purchase form is saved
        Snackbar?.Add("Item added (pending save)", Severity.Info);
        
        // Reset fields after adding
        Productid = null;
        Qty = 0;
        Unitprice = 0;

        UpdateTotalAmount();
        if (OnItemsChanged.HasDelegate)
            await OnItemsChanged.InvokeAsync(Items);
    }

    private void UpdateTotalAmount()
    {
        double total = Items.Sum(i => i.Qty * i.UnitPrice);
        OnTotalAmountChanged?.Invoke(total);
        StateHasChanged();
    }

    private async Task RemoveItem(PurchaseItemDto item)
    {
        // Always remove locally; if the item existed server-side, notify parent for deletion tracking
        var id = item.Id;
        Items.Remove(item);
        if (id != Guid.Empty)
        {
            OnItemRemoved?.Invoke(id);
        }
        Snackbar?.Add("Item removed (pending save)", Severity.Info);
        UpdateTotalAmount();
        if (OnItemsChanged.HasDelegate)
            await OnItemsChanged.InvokeAsync(Items);
    }

    private static IReadOnlyList<PurchaseStatus> PurchaseStatusList { get; } = Enum.GetValues<PurchaseStatus>();

    private static string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                         .Cast<DisplayAttribute>()
                         .FirstOrDefault();
        return attr?.Name ?? value.ToString();
    }
}
