using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Services;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using AMIS.WebApi.Catalog.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Features.PhysicalAssets.Reclassify.v1;

public class BulkReclassifyAssetsHandler : IRequestHandler<BulkReclassifyAssetsCommand, BulkReclassifyAssetsResponse>
{
    private readonly CatalogDbContext _context;
    private readonly IAssetClassificationService _classificationService;
    private readonly ILogger<BulkReclassifyAssetsHandler> _logger;

    public BulkReclassifyAssetsHandler(
        CatalogDbContext context,
        IAssetClassificationService classificationService,
        ILogger<BulkReclassifyAssetsHandler> logger)
    {
        _context = context;
        _classificationService = classificationService;
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

        foreach (var asset in assets)
        {
            var oldClassification = asset.CurrentClassification;

            // Determine new classification based on updated rules
            var newClassification = await _classificationService
                .DetermineClassificationAsync(
                    asset.AcquisitionCost,
                    asset.EstimatedUsefulLife,
                    cancellationToken);

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
}