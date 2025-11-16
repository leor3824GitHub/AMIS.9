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
    private List<ProductResponse> _products = new();
    private List<IBrowserFile> _uploadedFiles = new();

    private ICollection<PurchaseItemResponse>? GetPurchaseItemsForSelectedRequest()
    {
        if (Model?.InspectionRequestId == null) return null;
        var selectedReq = InspectionRequests.FirstOrDefault(r => r.Id == Model.InspectionRequestId);
        return selectedReq?.Purchase?.Items;
    }

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

        // Load allowed (Assigned/InProgress) requests from API when dialog is used for creating
        if (IsCreate)
        {
            try
            {
                var statuses = new List<InspectionRequestStatus> { InspectionRequestStatus.Assigned, InspectionRequestStatus.InProgress };
                var resp = await InspectionClient.SearchInspectionRequestsEndpointAsync("1", new SearchInspectionRequestsCommand
                {
                    PageNumber = 1,
                    PageSize = 200,
                    Statuses = statuses
                });
                var serverList = resp?.Items?.ToList() ?? new List<InspectionRequestResponse>();
                // Merge if parent passed some items (e.g., single pre-selected) to keep selection stable
                if (InspectionRequests.Count == 0)
                {
                    InspectionRequests = serverList;
                }
                else
                {
                    var existing = InspectionRequests.Select(r => r.Id).ToHashSet();
                    foreach (var r in serverList)
                    {
                        if (!existing.Contains(r.Id)) InspectionRequests.Add(r);
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to load requests: {ex.Message}", Severity.Error);
            }
        }

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

        StateHasChanged();
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

            // Convert editor inputs to InspectionItemDto
            var itemsToSubmit = _itemsEditor?.Inputs?
                .Where(i => !i.AlreadyInspected && i.QtyInspected > 0)
                .Select(i => new InspectionItemDto
                {
                    InspectionId = Guid.Empty, // Will be set by the command handler
                    PurchaseItemId = i.PurchaseItemId,
                    QtyInspected = i.QtyInspected,
                    QtyPassed = i.QtyPassed,
                    QtyFailed = i.QtyFailed,
                    Remarks = i.Remarks ?? string.Empty,
                    InspectionItemStatus = i.Status
                })
                .ToList();

            var model = new CreateInspectionCommand
            {
                InspectionDate = Model.InspectionDate,
                InspectorId = Model.InspectorId.Value,
                InspectionRequestId = Model.InspectionRequestId.Value,
                PurchaseId = purchaseId,
                Remarks = Model.Remarks ?? string.Empty,
                Items = itemsToSubmit
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => InspectionClient.CreateInspectionEndpointAsync("1", model),
                Snackbar,
                Navigation
            );

            if (response?.Id != null && response.Id.Value != Guid.Empty)
            {
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

    private void OnFilesChanged(InputFileChangeEventArgs e)
    {
        _uploadedFiles.Clear();
        foreach (var file in e.GetMultipleFiles(5))
        {
            _uploadedFiles.Add(file);
        }
        StateHasChanged();
    }

    private void RemoveFile(IBrowserFile file)
    {
        _uploadedFiles.Remove(file);
        StateHasChanged();
    }

    private string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }

    private async Task ApproveInspection()
    {
        if (IsReadOnly)
        {
            Snackbar.Add("This inspection is read-only.", Severity.Info);
            return;
        }

        var confirmed = await DialogService.ShowMessageBox(
            "Accept & Add to Inventory",
            "Are you sure you want to approve this inspection? This will automatically update the inventory with accepted quantities.",
            yesText: "Accept", cancelText: "Cancel");

        if (confirmed != true) return;

        _isSaving = true;
        StateHasChanged();

        try
        {
            // Call the approve endpoint
            await ApiHelper.ExecuteCallGuardedAsync(
                () => InspectionClient.ApproveInspectionEndpointAsync("1", Model.Id),
                Snackbar,
                Navigation
            );

            Snackbar.Add("Inspection approved successfully. Inventory has been updated.", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
            Refresh?.Invoke();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error approving inspection: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isSaving = false;
            StateHasChanged();
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
