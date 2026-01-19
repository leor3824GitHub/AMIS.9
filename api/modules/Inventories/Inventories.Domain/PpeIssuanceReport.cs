using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a PPE Issuance Report (PPEIR) from the National Food Authority (NFA)
/// This is an official form used to document the issuance, transfer, or disposal of Property, Plant and Equipment (PPE)
/// from one department/custodian to another, or to external entities.
/// </summary>
public class PpeIssuanceReport : AuditableEntity, IAggregateRoot
{
    // Header and Tracking
    public string ReportNumber { get; private set; }
    public PpeReportStatus Status { get; private set; } = PpeReportStatus.Draft;

    // Recipient Information
    public PpeRecipientInfo Recipient { get; private set; }

    // Nature of Issuance
    public PpeIssuanceType IssuanceType { get; private set; }

    // Issuance Date
    public DateTime IssuanceDate { get; private set; }

    // Line Items
    private readonly List<PpeIssuanceLineItem> _lineItems = [];
    public IReadOnlyCollection<PpeIssuanceLineItem> LineItems => _lineItems.AsReadOnly();

    // Distribution Tracking
    public bool DistributedToVoucher { get; private set; }
    public bool DistributedToPMSDS { get; private set; }
    public bool DistributedToAccounting { get; private set; }
    public bool DistributedToFile { get; private set; }

    // Additional metadata
    public string? Notes { get; private set; }

    private PpeIssuanceReport()
    {
        ReportNumber = string.Empty;
        Recipient = null!;
        IssuanceType = null!;
    }

    /// <summary>
    /// Creates a new PPE Issuance Report
    /// </summary>
    public PpeIssuanceReport(
        string reportNumber,
        PpeRecipientInfo recipient,
        PpeIssuanceType issuanceType,
        DateTime issuanceDate,
        string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(reportNumber);
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(issuanceType);

        ReportNumber = reportNumber;
        Recipient = recipient;
        IssuanceType = issuanceType;
        IssuanceDate = issuanceDate;
        Notes = notes;
    }

    /// <summary>
    /// Adds a line item to the issuance report
    /// </summary>
    public void AddLineItem(PpeIssuanceLineItem lineItem)
    {
        EnsureDraft();
        ArgumentNullException.ThrowIfNull(lineItem);
        _lineItems.Add(lineItem);
    }

    /// <summary>
    /// Adds multiple line items to the issuance report
    /// </summary>
    public void AddLineItems(IEnumerable<PpeIssuanceLineItem> lineItems)
    {
        ArgumentNullException.ThrowIfNull(lineItems);

        foreach (var item in lineItems)
        {
            AddLineItem(item);
        }
    }

    /// <summary>
    /// Gets the total acquisition cost of all PPE issued
    /// </summary>
    public decimal GetTotalAcquisitionCost() => _lineItems.Sum(li => li.AcquisitionCost);

    /// <summary>
    /// Gets the total accumulated depreciation
    /// </summary>
    public decimal GetTotalAccumulatedDepreciation() => _lineItems.Sum(li => li.AccumulatedDepreciation ?? 0m);

    /// <summary>
    /// Gets the total book value
    /// </summary>
    public decimal GetTotalBookValue() => _lineItems.Sum(li => li.BookValue ?? 0m);

    /// <summary>
    /// Marks the report as distributed to specified party
    /// </summary>
    public void MarkDistributedToVoucher() => DistributedToVoucher = true;
    public void MarkDistributedToPMSDS() => DistributedToPMSDS = true;
    public void MarkDistributedToAccounting() => DistributedToAccounting = true;
    public void MarkDistributedToFile() => DistributedToFile = true;

    /// <summary>
    /// Gets the distribution status of all copies
    /// </summary>
    public IReadOnlyDictionary<string, bool> GetDistributionStatus() => new Dictionary<string, bool>
    {
        { "Voucher", DistributedToVoucher },
        { "PMSDS", DistributedToPMSDS },
        { "Accounting", DistributedToAccounting },
        { "File", DistributedToFile }
    };

    /// <summary>
    /// Posts the report, making it immutable and ready for registry update
    /// </summary>
    public void Post()
    {
        EnsureDraft();
        if (_lineItems.Count == 0)
        {
            throw new InvalidOperationException("Cannot post a report with no line items.");
        }
        Status = PpeReportStatus.Posted;
    }

    /// <summary>
    /// Cancels the report, marking it as void
    /// </summary>
    public void Cancel()
    {
        if (Status == PpeReportStatus.Cancelled)
        {
            throw new InvalidOperationException("Report is already cancelled.");
        }
        Status = PpeReportStatus.Cancelled;
    }

    /// <summary>
    /// Clears all line items (only allowed in Draft)
    /// </summary>
    public void ClearLineItems()
    {
        EnsureDraft();
        _lineItems.Clear();
    }

    /// <summary>
    /// Updates report header (only allowed in Draft)
    /// </summary>
    public void UpdateHeader(PpeRecipientInfo recipient, PpeIssuanceType issuanceType, DateTime issuanceDate, string? notes)
    {
        EnsureDraft();
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(issuanceType);

        Recipient = recipient;
        IssuanceType = issuanceType;
        IssuanceDate = issuanceDate;
        Notes = notes;
    }

    private void EnsureDraft()
    {
        if (Status != PpeReportStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot modify report in {Status} status. Only Draft reports can be edited.");
        }
    }
}

/// <summary>
/// Value object representing the recipient of PPE issuance
/// </summary>
public sealed record PpeRecipientInfo(
    string Name,
    string Address);

/// <summary>
/// Value object representing the type of PPE issuance
/// </summary>
public sealed record PpeIssuanceType(string Value)
{
    public static PpeIssuanceType TransferToCO => new("Transfer to CO");
    public static PpeIssuanceType TransferToRO => new("Transfer to RO");
    public static PpeIssuanceType TransferToPO => new("Transfer to PO");
    public static PpeIssuanceType Donation => new("Donation");
    public static PpeIssuanceType Dumping => new("Dumping");
    public static PpeIssuanceType Destruction => new("Destruction");
    public static PpeIssuanceType Sale => new("Sale");
    public static PpeIssuanceType Others => new("Others");

    public static PpeIssuanceType FromString(string value) =>
        value switch
        {
            "Transfer to CO" => TransferToCO,
            "Transfer to RO" => TransferToRO,
            "Transfer to PO" => TransferToPO,
            "Donation" => Donation,
            "Dumping" => Dumping,
            "Destruction" => Destruction,
            "Sale" => Sale,
            "Others" => Others,
            _ => throw new ArgumentException($"Unknown issuance type: {value}")
        };
}

/// <summary>
/// Value object representing a PPE line item for issuance
/// </summary>
public sealed record PpeIssuanceLineItem(
    string PropertyCode,
    string? SerialNumber,
    string Specification,
    DateTime DateAcquired,
    decimal AcquisitionCost,
    decimal? AccumulatedDepreciation = null,
    decimal? BookValue = null);

/// <summary>
/// Value object representing authentication for PPE issuance
/// </summary>
public sealed record PpeIssuanceAuthentication(
    string IssuedByName,
    string IssuedBySignature,
    DateTime IssuedDate,
    string CountersignedByName,
    string CountersignedBySignature,
    DateTime CountersignedDate,
    string ReceivedByName,
    string ReceivedBySignature,
    DateTime DateReceived,
    string ApprovedByName,
    string ApprovedBySignature,
    DateTime ApprovedDate,
    string? DriverName = null,
    string? DriverSignature = null,
    string? BillOfLadingNumber = null);

