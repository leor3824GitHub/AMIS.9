//csharp api/modules/Catalog/Catalog.Application/Inspections/Events/InspectionApprovedHandler.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;

namespace AMIS.WebApi.Catalog.Application.Inspections.Events;

public sealed class InspectionApprovedHandler : INotificationHandler<InspectionApproved>
{
    private readonly IReadRepository<Inspection> _inspectionReadRepo;
    private readonly IReadRepository<PurchaseItem> _purchaseItemReadRepo;
    private readonly IRepository<Inventory> _inventoryRepo;
    private readonly ILogger<InspectionApprovedHandler> _logger;

    public InspectionApprovedHandler(
        IReadRepository<Inspection> inspectionReadRepo,
        IReadRepository<PurchaseItem> purchaseItemReadRepo,
        IRepository<Inventory> inventoryRepo,
        ILogger<InspectionApprovedHandler> logger)
    {
        _inspectionReadRepo = inspectionReadRepo ?? throw new ArgumentNullException(nameof(inspectionReadRepo));
        _purchaseItemReadRepo = purchaseItemReadRepo ?? throw new ArgumentNullException(nameof(purchaseItemReadRepo));
        _inventoryRepo = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(InspectionApproved notification, CancellationToken cancellationToken)
    {
        try
        {
            var spec = new GetInspectionWithItemsSpec(notification.InspectionId);
            var inspection = await _inspectionReadRepo.FirstOrDefaultAsync(spec, cancellationToken);
            if (inspection == null)
            {
                _logger.LogWarning("Inspection {InspectionId} not found when handling InspectionApproved.", notification.InspectionId);
                return;
            }

            // Consider an item accepted if it has QtyPassed > 0 or an explicit passed status
            var acceptedItems = inspection.Items
                .Where(i => i.QtyPassed > 0 || i.InspectionItemStatus == InspectionItemStatus.Passed)
                .ToList();

            if (!acceptedItems.Any())
            {
                _logger.LogInformation("Inspection {InspectionId} approved but no accepted items to update inventory.", notification.InspectionId);
                return;
            }

            foreach (var item in acceptedItems)
            {
                // Load purchase item details (use navigation if present)
                PurchaseItem? purchaseItem = item.PurchaseItem;
                if (purchaseItem == null)
                {
                    // Adjust spec call to your actual spec implementation if necessary
                    purchaseItem = await _purchaseItemReadRepo.FirstOrDefaultAsync(
                        new GetPurchaseItemSpecs(item.PurchaseItemId),
                        cancellationToken);
                }

                if (purchaseItem == null)
                {
                    _logger.LogWarning("PurchaseItem {PurchaseItemId} not found when processing inspection {InspectionId}. Skipping.", item.PurchaseItemId, inspection.Id);
                    continue;
                }

                if (purchaseItem.ProductId == null)
                {
                    _logger.LogWarning("PurchaseItem {PurchaseItemId} has no ProductId. Skipping inventory update.", purchaseItem.Id);
                    continue;
                }

                var productId = purchaseItem.ProductId.Value;
                var qtyToAdd = item.QtyPassed;
                if (qtyToAdd <= 0)
                {
                    _logger.LogInformation("InspectionItem {InspectionItemId} has no passed qty. Skipping.", item.Id);
                    continue;
                }

                var unitCost = purchaseItem.UnitPrice; // unit price from purchase item

                // Try find existing inventory by ProductId
                var inventorySpec = new AMIS.WebApi.Catalog.Application.Inventories.Specifications.GetInventoryByProductSpec(productId);
                var existingInventory = await _inventoryRepo.FirstOrDefaultAsync(inventorySpec, cancellationToken);

                if (existingInventory == null)
                {
                    // Create new inventory aggregate using factory (if available) or new instance
                    var newInventory = Inventory.Create(productId, qtyToAdd, unitCost);
                    await _inventoryRepo.AddAsync(newInventory, cancellationToken);
                    _logger.LogInformation("Created inventory for Product {ProductId} with Qty {Qty}.", productId, qtyToAdd);
                }
                else
                {
                    // Use domain method to add stock and recalc average price
                    existingInventory.AddStock(qtyToAdd, unitCost);
                    await _inventoryRepo.UpdateAsync(existingInventory, cancellationToken);
                    _logger.LogInformation("Updated inventory for Product {ProductId}: +{AddQty} => Qty {NewQty}.", productId, qtyToAdd, existingInventory.Qty);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling InspectionApproved for Inspection {InspectionId}", notification.InspectionId);
            throw;
        }
    }
}
