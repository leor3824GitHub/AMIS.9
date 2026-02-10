using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Data;

/// <summary>
/// Seeds default asset classification rules based on standard DBM thresholds.
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

        var effectiveDate = new DateTime(2022, 1, 1);

        var rules = new[]
        {
            // Rule 1: Semi-Expendable (₱500 - ₱49,999)
            AssetClassificationRule.Create(
                name: "Semi-Expendable Property",
                classification: PropertyClassification.SemiExpendable,
                minimumCost: 500,
                maximumCost: 49999,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.SemiExpendablePropertyInventory,
                priority: 10),

            // Rule 2: Property, Plant and Equipment (≥ ₱50,000)
            AssetClassificationRule.Create(
                name: "Property, Plant and Equipment",
                classification: PropertyClassification.PropertyPlantEquipment,
                minimumCost: 50000,
                maximumCost: 999999999,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.MachineryAndEquipment,
                priority: 20)
        };

        context.AssetClassificationRules.AddRange(rules);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully seeded {Count} asset classification rules", rules.Length);
    }

    /// <summary>
    /// Creates new rules when thresholds change via COA directive.
    /// </summary>
    public static async Task CreateThresholdUpdateAsync(
        InventoriesDbContext context,
        decimal newPPEThreshold,
        DateTime effectiveDate,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Creating new threshold rules: PPE threshold changed to ₱{Threshold} effective {Date}",
            newPPEThreshold,
            effectiveDate);

        // Deactivate old rules
        var oldRules = await context.AssetClassificationRules
            .Where(r => r.IsActive && (r.ExpiryDate == null || r.ExpiryDate >= effectiveDate))
            .ToListAsync(cancellationToken);

        foreach (var rule in oldRules)
        {
            rule.Deactivate();
        }

        // Create new rules with updated threshold
        var newRules = new[]
        {
            AssetClassificationRule.Create(
                name: "Semi-Expendable Property (Updated)",
                classification: PropertyClassification.SemiExpendable,
                minimumCost: 500,
                maximumCost: newPPEThreshold - 1,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.SemiExpendablePropertyInventory,
                priority: 10),

            AssetClassificationRule.Create(
                name: "Property, Plant and Equipment (Updated)",
                classification: PropertyClassification.PropertyPlantEquipment,
                minimumCost: newPPEThreshold,
                maximumCost: 999999999,
                effectiveDate: effectiveDate,
                rcaAccountCode: RCAAccountCode.MachineryAndEquipment,
                priority: 20)
        };

        context.AssetClassificationRules.AddRange(newRules);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Successfully created {Count} updated asset classification rules",
            newRules.Length);
    }
}

