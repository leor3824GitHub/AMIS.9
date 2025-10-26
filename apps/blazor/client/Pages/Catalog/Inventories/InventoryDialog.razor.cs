using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Inventories;

public partial class InventoryDialog
{
    [Inject]
    private IApiClient ApiClient { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public InventoryEditModel Model { get; set; } = default!;
    [Parameter] public bool? IsCreate { get; set; }
    [Parameter] public List<ProductResponse> Products { get; set; } = new();

    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    private FshValidation? _customValidation;
    private Guid? SelectedProductId;

    protected override void OnParametersSet()
    {
        // Initialize the local nullable selection from the incoming model
        // Prefer the model's value when editing; otherwise pick the first product if available
        if (Model != null && Model.ProductId != Guid.Empty)
        {
            SelectedProductId = Model.ProductId;
        }
        else
        {
            SelectedProductId = Products?.FirstOrDefault()?.Id;
        }
    }

    private async Task OnValidSubmit()
    {
        if (IsCreate == null) return;

        if (SelectedProductId is null)
        {
            Snackbar.Add("Please select a product.", Severity.Warning);
            return;
        }

        // Ensure the bound model gets the selected value (non-nullable)
        Model.ProductId = SelectedProductId.Value;

        Snackbar.Add(IsCreate.Value ? "Creating inventory..." : "Updating inventory...", Severity.Info);

        if (IsCreate.Value)
        {
            var cmd = new CreateInventoryCommand
            {
                ProductId = Model.ProductId,
                Qty = Model.Qty,
                AvePrice = (double)Model.AvePrice
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.CreateInventoryEndpointAsync("1", cmd),
                Snackbar,
                Navigation);

            if (response != null)
            {
                Snackbar.Add("Inventory created successfully!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            var cmd = new UpdateInventoryCommand
            {
                Id = Model.Id,
                ProductId = Model.ProductId,
                Qty = Model.Qty,
                AvePrice = (double)Model.AvePrice
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.UpdateInventoryEndpointAsync("1", cmd.Id, cmd),
                Snackbar,
                Navigation);

            if (response != null)
            {
                Snackbar.Add("Inventory updated successfully!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
    }

    private void Cancel() => MudDialog.Cancel();
}
