using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Aggregate Root for Asset Disposal workflow
/// Manages complete lifecycle of asset retirement/disposal
/// Complies with COA and DBM guidelines for asset retirement documentation
/// 
/// Workflow:
/// 1. Request (Pending) - Supply officer initiates disposal
/// 2. Approve (Approved) - Manager authorizes based on condition/policy
/// 3. Complete (Completed) - Disposal executed, financial impact recorded
/// OR
/// 3. Cancel (Cancelled) - Disposal denied/cancelled with reason
/// </summary>
public class AssetDisposal : AuditableEntity, IAggregateRoot
{
    // Core Identity
    public Guid PhysicalAssetId { get; private set; }
    public string AssetPropertyCode { get; private set; } = default!; // Denormalized for traceability
    public string AssetDescription { get; private set; } = default!; // Denormalized for traceability

    // Disposal Request Details
    public Guid RequestedBy { get; private set; } // Supply Officer / Custodian
    public DateTime RequestDate { get; private set; }
    public DisposalMethod DisposalMethod { get; private set; } = default!;
    public string? JustificationReason { get; private set; } // Why disposal is needed (condition, obsolescence, etc.)
    public AssetCondition AssetConditionAtDisposal { get; private set; } = default!;

    // Approval Workflow
    public DisposalStatus Status { get; private set; } = DisposalStatus.Pending;
    public Guid? ApprovedBy { get; private set; } // Manager/Authorized Personnel
    public DateTime? ApprovedOn { get; private set; }
    public string? ApprovalNotes { get; private set; }

    // Completion Details
    public DateTime? CompletedOn { get; private set; }
    public Guid? CompletedBy { get; private set; }

    // Financial Impact (recorded on completion)
    public decimal? SalvageValue { get; private set; } // Expected/actual salvage/proceeds
    public decimal? GainOrLoss { get; private set; } // Book Value - Salvage Value (Loss if negative)
    public string? DisposalReferenceNumber { get; private set; } // Government/Auction ID if applicable

    // Cancellation Details
    public DateTime? CancelledOn { get; private set; }
    public Guid? CancelledBy { get; private set; }
    public string? CancellationReason { get; private set; }

    // Navigation Properties
    public virtual PhysicalAsset Asset { get; private set; } = default!;
    public virtual Employee RequestedByEmployee { get; private set; } = default!;
    public virtual Employee? ApprovedByEmployee { get; private set; }
    public virtual Employee? CompletedByEmployee { get; private set; }
    public virtual Employee? CancelledByEmployee { get; private set; }

    // Computed Properties
    public bool IsApprovalPending => Status == DisposalStatus.Pending;
    public bool IsApproved => Status == DisposalStatus.Approved;
    public bool IsCompleted => Status == DisposalStatus.Completed;
    public bool IsCancelled => Status == DisposalStatus.Cancelled;
    public bool CanBeApproved => Status == DisposalStatus.Pending;
    public bool CanBeCompleted => Status == DisposalStatus.Approved;
    public bool CanBeCancelled => Status == DisposalStatus.Pending || Status == DisposalStatus.Approved;

    private AssetDisposal() { }

    private AssetDisposal(
        Guid id,
        Guid physicalAssetId,
        string assetPropertyCode,
        string assetDescription,
        Guid requestedBy,
        DateTime requestDate,
        DisposalMethod disposalMethod,
        string? justificationReason,
        AssetCondition assetConditionAtDisposal)
    {
        Id = id;
        PhysicalAssetId = physicalAssetId;
        AssetPropertyCode = assetPropertyCode;
        AssetDescription = assetDescription;
        RequestedBy = requestedBy;
        RequestDate = requestDate;
        DisposalMethod = disposalMethod;
        JustificationReason = justificationReason;
        AssetConditionAtDisposal = assetConditionAtDisposal;
        Status = DisposalStatus.Pending;

        QueueDomainEvent(new DisposalRequested { Disposal = this });
    }

