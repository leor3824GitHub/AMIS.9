using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class SuppliesAndMaterialsIssuanceReport : ComponentBase
{
    private const string ApiVersion = "1";

    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    private MudForm? _form;
    private SmirModel _model = new();
    private SmirLineItemModel _draft = new();
    private Guid? _createdId;

    private static readonly string[] Reasons = ["Sale", "Transfer", "Donation"];

    protected override void OnInitialized()
    {
        Reset();
    }

    private void Reset()
    {
        _model = new SmirModel();
        _draft = new SmirLineItemModel();
        _createdId = null;
        StateHasChanged();
    }

    private void ResetDraft()
    {
        _draft = new SmirLineItemModel { AcquisitionDate = _model.TransactionDate };
    }

    private void AddLineItem()
    {
        if (string.IsNullOrWhiteSpace(_draft.PropertyCode))
        {
            Snackbar.Add("Property code is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_draft.Name))
        {
            Snackbar.Add("Item name is required", Severity.Warning);
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

        _model.LineItems.Add(new SmirLineItemModel
        {
            PropertyCode = _draft.PropertyCode.Trim(),
            Name = _draft.Name.Trim(),
            Description = _draft.Description?.Trim(),
            AcquisitionDate = _draft.AcquisitionDate,
            Quantity = _draft.Quantity,
            Unit = _draft.Unit?.Trim(),
            UnitCost = _draft.UnitCost,
        });

        ResetDraft();
    }

    private void RemoveItem(SmirLineItemModel item)
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

        if (_model.TransactionDate?.Date > DateTime.Today)
        {
            Snackbar.Add("Transaction date cannot be in the future", Severity.Warning);
            return;
        }

        var command = new CreateSuppliesAndMaterialsIssuanceReportCommand
        {
            SmirNumber = _model.SmirNumber,
            TransactionDate = _model.TransactionDate ?? DateTime.Today,
            RecipientName = _model.RecipientName,
            RecipientAddress = _model.RecipientAddress,
            RecipientContactNumber = _model.RecipientContactNumber,
            IssuanceReason = _model.IssuanceReason,
            IssuingOfficerName = _model.IssuingOfficerName,
            IssuingDate = _model.IssuingDate ?? _model.TransactionDate ?? DateTime.Today,
            ApprovingOfficerName = _model.ApprovingOfficerName,
            ApprovingDate = _model.ApprovingDate ?? _model.TransactionDate ?? DateTime.Today,
            AuthRecipientName = _model.AuthRecipientName,
            AuthReceiptDate = _model.AuthReceiptDate ?? _model.TransactionDate ?? DateTime.Today,
            DriverName = _model.DriverName,
            BillOfLadingNumber = _model.BillOfLadingNumber,
            Notes = _model.Notes,
            LineItems = _model.LineItems.Select(li => new CreateIssuanceLineItemRequest
            {
                PropertyCode = li.PropertyCode,
                Name = li.Name,
                Description = li.Description,
                AcquisitionDate = li.AcquisitionDate ?? _model.TransactionDate ?? DateTime.Today,
                Quantity = li.Quantity,
                Unit = li.Unit,
                UnitCost = li.UnitCost,
            }).ToList(),
        };

        try
        {
            var response = await ApiClient.CreateSuppliesAndMaterialsIssuanceReportEndpointAsync(ApiVersion, command);
            _createdId = response.Id;
            Snackbar.Add("SMIR saved", Severity.Success);
            Reset();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save SMIR", Severity.Error);
        }
    }
}

#region View models
public class SmirModel
{
    [Required, MaxLength(50)]
    public string SmirNumber { get; set; } = string.Empty;

    [Required]
    public DateTime? TransactionDate { get; set; } = DateTime.Today;

    [Required, MaxLength(200)]
    public string RecipientName { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string RecipientAddress { get; set; } = string.Empty;

    public string RecipientContactNumber { get; set; } = string.Empty;

    [Required]
    public string IssuanceReason { get; set; } = "Sale";

    [Required]
    public string IssuingOfficerName { get; set; } = string.Empty;

    public DateTime? IssuingDate { get; set; } = DateTime.Today;

    public string ApprovingOfficerName { get; set; } = string.Empty;

    public DateTime? ApprovingDate { get; set; } = DateTime.Today;

    [Required]
    public string AuthRecipientName { get; set; } = string.Empty;

    public DateTime? AuthReceiptDate { get; set; } = DateTime.Today;

    public string DriverName { get; set; } = string.Empty;

    public string BillOfLadingNumber { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public List<SmirLineItemModel> LineItems { get; set; } = new();
}

public class SmirLineItemModel
{
    [Required, MaxLength(50)]
    public string PropertyCode { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? AcquisitionDate { get; set; } = DateTime.Today;

    [Range(0.01, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    public string? Unit { get; set; }

    [Range(0, double.MaxValue)]
    public double UnitCost { get; set; }
}
#endregion
