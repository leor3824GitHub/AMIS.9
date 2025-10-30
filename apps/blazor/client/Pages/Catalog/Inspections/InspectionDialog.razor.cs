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
    [EditorRequired]
    [Parameter]
    public required UpdateInspectionCommand Model { get; set; } = default!;
    [Parameter] public EventCallback OnCancel { get; set; }
   
    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool IsCreate { get; set; } = false;
    [Parameter] public bool IsReadOnly { get; set; } = false;
    [Parameter] public List<InspectionRequestResponse> InspectionRequests { get; set; } = new();
    [Parameter] public List<EmployeeResponse> Employees { get; set; } = new();
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    private string? _successMessage;
    private FshValidation? _customValidation;

    private DateTime? _inspectionDate;
    private bool _isSaving;
    private bool _inspectorAutoFilled;

    private InspectionItemsEditor? _itemsEditor;
    private List<PurchaseItemResponse> _poItems = new();
    private List<ProductResponse> _products = new();

    protected override async Task OnParametersSetAsync()
    {
        // Fallback: if dialog wasn't passed IsCreate, infer it from the model (no Id means create)
        if (!IsCreate)
        {
            try
            {
                // many NSwag Update* commands include Id; treat default/empty as create
                if (Model == null || Model.Id == Guid.Empty)
                {
                    IsCreate = true;
                }
            }
            catch
            {
                // ignore if Model has no Id property
            }
        }

        _inspectionDate = Model?.InspectionDate == default ? DateTime.Now : Model?.InspectionDate;
        await LoadProducts();
        SyncPoItems();

        // Auto-fill inspector from the selected request if not already set
        if (Model?.InspectionRequestId != null && Model.InspectorId == null)
        {
            var req = InspectionRequests.FirstOrDefault(r => r.Id == Model.InspectionRequestId);
            if (req?.InspectorId != null)
            {
                Model.InspectorId = req.InspectorId;
                _inspectorAutoFilled = true;
            }
        }
    }

    private void OnRequestChanged(Guid? requestId)
    {
        Model.InspectionRequestId = requestId;

        // When user changes the request, auto-fill inspector from that request
        var selectedReq = InspectionRequests.FirstOrDefault(r => r.Id == requestId);
        if (selectedReq?.InspectorId != null)
        {
            Model.InspectorId = selectedReq.InspectorId;
            _inspectorAutoFilled = true;
        }
        else
        {
            _inspectorAutoFilled = false;
        }

        SyncPoItems();
        StateHasChanged();
    }

    private void SyncPoItems()
    {
        _poItems.Clear();
        var selectedReq = InspectionRequests.FirstOrDefault(r => r.Id == Model.InspectionRequestId);
        if (selectedReq?.Purchase?.Items != null)
        {
            _poItems = selectedReq.Purchase.Items.ToList();
        }
    }

    private async Task LoadProducts()
    {
        try
        {
            // Fetch products for display names
            var resp = await InspectionClient.SearchProductsEndpointAsync("1", new SearchProductsCommand { PageNumber = 1, PageSize = 1000 });
            _products = resp?.Items?.ToList() ?? new List<ProductResponse>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load products: {ex.Message}", Severity.Error);
            _products = new List<ProductResponse>();
        }
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
        if (IsReadOnly)
        {
            // No-op in read-only mode
            Snackbar.Add("This inspection is read-only.", Severity.Info);
            return;
        }
        // Set saving state
        _isSaving = true;
        StateHasChanged();

        try
        {
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

            // Validate that at least one item exists
            if (_itemsEditor.Inputs?.Count == 0)
            {
                Snackbar.Add("Please add at least one inspection item.", Severity.Warning);
                return;
            }

            // Validate that at least one item is inspected
            if (_itemsEditor.Inputs == null || _itemsEditor.Inputs.All(i => i.QtyInspected == 0))
            {
                Snackbar.Add("Please inspect at least one item.", Severity.Warning);
                return;
            }

            Snackbar.Add(IsCreate ? "Creating inspection..." : "Updating inspection...", Severity.Info);

            if (IsCreate)
            {
            var selectedReq = InspectionRequests.FirstOrDefault(r => r.Id == Model.InspectionRequestId);
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
                // Skip items that are already inspected (single-shot) and items with zero inspected qty
                foreach (var input in _itemsEditor.Inputs.Where(i => !i.AlreadyInspected && i.QtyInspected > 0))
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

                // Inventory updates are now handled server-side upon approval via domain events.
                // No client-side inventory mutation here to avoid duplication.

                // Determine request status: Failed if any item has failures; otherwise Completed
                var requestStatus = ResolveRequestStatusFromEditor();

                // Update inspection request status accordingly
                await UpdateInspectionRequestStatusAsync(
                    Model.InspectionRequestId,
                    purchaseId,
                    Model.InspectorId,
                    requestStatus);

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
                // Determine request status after update
                var purchaseId = InspectionRequests.FirstOrDefault(r => r.Id == Model.InspectionRequestId)?.PurchaseId;
                var requestStatus = ResolveRequestStatusFromEditor();

                await UpdateInspectionRequestStatusAsync(
                    Model.InspectionRequestId,
                    purchaseId,
                    Model.InspectorId,
                    requestStatus);

                _successMessage = "Inspection updated successfully!";
                Snackbar.Add(_successMessage, Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
            }
        }
        finally
        {
            _isSaving = false;
            StateHasChanged();
        }
    }

    private InspectionRequestStatus ResolveRequestStatusFromEditor()
    {
        if (_itemsEditor?.Inputs is null || _itemsEditor.Inputs.Count == 0)
            return InspectionRequestStatus.Pending;

        var totalInspected = _itemsEditor.Inputs.Sum(i => i.QtyInspected);
        var anyFailed = _itemsEditor.Inputs.Any(i => i.QtyFailed > 0);

        if (totalInspected == 0)
            return InspectionRequestStatus.Pending;

        return anyFailed ? InspectionRequestStatus.Failed : InspectionRequestStatus.Completed;
    }

    private async Task UpdateInspectionRequestStatusAsync(Guid? requestId, Guid? purchaseId, Guid? inspectorId, InspectionRequestStatus status)
    {
        try
        {
            if (!requestId.HasValue) return;

            var cmd = new UpdateInspectionRequestCommand
            {
                Id = requestId.Value,
                PurchaseId = purchaseId,
                InspectorId = inspectorId,
                Status = status
            };

            await ApiHelper.ExecuteCallGuardedAsync(
                () => InspectionClient.UpdateInspectionRequestEndpointAsync("1", requestId.Value, cmd),
                Snackbar,
                Navigation
            );
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to update inspection request status: {ex.Message}", Severity.Error);
        }
    }

    // Note: Inventory adjustments are performed by the API when an inspection is approved.

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
