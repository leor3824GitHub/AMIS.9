using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Purchases;
public partial class PurchaseItemList
{
    [Inject]
    protected IApiClient Purchaseclient { get; set; } = default!;
    [Inject]
    private ISnackbar? Snackbar { get; set; }
    [Parameter] public List<ProductResponse> Products { get; set; } = new();
    [Parameter] public List<SupplierResponse> Suppliers { get; set; } = new();
    [Parameter] public PurchaseStatus? Status { get; set; }
    [Parameter] public Guid? PurchaseId { get; set; }
    [Parameter] public Action<Double> OnTotalAmountChanged { get; set; }
    [Parameter]
    public ICollection<PurchaseItemUpdateDto> Items { get; set; } = new List<PurchaseItemUpdateDto>();
    [Parameter]
    public EventCallback<ICollection<PurchaseItemUpdateDto>> ItemsChanged { get; set; }


    private Guid? Productid { get; set; }
    private int Qty { get; set; }
    private double Unitprice { get; set; }
    private PurchaseStatus? Itemstatus { get; set; }
    private PurchaseItemUpdateDto EditingItem { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //Model ??= new UpdatePurchaseCommand();
        //Model.Items ??= new List<PurchaseItemUpdateDto>();
        //await LoadSupplierAsync();
        //await LoadProductAsync();
    }
   
    private void EditItem(PurchaseItemUpdateDto item)
    {
        EditingItem = item;
    }

    private async Task SaveEdit()
    {
        EditingItem = null;
        await ItemsChanged.InvokeAsync(Items); // Notify parent
        UpdateTotalAmount();
    }

    private void CancelEdit()
    {
        EditingItem = null;
    }

    private async Task AddNewItem()
    {
        if (Productid == null || Qty <= 0 || Unitprice <= 0)
            return;

        var newItem = new PurchaseItemUpdateDto
        {
            Id = null,
            ProductId = Productid.Value,
            Qty = Qty,
            UnitPrice = Unitprice,
            ItemStatus = Itemstatus ?? Status ?? PurchaseStatus.Pending
        };

        Items.Add(newItem);
        await ItemsChanged.InvokeAsync(Items); // Notify parent

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

    private async Task RemoveItem(PurchaseItemUpdateDto item)
    {
        Items.Remove(item);
        await ItemsChanged.InvokeAsync(Items); // Notify parent
        UpdateTotalAmount();

    }

    private List<PurchaseStatus> PurchaseStatusList =>
    Enum.GetValues(typeof(PurchaseStatus)).Cast<PurchaseStatus>().ToList();
    private static string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                         .Cast<DisplayAttribute>()
                         .FirstOrDefault();
        return attr?.Name ?? value.ToString();
    }
}
