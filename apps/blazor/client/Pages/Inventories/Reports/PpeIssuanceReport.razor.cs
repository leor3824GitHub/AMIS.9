using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.Authorization;

#pragma warning disable CA1515 // Type can be internal
#pragma warning disable CA2227 // Collection property should be read-only

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class PpeIssuanceReport : ComponentBase
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

    private PpeIssuanceModel _model = new();
    private PpeIssuanceLineItemModel _draft = new();
    private Guid? _createdId;
    private Guid? _reportId;
    private int _reportStatus = 0; // 0 = Draft, 1 = Posted
    private string _reportStatusText = "Draft";
    private string _actionButtonText = "CREATE PPEIR";

    // Inventory search fields
    private string _searchKeyword = string.Empty;
    private List<InventoryRegistryResponse> _inventorySearchResults = new();
    private HashSet<InventoryRegistryResponse> _selectedInventoryItems = new();
    private bool _isLoadingInventory;
    private bool _hasSearchedInventory;

    private bool CanSave => !_isReadOnly && _reportStatus == 0 && ((_reportId is null && _canCreate) || (_reportId.HasValue && _canUpdate));
    private bool CanPost => !_isReadOnly && _reportStatus == 0 && _reportId.HasValue && _canPost;

    private static readonly string[] IssuanceTypes = ["Sale", "Transfer to CO", "Transfer to RO", "Transfer to PO", "Donation", "Dumping", "Destruction", "Others"];

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canCreate = await AuthService.HasPermissionAsync(user, FshActions.Create, FshResources.PpeIssuance);
        _canUpdate = await AuthService.HasPermissionAsync(user, FshActions.Update, FshResources.PpeIssuance);
        _canDelete = await AuthService.HasPermissionAsync(user, FshActions.Delete, FshResources.PpeIssuance);
        _canPost = await AuthService.HasPermissionAsync(user, FshActions.Post, FshResources.PpeIssuance);

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

    private async Task LoadReportAsync(Guid id)
    {
        try
        {
            var report = await ApiClient.GetPpeIssuanceReportEndpointAsync(ApiVersion, id);
            if (report is null)
            {
                Snackbar.Add("Report not found", Severity.Error);
                NavigationManager.NavigateTo("/inventories/reports/ppeir-list");
                return;
            }

            _reportId = report.Id;
            _model = new PpeIssuanceModel
            {
                ReportNumber = report.ReportNumber,
                RecipientName = report.RecipientName,
                RecipientAddress = report.RecipientAddress,
                IssuanceType = report.IssuanceType,
                IssuanceDate = report.IssuanceDate,
                Notes = report.Notes,
            };

            _reportStatus = report.Status;
            _reportStatusText = report.Status == 0 ? "Draft" : "Posted";
            _actionButtonText = report.Status == 0 ? "UPDATE PPEIR" : "View Only";

            // Load line items if available
            if (report.LineItems?.Count > 0)
            {
                foreach (var item in report.LineItems)
                {
                    _model.LineItems.Add(new PpeIssuanceLineItemModel
                    {
                        PropertyCode = item.PropertyCode,
                        Description = item.Description,
                        DateAcquired = item.DateAcquired,
                        Quantity = item.Quantity,
                        Unit = item.Unit,
                        AcquisitionCost = item.AcquisitionCost,
                        AccumulatedDepreciation = item.AccumulatedDepreciation,
                        BookValue = item.BookValue,
                    });
                }
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Error loading report", Severity.Error);
            NavigationManager.NavigateTo("/inventories/reports/ppeir-list");
        }
    }

    private void Reset()
    {
        _model = new PpeIssuanceModel();
        _draft = new PpeIssuanceLineItemModel { DateAcquired = _model.IssuanceDate };
        _reportId = null;
        _reportStatus = 0;
        _reportStatusText = "Draft";
        _actionButtonText = "CREATE PPEIR";
        _createdId = null;
        StateHasChanged();
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

    private void ResetDraft()
    {
        _draft = new PpeIssuanceLineItemModel { DateAcquired = _model.IssuanceDate };
        _isEditingLineItem = false;
    }

    private async Task SearchInventoryAsync()
    {
        _isLoadingInventory = true;
        _hasSearchedInventory = true;

        try
        {
            var effectiveKeyword = string.IsNullOrWhiteSpace(_searchKeyword) ? null : _searchKeyword;

            var searchCommand = new SearchInventoryRegistriesCommand
            {
                Keyword = effectiveKeyword,
                PageSize = 50,
                PageNumber = 1
            };

            var result = await ApiClient.SearchInventoryRegistriesEndpointAsync(ApiVersion, searchCommand);
            _inventorySearchResults = result?.Items?.ToList() ?? new List<InventoryRegistryResponse>();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _inventorySearchResults.Clear();
            Snackbar.Add($"Search error: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoadingInventory = false;
        }
    }
    private async Task AddSelectedItemsAsync()
    {
        if (!_selectedInventoryItems.Any())
        {
            Snackbar.Add("No items selected", Severity.Warning);
            return;
        }

        var addedCount = 0;
        foreach (var item in _selectedInventoryItems)
        {
            // Skip if already added
            if (_model.LineItems.Any(li => li.PropertyCode == item.PropertyCode))
            {
                Snackbar.Add($"{item.PropertyCode} already in line items", Severity.Warning);
                continue;
            }

            _model.LineItems.Add(new PpeIssuanceLineItemModel
            {
                PropertyCode = item.PropertyCode,
                Description = item.Description,
                DateAcquired = _model.IssuanceDate ?? DateTime.Now,
                Quantity = 1, // Default to 1 for PPE
                Unit = "PC",
                AcquisitionCost = 0d, // User can edit later if needed
                Location = item.Location ?? string.Empty,
            });
            addedCount++;
        }

        if (addedCount > 0)
        {
            Snackbar.Add($"Added {addedCount} item(s) to line items", Severity.Success);
            ClearSelection();
        }

        await Task.CompletedTask;
    }

    private void ClearSelection()
    {
        _selectedInventoryItems.Clear();
        _searchKeyword = string.Empty;
        _inventorySearchResults.Clear();
        _hasSearchedInventory = false;
    }

    private Color GetStatusColor(InventoryItemStatus status)
    {
        return status switch
        {
            InventoryItemStatus._0 => Color.Warning,
            InventoryItemStatus._1 => Color.Success,
            InventoryItemStatus._2 => Color.Info,
            InventoryItemStatus._3 => Color.Info,
            InventoryItemStatus._4 => Color.Error,
            InventoryItemStatus._5 => Color.Error,
            _ => Color.Default
        };
    }


    private void CancelEdit()
    {
        _reportId = null;
        Reset();
        NavigationManager.NavigateTo("/inventories/reports/ppeir-list");
    }

    private void RemoveItem(PpeIssuanceLineItemModel item)
    {
        _model.LineItems.Remove(item);
    }

    private void EditItem(PpeIssuanceLineItemModel item)
    {
        _draft = new PpeIssuanceLineItemModel
        {
            PropertyCode = item.PropertyCode,
            Description = item.Description,
            DateAcquired = item.DateAcquired,
            Quantity = item.Quantity,
            Unit = item.Unit,
            AcquisitionCost = item.AcquisitionCost,
            AccumulatedDepreciation = item.AccumulatedDepreciation,
            BookValue = item.BookValue,
            Location = item.Location,
        };
        _model.LineItems.Remove(item);
        _isEditingLineItem = true;
        Snackbar.Add("Editing item - modify and click Save Item to save changes", Severity.Info);
    }

    private void DuplicateItem(PpeIssuanceLineItemModel item)
    {
        _model.LineItems.Add(new PpeIssuanceLineItemModel
        {
            PropertyCode = item.PropertyCode,
            Description = item.Description,
            DateAcquired = item.DateAcquired,
            Quantity = item.Quantity,
            Unit = item.Unit,
            AcquisitionCost = item.AcquisitionCost,
            AccumulatedDepreciation = item.AccumulatedDepreciation,
            BookValue = item.BookValue,
            Location = item.Location,
        });
        Snackbar.Add("Item duplicated", Severity.Success);
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

        // Preflight: validate that all property codes exist and are issuable
        var preflightErrors = await PreflightValidateBeforePostAsync();
        if (preflightErrors.Any())
        {
            foreach (var err in preflightErrors)
                Snackbar.Add(err, Severity.Error);
            Snackbar.Add("Fix the issues above before posting.", Severity.Warning);
            return;
        }

        var confirmed = await JS.InvokeAsync<bool>("confirm", "Posting will lock this report and update inventory. Continue?");
        if (!confirmed)
        {
            return;
        }

        try
        {
            await ApiClient.PostPpeIssuanceReportEndpointAsync(ApiVersion, _reportId.Value);
            Snackbar.Add("PPE Issuance Report posted", Severity.Success);
            await LoadReportAsync(_reportId.Value);
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to post PPEIR", Severity.Error);
        }
    }

    private async Task<List<string>> PreflightValidateBeforePostAsync()
    {
        var errors = new List<string>();

        // Collect distinct property codes from line items
        var codes = _model.LineItems
            .Select(li => li.PropertyCode?.Trim())
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (codes.Count == 0)
        {
            errors.Add("Report has no valid property codes.");
            return errors;
        }

        foreach (var code in codes)
        {
            try
            {
                var cmd = new SearchInventoryRegistriesCommand
                {
                    Keyword = code,
                    PageSize = 5,
                    PageNumber = 1
                };
                var result = await ApiClient.SearchInventoryRegistriesEndpointAsync(ApiVersion, cmd);
                var match = result?.Items?.FirstOrDefault(x => string.Equals(x.PropertyCode?.Trim(), code, StringComparison.OrdinalIgnoreCase));
                if (match is null)
                {
                    errors.Add($"Inventory registry not found for property code '{code}'.");
                    continue;
                }

                // Status '_1' maps to InStock in UI mapping; treat others as non-issuable
                var isInStock = match.Status == InventoryItemStatus._1;
                if (!isInStock)
                {
                    errors.Add($"Property code '{code}' is not issuable (status: {match.Status}).");
                    continue;
                }

                var qty = match.Quantity;
                if (qty <= 0)
                {
                    errors.Add($"Insufficient inventory for '{code}'. Available: {qty}.");
                }
            }
            catch (Exception ex)
            {
                errors.Add($"Preflight check failed for '{code}': {ex.Message}");
            }
        }

        return errors;
    }

    private async Task SubmitAsync()
    {
        // Guard against editing Posted reports
        if (_reportStatus != 0)
        {
            Snackbar.Add("Cannot edit a Posted report. Cancel it first to make changes.", Severity.Warning);
            return;
        }

        // Manual required-field checks now that MudForm is removed
        if (string.IsNullOrWhiteSpace(_model.ReportNumber))
        {
            Snackbar.Add("Report Number is required", Severity.Warning);
            return;
        }

        if (!_model.IssuanceDate.HasValue)
        {
            Snackbar.Add("Issuance Date is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_model.IssuanceType))
        {
            Snackbar.Add("Issuance Type is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_model.RecipientName))
        {
            Snackbar.Add("Recipient Name is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_model.RecipientAddress))
        {
            Snackbar.Add("Recipient Address is required", Severity.Warning);
            return;
        }

        if (_model.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item", Severity.Warning);
            return;
        }

        if (_model.IssuanceDate?.Date > DateTime.Today)
        {
            Snackbar.Add("Issuance date cannot be in the future", Severity.Warning);
            return;
        }

        try
        {
            if (_reportId.HasValue)
            {
                var updateCommand = new UpdatePpeIssuanceReportCommand
                {
                    Id = _reportId.Value,
                    RecipientName = _model.RecipientName,
                    RecipientAddress = _model.RecipientAddress,
                    IssuanceType = _model.IssuanceType,
                    IssuanceDate = _model.IssuanceDate ?? DateTime.Today,
                    Notes = _model.Notes,
                    LineItems = _model.LineItems.Select(li => new UpdatePpeIssuanceLineItemRequest
                    {
                        PropertyCode = li.PropertyCode,
                        Description = li.Description,
                        DateAcquired = li.DateAcquired ?? _model.IssuanceDate ?? DateTime.Today,
                        Quantity = li.Quantity,
                        Unit = li.Unit,
                        AcquisitionCost = li.AcquisitionCost,
                        AccumulatedDepreciation = li.AccumulatedDepreciation,
                        BookValue = li.BookValue,
                    }).ToList(),
                };

                await ApiClient.UpdatePpeIssuanceReportEndpointAsync(ApiVersion, _reportId.Value, updateCommand);
                Snackbar.Add("PPE Issuance Report updated", Severity.Success);
                await LoadReportAsync(_reportId.Value);
            }
            else
            {
                var command = new CreatePpeIssuanceReportCommand
                {
                    ReportNumber = _model.ReportNumber,
                    RecipientName = _model.RecipientName,
                    RecipientAddress = _model.RecipientAddress,
                    IssuanceType = _model.IssuanceType,
                    IssuanceDate = _model.IssuanceDate ?? DateTime.Today,
                    Notes = _model.Notes,
                    LineItems = _model.LineItems.Select(li => new CreatePpeIssuanceLineItemRequest
                    {
                        PropertyCode = li.PropertyCode,
                        Description = li.Description,
                        DateAcquired = li.DateAcquired ?? _model.IssuanceDate ?? DateTime.Today,
                        Quantity = li.Quantity,
                        Unit = li.Unit,
                        AcquisitionCost = li.AcquisitionCost,
                        AccumulatedDepreciation = li.AccumulatedDepreciation,
                        BookValue = li.BookValue,
                    }).ToList(),
                };

                var response = await ApiClient.CreatePpeIssuanceReportEndpointAsync(ApiVersion, command);
                _reportId = response.Id;
                _createdId = response.Id;
                _actionButtonText = "UPDATE PPEIR";
                Snackbar.Add("PPE Issuance Report created", Severity.Success);
                NavigationManager.NavigateTo("/inventories/reports/ppeir-list");
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save PPEIR", Severity.Error);
        }
    }

    private void PrintAsync()
    {
        if (_model.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item before printing", Severity.Warning);
            return;
        }

        _isPrintDialogOpen = true;
    }

    private void GoToList()
    {
        NavigationManager.NavigateTo("/inventories/reports/ppeir-list");
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

    private void ClosePrintDialog()
    {
        _isPrintDialogOpen = false;
    }
}

#region View models
public class PpeIssuanceModel
{
    [Required, MaxLength(10)]
    public string ReportNumber { get; set; } = string.Empty;

    [Required]
    public string RecipientName { get; set; } = string.Empty;

    [Required]
    public string RecipientAddress { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string IssuanceType { get; set; } = "Sale";

    [Required]
    public DateTime? IssuanceDate { get; set; } = DateTime.Today;

    public string Notes { get; set; } = string.Empty;

    public List<PpeIssuanceLineItemModel> LineItems { get; } = new();
}

public class PpeIssuanceLineItemModel
{
    public double Quantity { get; set; } = 1;
    public string PropertyCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Unit { get; set; } = "pc";
    public DateTime? DateAcquired { get; set; }
    public double AcquisitionCost { get; set; }
    public double? AccumulatedDepreciation { get; set; }
    public double? BookValue { get; set; }
    public string Location { get; set; } = string.Empty;
}
#endregion