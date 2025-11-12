using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1;

public sealed class UpdatePurchaseWithItemsHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> purchaseRepo
) : IRequestHandler<UpdatePurchaseWithItemsCommand, UpdatePurchaseWithItemsResponse>
{
    public async Task<UpdatePurchaseWithItemsResponse> Handle(UpdatePurchaseWithItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Load with tracking to allow EF to detect collection changes
        var purchase = await purchaseRepo.FirstOrDefaultAsync(new GetPurchaseWithItemsSpecs(request.Id), cancellationToken)
            .ConfigureAwait(false);
        if (purchase is null)
            throw new InvalidOperationException($"Purchase {request.Id} not found.");

        // Update purchase header
        purchase.Update(
            request.SupplierId,
            request.PurchaseDate,
            request.TotalAmount,
            request.Status,
            request.ReferenceNumber,
            request.Notes,
            request.Currency);

    // Work through the aggregate collection
    var itemsForPurchase = purchase.Items.ToList();
    var byId = itemsForPurchase.Where(i => i.Id != Guid.Empty).ToDictionary(i => i.Id);

    var updatedIds = new List<Guid>();

    // Track original persisted item IDs for diff-based removal (in case client omitted DeletedItemIds)
    var originalPersistedIds = purchase.Items.Where(i => i.Id != Guid.Empty).Select(i => i.Id).ToHashSet();

        // Upsert incoming items
        foreach (var dto in request.Items)
        {
            if (dto.Id.HasValue && byId.TryGetValue(dto.Id.Value, out var entity))
            {
                entity.Update(dto.ProductId, dto.Qty, dto.UnitPrice, dto.ItemStatus);
                updatedIds.Add(entity.Id);
            }
            else
            {
                // Create via aggregate helper to keep invariants
                purchase.AddItem(dto.ProductId, dto.Qty, dto.UnitPrice, dto.ItemStatus);
                var newItem = purchase.Items.Last();
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
                    purchase.Items.Remove(toDelete);
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
                purchase.Items.Remove(staleEntity);
            }
        }

        // Persist header last
        await purchaseRepo.UpdateAsync(purchase, cancellationToken).ConfigureAwait(false);

        return new UpdatePurchaseWithItemsResponse(purchase.Id, updatedIds);
    }
}
