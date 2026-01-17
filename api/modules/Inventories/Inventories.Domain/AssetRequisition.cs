using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents an asset requisition/issuance request sent to an end-user for acceptance.
/// Tracks the workflow from issuance creation through end-user acceptance/rejection.
/// Part of the "My Accountability" dashboard feature.
/// </summary>
public class AssetRequisition : AuditableEntity, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public Guid IssuanceId { get; private set; }
    public DateTime RequisitionDate { get; private set; }
    public DateTime? ResponseDate { get; private set; }
    public AssetRequisitionStatus Status { get; private set; }
    public string? RejectionReason { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    
    // Digital signature for acceptance audit trail
    public DigitalSignature? AcceptanceSignature { get; private set; }

    // Navigation
    public virtual Employee Employee { get; private set; } = default!;
    public virtual Issuance Issuance { get; private set; } = default!;

    private AssetRequisition() { }

    private AssetRequisition(
        Guid id,
        Guid employeeId,
        Guid issuanceId,
        DateTime requisitionDate,
        DateTime? expirationDate = null)
    {
        Id = id;
        EmployeeId = employeeId;
        IssuanceId = issuanceId;
        RequisitionDate = requisitionDate;
        ExpirationDate = expirationDate;
        Status = AssetRequisitionStatus.Pending;
    }

    /// <summary>
    /// Creates a new asset requisition for an employee.
    /// </summary>
    public static AssetRequisition Create(
        Guid employeeId,
        Guid issuanceId,
        DateTime requisitionDate,
        int? expirationDays = null)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException("Employee ID must be provided.", nameof(employeeId));

        if (issuanceId == Guid.Empty)
            throw new ArgumentException("Issuance ID must be provided.", nameof(issuanceId));

        if (requisitionDate == default || requisitionDate > DateTime.UtcNow)
            throw new ArgumentException("Requisition date must be valid and not in the future.", nameof(requisitionDate));

        var expirationDate = expirationDays.HasValue
            ? (DateTime?)requisitionDate.AddDays(expirationDays.Value)
            : null;

        return new AssetRequisition(Guid.NewGuid(), employeeId, issuanceId, requisitionDate, expirationDate);
    }

    /// <summary>
    /// Marks the requisition as accepted with digital signature.
    /// </summary>
    public void Accept(DigitalSignature signature)
    {
        if (Status != AssetRequisitionStatus.Pending)
            throw new InvalidOperationException($"Cannot accept a requisition with status {Status}.");

        if (ExpirationDate.HasValue && DateTime.UtcNow > ExpirationDate.Value)
            throw new InvalidOperationException("This requisition has expired.");

        ArgumentNullException.ThrowIfNull(signature);

        Status = AssetRequisitionStatus.Accepted;
        ResponseDate = DateTime.UtcNow;
        AcceptanceSignature = signature;
    }

    /// <summary>
    /// Marks the requisition as rejected with a reason.
    /// </summary>
    public void Reject(string reason)
    {
        if (Status != AssetRequisitionStatus.Pending)
            throw new InvalidOperationException($"Cannot reject a requisition with status {Status}.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection reason must be provided.", nameof(reason));

        Status = AssetRequisitionStatus.Rejected;
        ResponseDate = DateTime.UtcNow;
        RejectionReason = reason;
    }

    /// <summary>
    /// Cancels the requisition (by administrator).
    /// </summary>
    public void Cancel()
    {
        if (Status != AssetRequisitionStatus.Pending)
            throw new InvalidOperationException($"Cannot cancel a requisition with status {Status}.");

        Status = AssetRequisitionStatus.Cancelled;
    }

    /// <summary>
    /// Checks if the requisition has expired.
    /// </summary>
    public bool IsExpired => ExpirationDate.HasValue && DateTime.UtcNow > ExpirationDate.Value;

    /// <summary>
    /// Returns true if the requisition is awaiting response.
    /// </summary>
    public bool IsPending => Status == AssetRequisitionStatus.Pending && !IsExpired;
}

