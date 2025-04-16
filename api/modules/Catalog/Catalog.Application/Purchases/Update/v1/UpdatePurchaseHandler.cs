using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;
public sealed class UpdatePurchaseHandler(
    ILogger<UpdatePurchaseHandler> logger,
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<UpdatePurchaseCommand, UpdatePurchaseResponse>
{
    public async Task<UpdatePurchaseResponse> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Retrieve the purchase entity to update
        var purchase = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = purchase ?? throw new PurchaseNotFoundException(request.Id);

        // Update purchase basic details (supplier, date, status, total amount)
        purchase.Update(request.SupplierId, request.PurchaseDate, request.TotalAmount, request.Status);

        // Convert incoming items (request.Items) to a dictionary for easier lookups
        var incomingItems = request.Items.ToDictionary(i => i.Id ?? Guid.NewGuid()); // Handle new items with a generated ID
        var existingItems = purchase.Items.ToDictionary(i => i.Id); // Existing items in the current purchase

        // 1. Handle item updates and new items
        foreach (var (incomingId, incoming) in incomingItems)
        {
            if (existingItems.TryGetValue(incomingId, out var existingItem))
            {
                // Update the item if there are any changes
                var changed = existingItem.Update(incoming.ProductId, incoming.Qty, incoming.UnitPrice, incoming.ItemStatus);

                if (changed != null)
                {
                    // Queue domain event for the update
                    purchase.QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = existingItem });
                }
            }
            else
            {
                // Create a new item if it's not found in the existing items
                var newItem = PurchaseItem.Create(purchase.Id, incoming.ProductId, incoming.Qty, incoming.UnitPrice, incoming.ItemStatus);
                purchase.Items.Add(newItem);

                // Queue domain event for the new item
                purchase.QueueDomainEvent(new PurchaseItemCreated { PurchaseItem = newItem });
            }
        }

        // 2. Handle removed items
        var toRemove = existingItems.Values
            .Where(existingItem => !incomingItems.ContainsKey(existingItem.Id))
            .ToList();

        foreach (var item in toRemove)
        {
            // Remove the item (soft-delete is handled by the interceptor)
            purchase.Items.Remove(item);

            // Queue domain event for the removed item
            purchase.QueueDomainEvent(new PurchaseItemRemoved { PurchaseItem = item });
        }

        // 3. Recalculate the total amount of the purchase
        var total = purchase.Items.Sum(i => i.Qty * i.UnitPrice);
        purchase.Update(purchase.SupplierId, purchase.PurchaseDate, total, purchase.Status);

        // Save changes to the repository
        await repository.SaveChangesAsync(cancellationToken);

        // Prepare the response DTO with the updated purchase items
        var itemDtos = purchase.Items.Select(i =>
            new PurchaseItemDto(i.Id, i.ProductId, i.Qty, i.UnitPrice, i.ItemStatus)).ToList();

        // Log the purchase update
        logger.LogInformation("Purchase with id {PurchaseId} updated.", purchase.Id);

        return new UpdatePurchaseResponse(purchase.Id, itemDtos);
    }
}
