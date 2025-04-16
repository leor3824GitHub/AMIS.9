using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;

public sealed class UpdatePurchaseHandler(
    ILogger<UpdatePurchaseHandler> logger,
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<UpdatePurchaseCommand, UpdatePurchaseResponse>
{
    public async Task<UpdatePurchaseResponse> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Fetch the existing purchase
        var spec = new GetUpdatePurchaseSpecs(request.Id);
        var purchase = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (purchase is null)
            throw new PurchaseNotFoundException(request.Id);

        // Update basic purchase fields
        purchase.Update(request.SupplierId, request.PurchaseDate, request.TotalAmount, request.Status);

        // Sync items at application level (not in domain!)
        SyncPurchaseItems(request.Id, purchase, request.Items);

        // Persist changes
        await repository.UpdateAsync(purchase, cancellationToken);

        logger.LogInformation("Purchase with Id {PurchaseId} successfully updated.", purchase.Id);

        return new UpdatePurchaseResponse(purchase.Id);
    }

    private static void SyncPurchaseItems(Guid purchaseId, Purchase purchase, ICollection<PurchaseItemUpdateDto>? updates)
    {
        // Create a lookup of existing items
        var existingMap = purchase.Items.ToDictionary(i => i.Id, i => i);

        foreach (var update in updates)
        {
            switch (update.OperationType)
            {
                case ItemOperationType.Add:
                    {
                        purchase.AddItem(purchaseId, update.ProductId, update.Qty, update.UnitPrice, update.ItemStatus);
                        break;
                    }

                case ItemOperationType.Update:
                    {
                        if (update.Id.HasValue && existingMap.TryGetValue(update.Id.Value, out var existing))
                        {
                            var before = (existing.ProductId, existing.Qty, existing.UnitPrice, existing.ItemStatus);
                            existing.Update(update.ProductId, update.Qty, update.UnitPrice, update.ItemStatus);

                            if (before != (existing.ProductId, existing.Qty, existing.UnitPrice, existing.ItemStatus))
                            {
                                purchase.QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = existing });
                            }
                        }
                        break;
                    }

                case ItemOperationType.Remove:
                    {
                        if (update.Id.HasValue && existingMap.TryGetValue(update.Id.Value, out var toRemove))
                        {
                            purchase.Items.Remove(toRemove);
                            purchase.QueueDomainEvent(new PurchaseItemRemoved { PurchaseItem = toRemove });
                        }
                        break;
                    }
            }
        }
    }
}
