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
    [Inject] private IApiClient PurchaseClient { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public UpdatePurchaseCommand Model { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }    
    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    

    private List<SupplierResponse> _suppliers = new();
    private List<ProductResponse> _products = new();

    private PurchaseItemUpdateDto? EditingItem { get; set; }

    private string? _successMessage;

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
        if (!Model.SupplierId.HasValue || Model.SupplierId.Value == Guid.Empty)
        {
            Snackbar.Add("Supplier is required.", Severity.Warning);
            return;
        }

        if (IsCreate is not true and not false) return;

        Snackbar.Add(IsCreate.Value ? "Creating purchase order..." : "Updating purchase order...", Severity.Info);

        if (IsCreate.Value) // Create product
        {
            var model = Model.Adapt<CreatePurchaseCommand>();
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => PurchaseClient.CreatePurchaseEndpointAsync("1", model),
                Snackbar,
                Navigation
            );

            if (response?.Id is not null)
            {
                Model.Id = (Guid)response.Id;
                StateHasChanged();
                _successMessage = "Purchase order created successfully!";
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
                _successMessage = "Purchase order updated successfully!";
                Refresh?.Invoke();
            }
        }

        
    }
    
    private void UpdateTotalAmount(double value)
    {
        Model.TotalAmount = value;
        StateHasChanged();
    }

    private void Cancel() => MudDialog.Cancel();

}
