//csharp api/modules/Catalog/Catalog.Application/Inspections/Events/InspectionRejectedHandler.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection; // For FromKeyedServices

namespace AMIS.WebApi.Catalog.Application.Inspections.Events;

public sealed class InspectionRejectedHandler : INotificationHandler<InspectionRejected>
{
    private readonly IReadRepository<Inspection> _inspectionReadRepo;
    private readonly IRepository<Purchase> _purchaseRepo;
    private readonly ILogger<InspectionRejectedHandler> _logger;

    public InspectionRejectedHandler(
        [FromKeyedServices("catalog:inspections")] IReadRepository<Inspection> inspectionReadRepo,
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> purchaseRepo,
        ILogger<InspectionRejectedHandler> logger)
    {
        _inspectionReadRepo = inspectionReadRepo ?? throw new ArgumentNullException(nameof(inspectionReadRepo));
    _purchaseRepo = purchaseRepo ?? throw new ArgumentNullException(nameof(purchaseRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(InspectionRejected notification, CancellationToken cancellationToken)
    {
        try
        {
            var spec = new GetInspectionWithItemsSpec(notification.InspectionId);
            var inspection = await _inspectionReadRepo.FirstOrDefaultAsync(spec, cancellationToken);
            if (inspection is null)
            {
                _logger.LogWarning("Inspection {InspectionId} not found while handling InspectionRejected.", notification.InspectionId);
                return;
            }

            _logger.LogInformation(
                "Handling InspectionRejected for Inspection {InspectionId} (InspectionRequest: {InspectionRequestId}). Reason: {Reason}",
                notification.InspectionId, notification.InspectionRequestId, notification.Reason);

            // Load purchase aggregate with items once (inspection should reference a purchase via its items)
            var purchaseId = inspection.Items.FirstOrDefault()?.PurchaseItem?.PurchaseId;
            Purchase? purchase = null;
            if (purchaseId.HasValue)
            {
                // Re-use existing specification pattern for purchase+items
                var purchaseSpec = new AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1.GetPurchaseWithItemsSpecs(purchaseId.Value);
                purchase = await _purchaseRepo.FirstOrDefaultAsync(purchaseSpec, cancellationToken);
            }

            if (purchase is null)
            {
                _logger.LogWarning("Purchase aggregate not found while handling InspectionRejected for Inspection {InspectionId}.", inspection.Id);
            }
            else
            {
                foreach (var item in inspection.Items)
                {
                    var purchaseItem = purchase.Items.FirstOrDefault(pi => pi.Id == item.PurchaseItemId);
                    if (purchaseItem is null)
                    {
                        _logger.LogWarning("PurchaseItem {PurchaseItemId} missing in Purchase {PurchaseId} for Inspection {InspectionId}.", item.PurchaseItemId, purchase.Id, inspection.Id);
                        continue;
                    }

                    purchaseItem.UpdateInspectionSummary(
                        inspected: (purchaseItem.QtyInspected ?? 0) + item.QtyInspected,
                        passed:    (purchaseItem.QtyPassed ?? 0)    + item.QtyPassed,
                        failed:    (purchaseItem.QtyFailed ?? 0)    + item.QtyFailed);

                    purchaseItem.UpdateInspectionStatus(PurchaseItemInspectionStatus.Rejected);

                    _logger.LogInformation(
                        "Aggregated rejection applied to PurchaseItem {PurchaseItemId}: Inspected={Inspected}, Passed={Passed}, Failed={Failed}",
                        purchaseItem.Id, purchaseItem.QtyInspected, purchaseItem.QtyPassed, purchaseItem.QtyFailed);
                }

                await _purchaseRepo.UpdateAsync(purchase, cancellationToken);
            }

            _logger.LogInformation("Finished handling InspectionRejected for Inspection {InspectionId}", notification.InspectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling InspectionRejected for Inspection {InspectionId}: {Message}", notification.InspectionId, ex.Message);
            throw new InvalidOperationException($"Failed handling InspectionRejected for {notification.InspectionId}", ex);
        }
    }
}
