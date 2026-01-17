namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Property classification based on COA/DBM 2023-2025 Standards
/// </summary>
public enum PropertyClassification
{
    /// <summary>
    /// Consumable items used within 1 year (≤ ₱50,000)
    /// RCA: 10501000 - Supplies and Materials Inventory
    /// Document: RSMI (Requisition and Issue Slip)
    /// </summary>
    Consumable = 1,

    /// <summary>
    /// Non-consumable with useful life > 1 year (≤ ₱50,000)
    /// RCA: 10599020 - Semi-Expendable Property Inventory
    /// Document: ICS (Inventory Custodian Slip)
    /// </summary>
    SemiExpendable = 2,

    /// <summary>
    /// Property, Plant and Equipment (> ₱50,000)
    /// RCA: 1-06-*** (PPE accounts)
    /// Document: PAR (Property Acknowledgment Receipt)
    /// </summary>
    PropertyPlantEquipment = 3
}

