using System.ComponentModel;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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

    private PurchaseStatus selectedStatus = PurchaseStatus.Draft;

    private string? _successMessage;
    private FshValidation? _customValidation;
    private bool _uploading;
    private string? _uploadErrorMessage;
    private bool _isUploading;
    private string? searchText;

    protected override async Task OnInitializedAsync()
    {
        
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
}
 public enum PurchaseStatus
{
    Draft, //The purchase order has been created but is not yet finalized. It may still be edited.
    Submitted, //The purchase order has been sent to the supplier or vendor but has not yet been acknowledged or accepted.
    Approved, //The purchase order has been reviewed and authorized by the appropriate personnel within the organization.
    Acknowledged, //The supplier has received and confirmed the purchase order.
    InProgress, //The supplier is processing the order, but it has not yet been shipped.
    Shipped, //The goods or services ordered have been dispatched by the supplier.
    PartiallyDelivered, //Only some of the items in the purchase order have been delivered.
    Delivered, //All items in the purchase order have been successfully delivered.
    Closed, //The purchase order is fully completed, and the transaction is finished. This status usually follows delivery and invoicing.
    Cancelled, //The purchase order has been voided before completion. This could happen due to changes in needs or issues with the supplier.
    Pending //The purchase order is waiting for further action, such as approval or payment.
}
