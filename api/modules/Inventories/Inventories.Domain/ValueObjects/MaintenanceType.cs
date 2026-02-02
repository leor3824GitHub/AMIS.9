namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Value object representing types of asset maintenance
/// Supports preventive (planned), corrective (unplanned), and emergency repairs
/// </summary>
public enum MaintenanceType
{
    /// <summary>
    /// Preventive/Scheduled maintenance
    /// Routine upkeep to prevent failures
    /// </summary>
    Preventive = 0,

    /// <summary>
    /// Corrective/Unplanned maintenance
    /// Repair of identified defects
    /// </summary>
    Corrective = 1,

    /// <summary>
    /// Emergency maintenance
    /// Immediate response to critical failure
    /// </summary>
    Emergency = 2,

    /// <summary>
    /// Inspection or condition assessment
    /// Non-repair maintenance activity
    /// </summary>
    Inspection = 3
}
