namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// COA-required document types for property issuance
/// </summary>
public enum DocumentType
{
    /// <summary>
    /// No document type
    /// </summary>
    None = 0,

    /// <summary>
    /// Requisition and Issue Slip - for Consumables
    /// </summary>
    RSMI = 1,

    /// <summary>
    /// Inventory Custodian Slip - for Semi-Expendables
    /// </summary>
    ICS = 2,

    /// <summary>
    /// Property Acknowledgment Receipt - for PPE
    /// </summary>
    PAR = 3
}
