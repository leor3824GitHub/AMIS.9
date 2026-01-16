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
    protected IApiClient Purchaseclient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Blazor parameter requires setter for binding mutable collection.")]
    [Parameter]
    public ICollection<PurchaseItemDto> Items { get; set; } = new List<PurchaseItemDto>();
    [Parameter] public IReadOnlyList<ProductResponse> Products { get; set; } = Array.Empty<ProductResponse>();
    [Parameter] public List<SupplierResponse> Suppliers { get; set; } = new();
    [Parameter] public PurchaseStatus? Status { get; set; }
    [Parameter] public Guid? PurchaseId { get; set; }
    [Parameter] public Action<Double> OnTotalAmountChanged { get; set; }
    [Parameter] public bool? IsCreate { get; set; }

    private Guid? Productid { get; set; }
    private int Qty { get; set; }
    private double Unitprice { get; set; }
    private PurchaseItemDto? EditingItem { get; set; }
    private bool ItemsLocked => Status is PurchaseStatus.Closed or PurchaseStatus.Cancelled;

    protected override async Task OnInitializedAsync()
    {
        //Model ??= new UpdatePurchaseCommand();
        //Model.Items ??= new List<PurchaseItemDto>();
        //await LoadSupplierAsync();
        //await LoadProductAsync();
    }

    protected override void OnParametersSet()
    {
        Items ??= new List<PurchaseItemDto>();
    }

    private void EditItem(PurchaseItemDto item)
    {
        if (ItemsLocked)
        {
            Snackbar?.Add("Items cannot be edited when the PO is locked.", Severity.Info);
            return;
        }

        EditingItem = item;
    }

    private void SaveEdit()
    {
        if (ItemsLocked)
        {
            Snackbar?.Add("Items cannot be edited when the PO is locked.", Severity.Info);
            return;
        }

        if (EditingItem == null || EditingItem.Qty <= 0 || EditingItem.UnitPrice <= 0)
            return;
        try
        {
            if (IsCreate == false)
            {
                // TODO: Use nested endpoint /purchases/{purchaseId}/items/{itemId}
                // var model = EditingItem.Adapt<UpdatePurchaseItemCommand>();
                // await ApiClient.UpdatePurchaseItemAsync(PurchaseId, EditingItem.Id, model);
                Snackbar?.Add("Item editing temporarily disabled. Use purchase management instead.", Severity.Warning);
            }

            EditingItem = null;

            UpdateTotalAmount();

        }
        catch (ApiException ex)
        {
            Snackbar?.Add($"Error: {ex.Message}", Severity.Error);
            Snackbar?.Add("The item product was not updated.", Severity.Error);
        }

    }

    private void CancelEdit()
    {
        EditingItem = null;
    }

    private void AddNewItem()
    {
        if (Productid == null)
        {
            Snackbar?.Add("Select a product before adding.", Severity.Warning);
            return;
        }

        if (ItemsLocked)
        {
            Snackbar?.Add("This purchase order is locked. Add items via workflow actions.", Severity.Info);
            return;
        }

        if (Qty <= 0 || Unitprice <= 0)
        {
            Snackbar?.Add("Quantity and unit price must be greater than zero.", Severity.Warning);
            return;
        }

        var existing = Items.FirstOrDefault(i => i.ProductId == Productid.Value);
        if (existing is not null)
        {
            existing.Qty += Qty;
            existing.UnitPrice = Unitprice;
            Snackbar?.Add("Updated existing line item.", Severity.Info);
        }
        else
        {
            var newItem = new PurchaseItemDto
            {
                ProductId = Productid.Value,
                Qty = Qty,
                UnitPrice = Unitprice,
                ItemStatus = Status ?? PurchaseStatus.Submitted
            };
            Items.Add(newItem);
        }

        if (IsCreate == false)
        {
            // TODO: Use nested endpoint POST /purchases/{purchaseId}/items
            // var model = new { PurchaseId, ProductId, Qty, UnitPrice, ItemStatus };
            // await ApiClient.AddPurchaseItemAsync(PurchaseId, model);
            Snackbar?.Add("Item addition temporarily disabled. Use purchase management instead.", Severity.Warning);
        }

        // Reset fields after adding
        Productid = null;
        Qty = 0;
        Unitprice = 0;

        UpdateTotalAmount();
    }

    private void UpdateTotalAmount()
    {
        double total = Items.Sum(i => i.Qty * i.UnitPrice);
        OnTotalAmountChanged?.Invoke(total);
        StateHasChanged();
    }

    private void RemoveItem(PurchaseItemDto item)
    {
        if (ItemsLocked)
        {
            Snackbar?.Add("This purchase order is locked. Item removal is disabled.", Severity.Info);
            return;
        }

        try
        {
            // TODO: Use nested endpoint DELETE /purchases/{purchaseId}/items/{itemId}
            // await ApiClient.DeletePurchaseItemAsync(PurchaseId, id);
            Snackbar?.Add("Item deletion temporarily disabled. Use purchase management instead.", Severity.Warning);

            Items.Remove(item);
            UpdateTotalAmount();
        }
        catch (ApiException ex)
        {
            Snackbar?.Add($"Error: {ex.Message}", Severity.Error);
            Snackbar?.Add("The item product was not removed.", Severity.Error);
        }
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
