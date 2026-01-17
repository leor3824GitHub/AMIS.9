using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Data;

/// <summary>
/// Seeds default asset classification rules based on COA Circular 2022-004
/// </summary>
public static class AssetClassificationRuleSeeder
{
    public static async Task SeedDefaultRulesAsync(
        InventoriesDbContext context,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        // Check if rules already exist
        var existingRules = await context.AssetClassificationRules.AnyAsync(cancellationToken);
        if (existingRules)
        {
            logger.LogInformation("Asset classification rules already exist. Skipping seed.");
            return;
        }

        logger.LogInformation("Seeding default asset classification rules...");

        var effectiveDate = new DateTime(2022, 1, 1); // COA Circular 2022-004 effective date

        var rules = new[]
        {
            // Rule 1: Consumable (≤ ₱1,000)
            AssetClassificationRule.Create(
                ruleName: "Consumable - Low Value",
                classification: PropertyClassification.Consumable,
                minimumCost: 0,
                maximumCost: 1000,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.SuppliesAndMaterialsInventory,
                expenseAccountCode: RCAAccountCode.SuppliesAndMaterialsExpense,
                documentType: "RSMI",
                coaReference: "COA Circular 2022-004",
                minimumUsefulLifeMonths: null,
                priority: 10),

            // Rule 2: Semi-Expendable (₱1,001 - ₱50,000 with useful life > 1 year)
            AssetClassificationRule.Create(
                ruleName: "Semi-Expendable Property",
                classification: PropertyClassification.SemiExpendable,
                minimumCost: 1001,
                maximumCost: 50000,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.SemiExpendablePropertyInventory,
                expenseAccountCode: RCAAccountCode.SemiExpendablePropertyExpense,
                documentType: "ICS",
                coaReference: "COA Circular 2022-004",
                minimumUsefulLifeMonths: 12, // Must be > 1 year
                priority: 20),

            // Rule 3: Consumable (₱1,001 - ₱50,000 with useful life < 1 year)
            AssetClassificationRule.Create(
                ruleName: "Consumable - Short Useful Life",
                classification: PropertyClassification.Consumable,
                minimumCost: 1001,
                maximumCost: 50000,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.SuppliesAndMaterialsInventory,
                expenseAccountCode: RCAAccountCode.SuppliesAndMaterialsExpense,
                documentType: "RSMI",
                coaReference: "COA Circular 2022-004",
                minimumUsefulLifeMonths: null,
                priority: 15), // Lower priority than semi-expendable

            // Rule 4: Property, Plant and Equipment (> ₱50,000)
            AssetClassificationRule.Create(
                ruleName: "Property, Plant and Equipment",
                classification: PropertyClassification.PropertyPlantEquipment,
                minimumCost: 50001,
                maximumCost: 999999999,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.MachineryAndEquipment, // Default, varies by type
                expenseAccountCode: RCAAccountCode.SemiExpendablePropertyExpense, // Use generic expense
                documentType: "PAR",
                coaReference: "COA Circular 2022-004",
                minimumUsefulLifeMonths: null,
                priority: 30)
        };

        context.AssetClassificationRules.AddRange(rules);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully seeded {Count} asset classification rules", rules.Length);
    }

    /// <summary>
    /// Create a new threshold update (e.g., when COA changes PPE threshold)
    /// </summary>
    public static async Task CreateThresholdUpdateAsync(
        InventoriesDbContext context,
        decimal newPPEThreshold,
        DateTime effectiveDate,
        string coaReference,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Creating new threshold rules: PPE threshold changed to ₱{Threshold} effective {Date}",
            newPPEThreshold,
            effectiveDate);

        // Expire old rules
        var oldRules = await context.AssetClassificationRules
            .Where(r => r.IsActive && (r.ExpiryDate == null || r.ExpiryDate >= effectiveDate))
            .ToListAsync(cancellationToken);

        foreach (var rule in oldRules)
        {
            rule.SetExpiryDate(effectiveDate.AddDays(-1));
        }

        // Create new rules with updated threshold
        var newRules = new[]
        {
            AssetClassificationRule.Create(
                ruleName: "Consumable - Low Value (Updated)",
                classification: PropertyClassification.Consumable,
                minimumCost: 0,
                maximumCost: 1000,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.SuppliesAndMaterialsInventory,
                expenseAccountCode: RCAAccountCode.SuppliesAndMaterialsExpense,
                documentType: "RSMI",
                coaReference: coaReference,
                minimumUsefulLifeMonths: null,
                priority: 10),

            AssetClassificationRule.Create(
                ruleName: "Semi-Expendable Property (Updated)",
                classification: PropertyClassification.SemiExpendable,
                minimumCost: 1001,
                maximumCost: newPPEThreshold,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.SemiExpendablePropertyInventory,
                expenseAccountCode: RCAAccountCode.SemiExpendablePropertyExpense,
                documentType: "ICS",
                coaReference: coaReference,
                minimumUsefulLifeMonths: 12,
                priority: 20),

            AssetClassificationRule.Create(
                ruleName: "Consumable - Short Useful Life (Updated)",
                classification: PropertyClassification.Consumable,
                minimumCost: 1001,
                maximumCost: newPPEThreshold,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.SuppliesAndMaterialsInventory,
                expenseAccountCode: RCAAccountCode.SuppliesAndMaterialsExpense,
                documentType: "RSMI",
                coaReference: coaReference,
                minimumUsefulLifeMonths: null,
                priority: 15),

            AssetClassificationRule.Create(
                ruleName: "Property, Plant and Equipment (Updated)",
                classification: PropertyClassification.PropertyPlantEquipment,
                minimumCost: newPPEThreshold + 1,
                maximumCost: 999999999,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.MachineryAndEquipment,
                expenseAccountCode: RCAAccountCode.SemiExpendablePropertyExpense, // Use generic expense
                documentType: "PAR",
                coaReference: coaReference,
                minimumUsefulLifeMonths: null,
                priority: 30)
        };

        context.AssetClassificationRules.AddRange(newRules);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Successfully created {Count} new classification rules with PPE threshold ₱{Threshold}",
            newRules.Length,
            newPPEThreshold);
    }
}


