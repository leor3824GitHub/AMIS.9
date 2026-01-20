using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class PpeReceivingReport : ComponentBase
{
    private const string ApiVersion = "1";

    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IJSRuntime JS { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private MudForm? _form;
    private PpeReceivingModel _model = new();
    private PpeReceivingLineItemModel _draft = new();
    private Guid? _createdId;
    private Guid? _reportId;
    private int _reportStatus = 0; // 0 = Draft, 1 = Posted
    private string _reportStatusText = "Draft";
    private string _actionButtonText = "Create PPER";

    private static readonly string[] ReceiptTypes = ["Purchase", "Transfer", "Donation", "Return", "Others"];

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrEmpty(ReportId) && Guid.TryParse(ReportId, out var id))
        {
            await LoadReportAsync(id);
        }
        else
        {
            Reset();
        }
    }

    private void Reset()
    {
        _model = new PpeReceivingModel();
        _draft = new PpeReceivingLineItemModel { DateAcquired = _model.SourceReceiptDate };
        _reportId = null;
        _reportStatus = 0;
        _reportStatusText = "Draft";
        _actionButtonText = "Create PPER";
        _createdId = null;
        StateHasChanged();
    }

    private void ResetDraft()
    {
        _draft = new PpeReceivingLineItemModel { DateAcquired = _model.SourceReceiptDate };
    }

    private void AddLineItem()
    {
        // Just add the line item - API will perform detailed validation
        _model.LineItems.Add(new PpeReceivingLineItemModel
        {
            PropertyCode = _draft.PropertyCode?.Trim() ?? string.Empty,
            Description = _draft.Description?.Trim() ?? string.Empty,
            DateAcquired = _draft.DateAcquired,
            Quantity = _draft.Quantity,
            Unit = _draft.Unit?.Trim() ?? string.Empty,
            UnitCost = _draft.UnitCost,
        });

        ResetDraft();
    }

    private void RemoveItem(PpeReceivingLineItemModel item)
    {
        _model.LineItems.Remove(item);
    }

    private void CancelEdit()
    {
        _reportId = null;
        Reset();
        NavigationManager.NavigateTo("/inventories/reports/pper");
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

            _model = new PpeReceivingModel
            {
                ReportNumber = response.ReportNumber,
                Location = response.Location,
                SourceName = response.SourceName,
                SourceAddress = response.SourceAddress,
                ReceiptType = response.ReceiptType,
                SourceReceiptDate = response.SourceReceiptDate,
                Notes = response.Notes,
                LineItems = response.LineItems?.Select(li => new PpeReceivingLineItemModel
                {
                    PropertyCode = li.PropertyCode,
                    Description = li.Description,
                    DateAcquired = li.DateAcquired,
                    Quantity = li.Quantity,
                    Unit = li.Unit,
                    UnitCost = li.UnitCost,
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

        if (_model.SourceReceiptDate?.Date > DateTime.Today)
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
                    Location = _model.Location,
                    SourceName = _model.SourceName,
                    SourceAddress = _model.SourceAddress,
                    ReceiptType = _model.ReceiptType,
                    SourceReceiptDate = _model.SourceReceiptDate ?? DateTime.Today,
                    Notes = _model.Notes,
                    LineItems = _model.LineItems.Select(li => new UpdatePpeReceivingLineItemRequest
                    {
                        PropertyCode = li.PropertyCode,
                        Description = li.Description,
                        DateAcquired = li.DateAcquired ?? _model.SourceReceiptDate ?? DateTime.Today,
                        Quantity = li.Quantity,
                        Unit = li.Unit,
                        UnitCost = li.UnitCost,
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
                    ReportNumber = _model.ReportNumber,
                    Location = _model.Location,
                    SourceName = _model.SourceName,
                    SourceAddress = _model.SourceAddress,
                    ReceiptType = _model.ReceiptType,
                    SourceReceiptDate = _model.SourceReceiptDate ?? DateTime.Today,
                    Notes = _model.Notes,
                    LineItems = _model.LineItems.Select(li => new CreatePpeReceivingLineItemRequest
                    {
                        PropertyCode = li.PropertyCode,
                        Description = li.Description,
                        DateAcquired = li.DateAcquired ?? _model.SourceReceiptDate ?? DateTime.Today,
                        Quantity = li.Quantity,
                        Unit = li.Unit,
                        UnitCost = li.UnitCost,
                    }).ToList(),
                };

                var response = await ApiClient.CreatePpeReceivingReportEndpointAsync(ApiVersion, command);
                _reportId = response.Id;
                _createdId = response.Id;
                _actionButtonText = "Update PPER";
                Snackbar.Add("PPE Receiving Report created", Severity.Success);
                if (_reportId.HasValue)
                {
                    await LoadReportAsync(_reportId.Value);
                }
                else
                {
                    Reset();
                }
            }
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save PPER", Severity.Error);
        }
    }
}

#region View models
public class PpeReceivingModel
{
    [Required]
    public string ReportNumber { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string SourceName { get; set; } = string.Empty;

    [Required]
    public string SourceAddress { get; set; } = string.Empty;

    [Required]
    public string ReceiptType { get; set; } = "Purchase";

    [Required]
    public DateTime? SourceReceiptDate { get; set; } = DateTime.Today;

    public string Notes { get; set; } = string.Empty;

    public List<PpeReceivingLineItemModel> LineItems { get; set; } = new();
}

public class PpeReceivingLineItemModel
{
    
    public string PropertyCode { get; set; } = string.Empty;

    
    public string Description { get; set; } = string.Empty;

    public DateTime? DateAcquired { get; set; } = DateTime.Today;

    [Range(0.01, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    
    public string Unit { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public double UnitCost { get; set; }
}
#endregion
