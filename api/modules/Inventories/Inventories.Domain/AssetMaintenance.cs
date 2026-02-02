using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Entity representing a maintenance record for a physical asset
/// Tracks all maintenance activities (preventive, corrective, emergency, inspection)
/// Enables historical reporting and maintenance analytics
/// 
/// Note: This is NOT an aggregate root (no IAggregateRoot)
/// It is owned by PhysicalAsset aggregate to ensure consistency
/// 
/// Workflow:
/// 1. Schedule (Scheduled) - Maintenance need is identified
/// 2. Start (InProgress) - Maintenance work begins
/// 3. Complete (Completed) - Work finished, results recorded
/// OR
/// 3. Cancel (Cancelled) - Maintenance request cancelled
/// </summary>
public class AssetMaintenance : AuditableEntity
{
    // Core Identity
    public Guid PhysicalAssetId { get; private set; }
    public string AssetPropertyCode { get; private set; } = default!; // Denormalized for traceability
    public string AssetDescription { get; private set; } = default!; // Denormalized for traceability

    // Maintenance Details
    public MaintenanceType Type { get; private set; }
    public string Description { get; private set; } = default!; // What needs to be done
    public DateTime ScheduledDate { get; private set; }

    // Status Tracking
    public MaintenanceStatus Status { get; private set; } = MaintenanceStatus.Scheduled;
    public DateTime? StartedOn { get; private set; }
    public DateTime? CompletedOn { get; private set; }
    public string? CompletionNotes { get; private set; } // What was done
    public string? FindingsNotes { get; private set; } // Condition assessment results

    // Personnel & Responsibility
    public Guid ScheduledBy { get; private set; }
    public Guid? PerformedBy { get; private set; } // Maintenance technician
    public Guid? ApprovedBy { get; private set; } // Supervisor/Manager approving results

    // Cost Tracking
    public decimal? EstimatedCost { get; private set; }
    public decimal? ActualCost { get; private set; }
    public string? CostReference { get; private set; } // PO, Job Order, etc.

    // Cancellation Details
    public DateTime? CancelledOn { get; private set; }
    public Guid? CancelledBy { get; private set; }
    public string? CancellationReason { get; private set; }

    // Navigation Properties
    public virtual PhysicalAsset Asset { get; private set; } = default!;
    public virtual Employee ScheduledByEmployee { get; private set; } = default!;
    public virtual Employee? PerformedByEmployee { get; private set; }
    public virtual Employee? ApprovedByEmployee { get; private set; }
    public virtual Employee? CancelledByEmployee { get; private set; }

    // Computed Properties
    public bool IsScheduled => Status == MaintenanceStatus.Scheduled;
    public bool IsInProgress => Status == MaintenanceStatus.InProgress;
    public bool IsCompleted => Status == MaintenanceStatus.Completed;
    public bool IsCancelled => Status == MaintenanceStatus.Cancelled;
    public bool CanBeStarted => Status == MaintenanceStatus.Scheduled;
    public bool CanBeCompleted => Status == MaintenanceStatus.InProgress;
    public bool CanBeCancelled => Status == MaintenanceStatus.Scheduled || Status == MaintenanceStatus.InProgress;
    public bool IsOverdue => !IsCompleted && !IsCancelled && ScheduledDate < DateTime.UtcNow;
    public int DaysOverdue => IsOverdue ? (int)(DateTime.UtcNow - ScheduledDate).TotalDays : 0;

    private AssetMaintenance() { }

    private AssetMaintenance(
        Guid id,
        Guid physicalAssetId,
        string assetPropertyCode,
        string assetDescription,
        MaintenanceType maintenanceType,
        string description,
        DateTime scheduledDate,
        Guid scheduledBy,
        decimal? estimatedCost = null,
        string? costReference = null)
    {
        Id = id;
        PhysicalAssetId = physicalAssetId;
        AssetPropertyCode = assetPropertyCode;
        AssetDescription = assetDescription;
        Type = maintenanceType;
        Description = description;
        ScheduledDate = scheduledDate;
        ScheduledBy = scheduledBy;
        EstimatedCost = estimatedCost;
        CostReference = costReference;
        Status = MaintenanceStatus.Scheduled;

        QueueDomainEvent(new MaintenanceScheduled { Maintenance = this });
    }

