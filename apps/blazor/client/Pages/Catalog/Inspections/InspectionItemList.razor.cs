using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Inspections;
public partial class InspectionItemList
{
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Blazor parameter requires setter for binding mutable collection.")]
    [Parameter]
    public ICollection<PurchaseItemDto> Items { get; set; } = new List<PurchaseItemDto>();
    [Parameter] public IReadOnlyList<ProductResponse> Products { get; set; } = Array.Empty<ProductResponse>();
    [Parameter] public PurchaseStatus? Status { get; set; }
    [Parameter] public Guid? PurchaseId { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Parameter] public EventCallback<ICollection<PurchaseItemDto>> OnItemsChanged { get; set; }

    private Guid? Productid { get; set; }
    private int Qty { get; set; }
    private double Unitprice { get; set; }
    private PurchaseItemDto? EditingItem { get; set; }

    protected override async Task OnInitializedAsync()
    {
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
        // Persisting is now handled by the parent via UpdatePurchaseWithItems aggregate endpoint.
        // Here we just notify that items changed.
        EditingItem = null;
        Snackbar?.Add("Item updated locally. Don't forget to save changes.", Severity.Info);
        if (OnItemsChanged.HasDelegate)
            await OnItemsChanged.InvokeAsync(Items);

        StateHasChanged();

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
        Snackbar?.Add("Item added locally.", Severity.Success);
        if (OnItemsChanged.HasDelegate)
            await OnItemsChanged.InvokeAsync(Items);
        
        // Reset fields after adding
        Productid = null;
        Qty = 0;
        Unitprice = 0;

    StateHasChanged();
    }

    private async Task RemoveItem(PurchaseItemDto item)
    {
        if (item.Id == Guid.Empty)
        {
            Snackbar?.Add("Item ID is null and cannot be removed.", Severity.Error);
            return;
        }
        Items.Remove(item);
        Snackbar?.Add("Item removed locally.", Severity.Success);
        if (OnItemsChanged.HasDelegate)
            await OnItemsChanged.InvokeAsync(Items);
        StateHasChanged();
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
