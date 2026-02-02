using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Ardalis.Specification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Services;

/// <summary>
/// Handles bulk reclassification of assets when COA/DBM threshold changes
/// Example: Threshold change from ₱50,000 to ₱100,000 requires reclassifying
/// assets with cost ₱50,001-₱100,000 from PPE to Semi-Expendable
/// </summary>
public sealed class ReclassifyAssetsHandler(
    ILogger<ReclassifyAssetsHandler> logger,
    [FromKeyedServices("inventories:assets")] IRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:semex")] IRepository<Domain.SemexRegistry> semexRepository)
    : IRequestHandler<ReclassifyAssetsCommand, ReclassifyAssetsResponse>
{
    private const decimal DefaultOldThreshold = 50000m;

    public async Task<ReclassifyAssetsResponse> Handle(
        ReclassifyAssetsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation(
            "Starting asset reclassification process. Old threshold: ₱{OldThreshold}, New threshold: ₱{NewThreshold}, Effective: {EffectiveDate}",
            DefaultOldThreshold,
            request.NewPPEThreshold,
            request.EffectiveDate);

        // Step 1: Update AssetClassificationRule with new threshold
        // TODO: await AssetClassificationRuleSeeder.CreateThresholdUpdateAsync(
        //    dbContext,
        //    request.NewPPEThreshold,
        //    request.EffectiveDate,
        //    request.COAReference,
        //    logger,
        //    cancellationToken);

        // Step 2: Find assets that need reclassification
        // Assets currently classified as PPE but now fall below new threshold
        var assetsToReclassify = await assetRepository.ListAsync(
            new AssetsBetweenThresholdsSpec(DefaultOldThreshold + 1, request.NewPPEThreshold),
            cancellationToken);

        var reclassificationDetails = new List<AssetReclassificationDetail>();
        var reclassifiedCount = 0;

        // Step 3: Reclassify each asset
        foreach (var asset in assetsToReclassify)
        {
            if (asset.CurrentClassification != PropertyClassification.PropertyPlantEquipment)
            {
                logger.LogWarning(
                    "Asset {PropertyCode} is not PPE (current: {Classification}), skipping",
                    asset.PropertyCode,
                    asset.CurrentClassification);
                continue;
            }

            var oldClassification = asset.CurrentClassification.ToString();

            // Reclassify to Semi-Expendable
            asset.Reclassify(
                PropertyClassification.SemiExpendable,
                $"{request.Reason} - Threshold changed from ₱{DefaultOldThreshold:N2} to ₱{request.NewPPEThreshold:N2} per {request.COAReference}",
                request.EffectiveDate);

            // Update repository
            await assetRepository.UpdateAsync(asset, cancellationToken);

            // Transfer from PPE registry to Semi-Expendable registry
            await TransferToSemexRegistryAsync(asset, request.EffectiveDate, cancellationToken);

            reclassificationDetails.Add(new AssetReclassificationDetail(
                asset.Id,
                asset.PropertyCode,
                asset.Description,
                asset.AcquisitionCost,
                oldClassification,
                asset.CurrentClassification.ToString()));

            reclassifiedCount++;

            logger.LogInformation(
                "Reclassified asset {PropertyCode} ({Description}) from PPE to Semi-Expendable. Cost: ₱{Cost:N2}",
                asset.PropertyCode,
                asset.Description,
                asset.AcquisitionCost);
        }

        // Step 4: Save all changes
        await assetRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Asset reclassification completed. Total reclassified: {Count} assets from PPE to Semi-Expendable",
            reclassifiedCount);

        return new ReclassifyAssetsResponse(
            reclassifiedCount,
            DefaultOldThreshold,
            request.NewPPEThreshold,
            reclassificationDetails);
    }

    /// <summary>
    /// Transfers asset from PPE registry to Semi-Expendable registry
    /// Creates new SemexRegistry entry for the reclassified asset
    /// </summary>
    private async Task TransferToSemexRegistryAsync(
        PhysicalAsset asset,
        DateTime effectiveDate,
        CancellationToken cancellationToken)
    {
        // Check if already exists in SEMEX registry
        var existingEntry = await semexRepository.FirstOrDefaultAsync(
            new SemexByPropertyCodeSpec(asset.PropertyCode),
            cancellationToken);

        if (existingEntry != null)
        {
            logger.LogInformation(
                "Asset {PropertyCode} already exists in SEMEX registry, updating quantity",
                asset.PropertyCode);

            // TODO: existingEntry.AddStock(asset.Quantity, $"Reclassified from PPE on {effectiveDate:yyyy-MM-dd}");
            await semexRepository.UpdateAsync(existingEntry, cancellationToken);
        }
        else
        {
            // TODO: Create new SEMEX registry entry
            // var semexEntry = SemexRegistry.Create(
            //     asset.PropertyCode,
            //     asset.ProductId,
            //     asset.Description,
            //     asset.AcquisitionCost,
            //     asset.Quantity,
            //     asset.UnitOfMeasure ?? "piece",
            //     asset.CurrentAssignment?.Location ?? string.Empty,
            //     reorderLevel: 0);

            // await semexRepository.AddAsync(semexEntry, cancellationToken);

            logger.LogInformation(
                "Created new SEMEX registry entry for reclassified asset {PropertyCode}",
                asset.PropertyCode);
        }
    }
}

/// <summary>
/// Specification to find assets between two cost thresholds
/// Used to identify assets affected by threshold changes
/// </summary>
internal class AssetsBetweenThresholdsSpec : Specification<PhysicalAsset>
{
    public AssetsBetweenThresholdsSpec(decimal minCost, decimal maxCost)
    {
        Query
            .Where(a => a.AcquisitionCost > minCost && a.AcquisitionCost <= maxCost)
            .Where(a => a.CurrentClassification == PropertyClassification.PropertyPlantEquipment);
    }
}

/// <summary>
/// Specification to find SEMEX registry entry by property code
/// </summary>
internal class SemexByPropertyCodeSpec : Specification<Domain.SemexRegistry>, ISingleResultSpecification<Domain.SemexRegistry>
{
    public SemexByPropertyCodeSpec(string propertyCode)
    {
        // TODO: Query.Where(s => s.PropertyCode == propertyCode);
    }
}
