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
    private readonly IReadRepository<PurchaseItem> _purchaseItemReadRepo;
    private readonly IRepository<PurchaseItem> _purchaseItemRepo;
    private readonly ILogger<InspectionRejectedHandler> _logger;

    public InspectionRejectedHandler(
        [FromKeyedServices("catalog:inspections")] IReadRepository<Inspection> inspectionReadRepo,
        [FromKeyedServices("catalog:purchaseItems")] IReadRepository<PurchaseItem> purchaseItemReadRepo,
        [FromKeyedServices("catalog:purchaseItems")] IRepository<PurchaseItem> purchaseItemRepo,
        ILogger<InspectionRejectedHandler> logger)
    {
        _inspectionReadRepo = inspectionReadRepo ?? throw new ArgumentNullException(nameof(inspectionReadRepo));
        _purchaseItemReadRepo = purchaseItemReadRepo ?? throw new ArgumentNullException(nameof(purchaseItemReadRepo));
        _purchaseItemRepo = purchaseItemRepo ?? throw new ArgumentNullException(nameof(purchaseItemRepo));
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

            foreach (var item in inspection.Items)
            {
                var purchaseItem = item.PurchaseItem ?? await _purchaseItemReadRepo.GetByIdAsync(item.PurchaseItemId, cancellationToken);

                if (purchaseItem is null)
                {
                    _logger.LogWarning(
                        "PurchaseItem {PurchaseItemId} not found for InspectionItem in inspection {InspectionId}. Skipping.",
                        item.PurchaseItemId, inspection.Id);
                    continue;
                }

                purchaseItem.UpdateInspectionSummary(
                    inspected: (purchaseItem.QtyInspected ?? 0) + item.QtyInspected,
                    passed:    (purchaseItem.QtyPassed ?? 0)    + item.QtyPassed,
                    failed:    (purchaseItem.QtyFailed ?? 0)    + item.QtyFailed);

                purchaseItem.UpdateInspectionStatus(PurchaseItemInspectionStatus.Rejected);

                await _purchaseItemRepo.UpdateAsync(purchaseItem, cancellationToken);

                _logger.LogInformation(
                    "Updated PurchaseItem {PurchaseItemId} after inspection rejection: Inspected={Inspected}, Passed={Passed}, Failed={Failed}",
                    purchaseItem.Id, purchaseItem.QtyInspected, purchaseItem.QtyPassed, purchaseItem.QtyFailed);
            }

            _logger.LogInformation("Finished handling InspectionRejected for Inspection {InspectionId}", notification.InspectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling InspectionRejected for Inspection {InspectionId}", notification.InspectionId);
            throw;
        }
    }
}
