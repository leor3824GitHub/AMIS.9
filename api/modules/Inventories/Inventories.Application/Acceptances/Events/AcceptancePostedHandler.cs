using System;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.Acceptances.Services;
using AMIS.WebApi.Inventories.Application.Acceptances.Specifications;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Events;

public sealed class AcceptancePostedHandler : INotificationHandler<AcceptancePosted>
{
    private readonly IRepository<Acceptance> _acceptanceRepo;
    private readonly IRepository<PhysicalAsset> _assetRepo;
    private readonly IReadRepository<Employee> _employeeRepo;
    private readonly ILogger<AcceptancePostedHandler> _logger;
    private readonly IAssetPropertyCodeGenerator _codeGenerator;
    private readonly IAssetClassificationResolver _classificationResolver;

    public AcceptancePostedHandler(
        [FromKeyedServices("inventories:acceptances")] IRepository<Acceptance> acceptanceRepo,
        [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> assetRepo,
        [FromKeyedServices("inventories:employees")] IReadRepository<Employee> employeeRepo,
        IAssetPropertyCodeGenerator codeGenerator,
        IAssetClassificationResolver classificationResolver,
        ILogger<AcceptancePostedHandler> logger)
    {
        _acceptanceRepo = acceptanceRepo ?? throw new ArgumentNullException(nameof(acceptanceRepo));
        _assetRepo = assetRepo ?? throw new ArgumentNullException(nameof(assetRepo));
        _employeeRepo = employeeRepo ?? throw new ArgumentNullException(nameof(employeeRepo));
        _codeGenerator = codeGenerator ?? throw new ArgumentNullException(nameof(codeGenerator));
        _classificationResolver = classificationResolver ?? throw new ArgumentNullException(nameof(classificationResolver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(AcceptancePosted notification, CancellationToken cancellationToken)
    {
        var spec = new AcceptanceWithItemsSpec(notification.AcceptanceId);
        var acceptance = await _acceptanceRepo.FirstOrDefaultAsync(spec, cancellationToken);

        if (acceptance is null)
        {
            _logger.LogWarning("Acceptance {AcceptanceId} not found when handling AcceptancePosted.", notification.AcceptanceId);
            return;
        }

        try
        {
            var assets = AcceptanceAssetFactory.CreateAssets(
                acceptance,
                _codeGenerator,
                _classificationResolver);

            var supplyOfficer = await _employeeRepo.GetByIdAsync(acceptance.SupplyOfficerId, cancellationToken);

            foreach (var asset in assets)
            {
                IssueAssetToSupplyOfficer(asset, acceptance, supplyOfficer);
                await _assetRepo.AddAsync(asset, cancellationToken);
            }

            _logger.LogInformation("Created {Count} physical assets from Acceptance {AcceptanceId}.", assets.Count, acceptance.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create physical assets for Acceptance {AcceptanceId}.", notification.AcceptanceId);
            throw new InvalidOperationException($"Asset creation failed for acceptance {notification.AcceptanceId}", ex);
        }
    }

    private static void IssueAssetToSupplyOfficer(PhysicalAsset asset, Acceptance acceptance, Employee? officer)
    {
        var officerId = acceptance.SupplyOfficerId;
        var officerName = officer?.Name ?? "Accountable Officer";
        var documentNumber = acceptance.Id.ToString("N");

        // For semi-expendable, we issue the accepted quantity; for PPE, defaults to quantity 1 per asset batch
        var qtyToIssue = asset.CurrentClassification == Domain.ValueObjects.PropertyClassification.SemiExpendable
            ? asset.Quantity
            : (int?)null;

        asset.Issue(officerId, officerName, documentNumber, qtyToIssue);
    }
}

