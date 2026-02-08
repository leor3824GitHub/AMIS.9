using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a PPE Receiving Report (PPERR) from the National Food Authority (NFA)
/// This is an official form used to document the receipt of Property, Plant and Equipment (PPE)
/// from suppliers, transfers, or donations.
/// </summary>
public class PPERR : AuditableEntity, IAggregateRoot
{
    // Header and Tracking
    public PpeReportStatus Status { get; private set; } = PpeReportStatus.Draft;

    // The RR No. serves as the Identity
    public string RRNumber { get; private set; }
    public string ReceivedFrom { get; private set; }
    public string Address { get; private set; }
    public PpeReceiptType Type { get; private set; } = PpeReceiptType.Purchase;
    public DateTime Date { get; private set; }

    // Navigation property: The collection of property references
    private readonly List<PPERRLineItem> _items = new();
    public IReadOnlyCollection<PPERRLineItem> Items => _items.AsReadOnly();

    // Additional metadata
    public string? Notes { get; private set; }

    private PPERR()
    {
        RRNumber = string.Empty;
        ReceivedFrom = string.Empty;
        Address = string.Empty;
    }

    /// <summary>
    /// Creates a new PPE Receiving Report
    /// </summary>
    public PPERR(
        string rrNumber,
        string receivedFrom,
        string address,
        PpeReceiptType type,
        DateTime date,
        string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(rrNumber);
        ArgumentNullException.ThrowIfNull(receivedFrom);
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(type);

        RRNumber = rrNumber;
        ReceivedFrom = receivedFrom;
        Address = address;
        Type = type;
        Date = date;
        Notes = notes;
    }

    /// <summary>
    /// Adds a line item to the receiving report
    /// </summary>
    public void AddItem(PPERRLineItem item)
    {
        EnsureDraft();
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    /// <summary>
    /// Adds multiple line items to the receiving report
    /// </summary>
    public void AddItems(IEnumerable<PPERRLineItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    /// <summary>
    /// Gets the total acquisition cost of all items received
    /// </summary>
    public decimal GetTotalAmount() => _items.Count;

    /// <summary>
    /// Posts the report, making it immutable and ready for registry update
    /// </summary>
    public void Post()
    {
        if (Status == PpeReportStatus.Posted)
        {
            throw new InvalidOperationException($"Report {RRNumber} is already posted. Cannot post again.");
        }

        EnsureDraft();
        if (_items.Count == 0)
        {
            throw new InvalidOperationException("Cannot post a report with no items.");
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
    /// Clears all items (only allowed in Draft)
    /// </summary>
    public void ClearItems()
    {
        EnsureDraft();
        _items.Clear();
    }

    /// <summary>
    /// Updates report header (only allowed in Draft)
    /// </summary>
    public void UpdateHeader(string receivedFrom, string address, PpeReceiptType type, DateTime date, string? notes)
    {
        EnsureDraft();
        ArgumentNullException.ThrowIfNull(receivedFrom);
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(type);

        ReceivedFrom = receivedFrom;
        Address = address;
        Type = type;
        Date = date;
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
