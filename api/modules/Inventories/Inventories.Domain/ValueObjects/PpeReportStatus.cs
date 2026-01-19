namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Represents the status of a PPE report (PPERR/PPEIR)
/// </summary>
public enum PpeReportStatus
{
    /// <summary>
    /// Draft status - report can be edited, registry not updated
    /// </summary>
    Draft = 0,

    /// <summary>
    /// Posted status - report is final, registry updated, immutable
    /// </summary>
    Posted = 1,

    /// <summary>
    /// Cancelled status - report is voided, reversal entries created
    /// </summary>
    Cancelled = 2
}
