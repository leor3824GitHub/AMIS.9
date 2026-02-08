using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a PPE Issuance Report (PPEIR) from the National Food Authority (NFA)
/// This is an official form used to document the issuance of Property, Plant and Equipment (PPE)
/// to employees or departments.
/// </summary>
public class PPEIR : AuditableEntity, IAggregateRoot
{
    // Header and Tracking
    public PpeReportStatus Status { get; private set; } = PpeReportStatus.Draft;

    // The IR No. serves as the Identity
    public string IRNumber { get; private set; }
    public string IssuedTo { get; private set; }
    public string Address { get; private set; }
    public PpeIssueType Type { get; private set; } = PpeIssueType.IssuedToEmployee;
    public DateTime Date { get; private set; }

    // Navigation property: The collection of property references
    private readonly List<PPEIRLineItem> _items = new();
    public IReadOnlyCollection<PPEIRLineItem> Items => _items.AsReadOnly();

    // Additional metadata
    public string? Notes { get; private set; }

    private PPEIR()
    {
        IRNumber = string.Empty;
        IssuedTo = string.Empty;
        Address = string.Empty;
    }

    /// <summary>
    /// Creates a new PPE Issuance Report
    /// </summary>
    public PPEIR(
        string irNumber,
        string issuedTo,
        string address,
        PpeIssueType type,
        DateTime date,
        string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(irNumber);
        ArgumentNullException.ThrowIfNull(issuedTo);
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(type);

        IRNumber = irNumber;
        IssuedTo = issuedTo;
        Address = address;
        Type = type;
        Date = date;
        Notes = notes;
    }

    /// <summary>
    /// Adds a line item to the issuance report
    /// </summary>
    public void AddItem(PPEIRLineItem item)
    {
        EnsureDraft();
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    /// <summary>
    /// Adds multiple line items to the issuance report
    /// </summary>
    public void AddItems(IEnumerable<PPEIRLineItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    /// <summary>
    /// Gets the total items issued
    /// </summary>
    public decimal GetTotalAmount() => _items.Count;

    /// <summary>
    /// Posts the report, making it immutable and ready for issuance
    /// </summary>
    public void Post()
    {
        if (Status == PpeReportStatus.Posted)
        {
            throw new InvalidOperationException($"Report {IRNumber} is already posted. Cannot post again.");
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
    public void UpdateHeader(string issuedTo, string address, PpeIssueType type, DateTime date, string? notes)
    {
        EnsureDraft();
        ArgumentNullException.ThrowIfNull(issuedTo);
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(type);

        IssuedTo = issuedTo;
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

public sealed record PpeIssueType(string Value)
{
    public static PpeIssueType IssuedToEmployee => new("IssuedToEmployee");
    public static PpeIssueType IssuedToDepartment => new("IssuedToDepartment");
    public static PpeIssueType Transfer => new("Transfer");
    public static PpeIssueType Return => new("Return");

    public static PpeIssueType FromString(string value) =>
        value switch
        {
            "IssuedToEmployee" => IssuedToEmployee,
            "IssuedToDepartment" => IssuedToDepartment,
            "Transfer" => Transfer,
            "Return" => Return,
            _ => throw new ArgumentException($"Unknown issue type: {value}")
        };
}
