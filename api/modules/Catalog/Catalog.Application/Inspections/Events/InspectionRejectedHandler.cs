//csharp api/modules/Catalog/Catalog.Application/Inspections/Events/InspectionRejectedHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1; // adjust if spec namespace differs
using AMIS.WebApi.Catalog.Application.Inventories.Specifications;
using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.Events;

public sealed class InspectionRejectedHandler : INotificationHandler<InspectionRejected>
{
    private readonly IReadRepository<Inspection> _inspectionReadRepo;
    private readonly IReadRepository<PurchaseItem> _purchaseItemReadRepo;
    private readonly IRepository<PurchaseItem> _purchaseItemRepo;
    private readonly ILogger<InspectionRejectedHandler> _logger;

    public InspectionRejectedHandler(
        IReadRepository<Inspection> inspectionReadRepo,
        IReadRepository<PurchaseItem> purchaseItemReadRepo,
        IRepository<PurchaseItem> purchaseItemRepo,
        ILogger<InspectionRejectedHandler> logger)
    {
        _inspectionReadRepo = inspectionReadRepo ?? throw new ArgumentNullException(nameof(inspectionReadRepo));
        _purchaseItemReadRepo = purchaseItemReadRepo ?? throw new ArgumentNullException(nameof(purchaseItemReadRepo));
        _purchaseItemRepo = purchaseItemRepo ?? throw new ArgumentNullException(nameof(purchaseItemRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(InspectionRejected notification, PurchaseItem purchaseItem, CancellationToken cancellationToken)
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

            _logger.LogInformation("Handling InspectionRejected for Inspection {InspectionId} (Purchase: {PurchaseId}). Reason: {Reason}",
                notification.InspectionId, notification.PurchaseId, notification.Reason);

            // For each inspection item, mark related purchase item as rejected / update counters
            foreach (var item in inspection.Items ?? Enumerable.Empty<InspectionItem>())
            {
                // Try use navigation property first; otherwise load via read repo/spec
                PurchaseItem? purchaseItem = item.PurchaseItem;
                if (purchaseItem is null)
                {
                    // adjust spec type/namespace if different in your project
                    var piSpec = new AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1.GetPurchaseItemSpecs(item.PurchaseItemId);
                    PurchaseItemResponse? purchaseItemDto = await _purchaseItemReadRepo.FirstOrDefaultAsync(piSpec, cancellationToken);
                    purchaseItem = purchaseItemDto;
                }

                if (purchaseItem is null)
                {
                    _logger.LogWarning("PurchaseItem {PurchaseItemId} not found for InspectionItem in inspection {InspectionId}. Skipping.", item.PurchaseItemId, inspection.Id);
                    continue;
                }

                purchaseItem.QtyInspected = (purchaseItem.QtyInspected ?? 0) + item.QtyInspected;
                purchaseItem.QtyPassed = (purchaseItem.QtyPassed ?? 0) + item.QtyPassed;
                purchaseItem.QtyFailed = (purchaseItem.QtyFailed ?? 0) + item.QtyFailed;


                // Mark item inspection status to Rejected (adjust enum name if different)
                try
                {
                    purchaseItem.UpdateInspectionStatus(PurchaseItemInspectionStatus.Rejected);
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "PurchaseItem.UpdateInspectionStatus threw for PurchaseItem {PurchaseItemId}", purchaseItem.Id);
                    // fallback: set underlying property if UpdateInspectionStatus is unavailable
                    purchaseItem.InspectionStatus = PurchaseItemInspectionStatus.Rejected;
                }

                await _purchaseItemRepo.UpdateAsync(purchaseItem, cancellationToken);
                _logger.LogInformation("Updated PurchaseItem {PurchaseItemId} after inspection rejection: Inspected={Inspected}, Passed={Passed}, Failed={Failed}",
                    purchaseItem.Id, purchaseItem.QtyInspected, purchaseItem.QtyPassed, purchaseItem.QtyFailed);
            }

            // Optionally: update Purchase header status (e.g., mark as PartiallyInspected/Rejected) if you have such logic.
            // Optionally: create notification/email, create task for supplier follow-up, or trigger workflow.

            _logger.LogInformation("Finished handling InspectionRejected for Inspection {InspectionId}", notification.InspectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling InspectionRejected for Inspection {InspectionId}", notification.InspectionId);
            throw;
        }
    }
}
