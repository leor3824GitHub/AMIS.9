using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Purchases.Create.v1;

public sealed class CreatePurchaseHandler(
    ILogger<CreatePurchaseHandler> logger,
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<CreatePurchaseCommand, CreatePurchaseResponse>
{
    public async Task<CreatePurchaseResponse> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create the purchase entity (aggregate root)
        var purchase = Purchase.Create(
            request.SupplierId,
            request.PurchaseDate,
            request.TotalAmount,
            request.Status
        );

        // If items are provided, sync them into the purchase
        if (request.Items is { Count: > 0 })
        {
            var itemUpdates = request.Items.Select(item => new PurchaseItemUpdate(
                null, // Id is null on create
                item.ProductId,
                item.Qty,
                item.UnitPrice,
                item.ItemStatus
            )).ToList();

            purchase.SyncItems(itemUpdates);
        }

        // Persist the entire aggregate (purchase + items)
        await repository.AddAsync(purchase, cancellationToken);

        // ✅ Convert domain PurchaseItem to DTO
        var itemDtos = purchase.Items.Select(i =>
            new PurchaseItemDto(i.Id, i.ProductId, i.Qty, i.UnitPrice, i.ItemStatus)).ToList();

        logger.LogInformation("Purchase created with ID {PurchaseId}", purchase.Id);
        return new CreatePurchaseResponse(purchase.Id, itemDtos);
    }
}
