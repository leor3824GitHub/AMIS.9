namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Value object representing the status of asset disposal process
/// Tracks workflow from request through approval to completion
/// Critical for audit trail compliance
/// </summary>
public enum DisposalStatus
{
    /// <summary>
    /// Disposal request created, awaiting approval
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Disposal has been approved by authorized personnel
    /// Ready for completion
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Disposal has been completed and asset retired
    /// Financial impact (gain/loss) recorded
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Disposal request was rejected/cancelled
    /// Asset remains in inventory
    /// </summary>
    Cancelled = 3
}
