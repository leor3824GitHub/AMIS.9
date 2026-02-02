namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Value object representing the status of maintenance activity
/// Tracks lifecycle from scheduling through completion
/// </summary>
public enum MaintenanceStatus
{
    /// <summary>
    /// Maintenance is scheduled but not yet started
    /// </summary>
    Scheduled = 0,

    /// <summary>
    /// Maintenance is currently in progress
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Maintenance has been completed
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Scheduled maintenance was cancelled or rescheduled
    /// </summary>
    Cancelled = 3
}
