using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Application.Acceptances.Specifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Acceptances.UpdateWithItems.v1;

public sealed class UpdateAcceptanceWithItemsHandler(
    ILogger<UpdateAcceptanceWithItemsHandler> logger,
    [FromKeyedServices("catalog:acceptances")] IRepository<Acceptance> acceptanceRepo,
    [FromKeyedServices("catalog:purchases")] IReadRepository<Purchase> purchaseRepo)
    : IRequestHandler<UpdateAcceptanceWithItemsCommand, UpdateAcceptanceWithItemsResponse>
{
    public async Task<UpdateAcceptanceWithItemsResponse> Handle(UpdateAcceptanceWithItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Load with tracking to allow EF to detect collection changes
        var acceptance = await acceptanceRepo.FirstOrDefaultAsync(new GetAcceptanceWithItemsSpec(request.Id), cancellationToken);
        if (acceptance is null)
            throw new InvalidOperationException($"Acceptance {request.Id} not found.");

        if (acceptance.IsPosted)
        {
            throw new InvalidOperationException("Cannot modify a posted acceptance.");
        }

        // Update acceptance header only if there are changes
        bool hasHeaderChanges = (request.SupplyOfficerId.HasValue && request.SupplyOfficerId.Value != acceptance.SupplyOfficerId) ||
                               (request.AcceptanceDate.HasValue && request.AcceptanceDate.Value != acceptance.AcceptanceDate) ||
                               (request.Remarks != acceptance.Remarks);

        if (hasHeaderChanges)
        {
            acceptance.Update(
                request.SupplyOfficerId ?? acceptance.SupplyOfficerId,
                request.AcceptanceDate ?? acceptance.AcceptanceDate,
                request.Remarks ?? acceptance.Remarks
            );
        }

        // Work through the aggregate collection
        var itemsForAcceptance = acceptance.Items.ToList();
        var byId = itemsForAcceptance.Where(i => i.Id != Guid.Empty).ToDictionary(i => i.Id);

        var updatedIds = new List<Guid>();

        // Track original persisted item IDs for diff-based removal
        var originalPersistedIds = acceptance.Items.Where(i => i.Id != Guid.Empty).Select(i => i.Id).ToHashSet();

        // Upsert incoming items
        if (request.Items != null)
        {
            // Load purchase for validation
            var purchaseSpec = new AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1.GetPurchaseWithItemsSpecs(acceptance.PurchaseId);
            var purchase = await purchaseRepo.FirstOrDefaultAsync(purchaseSpec, cancellationToken)
                          ?? throw new InvalidOperationException($"Purchase {acceptance.PurchaseId} not found.");

            foreach (var dto in request.Items)
            {
                var purchaseItem = purchase.Items.FirstOrDefault(pi => pi.Id == dto.PurchaseItemId)
                    ?? throw new InvalidOperationException($"Purchase item {dto.PurchaseItemId} not found in purchase {acceptance.PurchaseId}.");

                // Single-shot validation for new items
                if (!dto.Id.HasValue || !byId.ContainsKey(dto.Id.Value))
                {
                    var existingAcceptanceSpec = new AcceptanceItemExistsForPurchaseItemSpec(dto.PurchaseItemId);
                    var existingAcceptance = await acceptanceRepo.FirstOrDefaultAsync(existingAcceptanceSpec, cancellationToken);
                    if (existingAcceptance != null && existingAcceptance.Id != acceptance.Id)
                    {
                        throw new InvalidOperationException($"Purchase item {dto.PurchaseItemId} has already been accepted. Single-shot acceptance is enforced.");
                    }
                }

                // Quantity validation
                if (dto.QtyAccepted > purchaseItem.Qty)
                {
                    throw new InvalidOperationException($"Accepted quantity {dto.QtyAccepted} exceeds ordered quantity {purchaseItem.Qty} for purchase item {dto.PurchaseItemId}.");
                }

                if (dto.QtyAccepted <= 0)
                {
                    throw new InvalidOperationException("Accepted quantity must be greater than zero.");
                }

                if (dto.Id.HasValue && byId.TryGetValue(dto.Id.Value, out var entity))
                {
                    entity.Update(acceptance.Id, dto.PurchaseItemId, dto.QtyAccepted, dto.Remarks);
                    updatedIds.Add(entity.Id);
                }
                else
                {
                    // Create via aggregate helper to keep invariants
                    acceptance.AddItem(dto.PurchaseItemId, dto.QtyAccepted, dto.Remarks);
                    var newItem = acceptance.Items.Last();
                    updatedIds.Add(newItem.Id);
                }
            }
        }

        // Handle deletions from explicit list
        if (request.DeletedItemIds is not null && request.DeletedItemIds.Count > 0)
        {
            foreach (var delId in request.DeletedItemIds)
            {
                if (byId.TryGetValue(delId, out var toDelete))
                {
                    acceptance.Items.Remove(toDelete);
                }
            }
        }

        // Safety net: remove any persisted items that were not included in the incoming Items collection and not explicitly updated.
        var incomingIds = request.Items?.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToHashSet() ?? new HashSet<Guid>();
        foreach (var staleId in originalPersistedIds.Except(incomingIds).Except(request.DeletedItemIds ?? Array.Empty<Guid>()))
        {
            if (byId.TryGetValue(staleId, out var staleEntity))
            {
                acceptance.Items.Remove(staleEntity);
            }
        }

        // Persist header last
        await acceptanceRepo.UpdateAsync(acceptance, cancellationToken);

        logger.LogInformation("Acceptance {AcceptanceId} updated with items", acceptance.Id);

        return new UpdateAcceptanceWithItemsResponse(acceptance.Id, updatedIds);
    }
}