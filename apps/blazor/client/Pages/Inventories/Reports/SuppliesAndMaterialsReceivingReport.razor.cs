using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class SuppliesAndMaterialsReceivingReport : ComponentBase
{
    private const string ApiVersion = "1";

    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    private MudForm? _form;
    private SmrrModel _model = new();
    private SmrrLineItemModel _draft = new();
    private Guid? _createdId;

    private static readonly string[] TransactionTypes = ["Purchase", "Transfer", "Donation", "Others"];

    protected override void OnInitialized()
    {
        Reset();
    }

    private void Reset()
    {
        _model = new SmrrModel();
        _draft = new SmrrLineItemModel { AcquisitionDate = _model.ReceivingDate };
        _createdId = null;
        StateHasChanged();
    }

    private void ResetDraft()
    {
        _draft = new SmrrLineItemModel { AcquisitionDate = _model.ReceivingDate };
    }

    private void AddLineItem()
    {
        if (string.IsNullOrWhiteSpace(_draft.Name))
        {
            Snackbar.Add("Item name is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_draft.Description))
        {
            Snackbar.Add("Description is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_draft.Location))
        {
            Snackbar.Add("Location is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_draft.Unit))
        {
            Snackbar.Add("Unit is required", Severity.Warning);
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

        var acquisitionDate = _draft.AcquisitionDate ?? _model.ReceivingDate;

        _model.LineItems.Add(new SmrrLineItemModel
        {
            Name = _draft.Name.Trim(),
            Description = _draft.Description?.Trim(),
            AcquisitionDate = acquisitionDate,
            Quantity = _draft.Quantity,
            Unit = _draft.Unit?.Trim(),
            UnitCost = _draft.UnitCost,
            Reference = _draft.Reference?.Trim(),
            Location = _draft.Location.Trim(),
        });

        ResetDraft();
    }

    private void RemoveItem(SmrrLineItemModel item)
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

        if (_model.ReceivingDate?.Date > DateTime.Today)
        {
            Snackbar.Add("Receiving date cannot be in the future", Severity.Warning);
            return;
        }

        var command = new CreateSuppliesAndMaterialsReceivingReportCommand
        {
            SmrrNumber = _model.SmrrNumber,
            SourceName = _model.SourceName,
            SourceAddress = _model.SourceAddress,
            ReceivingDate = _model.ReceivingDate ?? DateTime.Today,
            TransactionType = _model.TransactionType,
            ReceivedByName = _model.ReceivedByName,
            ReceivedDate = _model.ReceivedDate ?? _model.ReceivingDate ?? DateTime.Today,
            NotedByName = _model.NotedByName,
            NotedDate = _model.NotedDate ?? _model.ReceivingDate ?? DateTime.Today,
            Notes = _model.Notes,
            LineItems = _model.LineItems.Select(li => new CreateReceivingLineItemRequest
            {
                Name = li.Name,
                Description = li.Description,
                AcquisitionDate = li.AcquisitionDate ?? _model.ReceivingDate ?? DateTime.Today,
                Quantity = li.Quantity,
                Unit = li.Unit,
                UnitCost = li.UnitCost,
                Reference = li.Reference,
                Location = li.Location,
            }).ToList(),
        };

        try
        {
            var response = await ApiClient.CreateSuppliesAndMaterialsReceivingReportEndpointAsync(ApiVersion, command);
            _createdId = response.Id;
            Snackbar.Add("SMRR saved", Severity.Success);
            Reset();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save SMRR", Severity.Error);
        }
    }
}

#region View models
public class SmrrModel
{
    [Required, MaxLength(50)]
    public string SmrrNumber { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string SourceName { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string SourceAddress { get; set; } = string.Empty;

    [Required]
    public DateTime? ReceivingDate { get; set; } = DateTime.Today;

    [Required]
    public string TransactionType { get; set; } = "Purchase";

    [Required]
    public string ReceivedByName { get; set; } = string.Empty;

    public DateTime? ReceivedDate { get; set; } = DateTime.Today;

    [Required]
    public string NotedByName { get; set; } = string.Empty;

    public DateTime? NotedDate { get; set; } = DateTime.Today;

    public string Notes { get; set; } = string.Empty;

    public List<SmrrLineItemModel> LineItems { get; set; } = new();
}

public class SmrrLineItemModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string? Description { get; set; }

    [Required]
    public string Location { get; set; } = string.Empty;

    public DateTime? AcquisitionDate { get; set; } = DateTime.Today;

    [Range(0.01, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    [Required]
    public string? Unit { get; set; } = "pc";

    [Range(0, double.MaxValue)]
    public double UnitCost { get; set; }

    public string? Reference { get; set; }
}
#endregion
