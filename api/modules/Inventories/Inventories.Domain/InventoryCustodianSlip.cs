using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents an Inventory Custodian Slip (ICS) for Semi-Expendable Assets
/// This is an official form used to document the issuance and assignment of semi-expendable assets 
/// (assets valued at ≤ ₱50,000) to individual custodians/employees, establishing their accountability for the assets.
/// </summary>
public class InventoryCustodianSlip : AuditableEntity, IAggregateRoot
{
    // Header and Tracking
    public string ICSNumber { get; private set; }
    public ICSStatus Status { get; private set; } = ICSStatus.Draft;

    // Custodian/Employee Information
    public Guid EmployeeId { get; private set; }
    public virtual Employee Employee { get; private set; } = default!;

    // Issuance Information
    public DateTime IssuanceDate { get; private set; }
    public string? IssuancePurpose { get; private set; }
    public string? IssuanceLocation { get; private set; }

    // Line Items
    private readonly List<ICSLineItem> _lineItems = [];
    public IReadOnlyCollection<ICSLineItem> LineItems => _lineItems.AsReadOnly();

    // Return Information
    public DateTime? ReturnDate { get; private set; }
    public string? ReturnRemarks { get; private set; }
    public Guid? ReceivedByEmployeeId { get; private set; }
    public virtual Employee? ReceivedByEmployee { get; private set; }

    // Authentication/Signatures
    public string? IssuedByName { get; private set; }
    public DateTime? IssuedByDate { get; private set; }
    public string? ReceivedByName { get; private set; }
    public DateTime? ReceivedByDate { get; private set; }
    public string? ApprovedByName { get; private set; }
    public DateTime? ApprovedByDate { get; private set; }

    // Additional metadata
    public string? Notes { get; private set; }

    private InventoryCustodianSlip()
    {
        ICSNumber = string.Empty;
    }

    /// <summary>
    /// Creates a new Inventory Custodian Slip
    /// </summary>
    public InventoryCustodianSlip(
        string icsNumber,
        Guid employeeId,
        DateTime issuanceDate,
        string? issuancePurpose = null,
        string? issuanceLocation = null,
        string? notes = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(icsNumber);
        if (employeeId == Guid.Empty)
            throw new ArgumentException("Employee ID cannot be empty.", nameof(employeeId));

        ICSNumber = icsNumber;
        EmployeeId = employeeId;
        IssuanceDate = issuanceDate;
        IssuancePurpose = issuancePurpose;
        IssuanceLocation = issuanceLocation;
        Notes = notes;
    }

    /// <summary>
    /// Adds a line item to the ICS
    /// </summary>
    public void AddLineItem(ICSLineItem lineItem)
    {
        EnsureDraft();
        ArgumentNullException.ThrowIfNull(lineItem);
        _lineItems.Add(lineItem);
    }

    /// <summary>
    /// Adds multiple line items to the ICS
    /// </summary>
    public void AddLineItems(IEnumerable<ICSLineItem> lineItems)
    {
        ArgumentNullException.ThrowIfNull(lineItems);

        foreach (var item in lineItems)
        {
            AddLineItem(item);
        }
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
    /// Updates the header information (only allowed in Draft)
    /// </summary>
    public void UpdateHeader(
        Guid employeeId,
        DateTime issuanceDate,
        string? issuancePurpose = null,
        string? issuanceLocation = null,
        string? notes = null)
    {
        EnsureDraft();

        if (employeeId == Guid.Empty)
            throw new ArgumentException("Employee ID cannot be empty.", nameof(employeeId));

        EmployeeId = employeeId;
        IssuanceDate = issuanceDate;
        IssuancePurpose = issuancePurpose;
        IssuanceLocation = issuanceLocation;
        Notes = notes;
    }

    /// <summary>
    /// Sets authentication information
    /// </summary>
    public void SetAuthentication(
        string issuedByName,
        DateTime issuedByDate,
        string receivedByName,
        DateTime receivedByDate,
        string? approvedByName = null,
        DateTime? approvedByDate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(issuedByName);
        ArgumentException.ThrowIfNullOrWhiteSpace(receivedByName);

        IssuedByName = issuedByName;
        IssuedByDate = issuedByDate;
        ReceivedByName = receivedByName;
        ReceivedByDate = receivedByDate;
        ApprovedByName = approvedByName;
        ApprovedByDate = approvedByDate;
    }

    /// <summary>
    /// Posts the ICS, assigning the semi-expendable assets to the custodian
    /// </summary>
    public void Post()
    {
        EnsureDraft();

        if (_lineItems.Count == 0)
            throw new InvalidOperationException("Cannot post ICS with no line items.");

        Status = ICSStatus.Posted;
    }

    /// <summary>
    /// Cancels the ICS, reversing custodian assignments
    /// </summary>
    public void Cancel()
    {
        if (Status == ICSStatus.Cancelled)
            throw new InvalidOperationException("ICS is already cancelled.");

        if (Status == ICSStatus.Returned)
            throw new InvalidOperationException("Cannot cancel a returned ICS.");

        Status = ICSStatus.Cancelled;
    }

    /// <summary>
    /// Marks the ICS as returned, unassigning semi-expendable assets from custodian
    /// </summary>
    public void Return(
        DateTime returnDate,
        Guid receivedByEmployeeId,
        string? returnRemarks = null)
    {
        if (Status != ICSStatus.Posted)
            throw new InvalidOperationException("Only posted ICS can be returned.");

        if (receivedByEmployeeId == Guid.Empty)
            throw new ArgumentException("Receiving employee ID cannot be empty.", nameof(receivedByEmployeeId));

        ReturnDate = returnDate;
        ReceivedByEmployeeId = receivedByEmployeeId;
        ReturnRemarks = returnRemarks;
        Status = ICSStatus.Returned;
    }

    /// <summary>
    /// Gets the total acquisition cost of all semi-expendable assets in this ICS
    /// </summary>
    public decimal GetTotalAcquisitionCost() => _lineItems.Sum(li => li.AcquisitionCost);

    /// <summary>
    /// Gets the count of line items
    /// </summary>
    public int GetLineItemsCount() => _lineItems.Count;

    private void EnsureDraft()
    {
        if (Status != ICSStatus.Draft)
            throw new InvalidOperationException("This operation is only allowed on Draft ICS.");
    }
}

/// <summary>
/// Value object representing an ICS line item for semi-expendable assets
/// </summary>
public sealed record ICSLineItem(
    string PropertyCode,
    string Description,
    int Quantity,
    DateTime DateAcquired,
    decimal UnitCost,
    string? Condition = null,
    string? Remarks = null)
{
    /// <summary>
    /// Gets the total acquisition cost for this line item
    /// </summary>
    public decimal AcquisitionCost => UnitCost * Quantity;

    public ICSLineItem() : this(string.Empty, string.Empty, 1, DateTime.MinValue, 0m)
    {
    }
};

/// <summary>
/// Enum representing ICS status
/// </summary>
public enum ICSStatus
{
    Draft = 0,      // Being prepared, can be edited
    Posted = 1,     // Issued to custodian, semi-expendables assigned
    Returned = 2,   // Semi-expendables returned by custodian
    Cancelled = 3   // ICS cancelled, custodian assignments reversed
}
