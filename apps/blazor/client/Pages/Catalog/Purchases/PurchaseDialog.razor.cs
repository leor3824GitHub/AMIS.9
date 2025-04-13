using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace AMIS.Blazor.Client.Pages.Catalog.Purchases;
public partial class PurchaseDialog
{
    [Inject]
    private IApiClient PurchaseClient { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public UpdatePurchaseCommand Model { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
   
    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Parameter] public List<SupplierResponse> _suppliers { get; set; }
    [Parameter] public List<ProductResponse> _products { get; set; }
    
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    private string? _successMessage;
    private FshValidation? _customValidation;
    private bool _uploading;
    private string? _uploadErrorMessage;
    private bool _isUploading;
    private string? searchText;

    private int Qty;
    private double Unitprice;
    private Guid? Productid;

    private List<PurchaseStatus> PurchaseStatusList =>
    Enum.GetValues(typeof(PurchaseStatus)).Cast<PurchaseStatus>().ToList();
    private string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                         .Cast<DisplayAttribute>()
                         .FirstOrDefault();
        return attr?.Name ?? value.ToString();
    }
    protected override void OnInitialized()
    {
        Model ??= new UpdatePurchaseCommand();
        Model.Items ??= new List<PurchaseItemUpdateDto>();
    }
    private async Task OnValidSubmit()
    {
        if (IsCreate == null) return;

        Snackbar.Add(IsCreate.Value ? "Creating product..." : "Updating product...", Severity.Info);

        if (IsCreate.Value) // Create product
        {
            var model = Model.Adapt<CreatePurchaseCommand>();
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => PurchaseClient.CreatePurchaseEndpointAsync("1", model),
                Snackbar,
                Navigation
            );

            if (response != null)
            {
                _successMessage = "Purchase created successfully!";
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
        else // Update product
        {
            var model = Model.Adapt<UpdatePurchaseCommand>();
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => PurchaseClient.UpdatePurchaseEndpointAsync("1", model.Id, model),
                Snackbar,
                Navigation
            );

            if (response != null)
            {
                _successMessage = "Purchase updated successfully!";
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
    }
    protected override async Task OnParametersSetAsync()
    {
        if (Model != null && Model.SupplierId == null && _suppliers.Count != 0)
        {
            Model.SupplierId = _suppliers.FirstOrDefault()?.Id;            
        }
    }     

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private void OnItemEdited(object item)
    {
        // Ensure the item is of the correct type
        if (item is PurchaseItemUpdateDto editedItem)
        {
            // Find the index of the item in the collection
            var index = Model.Items.ToList().FindIndex(i => i.Id == editedItem.Id);

            if (index >= 0)
            {
                // Update the existing item
                Model.Items.ElementAt(index).Qty = editedItem.Qty;
                Model.Items.ElementAt(index).UnitPrice = editedItem.UnitPrice;
                Model.Items.ElementAt(index).ItemStatus = editedItem.ItemStatus;
                Model.Items.ElementAt(index).ProductId = editedItem.ProductId;
            }
            else
            {
                // Add the item if it doesn't exist
                Model.Items.Add(editedItem);
            }

            // Notify the UI to refresh
            StateHasChanged();
        }
        else
        {
            // Log or handle invalid item type
            Snackbar.Add("Invalid item type provided for editing.", Severity.Error);
        }
    }
    private void OnRowEditCancel(object item)
    {
        // Ensure the item is of the correct type
        if (item is PurchaseItemUpdateDto canceledItem)
        {
            // Find the original item in the collection
            var originalItem = Model.Items.FirstOrDefault(i => i.Id == canceledItem.Id);

            if (originalItem != null)
            {
                // Revert any changes made to the item
                canceledItem.Qty = originalItem.Qty;
                canceledItem.UnitPrice = originalItem.UnitPrice;
                canceledItem.ItemStatus = originalItem.ItemStatus;
                canceledItem.ProductId = originalItem.ProductId;
            }

            // Notify the UI to refresh
            StateHasChanged();
        }
        else
        {
            // Log or handle invalid item type
            Snackbar.Add("Invalid item type provided for cancellation.", Severity.Error);
        }
    }

    private void AddNewItem()
    {
        
        var newItem = new PurchaseItemUpdateDto
        {
            ProductId = Productid ?? Guid.Empty, // Ensure this is valid
            Qty = Qty,
            UnitPrice = Unitprice,
            ItemStatus = Model.Status,
        };

        Model.Items.Add(newItem);

        Productid = null;
        Qty = 0;
        Unitprice = 0;
    }

    private void RemoveItem(PurchaseItemUpdateDto item)
    {
        Model.Items.Remove(item);
    }
}
