using System.Linq;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Acceptances.Specifications;
using AMIS.WebApi.Catalog.Application.Inventories.Specifications;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Events;

public sealed class AcceptancePostedHandler : INotificationHandler<AcceptancePosted>
{
    private readonly IReadRepository<Acceptance> _acceptanceReadRepository;
    private readonly IRepository<Inventory> _inventoryRepository;
    private readonly IRepository<PurchaseItem> _purchaseItemRepository;
    private readonly IReadRepository<PurchaseItem> _purchaseItemReadRepository;
    private readonly IRepository<InspectionRequest> _inspectionRequestRepository;
    private readonly ILogger<AcceptancePostedHandler> _logger;

    public AcceptancePostedHandler(
        [FromKeyedServices("catalog:acceptances")] IReadRepository<Acceptance> acceptanceReadRepository,
        [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepository,
        [FromKeyedServices("catalog:purchaseItems")] IRepository<PurchaseItem> purchaseItemRepository,
        [FromKeyedServices("catalog:purchaseItems")] IReadRepository<PurchaseItem> purchaseItemReadRepository,
        [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> inspectionRequestRepository,
        ILogger<AcceptancePostedHandler> logger)
    {
        _acceptanceReadRepository = acceptanceReadRepository ?? throw new ArgumentNullException(nameof(acceptanceReadRepository));
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        _purchaseItemRepository = purchaseItemRepository ?? throw new ArgumentNullException(nameof(purchaseItemRepository));
        _purchaseItemReadRepository = purchaseItemReadRepository ?? throw new ArgumentNullException(nameof(purchaseItemReadRepository));
        _inspectionRequestRepository = inspectionRequestRepository ?? throw new ArgumentNullException(nameof(inspectionRequestRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(AcceptancePosted notification, CancellationToken cancellationToken)
    {
        var spec = new GetAcceptanceWithItemsSpec(notification.AcceptanceId);
        var acceptance = await _acceptanceReadRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (acceptance is null)
        {
            _logger.LogWarning("Acceptance {AcceptanceId} not found when posting to inventory.", notification.AcceptanceId);
            return;
        }

        foreach (var acceptanceItem in acceptance.Items)
        {
            if (acceptanceItem.QtyAccepted <= 0)
            {
                continue;
            }

            var purchaseItem = acceptanceItem.PurchaseItem ?? await _purchaseItemRepository.GetByIdAsync(acceptanceItem.PurchaseItemId, cancellationToken);
            if (purchaseItem is null)
            {
                _logger.LogWarning("PurchaseItem {PurchaseItemId} not found for Acceptance {AcceptanceId}.", acceptanceItem.PurchaseItemId, acceptance.Id);
                continue;
            }

            if (!purchaseItem.ProductId.HasValue)
            {
                _logger.LogWarning("PurchaseItem {PurchaseItemId} has no ProductId. Skipping inventory update for Acceptance {AcceptanceId}.", purchaseItem.Id, acceptance.Id);
                continue;
            }

            var productId = purchaseItem.ProductId.Value;
            var qtyToAdd = acceptanceItem.QtyAccepted;
            var unitCost = purchaseItem.UnitPrice;

            var inventorySpec = new GetInventoryByProductSpec(productId);
            var inventory = await _inventoryRepository.FirstOrDefaultAsync(inventorySpec, cancellationToken);

            if (inventory is null)
            {
                inventory = Inventory.Create(productId, qtyToAdd, unitCost);
                await _inventoryRepository.AddAsync(inventory, cancellationToken);
                _logger.LogInformation("Created inventory for Product {ProductId} from Acceptance {AcceptanceId} with Qty {Qty}.", productId, acceptance.Id, qtyToAdd);
            }
            else
            {
                inventory.AddStock(qtyToAdd, unitCost);
                await _inventoryRepository.UpdateAsync(inventory, cancellationToken);
                _logger.LogInformation("Updated inventory for Product {ProductId} from Acceptance {AcceptanceId}: +{Qty} => Qty {NewQty}.", productId, acceptance.Id, qtyToAdd, inventory.Qty);
            }

            var totalAccepted = purchaseItem.AcceptanceItems.Any()
                ? purchaseItem.AcceptanceItems.Sum(ai => ai.QtyAccepted)
                : qtyToAdd;

            purchaseItem.UpdateAcceptanceSummary(totalAccepted);

            var approvedQty = purchaseItem.QtyPassed ?? purchaseItem.Qty;
            var targetStatus = totalAccepted >= approvedQty
                ? PurchaseItemAcceptanceStatus.Accepted
                : PurchaseItemAcceptanceStatus.PartiallyAccepted;

            purchaseItem.UpdateAcceptanceStatus(targetStatus);
            await _purchaseItemRepository.UpdateAsync(purchaseItem, cancellationToken);
        }

        // After updating items, update InspectionRequest status (PartiallyAccepted vs Accepted)
        var purchaseId = acceptance.PurchaseId;
        var itemsSpec = new AMIS.WebApi.Catalog.Application.PurchaseItems.Specifications.GetPurchaseItemsByPurchaseSpec(purchaseId);
        var purchaseItems = await _purchaseItemReadRepository.ListAsync(itemsSpec, cancellationToken);

        var allApprovedQty = purchaseItems.All(pi => (pi.QtyPassed ?? pi.Qty) > 0);
        var allFullyAccepted = purchaseItems.All(pi =>
        {
            var approved = pi.QtyPassed ?? pi.Qty;
            var accepted = pi.AcceptanceItems?.Sum(ai => ai.QtyAccepted) ?? 0;
            return accepted >= approved;
        });

        // Load related inspection request and set status
        var reqSpec = new AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications.GetInspectionRequestByPurchaseSpec(purchaseId);
        var inspectionRequest = await _inspectionRequestRepository.FirstOrDefaultAsync(reqSpec, cancellationToken);
        if (inspectionRequest is not null)
        {
            if (allApprovedQty && allFullyAccepted)
            {
                inspectionRequest.UpdateStatus(InspectionRequestStatus.Accepted);
            }
            else
            {
                inspectionRequest.UpdateStatus(InspectionRequestStatus.PartiallyAccepted);
            }

            await _inspectionRequestRepository.UpdateAsync(inspectionRequest, cancellationToken);
        }
    }
}
