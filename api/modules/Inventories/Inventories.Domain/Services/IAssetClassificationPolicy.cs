using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain.Services;

/// <summary>
/// Defines the contract for determining asset classification based on acquisition cost and other factors.
/// 
/// Implementations determine classification according to:
/// - Acquisition cost (primary factor)
/// - DBM/COA policy thresholds (may be hardcoded or database-driven)
/// - Effective date validation (for database-driven policies)
/// 
/// This pattern allows classification rules to evolve without changing domain entities.
/// Classifications are determined at operation time (Issue, Transfer, Return, etc.)
/// rather than stored as entity properties, ensuring policy consistency.
/// </summary>
public interface IAssetClassificationPolicy
{
    /// <summary>
    /// Determines the asset classification based on acquisition cost.
    /// </summary>
    /// <param name="acquisitionCost">The cost at which the asset was acquired, in pesos.</param>
    /// <returns>
    /// The PropertyClassification (Consumable, SemiExpendable, or PropertyPlantEquipment)
    /// determined by the policy's thresholds.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if acquisitionCost is negative.</exception>
    PropertyClassification DetermineClassification(decimal acquisitionCost);

    /// <summary>
    /// Gets the RCA (Real Property and Equipment Account) account code for a classification.
    /// Used for proper accounting of asset depreciation and disposal.
    /// </summary>
    /// <param name="classification">The asset classification.</param>
    /// <returns>
    /// The RCA account code as a string, typically one of:
    /// - "1010" for Supplies and Materials (Consumable)
    /// - "1020" for Semi-Expendable Property
    /// - "1030" for Property, Plant &amp; Equipment (PPE)
    /// Specific codes depend on the Chart of Accounts in use.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown if classification is unknown.</exception>
    string GetRCAAccountCode(PropertyClassification classification);
}
