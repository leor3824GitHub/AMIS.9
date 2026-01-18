using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a PPE Receiving Report (PPERR) from the National Food Authority (NFA)
/// This is an official form used to document the receipt of Property, Plant and Equipment (PPE)
/// from suppliers, transfers, or donations.
/// </summary>
public class PpeReceivingReport : AuditableEntity, IAggregateRoot
{
    // Header and Tracking
    public string ReportNumber { get; private set; }
    public string Location { get; private set; }

    // Source Information
    public PpeSourceInfo Source { get; private set; }

    // Nature of Receipt
    public PpeReceiptType ReceiptType { get; private set; }

    // Line Items
    private readonly List<PpeReceivingLineItem> _lineItems = [];
    public IReadOnlyCollection<PpeReceivingLineItem> LineItems => _lineItems.AsReadOnly();

    // Distribution Tracking
    public bool DistributedToVoucher { get; private set; }
    public bool DistributedToPMSDS { get; private set; }
    public bool DistributedToAccounting { get; private set; }
    public bool DistributedToFile { get; private set; }

    // Additional metadata
    public string? Notes { get; private set; }

    private PpeReceivingReport()
    {
        ReportNumber = string.Empty;
        Location = string.Empty;
        Source = null!;
        ReceiptType = null!;
    }

    /// <summary>
    /// Creates a new PPE Receiving Report
    /// </summary>
    public PpeReceivingReport(
        string reportNumber,
        string location,
        PpeSourceInfo source,
        PpeReceiptType receiptType,
        string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(reportNumber);
        ArgumentNullException.ThrowIfNull(location);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(receiptType);

        ReportNumber = reportNumber;
        Location = location;
        Source = source;
        ReceiptType = receiptType;
        Notes = notes;
    }

    /// <summary>
    /// Adds a line item to the receiving report
    /// </summary>
    public void AddLineItem(PpeReceivingLineItem lineItem)
    {
        ArgumentNullException.ThrowIfNull(lineItem);
        _lineItems.Add(lineItem);
    }

    /// <summary>
    /// Adds multiple line items to the receiving report
    /// </summary>
    public void AddLineItems(IEnumerable<PpeReceivingLineItem> lineItems)
    {
        ArgumentNullException.ThrowIfNull(lineItems);

        foreach (var item in lineItems)
        {
            AddLineItem(item);
        }
    }

    /// <summary>
    /// Gets the total acquisition cost of all PPE received
    /// </summary>
    public decimal GetTotalAmount() => _lineItems.Sum(li => li.Amount);

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
}

/// <summary>
/// Value object representing the source of PPE receipt
/// </summary>
public sealed record PpeSourceInfo(
    string Name,
    string Address,
    DateTime ReceiptDate);

/// <summary>
/// Value object representing the type of PPE receipt
/// </summary>
public sealed record PpeReceiptType(string Value)
{
    public static PpeReceiptType Purchase => new("Purchase");
    public static PpeReceiptType Transfer => new("Transfer");
    public static PpeReceiptType Donation => new("Donation");
    public static PpeReceiptType Others => new("Others");

    public static PpeReceiptType FromString(string value) =>
        value switch
        {
            "Purchase" => Purchase,
            "Transfer" => Transfer,
            "Donation" => Donation,
            "Others" => Others,
            _ => throw new ArgumentException($"Unknown receipt type: {value}")
        };
}

/// <summary>
/// Value object representing a PPE line item
/// </summary>
public sealed record PpeReceivingLineItem(
    string PropertyCode,
    string Description,
    DateTime DateAcquired,
    decimal Quantity,
    string Unit,
    decimal UnitCost)
{
    public decimal Amount => Quantity * UnitCost;
}

/// <summary>
/// Value object representing authentication for PPE receiving
/// </summary>
public sealed record PpeReceivingAuthentication(
    string ReceivedByName,
    DateTime ReceivedDate,
    string NotedByName,
    DateTime NotedDate);

