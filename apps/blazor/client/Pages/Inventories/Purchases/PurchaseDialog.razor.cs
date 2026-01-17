using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;



namespace AMIS.Blazor.Client.Pages.Inventories.Purchases;

public partial class PurchaseDialog
{
    [Inject] private IApiClient PurchaseClient { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public CreatePurchaseCommand Model { get; set; } = new();
    [Parameter] public Guid? PurchaseId { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }


    private List<SupplierResponse> _suppliers = new();
    private List<ProductResponse> _products = new();

    private double _totalAmount;

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

        Model.Items ??= new List<PurchaseItemDto>();

        _totalAmount = Model.Items.Sum(i => i.Qty * i.UnitPrice);
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
        if (IsCreate is not true && IsCreate is not false) return;

        if (IsCreate == false && (PurchaseId is null || PurchaseId == Guid.Empty))
        {
            Snackbar.Add("Cannot update purchase: missing PurchaseId.", Severity.Error);
            return;
        }

        if (Model.SupplierId is null)
        {
            Snackbar.Add("Select a supplier before saving.", Severity.Warning);
            return;
        }

        if (Model.Items is null || Model.Items.Count == 0)
        {
            Snackbar.Add("Add at least one item before saving.", Severity.Warning);
            return;
        }

        _totalAmount = Model.Items.Sum(i => i.Qty * i.UnitPrice);

        Snackbar.Add(IsCreate.Value ? "Creating purchase order..." : "Updating purchase order...", Severity.Info);

        var saved = false;

        try
        {
            if (IsCreate.Value) // Create Purchase Order
            {
                var response = await PurchaseClient.CreatePurchaseEndpointAsync("1", Model);

                if (response.Id.HasValue)
                {
                    PurchaseId = response.Id.Value;
                    saved = true;
                    Snackbar.Add("Purchase order created successfully!", Severity.Success);
                }
            }
            else // Update Purchase Order
            {
                var update = new UpdatePurchaseCommand
                {
                    Id = PurchaseId!.Value,
                    SupplierId = Model.SupplierId,
                    PurchaseDate = Model.PurchaseDate,
                    Status = Model.Status,
                    DeliveryAddress = Model.DeliveryAddress
                };

                var response = await PurchaseClient.UpdatePurchaseEndpointAsync("1", update.Id, update);

                if (response != null)
                {
                    saved = true;
                    Snackbar.Add("Purchase order updated successfully!", Severity.Success);
                }
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }

        if (saved)
        {
            await Refresh.InvokeAsync();
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
    private void UpdateTotalAmount(double value)
    {
        _totalAmount = value;
        StateHasChanged();
    }

    private void Cancel()
    {
        MudDialog.Cancel();
        Refresh.InvokeAsync();
    }

    // Workflow Action Methods
    private async Task SubmitPurchase()
    {
        if (Model.Status != PurchaseStatus.Draft && Model.Status != PurchaseStatus.Submitted)
        {
            Snackbar.Add("Purchase order must be in Draft status to submit.", Severity.Warning);
            return;
        }

        Model.Status = PurchaseStatus.Submitted;
        await SavePurchaseState();
        Snackbar.Add("Purchase order submitted for approval.", Severity.Success);
    }

    private async Task IssuePurchase()
    {
        if (Model.Status != PurchaseStatus.Submitted)
        {
            Snackbar.Add("Purchase order must be submitted before issuing.", Severity.Warning);
            return;
        }

        // In real implementation, this would send PO to supplier
        Snackbar.Add("Purchase order issued to supplier.", Severity.Success);
    }

    private async Task ClosePurchase()
    {
        if (Model.Status != PurchaseStatus.Delivered)
        {
            Snackbar.Add("Purchase order must be fully delivered before closing.", Severity.Warning);
            return;
        }

        Model.Status = PurchaseStatus.Closed;
        await SavePurchaseState();
        Snackbar.Add("Purchase order closed successfully.", Severity.Success);
    }

    private async Task CancelPurchase()
    {
        if (Model.Status == PurchaseStatus.Closed)
        {
            Snackbar.Add("Cannot cancel a closed purchase order.", Severity.Error);
            return;
        }

        var confirmed = await DialogService.ShowMessageBox(
            "Cancel Purchase Order",
            "Are you sure you want to cancel this purchase order? This action cannot be undone.",
            yesText: "Cancel PO", cancelText: "Keep PO");

        if (confirmed == true)
        {
            Model.Status = PurchaseStatus.Cancelled;
            await SavePurchaseState();
            Snackbar.Add("Purchase order cancelled.", Severity.Info);
        }
    }

    private async Task SavePurchaseState()
    {
        try
        {
            if (PurchaseId is null || PurchaseId == Guid.Empty)
            {
                Snackbar.Add("Cannot update purchase: missing PurchaseId.", Severity.Error);
                return;
            }

            var update = new UpdatePurchaseCommand
            {
                Id = PurchaseId.Value,
                SupplierId = Model.SupplierId,
                PurchaseDate = Model.PurchaseDate,
                Status = Model.Status,
                DeliveryAddress = Model.DeliveryAddress
            };

            await PurchaseClient.UpdatePurchaseEndpointAsync("1", update.Id, update);
            await Refresh.InvokeAsync();
            StateHasChanged();
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Error updating purchase: {ex.Message}", Severity.Error);
        }
    }

    // Workflow UI Helper Methods
    private string GetWorkflowStepIcon() => Model.Status switch
    {
        PurchaseStatus.Draft => Icons.Material.Filled.Edit,
        PurchaseStatus.Submitted => Icons.Material.Filled.Send,
        PurchaseStatus.PartiallyDelivered => Icons.Material.Filled.LocalShipping,
        PurchaseStatus.Delivered => Icons.Material.Filled.Inventory,
        PurchaseStatus.Closed => Icons.Material.Filled.CheckCircle,
        PurchaseStatus.Cancelled => Icons.Material.Filled.Cancel,
        _ => Icons.Material.Filled.Help
    };

    private Color GetWorkflowStepColor() => Model.Status switch
    {
        PurchaseStatus.Draft => Color.Default,
        PurchaseStatus.Submitted => Color.Primary,
        PurchaseStatus.PartiallyDelivered => Color.Info,
        PurchaseStatus.Delivered => Color.Success,
        PurchaseStatus.Closed => Color.Dark,
        PurchaseStatus.Cancelled => Color.Error,
        _ => Color.Default
    };

    private string GetWorkflowStepText() => Model.Status switch
    {
        PurchaseStatus.Draft => "Step 1: Draft",
        PurchaseStatus.Submitted => "Step 1: Submitted",
        PurchaseStatus.PartiallyDelivered => "Step 2: Partially Delivered",
        PurchaseStatus.Delivered => "Step 3: Ready for Inspection",
        PurchaseStatus.Closed => "Complete",
        PurchaseStatus.Cancelled => "Cancelled",
        _ => "Unknown"
    };

}

