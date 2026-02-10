using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.Authorization;

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class PperrReport : ComponentBase
{
    private const string ApiVersion = "1";

    [CascadingParameter] public Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject] public IAuthorizationService AuthService { get; set; } = default!;
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IJSRuntime JS { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canPost;
    private bool _isReadOnly;
    private bool _isPrintDialogOpen;
    private bool _isEditingLineItem;
    private bool _isLoadInventoryDialogOpen;
    private bool _isLoadingInventory;
    private string _physicalAssetSearch = string.Empty;

    private MudForm? _form;
    private PperrModel _model = new();
    private PperrLineItemModel _draft = new();
    private Guid? _createdId;
    private Guid? _reportId;
    private int _reportStatus = 0; // 0 = Draft, 1 = Posted
    private string _reportStatusText = "Draft";
    private string _actionButtonText = "Create PPER";
    private List<PhysicalAssetResponse>? _physicalAssets;

    private IEnumerable<PhysicalAssetResponse> FilteredPhysicalAssets =>
        string.IsNullOrWhiteSpace(_physicalAssetSearch)
            ? _physicalAssets ?? Enumerable.Empty<PhysicalAssetResponse>()
            : (_physicalAssets ?? Enumerable.Empty<PhysicalAssetResponse>()).Where(asset =>
                (!string.IsNullOrWhiteSpace(asset.PropertyCode) && asset.PropertyCode.Contains(_physicalAssetSearch, StringComparison.OrdinalIgnoreCase))
                || (!string.IsNullOrWhiteSpace(asset.Description) && asset.Description.Contains(_physicalAssetSearch, StringComparison.OrdinalIgnoreCase))
                || (!string.IsNullOrWhiteSpace(asset.Location) && asset.Location.Contains(_physicalAssetSearch, StringComparison.OrdinalIgnoreCase))
                || (!string.IsNullOrWhiteSpace(asset.UnitOfMeasure) && asset.UnitOfMeasure.Contains(_physicalAssetSearch, StringComparison.OrdinalIgnoreCase)));

    private bool CanSave => !_isReadOnly && _reportStatus == 0 && ((_reportId is null && _canCreate) || (_reportId.HasValue && _canUpdate));
    private bool CanPost => !_isReadOnly && _reportStatus == 0 && _reportId.HasValue && _canPost;

    private static readonly string[] ReceiptTypes = ["Purchase", "Transfer", "Donation", "Return", "Others"];

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.Pper);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.Pper);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.Pper);
        _canPost = await AuthService.HasPermissionAsync(user, FshActions.Post, FshResources.Pper);

        // Check for readonly query parameter
        var uri = new Uri(NavigationManager.Uri);
        var query = uri.Query;
        _isReadOnly = query.Contains("readonly=true", StringComparison.OrdinalIgnoreCase);
        var shouldPrint = query.Contains("print=true", StringComparison.OrdinalIgnoreCase);

        if (!string.IsNullOrEmpty(ReportId) && Guid.TryParse(ReportId, out var id))
        {
            await LoadReportAsync(id);

            // Auto-open print dialog if requested
            if (shouldPrint && _model.LineItems.Count > 0)
            {
                _isPrintDialogOpen = true;
                StateHasChanged();
            }
        }
        else
        {
            Reset();
        }
    }

    private void Reset()
    {
        _model = new PperrModel();
        _draft = new PperrLineItemModel { DateAcquired = _model.Date };
        _reportId = null;
        _reportStatus = 0;
        _reportStatusText = "Draft";
        _actionButtonText = "Create PPER";
        _createdId = null;
        StateHasChanged();
    }

    private void ResetDraft()
    {
        _draft = new PperrLineItemModel { DateAcquired = _model.Date };
        _isEditingLineItem = false;
    }

    private async Task ResetWithConfirmation()
    {
        if (_model.LineItems.Count > 0)
        {
            var confirmed = await JS.InvokeAsync<bool>("confirm", "This will clear all line items. Do you want to continue?");
            if (!confirmed) return;
        }

        Reset();
    }

    private void AddLineItem()
    {
        if (string.IsNullOrWhiteSpace(_draft.Description)
            || string.IsNullOrWhiteSpace(_draft.Location))
        {
            Snackbar.Add("Description and location are required for each line item", Severity.Warning);
            return;
        }

        // Just add the line item - validation happens when saving/posting
        _model.LineItems.Add(new PperrLineItemModel
        {
            PropertyCode = _draft.PropertyCode?.Trim() ?? string.Empty,
            Description = _draft.Description?.Trim() ?? string.Empty,
            DateAcquired = _draft.DateAcquired,
            Quantity = _draft.Quantity,
            Unit = _draft.Unit?.Trim() ?? string.Empty,
            UnitCost = _draft.UnitCost,
            Location = _draft.Location?.Trim() ?? string.Empty,
        });

        ResetDraft();
    }

    private void RemoveItem(PperrLineItemModel item)
    {
        _model.LineItems.Remove(item);
    }

    private void EditItem(PperrLineItemModel item)
    {
        _draft = new PperrLineItemModel
        {
            PropertyCode = item.PropertyCode,
            Description = item.Description,
            DateAcquired = item.DateAcquired,
            Quantity = item.Quantity,
            Unit = item.Unit,
            UnitCost = item.UnitCost,
            Location = item.Location,
        };
        _model.LineItems.Remove(item);
        _isEditingLineItem = true;
        Snackbar.Add("Editing item - modify and click Save Item to save changes", Severity.Info);
    }

    private void DuplicateItem(PperrLineItemModel item)
    {
        _model.LineItems.Add(new PperrLineItemModel
        {
            PropertyCode = item.PropertyCode,
            Description = item.Description,
            DateAcquired = item.DateAcquired,
            Quantity = item.Quantity,
            Unit = item.Unit,
            UnitCost = item.UnitCost,
            Location = item.Location,
        });
        Snackbar.Add("Item duplicated", Severity.Success);
    }

    private void CancelEdit()
    {
        _reportId = null;
        Reset();
        NavigationManager.NavigateTo("/inventories/reports/pper");
    }

    private async Task PrintAsync()
    {
        if (_model.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item before printing", Severity.Warning);
            return;
        }

        _isPrintDialogOpen = true;
    }

    private void ClosePrintDialog()
    {
        _isPrintDialogOpen = false;
    }

    private async Task OpenLoadInventoryDialog()
    {
        _isLoadInventoryDialogOpen = true;
        await LoadPhysicalAssets();
    }

    private void CloseLoadInventoryDialog()
    {
        _isLoadInventoryDialogOpen = false;
        _physicalAssets = null;
    }

    private async Task LoadPhysicalAssets()
    {
        try
        {
            _isLoadingInventory = true;
            var command = new SearchPhysicalAssetsCommand { PageNumber = 1, PageSize = 100 };
            var response = await ApiClient.SearchPhysicalAssetsEndpointAsync(ApiVersion, command);
            _physicalAssets = response?.Items?.ToList() ?? new List<PhysicalAssetResponse>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load physical assets: {ex.Message}", Severity.Error);
            _physicalAssets = new List<PhysicalAssetResponse>();
        }
        finally
        {
            _isLoadingInventory = false;
        }
    }

    private void SelectPhysicalAsset(PhysicalAssetResponse asset)
    {
        // Add the selected physical asset to the line items
        _model.LineItems.Add(new PperrLineItemModel
        {
            PropertyCode = asset.PropertyCode ?? "",
            Description = asset.Description ?? "",
            DateAcquired = asset.AcquisitionDate,
            Quantity = asset.Quantity,
            Unit = asset.UnitOfMeasure ?? "",
            UnitCost = asset.AcquisitionCost,
            Location = asset.Location ?? ""
        });

        Snackbar.Add($"Added: {asset.Description ?? asset.PropertyCode}", Severity.Success);
        StateHasChanged();
    }

    private async Task PrintDocument()
    {
        try
        {
            await JS.InvokeVoidAsync("window.print");
            ClosePrintDialog();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Print failed: {ex.Message}", Severity.Error);
        }
    }

    private void GoToList()
    {
        NavigationManager.NavigateTo("/inventories/reports/pper-list");
    }

    private async Task PostReportAsync()
    {
        // TODO: Uncomment after API client regeneration
        /*
        if (!_reportId.HasValue)
        {
            Snackbar.Add("Save the report first before posting", Severity.Warning);
            return;
        }

        var confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to post this report? Once posted, it cannot be edited.");
        if (!confirmed) return;

        try
        {
            var postCommand = new PostPpeReceivingReportCommand { Id = _reportId.Value };
            await ApiClient.PostPpeReceivingReportEndpointAsync(ApiVersion, postCommand);

            _reportStatus = 1;
            _reportStatusText = "Posted";
            _actionButtonText = "Posted";
            Snackbar.Add("PPE Receiving Report posted successfully. Inventory has been updated.", Severity.Success);
            StateHasChanged();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to post report", Severity.Error);
        }
        */
    }

    private async Task CancelReportAsync()
    {
        // TODO: Uncomment after API client regeneration
        /*
        if (!_reportId.HasValue)
        {
            Snackbar.Add("No report to cancel", Severity.Warning);
            return;
        }

        var confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to cancel this posted report? This will reverse all inventory changes.");
        if (!confirmed) return;

        try
        {
            var cancelCommand = new CancelPpeReceivingReportCommand { Id = _reportId.Value };
            await ApiClient.CancelPpeReceivingReportEndpointAsync(ApiVersion, cancelCommand);

            _reportStatus = 0;
            _reportStatusText = "Draft";
            _actionButtonText = "Update PPER";
            Snackbar.Add("PPE Receiving Report cancelled. Inventory changes reversed.", Severity.Success);
            StateHasChanged();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to cancel report", Severity.Error);
        }
        */
    }

    private async Task DeleteReportAsync()
    {
        // TODO: Uncomment after API client regeneration
        /*
        if (!_reportId.HasValue)
        {
            Snackbar.Add("No report to delete", Severity.Warning);
            return;
        }

        if (_reportStatus != 0)
        {
            Snackbar.Add("Only Draft reports can be deleted. Cancel the posted report first.", Severity.Warning);
            return;
        }

        var confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this report? This action cannot be undone.");
        if (!confirmed) return;

        try
        {
            await ApiClient.DeletePpeReceivingReportEndpointAsync(ApiVersion, _reportId.Value);
            Snackbar.Add("PPE Receiving Report deleted", Severity.Success);
            NavigationManager.NavigateTo("/inventories/reports/ppe-receiving");
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to delete report", Severity.Error);
        }
        */
    }

    private async Task LoadReportAsync(Guid reportId)
    {
        try
        {
            var response = await ApiClient.GetPpeReceivingReportEndpointAsync(ApiVersion, reportId);
            _reportId = response.Id;
            _reportStatus = response.Status;
            _reportStatusText = response.Status switch
            {
                0 => "Draft",
                1 => "Posted",
                2 => "Cancelled",
                _ => "Unknown"
            };
            _actionButtonText = response.Status == 0 ? "Update PPER" : "Posted";

            _model = new PperrModel
            {
                RrNumber = response.RrNumber,
                ReceivedFrom = response.ReceivedFrom,
                Address = response.Address,
                Type = response.Type,
                Date = response.Date,
                Notes = response.Notes,
                LineItems = response.Items?.Select(li => new PperrLineItemModel
                {
                    PropertyCode = li.PropertyCode,
                    Description = string.Empty, // Not provided by API
                    DateAcquired = null, // Not provided by API
                    Quantity = 1, // Default
                    Unit = "pc", // Default
                    UnitCost = 0, // Not provided by API
                    Location = string.Empty, // Not provided by API
                }).ToList() ?? new()
            };

            StateHasChanged();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to load report", Severity.Error);
            NavigationManager.NavigateTo("/inventories/reports/pper");
        }
    }

    private async Task SubmitAsync()
    {
        if (_form is null)
        {
            return;
        }

        // Guard against editing Posted reports
        if (_reportStatus != 0)
        {
            Snackbar.Add("Cannot edit a Posted report. Cancel it first to make changes.", Severity.Warning);
            return;
        }

        await _form.Validate();

        if (!_form.IsValid)
        {
            Snackbar.Add("Please fix validation errors", Severity.Warning);
            return;
        }

        if (_model.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item", Severity.Warning);
            return;
        }

        if (_model.LineItems.Any(li => string.IsNullOrWhiteSpace(li.Location)))
        {
            Snackbar.Add("Each line item must have a location", Severity.Warning);
            return;
        }

        if (_model.Date?.Date > DateTime.Today)
        {
            Snackbar.Add("Receipt date cannot be in the future", Severity.Warning);
            return;
        }

        try
        {
            if (_reportId.HasValue)
            {
                var updateCommand = new UpdatePpeReceivingReportCommand
                {
                    Id = _reportId.Value,
                    ReceivedFrom = _model.ReceivedFrom,
                    Address = _model.Address,
                    Type = _model.Type,
                    Date = _model.Date ?? DateTime.Today,
                    Notes = _model.Notes,
                    LineItems = _model.LineItems.Select(li => new UpdatePpeReceivingLineItemRequest
                    {
                        PropertyCode = li.PropertyCode,
                        Description = li.Description,
                        DateAcquired = li.DateAcquired ?? _model.Date ?? DateTime.Today,
                        Quantity = li.Quantity,
                        Unit = li.Unit,
                        UnitCost = li.UnitCost,
                        Location = li.Location,
                    }).ToList(),
                };

                await ApiClient.UpdatePpeReceivingReportEndpointAsync(ApiVersion, _reportId.Value, updateCommand);
                Snackbar.Add("PPE Receiving Report updated", Severity.Success);
                await LoadReportAsync(_reportId.Value);
            }
            else
            {
                var command = new CreatePpeReceivingReportCommand
                {
                    RrNumber = _model.RrNumber,
                    ReceivedFrom = _model.ReceivedFrom,
                    Address = _model.Address,
                    Type = _model.Type,
                    Date = _model.Date ?? DateTime.Today,
                    Notes = _model.Notes,
                    LineItems = _model.LineItems.Select(li => new CreatePpeReceivingLineItemRequest
                    {
                        PropertyCode = li.PropertyCode,
                        Description = li.Description,
                        DateAcquired = li.DateAcquired ?? _model.Date ?? DateTime.Today,
                        Quantity = li.Quantity,
                        Unit = li.Unit,
                        UnitCost = li.UnitCost,
                        Location = li.Location,
                    }).ToList(),
                };

                var response = await ApiClient.CreatePpeReceivingReportEndpointAsync(ApiVersion, command);
                _reportId = response.Id;
                _createdId = response.Id;
                _actionButtonText = "Update PPER";
                Snackbar.Add("PPE Receiving Report created", Severity.Success);
                NavigationManager.NavigateTo("/inventories/reports/pper-list");
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save PPER", Severity.Error);
        }
    }

    private async Task PostAsync()
    {
        if (!CanPost)
        {
            Snackbar.Add("Not authorized to post this report", Severity.Warning);
            return;
        }

        if (!_reportId.HasValue)
        {
            Snackbar.Add("Save the report before posting", Severity.Warning);
            return;
        }

        if (_reportStatus != 0)
        {
            Snackbar.Add("Report is already posted", Severity.Info);
            return;
        }

        if (_model.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item before posting", Severity.Warning);
            return;
        }

        var confirmed = await JS.InvokeAsync<bool>("confirm", "Posting will lock this report and update inventory. Continue?");
        if (!confirmed)
        {
            return;
        }

        try
        {
            await ApiClient.PostPpeReceivingReportEndpointAsync(ApiVersion, _reportId.Value);
            Snackbar.Add("PPE Receiving Report posted", Severity.Success);
            await LoadReportAsync(_reportId.Value);
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to post PPER", Severity.Error);
        }
    }
}

#region View models
public class PperrModel
{
    [Required]
    public string RrNumber { get; set; } = string.Empty;

    [Required]
    public string ReceivedFrom { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = "Purchase";

    [Required]
    public DateTime? Date { get; set; } = DateTime.Today;

    public string Notes { get; set; } = string.Empty;

    public List<PperrLineItemModel> LineItems { get; set; } = new();
}

public class PperrLineItemModel
{
    public string PropertyCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DateAcquired { get; set; } = DateTime.Today;
    [Range(0.01, double.MaxValue)]
    public double Quantity { get; set; } = 1;
    public string Unit { get; set; } = "pc";
    [Range(0, double.MaxValue)]
    public double UnitCost { get; set; }
    public string Location { get; set; } = string.Empty;
}
#endregion
