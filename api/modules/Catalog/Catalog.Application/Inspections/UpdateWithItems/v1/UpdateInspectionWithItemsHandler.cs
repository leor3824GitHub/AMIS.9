using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Inspections.UpdateWithItems.v1;

public sealed class UpdateInspectionWithItemsHandler(
    ILogger<UpdateInspectionWithItemsHandler> logger,
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> inspectionRepo,
    [FromKeyedServices("catalog:inspectionRequests")] IReadRepository<InspectionRequest> inspectionRequestRepo,
    [FromKeyedServices("catalog:purchases")] IReadRepository<Purchase> purchaseRepo)
    : IRequestHandler<UpdateInspectionWithItemsCommand, UpdateInspectionWithItemsResponse>
{
    public async Task<UpdateInspectionWithItemsResponse> Handle(UpdateInspectionWithItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Load with tracking to allow EF to detect collection changes
        var inspection = await inspectionRepo.FirstOrDefaultAsync(new GetInspectionWithItemsSpec(request.Id), cancellationToken);
        if (inspection is null)
            throw new InvalidOperationException($"Inspection {request.Id} not found.");

        // Update inspection header
        if (request.InspectorId.HasValue)
        {
            inspection.SetEmployee(request.InspectorId.Value);
        }

        if (request.InspectionDate.HasValue)
        {
            inspection.SetInspectedOn(request.InspectionDate.Value);
        }

        inspection.UpdateRemarks(request.Remarks);

        // Work through the aggregate collection
        var itemsForInspection = inspection.Items.ToList();
        var byId = itemsForInspection.Where(i => i.Id != Guid.Empty).ToDictionary(i => i.Id);

        var updatedIds = new List<Guid>();

        // Track original persisted item IDs for diff-based removal
        var originalPersistedIds = inspection.Items.Where(i => i.Id != Guid.Empty).Select(i => i.Id).ToHashSet();

        // Upsert incoming items
        if (request.Items != null)
        {
            foreach (var dto in request.Items)
            {
                // Single-shot validation for new items
                if (!dto.Id.HasValue || !byId.ContainsKey(dto.Id.Value))
                {
                    var existingInspectionSpec = new InspectionItemExistsForPurchaseItemSpec(dto.PurchaseItemId);
                    var existingInspection = await inspectionRepo.FirstOrDefaultAsync(existingInspectionSpec, cancellationToken);
                    if (existingInspection != null && existingInspection.Id != inspection.Id)
                    {
                        throw new InvalidOperationException($"Purchase item {dto.PurchaseItemId} has already been inspected. Single-shot inspection is enforced.");
                    }
                }

                if (dto.Id.HasValue && byId.TryGetValue(dto.Id.Value, out var entity))
                {
                    entity.Update(inspection.Id, dto.PurchaseItemId, dto.QtyInspected, dto.QtyPassed, dto.QtyFailed, dto.Remarks, dto.InspectionItemStatus);
                    updatedIds.Add(entity.Id);
                }
                else
                {
                    // Create via aggregate helper to keep invariants
                    inspection.AddItem(dto.PurchaseItemId, dto.QtyInspected, dto.QtyPassed, dto.QtyFailed, dto.Remarks, dto.InspectionItemStatus);
                    var newItem = inspection.Items.Last();
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
                    inspection.Items.Remove(toDelete);
                }
            }
        }

        // Safety net: remove any persisted items that were not included in the incoming Items collection and not explicitly updated.
        var incomingIds = request.Items?.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToHashSet() ?? new HashSet<Guid>();
        foreach (var staleId in originalPersistedIds.Except(incomingIds).Except(request.DeletedItemIds ?? Array.Empty<Guid>()))
        {
            if (byId.TryGetValue(staleId, out var staleEntity))
            {
                inspection.Items.Remove(staleEntity);
            }
        }

        // Re-evaluate inspection status - fetch related data from their own repositories
        InspectionRequest? inspectionRequest = null;
        if (inspection.InspectionRequestId.HasValue)
        {
            inspectionRequest = await inspectionRequestRepo.GetByIdAsync(inspection.InspectionRequestId.Value, cancellationToken);
        }

        if (inspectionRequest?.PurchaseId != null)
        {
            var purchaseSpec = new AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1.GetPurchaseWithItemsSpecs(inspectionRequest.PurchaseId.Value);
            var purchase = await purchaseRepo.FirstOrDefaultAsync(purchaseSpec, cancellationToken);

            if (purchase != null)
            {
                // Pass data (not entity) across aggregate boundary
                var purchaseItemData = purchase.Items
                    .Select(pi => new PurchaseItemSummary(pi.Id, pi.Qty))
                    .ToList();

                inspection.EvaluateAndSetStatus(purchaseItemData);
            }
        }

        // Persist header last
        await inspectionRepo.UpdateAsync(inspection, cancellationToken);

        logger.LogInformation("Inspection {InspectionId} updated with items", inspection.Id);

        return new UpdateInspectionWithItemsResponse(inspection.Id, updatedIds);
    }
}