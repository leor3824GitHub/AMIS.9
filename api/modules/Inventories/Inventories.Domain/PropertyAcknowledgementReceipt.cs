using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a Property Acknowledgement Receipt (PAR) for PPE
/// This is an official form used to document the assignment of Property, Plant and Equipment (PPE)
/// to individual custodians/employees, establishing their acknowledgement and accountability for the property.
/// </summary>
public class PropertyAcknowledgementReceipt : AuditableEntity, IAggregateRoot
{
    // Header and Tracking
    public string PARNumber { get; private set; }
    public PARStatus Status { get; private set; } = PARStatus.Draft;

    // Custodian/Employee Information
    public Guid EmployeeId { get; private set; }
    public virtual Employee Employee { get; private set; } = default!;

    // Issuance Information
    public DateTime IssuanceDate { get; private set; }
    public string? IssuancePurpose { get; private set; }
    public string? IssuanceLocation { get; private set; }

    // Line Items
    private readonly List<PARLineItem> _lineItems = [];
    public IReadOnlyCollection<PARLineItem> LineItems => _lineItems.AsReadOnly();

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

    private PropertyAcknowledgementReceipt()
    {
        PARNumber = string.Empty;
    }

    /// <summary>
    /// Creates a new Property Acknowledgement Receipt
    /// </summary>
    public PropertyAcknowledgementReceipt(
        string parNumber,
        Guid employeeId,
        DateTime issuanceDate,
        string? issuancePurpose = null,
        string? issuanceLocation = null,
        string? notes = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parNumber);
        if (employeeId == Guid.Empty)
            throw new ArgumentException("Employee ID cannot be empty.", nameof(employeeId));

        PARNumber = parNumber;
        EmployeeId = employeeId;
        IssuanceDate = issuanceDate;
        IssuancePurpose = issuancePurpose;
        IssuanceLocation = issuanceLocation;
        Notes = notes;
    }

    /// <summary>
    /// Adds a line item to the PAR
    /// </summary>
    public void AddLineItem(PARLineItem lineItem)
    {
        EnsureDraft();
        ArgumentNullException.ThrowIfNull(lineItem);
        _lineItems.Add(lineItem);
    }

    /// <summary>
    /// Adds multiple line items to the PAR
    /// </summary>
    public void AddLineItems(IEnumerable<PARLineItem> lineItems)
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
    /// Posts the PAR, assigning the assets to the custodian
    /// </summary>
    public void Post()
    {
        EnsureDraft();

        if (_lineItems.Count == 0)
            throw new InvalidOperationException("Cannot post PAR with no line items.");

        Status = PARStatus.Posted;
    }

    /// <summary>
    /// Cancels the PAR, reversing custodian assignments
    /// </summary>
    public void Cancel()
    {
        if (Status == PARStatus.Cancelled)
            throw new InvalidOperationException("PAR is already cancelled.");

        if (Status == PARStatus.Returned)
            throw new InvalidOperationException("Cannot cancel a returned PAR.");

        Status = PARStatus.Cancelled;
    }

    /// <summary>
    /// Marks the PAR as returned, unassigning assets from custodian
    /// </summary>
    public void Return(
        DateTime returnDate,
        Guid receivedByEmployeeId,
        string? returnRemarks = null)
    {
        if (Status != PARStatus.Posted)
            throw new InvalidOperationException("Only posted PARs can be returned.");

        if (receivedByEmployeeId == Guid.Empty)
            throw new ArgumentException("Receiving employee ID cannot be empty.", nameof(receivedByEmployeeId));

        ReturnDate = returnDate;
        ReceivedByEmployeeId = receivedByEmployeeId;
        ReturnRemarks = returnRemarks;
        Status = PARStatus.Returned;
    }

    /// <summary>
    /// Gets the total acquisition cost of all PPE in this PAR
    /// </summary>
    public decimal GetTotalAcquisitionCost() => _lineItems.Sum(li => li.AcquisitionCost);

    /// <summary>
    /// Gets the count of line items
    /// </summary>
    public int GetLineItemsCount() => _lineItems.Count;

    private void EnsureDraft()
    {
        if (Status != PARStatus.Draft)
            throw new InvalidOperationException("This operation is only allowed on Draft PARs.");
    }
}

/// <summary>
/// Value object representing a PAR line item
/// </summary>
public sealed record PARLineItem(
    string PropertyCode,
    string Description,
    DateTime DateAcquired,
    decimal AcquisitionCost,
    string? Condition = null,
    string? Remarks = null)
{
    public PARLineItem() : this(string.Empty, string.Empty, DateTime.MinValue, 0m)
    {
    }
};

/// <summary>
/// Enum representing PAR status
/// </summary>
public enum PARStatus
{
    Draft = 0,      // Being prepared, can be edited
    Posted = 1,     // Issued to custodian, assets assigned
    Returned = 2,   // Assets returned by custodian
    Cancelled = 3   // PAR cancelled, custodian assignments reversed
}
