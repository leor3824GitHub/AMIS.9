using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class PpeReceivingReport : ComponentBase
{
    private const string ApiVersion = "1";

    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    private MudForm? _form;
    private PpeReceivingModel _model = new();
    private PpeReceivingLineItemModel _draft = new();
    private Guid? _createdId;

    private static readonly string[] ReceiptTypes = ["Purchase", "Transfer", "Donation", "Return", "Others"];

    protected override void OnInitialized()
    {
        Reset();
    }

    private void Reset()
    {
        _model = new PpeReceivingModel();
        _draft = new PpeReceivingLineItemModel { DateAcquired = _model.SourceReceiptDate };
        _createdId = null;
        StateHasChanged();
    }

    private void ResetDraft()
    {
        _draft = new PpeReceivingLineItemModel { DateAcquired = _model.SourceReceiptDate };
    }

    private void AddLineItem()
    {
        if (string.IsNullOrWhiteSpace(_draft.PropertyCode))
        {
            Snackbar.Add("Property code is required", Severity.Warning);
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

        if (_draft.UnitCost < 0)
        {
            Snackbar.Add("Unit cost cannot be negative", Severity.Warning);
            return;
        }

        _model.LineItems.Add(new PpeReceivingLineItemModel
        {
            PropertyCode = _draft.PropertyCode.Trim(),
            Description = _draft.Description?.Trim(),
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

    private async Task SubmitAsync()
    {
        if (_form is null)
        {
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

        try
        {
            var response = await ApiClient.CreatePpeReceivingReportEndpointAsync(ApiVersion, command);
            _createdId = response.Id;
            Snackbar.Add("PPE Receiving Report saved", Severity.Success);
            Reset();
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
    [Required]
    public string PropertyCode { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public DateTime? DateAcquired { get; set; } = DateTime.Today;

    [Range(0.01, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    [Required]
    public string Unit { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public double UnitCost { get; set; }
}
#endregion
