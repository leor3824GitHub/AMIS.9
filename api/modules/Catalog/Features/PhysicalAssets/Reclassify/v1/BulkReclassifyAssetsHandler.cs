using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using AMIS.WebApi.Catalog.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Features.PhysicalAssets.Reclassify.v1;

public class BulkReclassifyAssetsHandler : IRequestHandler<BulkReclassifyAssetsCommand, BulkReclassifyAssetsResponse>
{
    private readonly CatalogDbContext _context;
    private readonly IReadRepository<AssetClassificationRule> _classificationRulesRepo;
    private readonly ILogger<BulkReclassifyAssetsHandler> _logger;

    public BulkReclassifyAssetsHandler(
        CatalogDbContext context,
        [FromKeyedServices("catalog:classificationRules")] IReadRepository<AssetClassificationRule> classificationRulesRepo,
        ILogger<BulkReclassifyAssetsHandler> logger)
    {
        _context = context;
        _classificationRulesRepo = classificationRulesRepo;
        _logger = logger;
    }

    public async Task<BulkReclassifyAssetsResponse> Handle(
        BulkReclassifyAssetsCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting bulk reclassification. Reason: {Reason}", request.Reason);

        var assets = await _context.PhysicalAssets
            .Where(a => a.DisposalDate == null) // Only active assets
            .ToListAsync(cancellationToken);

        int ppeToSemi = 0;
        int semiToPPE = 0;
        var summary = new List<AssetReclassificationSummary>();

        // Load active classification rules
        var activeRules = await _classificationRulesRepo
            .ListAsync(cancellationToken);

        var effectiveRules = activeRules
            .Where(r => r.IsActive &&
                       r.EffectiveDate <= request.EffectiveDate &&
                       (r.ExpiryDate == null || r.ExpiryDate >= request.EffectiveDate))
            .OrderByDescending(r => r.Priority)
            .ToList();

        foreach (var asset in assets)
        {
            var oldClassification = asset.CurrentClassification;

            // Determine new classification based on updated rules
            var newClassification = DetermineClassification(
                asset.AcquisitionCost,
                asset.EstimatedUsefulLife,
                effectiveRules);

            if (oldClassification != newClassification)
            {
                asset.Reclassify(newClassification, request.Reason, request.EffectiveDate);

                if (oldClassification == PropertyClassification.PPE &&
                    newClassification == PropertyClassification.SemiExpendable)
                    ppeToSemi++;

                if (oldClassification == PropertyClassification.SemiExpendable &&
                    newClassification == PropertyClassification.PPE)
                    semiToPPE++;

                summary.Add(new AssetReclassificationSummary(
                    asset.Id,
                    asset.AssetNumber,
                    oldClassification.ToString(),
                    newClassification.ToString(),
                    asset.AcquisitionCost));

                _logger.LogInformation(
                    "Reclassified {AssetNumber} from {Old} to {New}",
                    asset.AssetNumber,
                    oldClassification,
                    newClassification);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Bulk reclassification completed. Total: {Total}, PPE→Semi: {PpeToSemi}, Semi→PPE: {SemiToPPE}",
            summary.Count,
            ppeToSemi,
            semiToPPE);

        return new BulkReclassifyAssetsResponse(
            summary.Count,
            ppeToSemi,
            semiToPPE,
            summary);
    }

    private static PropertyClassification DetermineClassification(
        decimal acquisitionCost,
        int estimatedUsefulLifeMonths,
        List<AssetClassificationRule> rules)
    {
        // Find matching rule with highest priority
        var matchingRule = rules
            .FirstOrDefault(r => r.AppliesToAsset(acquisitionCost, estimatedUsefulLifeMonths));

        if (matchingRule != null)
        {
            return matchingRule.Classification;
        }

        // Fallback to default logic if no rules configured
        // Based on COA Circular 2022-004 defaults
        if (acquisitionCost <= 1000)
            return PropertyClassification.Consumable;

        if (acquisitionCost > 50000) // Current threshold as of 2022
            return PropertyClassification.PropertyPlantEquipment;

        // Between 1000 and 50000
        if (estimatedUsefulLifeMonths >= 12) // > 1 year
            return PropertyClassification.SemiExpendable;

        return PropertyClassification.Consumable;
    }
}