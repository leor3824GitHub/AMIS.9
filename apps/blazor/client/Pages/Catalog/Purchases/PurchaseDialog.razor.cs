using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private List<SupplierResponse> _suppliers = new List<SupplierResponse>();
    private List<ProductResponse> _products = new List<ProductResponse>();

    private PurchaseItemUpdateDto? EditingItem { get; set; }

    private string? _successMessage;
    private FshValidation? _customValidation;
    private string? searchText;

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
    protected override async Task OnInitializedAsync()
    {       
        await LoadSupplierAsync();
        await LoadProductAsync();     

        //if (Model.SupplierId == null && _suppliers.Any())
        //{
        //    Model.SupplierId = _suppliers.First().Id;
        //}
    }

    private async Task LoadProductAsync()
    {
        if (_products.Count == 0)
        {
            var response = await PurchaseClient.SearchProductsEndpointAsync("1", new SearchProductsCommand());
            if (response?.Items != null)
            {
                _products = response.Items.ToList();
            }
        }
    }

    private async Task LoadSupplierAsync()
    {
        if (_suppliers.Count == 0)
        {
            var response = await PurchaseClient.SearchSuppliersEndpointAsync("1", new SearchSuppliersCommand());
            if (response?.Items != null)
            {
                _suppliers = response.Items.ToList();
            }
        }
        StateHasChanged();
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
                Model.Id = (Guid)response.Id;
                StateHasChanged();
                _successMessage = "Purchase created successfully!";
                //MudDialog.Close(DialogResult.Ok(true));
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
                StateHasChanged();
                _successMessage = "Purchase updated successfully!";
                //MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }

        
    }
    //protected override async Task OnParametersSetAsync()
    //{
    //    if (Model != null && Model.SupplierId == null && _suppliers.Count != 0)
    //    {
    //        Model.SupplierId = _suppliers.FirstOrDefault()?.Id;
    //    }
    //}

    //private async Task ShowEditFormDialog(
    // string title,
    // List<PurchaseItemResponse> items,
    // List<ProductResponse> products,
    // PurchaseStatus? status,
    // EventCallback<double> totalAmount,
    // List<SupplierResponse> suppliers)
    //{
    //    var parameters = new DialogParameters
    //{
    //    { nameof(PurchaseItemList.Items), items },
    //    { nameof(PurchaseItemList.Products), products },
    //    { nameof(PurchaseItemList.Status), status },
    //    { nameof(PurchaseItemList.TotalAmount), totalAmount },
    //    { nameof(PurchaseItemList), suppliers }
    //};

    //    var options = new DialogOptions
    //    {
    //        CloseButton = true,
    //        MaxWidth = MaxWidth.Medium,
    //        FullWidth = true
    //    };

    //    var dialog = await DialogService.ShowAsync<PurchaseDialog>(title, parameters, options);
    //    var state = await dialog.Result;

    //}

    private void UpdateTotalAmount(double value)
    {
        Model.TotalAmount = value;
        StateHasChanged();
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }    
   
}
