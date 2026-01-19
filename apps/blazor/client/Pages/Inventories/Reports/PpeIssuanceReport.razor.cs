using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

#pragma warning disable CA1515 // Type can be internal
#pragma warning disable CA2227 // Collection property should be read-only

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class PpeIssuanceReport : ComponentBase
{
    private const string ApiVersion = "1";

    [Parameter] public Guid? Id { get; set; }
    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private MudForm? _form;
    private PpeIssuanceModel _model = new();
    private PpeIssuanceLineItemModel _draft = new();
    private Guid? _createdId;
    private Guid? _editingId;
    private int _editingIndex = -1;
    private bool _isPrintDialogOpen;
    private bool _isLoading;
    private int _reportStatus = 0; // 0 = Draft, 1 = Posted
    private string _actionButtonText = "Save PPEIR";
    private string _reportStatusText = "Draft";

    protected override async Task OnInitializedAsync()
    {
        if (Id.HasValue)
        {
            _editingId = Id.Value;
            // TODO: Uncomment after API client regeneration
            // await LoadReport();
        }
        else
        {
            Reset();
        }
    }

    // TODO: Uncomment after API client regeneration with Status, Notes, and LineItems properties
    /*
    private async Task LoadReport()
    {
        _isLoading = true;
        try
        {
            var report = await ApiClient.GetPpeIssuanceReportEndpointAsync(ApiVersion, _editingId!.Value);
            if (report is null)
            {
                Snackbar.Add("Report not found", Severity.Error);
                NavigationManager.NavigateTo("/inventories/reports/ppeir-list");
                return;
            }

            _model = new PpeIssuanceModel
            {
                Id = report.Id,
                ReportNumber = report.ReportNumber,
                RecipientName = report.RecipientName,
                RecipientAddress = report.RecipientAddress,
                IssuanceType = report.IssuanceType,
                IssuanceDate = report.IssuanceDate,
                Notes = report.Notes,
            };

            _reportStatus = report.Status;
            _reportStatusText = report.Status == 0 ? "Draft" : "Posted";
            _actionButtonText = report.Status == 0 ? "Update PPEIR" : "View Only";

            // Load line items if available
            if (report.LineItems != null)
            {
                foreach (var item in report.LineItems)
                {
                    _model.LineItems.Add(new PpeIssuanceLineItemModel
                    {
                        PropertyCode = item.PropertyCode,
                        Description = item.Description,
                        DateAcquired = item.DateAcquired,
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
        finally
        {
            _isLoading = false;
        }
    }
    */

    private void Reset()
    {
        _model = new PpeIssuanceModel();
        _draft = new PpeIssuanceLineItemModel();
        _createdId = null;
        _editingId = null;
        StateHasChanged();
    }

    private void ResetDraft()
    {
        _draft = new PpeIssuanceLineItemModel();
        _editingIndex = -1;
    }

    private void AddLineItem()
    {
        if (_editingIndex >= 0)
        {
            // Update existing item
            _model.LineItems[_editingIndex] = new PpeIssuanceLineItemModel
            {
                PropertyCode = _draft.PropertyCode.Trim(),
                Description = _draft.Description.Trim(),
                Quantity = _draft.Quantity,
                Unit = _draft.Unit?.Trim() ?? string.Empty,
                DateAcquired = _draft.DateAcquired,
                AcquisitionCost = _draft.AcquisitionCost,
                AccumulatedDepreciation = _draft.AccumulatedDepreciation,
                BookValue = _draft.BookValue,
            };
            Snackbar.Add("Item updated", Severity.Success);
        }
        else
        {
            // Add new item
            _model.LineItems.Add(new PpeIssuanceLineItemModel
            {
                PropertyCode = _draft.PropertyCode.Trim(),
                Description = _draft.Description.Trim(),
                Quantity = _draft.Quantity,
                Unit = _draft.Unit?.Trim() ?? string.Empty,
                DateAcquired = _draft.DateAcquired,
                AcquisitionCost = _draft.AcquisitionCost,
                AccumulatedDepreciation = _draft.AccumulatedDepreciation,
                BookValue = _draft.BookValue,
            });
        }

        ResetDraft();
    }

    private void EditItem(PpeIssuanceLineItemModel item)
    {
        _editingIndex = _model.LineItems.IndexOf(item);
        _draft = new PpeIssuanceLineItemModel
        {
            PropertyCode = item.PropertyCode,
            Description = item.Description,
            Quantity = item.Quantity,
            Unit = item.Unit,
            DateAcquired = item.DateAcquired,
            AcquisitionCost = item.AcquisitionCost,
            AccumulatedDepreciation = item.AccumulatedDepreciation,
            BookValue = item.BookValue,
        };
    }

    private void DuplicateItem(PpeIssuanceLineItemModel item)
    {
        _editingIndex = -1;
        _draft = new PpeIssuanceLineItemModel
        {
            PropertyCode = item.PropertyCode,
            Description = item.Description,
            Quantity = item.Quantity,
            DateAcquired = item.DateAcquired,
            Unit = item.Unit,
            AcquisitionCost = item.AcquisitionCost,
            AccumulatedDepreciation = item.AccumulatedDepreciation,
            BookValue = item.BookValue,
        };
        Snackbar.Add("Item duplicated to draft. Click 'Add Item' to save.", Severity.Info);
    }

    private void RemoveItem(PpeIssuanceLineItemModel item)
    {
        _model.LineItems.Remove(item);
    }

    private async Task SubmitAsync()
    {
        if (_form is null)
        {
            return;
        }

        // If posted, cannot edit
        if (_reportStatus != 0)
        {
            Snackbar.Add("Cannot edit posted reports. Cancel to reverse.", Severity.Warning);
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

        if (_model.IssuanceDate?.Date > DateTime.Today)
        {
            Snackbar.Add("Issuance date cannot be in the future", Severity.Warning);
            return;
        }

        // TODO: Uncomment after API client regeneration with Update endpoints
        /*
        if (_editingId.HasValue)
        {
            // Edit mode - update existing
            var command = new UpdatePpeIssuanceReportCommand
            {
                Id = _editingId.Value,
                RecipientName = _model.RecipientName,
                RecipientAddress = _model.RecipientAddress,
                IssuanceType = _model.IssuanceType,
                IssuanceDate = _model.IssuanceDate ?? DateTime.Today,
                Notes = _model.Notes,
                LineItems = _model.LineItems.Select(li => new UpdatePpeIssuanceLineItemRequest
                {
                    PropertyCode = li.PropertyCode,
                    Description = li.Description,
                    AcquisitionCost = li.AcquisitionCost,
                    AccumulatedDepreciation = li.AccumulatedDepreciation,
                    BookValue = li.BookValue,
                }).ToList(),
            };

            try
            {
                var response = await ApiClient.UpdatePpeIssuanceReportEndpointAsync(ApiVersion, _editingId.Value, command);
                Snackbar.Add("PPE Issuance Report updated", Severity.Success);
                NavigationManager.NavigateTo("/inventories/reports/ppeir-list");
            }
            catch (ApiException ex)
            {
                Snackbar.Add(ex.Response ?? "Failed to update PPE issuance", Severity.Error);
            }
            return;
        }
        */

        var createCommand = new CreatePpeIssuanceReportCommand
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
                Quantity = li.Quantity,
                Unit = li.Unit,
                AcquisitionCost = li.AcquisitionCost,
                AccumulatedDepreciation = li.AccumulatedDepreciation,
                BookValue = li.BookValue,
            }).ToList(),
        };

        try
        {
            var response = await ApiClient.CreatePpeIssuanceReportEndpointAsync(ApiVersion, createCommand);
            _createdId = response.Id;
            Snackbar.Add("PPE Issuance Report saved", Severity.Success);
            NavigationManager.NavigateTo("/inventories/reports/ppeir-list");
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save PPE issuance", Severity.Error);
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

    private async Task PrintDocument()
    {
        try
        {
            await JS.InvokeVoidAsync("window.print");
            await Task.Delay(1000);
            _isPrintDialogOpen = false;
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

    // NOTE: The following methods will work after API client is regenerated with NSwag
    // Uncomment these methods after regenerating the client
    /*
    private async Task PostReportAsync()
    {
        if (_editingId is null)
        {
            Snackbar.Add("Report not saved yet. Save first before posting.", Severity.Warning);
            return;
        }

        var confirmed = await JS.InvokeAsync<bool>("confirm",
            "Post this report? It will be locked and cannot be edited. Inventory will be updated.");
        if (!confirmed) return;

        try
        {
            await ApiClient.PostPpeIssuanceReportEndpointAsync(ApiVersion, _editingId.Value);
            Snackbar.Add("Report posted successfully. Inventory updated.", Severity.Success);
            await LoadReport();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to post report", Severity.Error);
        }
    }

    private async Task CancelReportAsync()
    {
        if (_editingId is null)
        {
            Snackbar.Add("Report not found.", Severity.Warning);
            return;
        }

        var confirmed = await JS.InvokeAsync<bool>("confirm",
            "Cancel this report? This will reverse the inventory changes and create reversal entries.");
        if (!confirmed) return;

        try
        {
            await ApiClient.CancelPpeIssuanceReportEndpointAsync(ApiVersion, _editingId.Value);
            Snackbar.Add("Report cancelled. Inventory reversed.", Severity.Success);
            NavigationManager.NavigateTo("/inventories/reports/ppeir-list");
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to cancel report", Severity.Error);
        }
    }
    */
}

#region View models
public class PpeIssuanceModel
{
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string ReportNumber { get; set; } = string.Empty;

    [Required]
    public string RecipientName { get; set; } = string.Empty;

    [Required]
    public string RecipientAddress { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string IssuanceType { get; set; } = "Sale";

    [Required]
    public DateTime? IssuanceDate { get; set; } = DateTime.Today;

    public string Notes { get; set; } = string.Empty;

    public Collection<PpeIssuanceLineItemModel> LineItems { get; } = new();
}

public class PpeIssuanceLineItemModel
{
    [MaxLength(50)]
    public string PropertyCode { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public double Quantity { get; set; } = 1;

    [MaxLength(50)]
    public string Unit { get; set; } = "pcs";

    public DateTime? DateAcquired { get; set; }

    public double AcquisitionCost { get; set; }

    public double? AccumulatedDepreciation { get; set; } = 0;

    public double? BookValue { get; set; } = 0;
}
#endregion
