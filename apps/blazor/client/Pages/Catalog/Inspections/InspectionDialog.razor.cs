using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Inspections;
public partial class InspectionDialog
{
    [Inject]
    private IApiClient InspectionClient { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public UpdateInspectionCommand Model { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
   
    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Parameter] public List<InspectionRequestResponse> _inspectionrequests { get; set; }
    [Parameter] public List<EmployeeResponse> _employees { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    private string? _successMessage;
    private FshValidation? _customValidation;

    private DateTime? _inspectionDate;

    private InspectionItemsEditor? _itemsEditor;
    private List<PurchaseItemResponse> _poItems = new();
    private List<ProductResponse> _products = new();

    protected override async Task OnParametersSetAsync()
    {
        _inspectionDate = Model?.InspectionDate == default ? DateTime.Now : Model?.InspectionDate;
        await LoadProducts();
        SyncPoItems();

        // Auto-fill inspector from the selected request if not already set
        if (Model?.InspectionRequestId != null && Model.InspectorId == null)
        {
            var req = _inspectionrequests.FirstOrDefault(r => r.Id == Model.InspectionRequestId);
            if (req != null)
            {
                Model.InspectorId = req.InspectorId;
            }
        }
    }

    private void OnRequestChanged(Guid? requestId)
    {
        Model.InspectionRequestId = requestId;

        // When user changes the request, auto-fill inspector from that request
        var selectedReq = _inspectionrequests.FirstOrDefault(r => r.Id == requestId);
        Model.InspectorId = selectedReq?.InspectorId;

        SyncPoItems();
        StateHasChanged();
    }

    private void SyncPoItems()
    {
        _poItems.Clear();
        var selectedReq = _inspectionrequests.FirstOrDefault(r => r.Id == Model.InspectionRequestId);
        if (selectedReq?.Purchase?.Items != null)
        {
            _poItems = selectedReq.Purchase.Items.ToList();
        }
    }

    private async Task LoadProducts()
    {
        // Fetch products for display names
        var resp = await InspectionClient.SearchProductsEndpointAsync("1", new SearchProductsCommand { PageNumber = 1, PageSize = 1000 });
        _products = resp?.Items?.ToList() ?? new List<ProductResponse>();
    }

    private void OnDateChanged(DateTime? d)
    {
        _inspectionDate = d;
        if (d.HasValue)
        {
            Model.InspectionDate = d.Value;
        }
    }

    private async Task OnValidSubmit()
    {
        if (IsCreate == null) return;

        // ensure date is set
        if (!_inspectionDate.HasValue)
        {
            Snackbar.Add("Please select an Inspection Date.", Severity.Error);
            return;
        }
        Model.InspectionDate = _inspectionDate.Value;

        // ensure InspectorId and Request are set
        if (Model.InspectorId is null || Model.InspectionRequestId is null)
        {
            Snackbar.Add("Please select an Inspector and Inspection Request.", Severity.Error);
            return;
        }

        // validate item inputs
        if (_itemsEditor is null || !_itemsEditor.Validate())
        {
            Snackbar.Add("Please check item quantities and status.", Severity.Error);
            return;
        }

        Snackbar.Add(IsCreate.Value ? "Creating inspection..." : "Updating inspection...", Severity.Info);

        if (IsCreate.Value)
        {
            var selectedReq = _inspectionrequests.FirstOrDefault(r => r.Id == Model.InspectionRequestId);
            var purchaseId = selectedReq?.PurchaseId ?? Guid.Empty;

            var model = new CreateInspectionCommand
            {
                InspectionDate = Model.InspectionDate,
                InspectorId = Model.InspectorId.Value,
                InspectionRequestId = Model.InspectionRequestId.Value,
                PurchaseId = purchaseId,
                Remarks = Model.Remarks ?? string.Empty,
                Items = new List<InspectionItemDto>()
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => InspectionClient.CreateInspectionEndpointAsync("1", model),
                Snackbar,
                Navigation
            );

            if (response?.Id != null && response.Id.Value != Guid.Empty)
            {
                // Create inspection items from user inputs
                foreach (var input in _itemsEditor.Inputs)
                {
                    var createItem = new CreateInspectionItemCommand
                    {
                        InspectionId = response.Id.Value,
                        PurchaseItemId = input.PurchaseItemId,
                        QtyInspected = input.QtyInspected,
                        QtyPassed = input.QtyPassed,
                        QtyFailed = input.QtyFailed,
                        Remarks = input.Remarks ?? string.Empty,
                        InspectionItemStatus = input.Status
                    };

                    await ApiHelper.ExecuteCallGuardedAsync(
                        () => InspectionClient.CreateInspectionItemEndpointAsync("1", createItem),
                        Snackbar,
                        Navigation
                    );
                }

                // After creating items, update inventory balances for accepted quantities
                await UpdateInventoryForAcceptedItemsAsync();

                _successMessage = "Inspection created successfully!";
                Snackbar.Add(_successMessage, Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
        else
        {
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => InspectionClient.UpdateInspectionEndpointAsync("1", Model.Id, Model),
                Snackbar,
                Navigation
            );

            if (response != null)
            {
                _successMessage = "Inspection updated successfully!";
                Snackbar.Add(_successMessage, Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
    }

    private async Task UpdateInventoryForAcceptedItemsAsync()
    {
        try
        {
            if (_itemsEditor is null || _poItems.Count == 0) return;

            // Build quick lookup from purchase item id to its details
            var poMap = _poItems.Where(pi => pi.Id.HasValue).ToDictionary(pi => pi.Id!.Value, pi => pi);

            foreach (var input in _itemsEditor.Inputs)
            {
                if (input.QtyPassed <= 0) continue;
                if (!poMap.TryGetValue(input.PurchaseItemId, out var pi)) continue;

                var productId = pi.ProductId;
                var qtyToAdd = input.QtyPassed;
                var unitPrice = pi.UnitPrice; // double per NSwag client

                // find existing inventory for product
                var invList = await InspectionClient.SearchInventoriesEndpointAsync("1", new SearchInventoriesCommand { ProductId = productId, PageNumber = 1, PageSize = 1 });
                var existing = invList?.Items?.FirstOrDefault();

                if (existing == null || existing.Id == Guid.Empty)
                {
                    // create new inventory
                    await ApiHelper.ExecuteCallGuardedAsync(
                        () => InspectionClient.CreateInventoryEndpointAsync("1", new CreateInventoryCommand
                        {
                            ProductId = productId,
                            Qty = qtyToAdd,
                            AvePrice = unitPrice
                        }),
                        Snackbar,
                        Navigation
                    );
                }
                else
                {
                    // compute new average price and qty
                    var currentQty = existing.Qty;
                    var currentAve = existing.AvePrice;
                    var newQty = currentQty + qtyToAdd;
                    var newAve = newQty > 0 ? ((currentAve * currentQty) + (unitPrice * qtyToAdd)) / newQty : unitPrice;

                    await ApiHelper.ExecuteCallGuardedAsync(
                        () => InspectionClient.UpdateInventoryEndpointAsync("1", existing.Id!.Value, new UpdateInventoryCommand
                        {
                            Id = existing.Id!.Value,
                            ProductId = productId,
                            Qty = newQty,
                            AvePrice = newAve
                        }),
                        Snackbar,
                        Navigation
                    );
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Inventory update failed: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
