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
    [Parameter] public CreatePurchaseCommand Model { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    

    private List<SupplierResponse> _suppliers = new();
    private List<ProductResponse> _products = new();

    private PurchaseItemDto? EditingItem { get; set; }

    private static IReadOnlyList<PurchaseStatus> PurchaseStatusList { get; } = Enum.GetValues<PurchaseStatus>();

    private static string GetDisplayName(Enum value)
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
        {
            if (IsCreate is not true && IsCreate is not false) return;

            Snackbar.Add(IsCreate.Value ? "Creating purchase order..." : "Updating purchase order...", Severity.Info);

            try
            {
                if (IsCreate.Value) // Create Purchase Order
                {
                    var model = Model.Adapt<CreatePurchaseCommand>();

                    var response = await PurchaseClient.CreatePurchaseEndpointAsync("1", model);

                    if (response.Id.HasValue)
                    {
                        Model.Id = (Guid)response.Id;
                        StateHasChanged();
                        Snackbar.Add("Purchase order created successfully!", Severity.Success);
                        await Refresh.InvokeAsync();

                    }
                }
                else // Update Purchase Order
                {
                    var model = Model.Adapt<UpdatePurchaseCommand>();

                    var response = await PurchaseClient.UpdatePurchaseEndpointAsync("1", model.Id, model);

                    if (response != null)
                    {
                        StateHasChanged();
                        Snackbar.Add("Purchase order updated successfully!", Severity.Success);
                        await Refresh.InvokeAsync();

                    }
                }
            }
            catch (ApiException ex)
            {
                //if (ex.StatusCode == 400)
                //{
                //    var errors = await ex.GetValidationErrorsAsync();
                //    Validation?.DisplayErrors(errors);
                //}
                //else
                //{
                    Snackbar.Add($"Error: {ex.Message}", Severity.Error);
                //}
            }
        }
    }    
    private void UpdateTotalAmount(double value)
    {
        Model.TotalAmount = value;
        StateHasChanged();
    }

    private void Cancel() 
    {
        MudDialog.Cancel();
        Refresh.InvokeAsync();
    } 

}
