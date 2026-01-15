namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Enum for Depreciation Schedule Status
/// </summary>
public enum DepreciationScheduleStatus
{
    Pending = 0,        // Calculated but not yet posted
    Posted = 1,         // Posted to accounting records
    Reversed = 2        // Depreciation reversed (e.g., asset disposed)
}
