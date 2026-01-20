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

    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IJSRuntime JS { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private MudForm? _form;
    private PpeIssuanceModel _model = new();
    private PpeIssuanceLineItemModel _draft = new();
    private Guid? _createdId;
    private Guid? _reportId;
    private int _reportStatus = 0; // 0 = Draft, 1 = Posted
    private string _reportStatusText = "Draft";
    private string _actionButtonText = "CREATE PPEIR";
    private bool _isPrintDialogOpen;

    private static readonly string[] IssuanceTypes = ["Sale", "Transfer to CO", "Transfer to RO", "Transfer to PO", "Donation", "Dumping", "Destruction", "Others"];

    protected override void OnInitialized()
    {
        Reset();
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
    }

    private void AddLineItem()
    {
        if (string.IsNullOrWhiteSpace(_draft.PropertyCode))
        {
            Snackbar.Add("Property Code is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_draft.Description))
        {
            Snackbar.Add("Description is required", Severity.Warning);
            return;
        }

        if (_draft.Quantity <= 0)
        {
            Snackbar.Add("Quantity must be greater than zero", Severity.Warning);
            return;
        }

        if (_draft.AcquisitionCost < 0)
        {
            Snackbar.Add("Acquisition cost cannot be negative", Severity.Warning);
            return;
        }

        _model.LineItems.Add(new PpeIssuanceLineItemModel
        {
            PropertyCode = _draft.PropertyCode.Trim(),
            Description = _draft.Description.Trim(),
            DateAcquired = _draft.DateAcquired,
            Quantity = _draft.Quantity,
            Unit = _draft.Unit?.Trim() ?? string.Empty,
            AcquisitionCost = _draft.AcquisitionCost,
            AccumulatedDepreciation = _draft.AccumulatedDepreciation,
            BookValue = _draft.BookValue,
        });

        ResetDraft();
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
        };
        _model.LineItems.Remove(item);
        Snackbar.Add("Editing item - modify and click Add Item to save changes", Severity.Info);
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
        });
        Snackbar.Add("Item duplicated", Severity.Success);
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
            var postCommand = new PostPpeIssuanceReportCommand { Id = _reportId.Value };
            await ApiClient.PostPpeIssuanceReportEndpointAsync(ApiVersion, postCommand);
            
            _reportStatus = 1;
            _reportStatusText = "Posted";
            _actionButtonText = "Posted";
            Snackbar.Add("PPE Issuance Report posted successfully. Inventory has been updated.", Severity.Success);
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
            var cancelCommand = new CancelPpeIssuanceReportCommand { Id = _reportId.Value };
            await ApiClient.CancelPpeIssuanceReportEndpointAsync(ApiVersion, cancelCommand);
            
            _reportStatus = 0;
            _reportStatusText = "Draft";
            _actionButtonText = "Update PPEIR";
            Snackbar.Add("PPE Issuance Report cancelled. Inventory changes reversed.", Severity.Success);
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
            await ApiClient.DeletePpeIssuanceReportEndpointAsync(ApiVersion, _reportId.Value);
            Snackbar.Add("PPE Issuance Report deleted", Severity.Success);
            NavigationManager.NavigateTo("/inventories/reports/ppeir");
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to delete report", Severity.Error);
        }
        */
    }

    // TODO: Uncomment after API client regeneration with Get and Update endpoints
    /*
    private async Task LoadReportAsync(Guid reportId)
    {
        try
        {
            var response = await ApiClient.GetPpeIssuanceReportByIdEndpointAsync(ApiVersion, reportId);
            _reportId = response.Id;
            _reportStatus = response.Status;
            _reportStatusText = response.Status == 0 ? "Draft" : "Posted";
            _actionButtonText = response.Status == 0 ? "Update PPEIR" : "Posted";

            _model = new PpeIssuanceModel
            {
                ReportNumber = response.ReportNumber,
                RecipientName = response.RecipientName,
                RecipientAddress = response.RecipientAddress,
                IssuanceType = response.IssuanceType,
                IssuanceDate = response.IssuanceDate,
                Notes = response.Notes,
                LineItems = response.LineItems?.Select(li => new PpeIssuanceLineItemModel
                {
                    PropertyCode = li.PropertyCode,
                    Description = li.Description,
                    DateAcquired = li.DateAcquired,
                    Quantity = li.Quantity,
                    Unit = li.Unit,
                    AcquisitionCost = li.AcquisitionCost,
                    AccumulatedDepreciation = li.AccumulatedDepreciation,
                    BookValue = li.BookValue,
                }).ToList() ?? new()
            };

            StateHasChanged();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to load report", Severity.Error);
        }
    }
    */

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
            Snackbar.Add("Fix report details before submitting.", Severity.Warning);
            return;
        }

        if (_model.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item", Severity.Warning);
            return;
        }

        // Validate all line items
        foreach (var item in _model.LineItems)
        {
            if (string.IsNullOrWhiteSpace(item.PropertyCode))
            {
                Snackbar.Add("All line items must have a Property Code", Severity.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(item.Description))
            {
                Snackbar.Add("All line items must have a Description", Severity.Warning);
                return;
            }

            if (item.Quantity <= 0)
            {
                Snackbar.Add("All line items must have a quantity greater than zero", Severity.Warning);
                return;
            }

            if (item.AcquisitionCost < 0)
            {
                Snackbar.Add("Acquisition cost cannot be negative", Severity.Warning);
                return;
            }
        }

        if (_model.IssuanceDate?.Date > DateTime.Today)
        {
            Snackbar.Add("Issuance date cannot be in the future", Severity.Warning);
            return;
        }

        try
        {
            // Create new report (Update endpoint pending API client regeneration)
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
            Snackbar.Add("PPE Issuance Report created", Severity.Success);
            Reset();
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
}
#endregion