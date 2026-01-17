using System.ComponentModel.DataAnnotations;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class SupplyAndPpeReports : ComponentBase
{
    private const string ApiVersion = "1";

    [Inject] public IApiClient ApiClient { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    // SMIR state
    private MudForm? _smirForm;
    private SmirModel _smirModel = new();
    private SmirLineItemModel _smirDraft = new();
    private Guid? _smirCreatedId;

    // SMRR state
    private MudForm? _smrrForm;
    private SmrrModel _smrrModel = new();
    private SmrrLineItemModel _smrrDraft = new();
    private Guid? _smrrCreatedId;

    // PPE Issuance state
    private MudForm? _ppeIssuanceForm;
    private PpeIssuanceModel _ppeIssuanceModel = new();
    private PpeIssuanceLineItemModel _ppeIssuanceDraft = new();
    private Guid? _ppeIssuanceCreatedId;

    // PPE Receiving state
    private MudForm? _ppeReceivingForm;
    private PpeReceivingModel _ppeReceivingModel = new();
    private PpeReceivingLineItemModel _ppeReceivingDraft = new();
    private Guid? _ppeReceivingCreatedId;

    protected override void OnInitialized()
    {
        ResetAll();
    }

    private void ResetAll()
    {
        ResetSmir();
        ResetSmrr();
        ResetPpeIssuance();
        ResetPpeReceiving();
    }

    #region SMIR helpers
    private static readonly string[] SmirReasons = ["Sale", "Transfer", "Donation"];

    private void ResetSmir()
    {
        _smirModel = new SmirModel();
        _smirDraft = new SmirLineItemModel();
        _smirCreatedId = null;
        StateHasChanged();
    }

    private void ResetSmirDraft()
    {
        _smirDraft = new SmirLineItemModel { AcquisitionDate = _smirModel.TransactionDate };
    }

    private void AddSmirLineItem()
    {
        if (string.IsNullOrWhiteSpace(_smirDraft.Name))
        {
            Snackbar.Add("Item name is required", Severity.Warning);
            return;
        }

        if (_smirDraft.Quantity <= 0)
        {
            Snackbar.Add("Quantity must be greater than zero", Severity.Warning);
            return;
        }

        if (_smirDraft.UnitCost < 0)
        {
            Snackbar.Add("Unit cost cannot be negative", Severity.Warning);
            return;
        }

        _smirModel.LineItems.Add(new SmirLineItemModel
        {
            Name = _smirDraft.Name.Trim(),
            Description = _smirDraft.Description?.Trim(),
            AcquisitionDate = _smirDraft.AcquisitionDate,
            Quantity = _smirDraft.Quantity,
            Unit = _smirDraft.Unit?.Trim(),
            UnitCost = _smirDraft.UnitCost,
            Reference = _smirDraft.Reference?.Trim(),
        });

        ResetSmirDraft();
    }

    private void RemoveSmirItem(SmirLineItemModel item)
    {
        _smirModel.LineItems.Remove(item);
    }

    private async Task SubmitSmirAsync()
    {
        if (_smirForm is null)
        {
            return;
        }

        await _smirForm.Validate();

        if (!_smirForm.IsValid)
        {
            Snackbar.Add("Please fix validation errors in SMIR", Severity.Warning);
            return;
        }

        if (_smirModel.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item", Severity.Warning);
            return;
        }

        if (_smirModel.TransactionDate.Date > DateTime.Today)
        {
            Snackbar.Add("Transaction date cannot be in the future", Severity.Warning);
            return;
        }

        var command = new CreateSuppliesAndMaterialsIssuanceReportCommand
        {
            SmirNumber = _smirModel.SmirNumber,
            TransactionDate = _smirModel.TransactionDate,
            RecipientName = _smirModel.RecipientName,
            RecipientAddress = _smirModel.RecipientAddress,
            RecipientContactNumber = _smirModel.RecipientContactNumber,
            IssuanceReason = _smirModel.IssuanceReason,
            IssuingOfficerName = _smirModel.IssuingOfficerName,
            IssuingOfficerSignature = _smirModel.IssuingOfficerSignature,
            IssuingDate = _smirModel.IssuingDate ?? _smirModel.TransactionDate,
            ApprovingOfficerName = _smirModel.ApprovingOfficerName,
            ApprovingOfficerSignature = _smirModel.ApprovingOfficerSignature,
            ApprovingDate = _smirModel.ApprovingDate ?? _smirModel.TransactionDate,
            AuthRecipientName = _smirModel.AuthRecipientName,
            AuthRecipientSignature = _smirModel.AuthRecipientSignature,
            AuthReceiptDate = _smirModel.AuthReceiptDate ?? _smirModel.TransactionDate,
            DriverName = _smirModel.DriverName,
            DriverSignature = _smirModel.DriverSignature,
            BillOfLadingNumber = _smirModel.BillOfLadingNumber,
            Notes = _smirModel.Notes,
            LineItems = _smirModel.LineItems.Select(li => new CreateIssuanceLineItemRequest
            {
                Name = li.Name,
                Description = li.Description,
                AcquisitionDate = li.AcquisitionDate ?? _smirModel.TransactionDate,
                Quantity = li.Quantity,
                Unit = li.Unit,
                UnitCost = li.UnitCost,
            }).ToList(),
        };

        try
        {
            var response = await ApiClient.CreateSuppliesAndMaterialsIssuanceReportEndpointAsync(ApiVersion, command);
            _smirCreatedId = response.Id;
            Snackbar.Add("SMIR saved", Severity.Success);
            ResetSmir();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save SMIR", Severity.Error);
        }
    }
    #endregion

    #region SMRR helpers
    private static readonly string[] SmrrTransactionTypes = ["Purchase", "Transfer", "Donation", "Others"];

    private void ResetSmrr()
    {
        _smrrModel = new SmrrModel();
        _smrrDraft = new SmrrLineItemModel { AcquisitionDate = _smrrModel.ReceivingDate };
        _smrrCreatedId = null;
        StateHasChanged();
    }

    private void ResetSmrrDraft()
    {
        _smrrDraft = new SmrrLineItemModel { AcquisitionDate = _smrrModel.ReceivingDate };
    }

    private void AddSmrrLineItem()
    {
        if (string.IsNullOrWhiteSpace(_smrrDraft.Name))
        {
            Snackbar.Add("Item name is required", Severity.Warning);
            return;
        }

        if (_smrrDraft.Quantity <= 0)
        {
            Snackbar.Add("Quantity must be greater than zero", Severity.Warning);
            return;
        }

        if (_smrrDraft.UnitCost < 0)
        {
            Snackbar.Add("Unit cost cannot be negative", Severity.Warning);
            return;
        }

        var acquisitionDate = _smrrDraft.AcquisitionDate ?? _smrrModel.ReceivingDate;

        _smrrModel.LineItems.Add(new SmrrLineItemModel
        {
            Name = _smrrDraft.Name.Trim(),
            Description = _smrrDraft.Description?.Trim(),
            AcquisitionDate = acquisitionDate,
            Quantity = _smrrDraft.Quantity,
            Unit = _smrrDraft.Unit?.Trim(),
            UnitCost = _smrrDraft.UnitCost,
            Reference = _smrrDraft.Reference?.Trim(),
        });

        ResetSmrrDraft();
    }

    private void RemoveSmrrItem(SmrrLineItemModel item)
    {
        _smrrModel.LineItems.Remove(item);
    }

    private async Task SubmitSmrrAsync()
    {
        if (_smrrForm is null)
        {
            return;
        }

        await _smrrForm.Validate();

        if (!_smrrForm.IsValid)
        {
            Snackbar.Add("Please fix validation errors in SMRR", Severity.Warning);
            return;
        }

        if (_smrrModel.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item", Severity.Warning);
            return;
        }

        if (_smrrModel.ReceivingDate.Date > DateTime.Today)
        {
            Snackbar.Add("Receiving date cannot be in the future", Severity.Warning);
            return;
        }

        var command = new CreateSuppliesAndMaterialsReceivingReportCommand
        {
            SmrrNumber = _smrrModel.SmrrNumber,
            Location = _smrrModel.Location,
            SourceName = _smrrModel.SourceName,
            SourceAddress = _smrrModel.SourceAddress,
            ReceivingDate = _smrrModel.ReceivingDate,
            TransactionType = _smrrModel.TransactionType,
            ReceivedByName = _smrrModel.ReceivedByName,
            ReceivedBySignature = _smrrModel.ReceivedBySignature,
            ReceivedDate = _smrrModel.ReceivedDate ?? _smrrModel.ReceivingDate,
            NotedByName = _smrrModel.NotedByName,
            NotedBySignature = _smrrModel.NotedBySignature,
            NotedDate = _smrrModel.NotedDate ?? _smrrModel.ReceivingDate,
            Notes = _smrrModel.Notes,
            LineItems = _smrrModel.LineItems.Select(li => new CreateReceivingLineItemRequest
            {
                Name = li.Name,
                Description = li.Description,
                AcquisitionDate = li.AcquisitionDate ?? _smrrModel.ReceivingDate,
                Quantity = li.Quantity,
                Unit = li.Unit,
                UnitCost = li.UnitCost,
                Reference = li.Reference,
            }).ToList(),
        };

        try
        {
            var response = await ApiClient.CreateSuppliesAndMaterialsReceivingReportEndpointAsync(ApiVersion, command);
            _smrrCreatedId = response.Id;
            Snackbar.Add("SMRR saved", Severity.Success);
            ResetSmrr();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save SMRR", Severity.Error);
        }
    }
    #endregion

    #region PPE Issuance helpers
    private void ResetPpeIssuance()
    {
        _ppeIssuanceModel = new PpeIssuanceModel();
        _ppeIssuanceDraft = new PpeIssuanceLineItemModel();
        _ppeIssuanceCreatedId = null;
        StateHasChanged();
    }

    private void ResetPpeIssuanceDraft()
    {
        _ppeIssuanceDraft = new PpeIssuanceLineItemModel();
    }

    private void AddPpeIssuanceLineItem()
    {
        if (string.IsNullOrWhiteSpace(_ppeIssuanceDraft.PropertyCode))
        {
            Snackbar.Add("Property code is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_ppeIssuanceDraft.Description))
        {
            Snackbar.Add("Description is required", Severity.Warning);
            return;
        }

        if (_ppeIssuanceDraft.Quantity <= 0)
        {
            Snackbar.Add("Quantity must be greater than zero", Severity.Warning);
            return;
        }

        if (_ppeIssuanceDraft.AcquisitionCost < 0)
        {
            Snackbar.Add("Acquisition cost cannot be negative", Severity.Warning);
            return;
        }

        _ppeIssuanceModel.LineItems.Add(new PpeIssuanceLineItemModel
        {
            PropertyCode = _ppeIssuanceDraft.PropertyCode.Trim(),
            Description = _ppeIssuanceDraft.Description.Trim(),
            Quantity = _ppeIssuanceDraft.Quantity,
            Unit = _ppeIssuanceDraft.Unit?.Trim() ?? string.Empty,
            AcquisitionCost = _ppeIssuanceDraft.AcquisitionCost,
            AccumulatedDepreciation = _ppeIssuanceDraft.AccumulatedDepreciation,
            BookValue = _ppeIssuanceDraft.BookValue,
        });

        ResetPpeIssuanceDraft();
    }

    private void RemovePpeIssuanceItem(PpeIssuanceLineItemModel item)
    {
        _ppeIssuanceModel.LineItems.Remove(item);
    }

    private async Task SubmitPpeIssuanceAsync()
    {
        if (_ppeIssuanceForm is null)
        {
            return;
        }

        await _ppeIssuanceForm.Validate();

        if (!_ppeIssuanceForm.IsValid)
        {
            Snackbar.Add("Please fix validation errors in PPE Issuance", Severity.Warning);
            return;
        }

        if (_ppeIssuanceModel.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item", Severity.Warning);
            return;
        }

        if (_ppeIssuanceModel.IssuanceDate.Date > DateTime.Today)
        {
            Snackbar.Add("Issuance date cannot be in the future", Severity.Warning);
            return;
        }

        var command = new CreatePpeIssuanceReportCommand
        {
            ReportNumber = _ppeIssuanceModel.ReportNumber,
            RecipientName = _ppeIssuanceModel.RecipientName,
            RecipientAddress = _ppeIssuanceModel.RecipientAddress,
            IssuanceType = _ppeIssuanceModel.IssuanceType,
            IssuanceDate = _ppeIssuanceModel.IssuanceDate,
            Notes = _ppeIssuanceModel.Notes,
            LineItems = _ppeIssuanceModel.LineItems.Select(li => new CreatePpeIssuanceLineItemRequest
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
            var response = await ApiClient.CreatePpeIssuanceReportEndpointAsync(ApiVersion, command);
            _ppeIssuanceCreatedId = response.Id;
            Snackbar.Add("PPE Issuance Report saved", Severity.Success);
            ResetPpeIssuance();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save PPE issuance", Severity.Error);
        }
    }
    #endregion

    #region PPE Receiving helpers
    private static readonly string[] PpeReceiptTypes = ["Purchase", "Transfer", "Donation", "Return", "Others"];

    private void ResetPpeReceiving()
    {
        _ppeReceivingModel = new PpeReceivingModel();
        _ppeReceivingDraft = new PpeReceivingLineItemModel { DateAcquired = _ppeReceivingModel.SourceReceiptDate };
        _ppeReceivingCreatedId = null;
        StateHasChanged();
    }

    private void ResetPpeReceivingDraft()
    {
        _ppeReceivingDraft = new PpeReceivingLineItemModel { DateAcquired = _ppeReceivingModel.SourceReceiptDate };
    }

    private void AddPpeReceivingLineItem()
    {
        if (string.IsNullOrWhiteSpace(_ppeReceivingDraft.PropertyCode))
        {
            Snackbar.Add("Property code is required", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_ppeReceivingDraft.Description))
        {
            Snackbar.Add("Description is required", Severity.Warning);
            return;
        }

        if (_ppeReceivingDraft.Quantity <= 0)
        {
            Snackbar.Add("Quantity must be greater than zero", Severity.Warning);
            return;
        }

        if (_ppeReceivingDraft.UnitCost < 0)
        {
            Snackbar.Add("Unit cost cannot be negative", Severity.Warning);
            return;
        }

        _ppeReceivingModel.LineItems.Add(new PpeReceivingLineItemModel
        {
            PropertyCode = _ppeReceivingDraft.PropertyCode.Trim(),
            Description = _ppeReceivingDraft.Description?.Trim(),
            DateAcquired = _ppeReceivingDraft.DateAcquired,
            Quantity = _ppeReceivingDraft.Quantity,
            Unit = _ppeReceivingDraft.Unit?.Trim() ?? string.Empty,
            UnitCost = _ppeReceivingDraft.UnitCost,
        });

        ResetPpeReceivingDraft();
    }

    private void RemovePpeReceivingItem(PpeReceivingLineItemModel item)
    {
        _ppeReceivingModel.LineItems.Remove(item);
    }

    private async Task SubmitPpeReceivingAsync()
    {
        if (_ppeReceivingForm is null)
        {
            return;
        }

        await _ppeReceivingForm.Validate();

        if (!_ppeReceivingForm.IsValid)
        {
            Snackbar.Add("Please fix validation errors in PPER", Severity.Warning);
            return;
        }

        if (_ppeReceivingModel.LineItems.Count == 0)
        {
            Snackbar.Add("Add at least one line item", Severity.Warning);
            return;
        }

        if (_ppeReceivingModel.SourceReceiptDate.Date > DateTime.Today)
        {
            Snackbar.Add("Receipt date cannot be in the future", Severity.Warning);
            return;
        }

        var command = new CreatePpeReceivingReportCommand
        {
            ReportNumber = _ppeReceivingModel.ReportNumber,
            Location = _ppeReceivingModel.Location,
            SourceName = _ppeReceivingModel.SourceName,
            SourceAddress = _ppeReceivingModel.SourceAddress,
            ReceiptType = _ppeReceivingModel.ReceiptType,
            SourceReceiptDate = _ppeReceivingModel.SourceReceiptDate,
            Notes = _ppeReceivingModel.Notes,
            LineItems = _ppeReceivingModel.LineItems.Select(li => new CreatePpeReceivingLineItemRequest
            {
                PropertyCode = li.PropertyCode,
                Description = li.Description,
                DateAcquired = li.DateAcquired ?? _ppeReceivingModel.SourceReceiptDate,
                Quantity = li.Quantity,
                Unit = li.Unit,
                UnitCost = li.UnitCost,
            }).ToList(),
        };

        try
        {
            var response = await ApiClient.CreatePpeReceivingReportEndpointAsync(ApiVersion, command);
            _ppeReceivingCreatedId = response.Id;
            Snackbar.Add("PPE Receiving Report saved", Severity.Success);
            ResetPpeReceiving();
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Response ?? "Failed to save PPER", Severity.Error);
        }
    }
    #endregion
}

#region View models
public class SmirModel
{
    [Required, MaxLength(50)]
    public string SmirNumber { get; set; } = string.Empty;

    [Required]
    public DateTime TransactionDate { get; set; } = DateTime.Today;

    [Required, MaxLength(200)]
    public string RecipientName { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string RecipientAddress { get; set; } = string.Empty;

    public string RecipientContactNumber { get; set; } = string.Empty;

    [Required]
    public string IssuanceReason { get; set; } = "Sale";

    [Required]
    public string IssuingOfficerName { get; set; } = string.Empty;

    public string IssuingOfficerSignature { get; set; } = string.Empty;

    public DateTime? IssuingDate { get; set; } = DateTime.Today;

    public string ApprovingOfficerName { get; set; } = string.Empty;

    public string ApprovingOfficerSignature { get; set; } = string.Empty;

    public DateTime? ApprovingDate { get; set; } = DateTime.Today;

    [Required]
    public string AuthRecipientName { get; set; } = string.Empty;

    public string AuthRecipientSignature { get; set; } = string.Empty;

    public DateTime? AuthReceiptDate { get; set; } = DateTime.Today;

    public string DriverName { get; set; } = string.Empty;

    public string DriverSignature { get; set; } = string.Empty;

    public string BillOfLadingNumber { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public List<SmirLineItemModel> LineItems { get; set; } = new();
}

public class SmirLineItemModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? AcquisitionDate { get; set; } = DateTime.Today;

    [Range(0.01, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    public string? Unit { get; set; }

    [Range(0, double.MaxValue)]
    public double UnitCost { get; set; }

    public string? Reference { get; set; }
}

public class SmrrModel
{
    [Required, MaxLength(50)]
    public string SmrrNumber { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Location { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string SourceName { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string SourceAddress { get; set; } = string.Empty;

    [Required]
    public DateTime ReceivingDate { get; set; } = DateTime.Today;

    [Required]
    public string TransactionType { get; set; } = "Purchase";

    [Required]
    public string ReceivedByName { get; set; } = string.Empty;

    public string ReceivedBySignature { get; set; } = string.Empty;

    public DateTime? ReceivedDate { get; set; } = DateTime.Today;

    [Required]
    public string NotedByName { get; set; } = string.Empty;

    public string NotedBySignature { get; set; } = string.Empty;

    public DateTime? NotedDate { get; set; } = DateTime.Today;

    public string Notes { get; set; } = string.Empty;

    public List<SmrrLineItemModel> LineItems { get; set; } = new();
}

public class SmrrLineItemModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? AcquisitionDate { get; set; } = DateTime.Today;

    [Range(0.01, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    public string? Unit { get; set; }

    [Range(0, double.MaxValue)]
    public double UnitCost { get; set; }

    public string? Reference { get; set; }
}

public class PpeIssuanceModel
{
    [Required, MaxLength(50)]
    public string ReportNumber { get; set; } = string.Empty;

    [Required]
    public string RecipientName { get; set; } = string.Empty;

    [Required]
    public string RecipientAddress { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string IssuanceType { get; set; } = string.Empty;

    [Required]
    public DateTime IssuanceDate { get; set; } = DateTime.Today;

    public string Notes { get; set; } = string.Empty;

    public List<PpeIssuanceLineItemModel> LineItems { get; set; } = new();
}

public class PpeIssuanceLineItemModel
{
    [Required, MaxLength(50)]
    public string PropertyCode { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    [Required, MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public double AcquisitionCost { get; set; }

    public double? AccumulatedDepreciation { get; set; }

    public double? BookValue { get; set; }
}

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
    public DateTime SourceReceiptDate { get; set; } = DateTime.Today;

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

