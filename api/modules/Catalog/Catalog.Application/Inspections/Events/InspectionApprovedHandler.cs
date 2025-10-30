//csharp api/modules/Catalog/Catalog.Application/Inspections/Events/InspectionApprovedHandler.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using MediatR;
using Microsoft.Extensions.Logging;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection; // For FromKeyedServices
using Ardalis.Specification; // For local specifications

namespace AMIS.WebApi.Catalog.Application.Inspections.Events;

public sealed class InspectionApprovedHandler : INotificationHandler<InspectionApproved>
{
    private readonly IReadRepository<Inspection> _inspectionReadRepo;
    private readonly IRepository<InspectionRequest> _inspectionRequestRepo;
    private readonly IRepository<Inventory> _inventoryRepo;
    private readonly IRepository<InventoryTransaction> _inventoryTxnRepo;
    private readonly ILogger<InspectionApprovedHandler> _logger;

    public InspectionApprovedHandler(
        [FromKeyedServices("catalog:inspections")] IReadRepository<Inspection> inspectionReadRepo,
        [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> inspectionRequestRepo,
        [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepo,
    [FromKeyedServices("catalog:inventory-transactions")] IRepository<InventoryTransaction> inventoryTxnRepo,
        ILogger<InspectionApprovedHandler> logger)
    {
        _inspectionReadRepo = inspectionReadRepo ?? throw new ArgumentNullException(nameof(inspectionReadRepo));
        _inspectionRequestRepo = inspectionRequestRepo ?? throw new ArgumentNullException(nameof(inspectionRequestRepo));
        _inventoryRepo = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
        _inventoryTxnRepo = inventoryTxnRepo ?? throw new ArgumentNullException(nameof(inventoryTxnRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(InspectionApproved notification, CancellationToken cancellationToken)
    {
        var spec = new GetInspectionWithItemsSpec(notification.InspectionId);
        var inspection = await _inspectionReadRepo.FirstOrDefaultAsync(spec, cancellationToken);
        if (inspection == null)
        {
            _logger.LogWarning("Inspection {InspectionId} not found when handling InspectionApproved.", notification.InspectionId);
            return;
        }

        if (!notification.PurchaseId.HasValue)
        {
            return;
        }

        var requestSpec = new AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications.GetInspectionRequestByPurchaseSpec(notification.PurchaseId.Value);
        var inspectionRequest = await _inspectionRequestRepo.FirstOrDefaultAsync(requestSpec, cancellationToken);

        if (inspectionRequest != null && inspectionRequest.Status != AMIS.WebApi.Catalog.Domain.ValueObjects.InspectionRequestStatus.Completed)
        {
            inspectionRequest.MarkCompleted();
            await _inspectionRequestRepo.UpdateAsync(inspectionRequest, cancellationToken);
            _logger.LogInformation("Marked InspectionRequest {RequestId} as Completed after Inspection {InspectionId} was approved",
                inspectionRequest.Id, notification.InspectionId);
        }

        // Automatically add passed quantities to Inventory and record Inventory Transactions
        foreach (var item in inspection.Items)
        {
            // Only consider passed quantities
            if (item.QtyPassed <= 0)
                continue;

            var purchaseItem = item.PurchaseItem;
            if (purchaseItem?.ProductId is null)
            {
                _logger.LogWarning("InspectionItem {InspectionItemId} has no linked Product; skipping inventory update.", item.Id);
                continue;
            }

            var productId = purchaseItem.ProductId;
            var unitPrice = purchaseItem.UnitPrice; // use purchase unit price as cost basis

            // Find or create inventory for the product
            var invSpec = new GetInventoryProductIdSpecs(productId);
            var inventory = await _inventoryRepo.FirstOrDefaultAsync(invSpec, cancellationToken);

            if (inventory is null)
            {
                inventory = Inventory.Create(productId, item.QtyPassed, unitPrice);
                await _inventoryRepo.AddAsync(inventory, cancellationToken);
                _logger.LogInformation("Created inventory for Product {ProductId} with Qty {Qty} from Inspection {InspectionId}", productId, item.QtyPassed, inspection.Id);
            }
            else
            {
                inventory.AddStock(item.QtyPassed, unitPrice);
                await _inventoryRepo.UpdateAsync(inventory, cancellationToken);
                _logger.LogInformation("Added Qty {Qty} to inventory for Product {ProductId} from Inspection {InspectionId}", item.QtyPassed, productId, inspection.Id);
            }

            // Record inventory transaction (Purchase receipt) - idempotent guard
            var existingTxnSpec = new InventoryTxnBySourceProductAndTypeSpec(
                inspection.Id,
                productId.Value,
                TransactionType.Purchase);

            var existingTxn = await _inventoryTxnRepo.FirstOrDefaultAsync(existingTxnSpec, cancellationToken);
            if (existingTxn is null)
            {
                var txn = InventoryTransaction.Create(productId, item.QtyPassed, unitPrice, location: null, sourceId: inspection.Id, transactionType: TransactionType.Purchase);
                await _inventoryTxnRepo.AddAsync(txn, cancellationToken);
            }
            else
            {
                _logger.LogInformation("Inventory transaction already exists for Inspection {InspectionId}, Product {ProductId}; skipping duplicate.", inspection.Id, productId);
            }
        }
    }
}

// Local specification to check for existing inventory transactions for a given source and product
file sealed class InventoryTxnBySourceProductAndTypeSpec : Specification<InventoryTransaction>
{
    public InventoryTxnBySourceProductAndTypeSpec(Guid sourceId, Guid productId, TransactionType type)
    {
        Query.Where(t => t.SourceId == sourceId && t.ProductId == productId && t.TransactionType == type);
    }
}