    /// <summary>
    /// Schedule maintenance for an asset
    /// Called by maintenance planner or supply officer
    /// </summary>
    public static AssetMaintenance Schedule(
        Guid physicalAssetId,
        string assetPropertyCode,
        string assetDescription,
        MaintenanceType maintenanceType,
        string description,
        DateTime scheduledDate,
        Guid scheduledBy,
        decimal? estimatedCost = null,
        string? costReference = null)
    {
        if (physicalAssetId == Guid.Empty)
            throw new ArgumentException("Physical Asset ID must be provided.", nameof(physicalAssetId));

        if (string.IsNullOrWhiteSpace(assetPropertyCode))
            throw new ArgumentException("Asset Property Code must be provided.", nameof(assetPropertyCode));

        if (string.IsNullOrWhiteSpace(assetDescription))
            throw new ArgumentException("Asset Description must be provided.", nameof(assetDescription));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Maintenance description must be provided.", nameof(description));

        if (scheduledDate == default)
            throw new ArgumentException("Scheduled date must be provided.", nameof(scheduledDate));

        if (scheduledBy == Guid.Empty)
            throw new ArgumentException("Scheduled By employee ID must be provided.", nameof(scheduledBy));

        if (estimatedCost.HasValue && estimatedCost < 0)
            throw new ArgumentException("Estimated cost cannot be negative.", nameof(estimatedCost));

        return new AssetMaintenance(
            Guid.NewGuid(),
            physicalAssetId,
            assetPropertyCode,
            assetDescription,
            maintenanceType,
            description,
            scheduledDate,
            scheduledBy,
            estimatedCost,
            costReference);
    }

    /// <summary>
    /// Start maintenance work
    /// Transitions from Scheduled to InProgress
    /// Called by maintenance technician
    /// </summary>
    public void Start(Guid performedBy)
    {
        if (!CanBeStarted)
            throw new InvalidOperationException($"Maintenance cannot be started when status is '{Status}'.");

        if (performedBy == Guid.Empty)
            throw new ArgumentException("Performed By employee ID must be provided.", nameof(performedBy));

        StartedOn = DateTime.UtcNow;
        PerformedBy = performedBy;
        Status = MaintenanceStatus.InProgress;

        QueueDomainEvent(new MaintenanceStarted
        {
            Maintenance = this,
            StartedOn = StartedOn.Value
        });
    }

    /// <summary>
    /// Complete maintenance work
    /// Records completion date and results
    /// Called by technician or supervisor upon completion
    /// </summary>
    public void Complete(
        string? completionNotes = null,
        string? findingsNotes = null,
        decimal? actualCost = null,
        Guid? approvedBy = null)
    {
        if (!CanBeCompleted)
            throw new InvalidOperationException($"Maintenance cannot be completed when status is '{Status}'.");

        if (actualCost.HasValue && actualCost < 0)
            throw new ArgumentException("Actual cost cannot be negative.", nameof(actualCost));

        CompletedOn = DateTime.UtcNow;
        CompletionNotes = completionNotes;
        FindingsNotes = findingsNotes;
        ActualCost = actualCost;
        ApprovedBy = approvedBy;
        Status = MaintenanceStatus.Completed;

        QueueDomainEvent(new MaintenanceCompleted
        {
            Maintenance = this,
            CompletedOn = CompletedOn.Value
        });
    }

    /// <summary>
    /// Cancel scheduled or in-progress maintenance
    /// Records reason for audit trail
    /// </summary>
    public void Cancel(Guid cancelledBy, string? cancellationReason = null)
    {
        if (!CanBeCancelled)
            throw new InvalidOperationException($"Maintenance cannot be cancelled when status is '{Status}'.");

        if (cancelledBy == Guid.Empty)
            throw new ArgumentException("Cancelled By employee ID must be provided.", nameof(cancelledBy));

        CancelledOn = DateTime.UtcNow;
        CancelledBy = cancelledBy;
        CancellationReason = cancellationReason;
        Status = MaintenanceStatus.Cancelled;

        QueueDomainEvent(new MaintenanceCancelled
        {
            Maintenance = this,
            CancellationReason = cancellationReason
        });
    }

    /// <summary>
    /// Reschedule maintenance to a different date
    /// Can be called from Scheduled or InProgress state
    /// </summary>
    public void Reschedule(DateTime newScheduledDate)
    {
        if (!IsScheduled && !IsInProgress)
            throw new InvalidOperationException($"Maintenance cannot be rescheduled when status is '{Status}'.");

        if (newScheduledDate == default)
            throw new ArgumentException("New scheduled date must be provided.", nameof(newScheduledDate));

        ScheduledDate = newScheduledDate;
    }

    /// <summary>
    /// Update cost information
    /// Can be called before completion
    /// </summary>
    public void UpdateCost(decimal? estimatedCost = null, string? costReference = null)
    {
        if (IsCompleted || IsCancelled)
            throw new InvalidOperationException($"Costs cannot be updated when status is '{Status}'.");

        if (estimatedCost.HasValue && estimatedCost < 0)
            throw new ArgumentException("Estimated cost cannot be negative.", nameof(estimatedCost));

        if (estimatedCost.HasValue)
            EstimatedCost = estimatedCost;

        if (!string.IsNullOrWhiteSpace(costReference))
            CostReference = costReference;
    }
}
