using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Application.Inventories.Specifications;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Issuances.UpdateWithItems.v1;

public sealed class UpdateIssuanceWithItemsHandler(
    [FromKeyedServices("catalog:issuances")] IRepository<Issuance> issuanceRepo,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepo,
    [FromKeyedServices("catalog:inventory-transactions")] IRepository<InventoryTransaction> inventoryTransactionRepo,
    ILogger<UpdateIssuanceWithItemsHandler> logger)
    : IRequestHandler<UpdateIssuanceWithItemsCommand, UpdateIssuanceWithItemsResponse>
{
    public async Task<UpdateIssuanceWithItemsResponse> Handle(UpdateIssuanceWithItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Load with tracking to allow EF to detect collection changes
        var issuance = await issuanceRepo.FirstOrDefaultAsync(new GetIssuanceWithItemsSpecs(request.Id), cancellationToken)
            .ConfigureAwait(false);
        if (issuance is null)
            throw new InvalidOperationException($"Issuance {request.Id} not found.");

        // Update issuance header
        issuance.Update(
            request.EmployeeId,
            request.IssuanceDate,
            request.TotalAmount,
            request.IsClosed);

        // Work through the aggregate collection
        var itemsForIssuance = issuance.Items.ToList();
        var byId = itemsForIssuance.Where(i => i.Id != Guid.Empty).ToDictionary(i => i.Id);

        var updatedIds = new List<Guid>();

        // Track original persisted item IDs for diff-based removal (in case client omitted DeletedItemIds)
        var originalPersistedIds = issuance.Items.Where(i => i.Id != Guid.Empty).Select(i => i.Id).ToHashSet();

        // Upsert incoming items
        foreach (var dto in request.Items)
        {
            if (dto.Id.HasValue && byId.TryGetValue(dto.Id.Value, out var entity))
            {
                entity.Update(issuance.Id, dto.ProductId, dto.Qty, dto.UnitPrice, dto.Status);
                updatedIds.Add(entity.Id);
            }
            else
            {
                // Create via aggregate helper to keep invariants
                issuance.AddItem(dto.ProductId, dto.Qty, dto.UnitPrice, dto.Status);
                var newItem = issuance.Items.Last();
                updatedIds.Add(newItem.Id);
            }
        }

        // Handle deletions from explicit list
        if (request.DeletedItemIds is not null && request.DeletedItemIds.Count > 0)
        {
            foreach (var delId in request.DeletedItemIds)
            {
                if (byId.TryGetValue(delId, out var toDelete))
                {
                    issuance.Items.Remove(toDelete);
                }
            }
        }

        // Safety net: remove any persisted items that were not included in the incoming Items collection and not explicitly updated.
        // This covers UI flows where DeletedItemIds was not populated but the item disappeared from the list.
        var incomingIds = request.Items.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToHashSet();
        foreach (var staleId in originalPersistedIds.Except(incomingIds).Except(request.DeletedItemIds ?? Array.Empty<Guid>()))
        {
            if (byId.TryGetValue(staleId, out var staleEntity))
            {
                issuance.Items.Remove(staleEntity);
            }
        }

        // Recalculate totals based on final items
        issuance.RecalculateTotals(issuance.Items);

        // Handle inventory deductions for issued items
        if (!issuance.IsClosed)
        {
            foreach (var issuanceItem in issuance.Items)
            {
                var inventorySpec = new GetInventoryByProductSpec(issuanceItem.ProductId);
                var inventory = await inventoryRepo.FirstOrDefaultAsync(inventorySpec, cancellationToken);
                if (inventory is null)
                {
                    throw new InvalidOperationException($"Inventory not found for ProductId: {issuanceItem.ProductId}");
                }

                if (inventory.Qty < issuanceItem.Qty)
                {
                    throw new InvalidOperationException($"Insufficient stock for ProductId: {issuanceItem.ProductId}. Requested: {issuanceItem.Qty}, Available: {inventory.Qty}");
                }

                inventory.DeductStock(issuanceItem.Qty);
                await inventoryRepo.UpdateAsync(inventory, cancellationToken);
                logger.LogInformation("Inventory updated for Product {ProductId}: -{Qty} => Qty {NewQty}", issuanceItem.ProductId, issuanceItem.Qty, inventory.Qty);

                // Create inventory transaction record for audit trail
                var transaction = InventoryTransaction.Create(
                    productId: issuanceItem.ProductId,
                    qty: issuanceItem.Qty,
                    purchasePrice: issuanceItem.UnitPrice,
                    location: inventory.Location, // Use inventory's location
                    sourceId: issuance.Id,
                    transactionType: TransactionType.Issuance
                );
                await inventoryTransactionRepo.AddAsync(transaction, cancellationToken);
                logger.LogInformation("Created inventory transaction for Issuance {IssuanceId}, Product {ProductId}.", issuance.Id, issuanceItem.ProductId);
            }
        }

        // Persist header last
        await issuanceRepo.UpdateAsync(issuance, cancellationToken);

        return new UpdateIssuanceWithItemsResponse(issuance.Id, updatedIds);
    }
}