    /// <summary>
    /// Create a new disposal request for an asset
    /// Called by Supply Officer or Asset Custodian
    /// </summary>
    public static AssetDisposal CreateRequest(
        Guid physicalAssetId,
        string assetPropertyCode,
        string assetDescription,
        Guid requestedBy,
        DisposalMethod disposalMethod,
        AssetCondition assetConditionAtDisposal,
        string? justificationReason = null)
    {
        if (physicalAssetId == Guid.Empty)
            throw new ArgumentException("Physical Asset ID must be provided.", nameof(physicalAssetId));

        if (string.IsNullOrWhiteSpace(assetPropertyCode))
            throw new ArgumentException("Asset Property Code must be provided.", nameof(assetPropertyCode));

        if (string.IsNullOrWhiteSpace(assetDescription))
            throw new ArgumentException("Asset Description must be provided.", nameof(assetDescription));

        if (requestedBy == Guid.Empty)
            throw new ArgumentException("Requested By employee ID must be provided.", nameof(requestedBy));

        ArgumentNullException.ThrowIfNull(disposalMethod);
        ArgumentNullException.ThrowIfNull(assetConditionAtDisposal);

        return new AssetDisposal(
            Guid.NewGuid(),
            physicalAssetId,
            assetPropertyCode,
            assetDescription,
            requestedBy,
            DateTime.UtcNow,
            disposalMethod,
            justificationReason,
            assetConditionAtDisposal);
    }

    /// <summary>
    /// Approve the disposal request
    /// Called by authorized manager/supervisor
    /// Required before disposal can be completed
    /// </summary>
    public void Approve(Guid approvedBy, string? approvalNotes = null)
    {
        if (!CanBeApproved)
            throw new InvalidOperationException($"Disposal cannot be approved when status is '{Status}'.");

        if (approvedBy == Guid.Empty)
            throw new ArgumentException("Approved By employee ID must be provided.", nameof(approvedBy));

        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovalNotes = approvalNotes;
        Status = DisposalStatus.Approved;

        QueueDomainEvent(new DisposalApproved
        {
            Disposal = this,
            ApprovedBy = approvedBy,
            ApprovedOn = ApprovedOn.Value
        });
    }

    /// <summary>
    /// Complete the disposal process
    /// Records salvage value and calculates gain/loss
    /// Called after actual disposal is executed
    /// </summary>
    public void Complete(
        Guid completedBy,
        decimal? salvageValue = null,
        string? disposalReferenceNumber = null)
    {
        if (!CanBeCompleted)
            throw new InvalidOperationException($"Disposal cannot be completed when status is '{Status}'.");

        if (completedBy == Guid.Empty)
            throw new ArgumentException("Completed By employee ID must be provided.", nameof(completedBy));

        if (salvageValue.HasValue && salvageValue < 0)
            throw new ArgumentException("Salvage value cannot be negative.", nameof(salvageValue));

        CompletedOn = DateTime.UtcNow;
        CompletedBy = completedBy;
        SalvageValue = salvageValue ?? 0;
        DisposalReferenceNumber = disposalReferenceNumber;
        Status = DisposalStatus.Completed;

        // Note: GainOrLoss will be calculated by application service after fetching asset book value
        // GainOrLoss = BookValue - SalvageValue (negative = loss)

        QueueDomainEvent(new DisposalCompleted
        {
            Disposal = this,
            CompletedOn = CompletedOn.Value,
            GainOrLoss = GainOrLoss
        });
    }

    /// <summary>
    /// Cancel or reject the disposal request
    /// Can be done in Pending or Approved state
    /// </summary>
    public void Cancel(Guid cancelledBy, string? cancellationReason = null)
    {
        if (!CanBeCancelled)
            throw new InvalidOperationException($"Disposal cannot be cancelled when status is '{Status}'.");

        if (cancelledBy == Guid.Empty)
            throw new ArgumentException("Cancelled By employee ID must be provided.", nameof(cancelledBy));

        CancelledOn = DateTime.UtcNow;
        CancelledBy = cancelledBy;
        CancellationReason = cancellationReason;
        Status = DisposalStatus.Cancelled;

        QueueDomainEvent(new DisposalCancelled
        {
            Disposal = this,
            CancellationReason = cancellationReason
        });
    }

    /// <summary>
    /// Calculate and record the gain/loss on disposal
    /// Called by application service with book value from asset
    /// </summary>
    public void RecordFinancialImpact(decimal bookValue)
    {
        if (!IsCompleted)
            throw new InvalidOperationException("Financial impact can only be recorded on completed disposals.");

        var salvage = SalvageValue ?? 0;
        GainOrLoss = bookValue - salvage; // Negative = Loss, Positive = Gain
    }

    /// <summary>
    /// Update justification reason before approval
    /// Useful if additional context is needed during review
    /// </summary>
    public void UpdateJustification(string? justificationReason)
    {
        if (!IsApprovalPending)
            throw new InvalidOperationException("Justification can only be updated in Pending state.");

        JustificationReason = justificationReason;
    }
}
