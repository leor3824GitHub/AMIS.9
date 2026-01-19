namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Represents the type of inventory transaction
/// </summary>
public enum InventoryTransactionType
{
    /// <summary>
    /// Items are being added to inventory (via PPERR - PPE Receiving Report)
    /// </summary>
    Add = 1,

    /// <summary>
    /// Items are being removed/deducted from inventory (via PPEIR - PPE Issuance Report)
    /// </summary>
    Deduct = 2
}
