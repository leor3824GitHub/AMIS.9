namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Represents the status of an inventory item in the registry
/// </summary>
public enum InventoryItemStatus
{
    /// <summary>
    /// Item exists in system but has not been received yet
    /// </summary>
    NotReceived = 0,

    /// <summary>
    /// Item is in stock and available for issuance
    /// </summary>
    InStock = 1,

    /// <summary>
    /// Item has been issued out and fully distributed
    /// </summary>
    Issued = 2,

    /// <summary>
    /// Item is in transit between locations
    /// </summary>
    InTransit = 3,

    /// <summary>
    /// Item is damaged and not usable
    /// </summary>
    Damaged = 4,

    /// <summary>
    /// Item is missing or lost
    /// </summary>
    Lost = 5
}
