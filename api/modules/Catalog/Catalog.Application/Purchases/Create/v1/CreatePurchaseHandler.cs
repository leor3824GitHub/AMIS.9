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
        
        // Create the purchase entity
        var purchase = Purchase.Create(request.SupplierId, request.PurchaseDate, request.TotalAmount);

        // Add items if any
        if (request.Items is not null && request.Items.Count > 0)
        {
            foreach (var item in request.Items)
            {
                purchase.AddItem(item.ProductId, item.Qty, item.UnitPrice, item.Status);
            }
        }

        // Save entire aggregate (purchase + items)
        await repository.AddAsync(purchase, cancellationToken);

        logger.LogInformation("purchase with items created {PurchaseId}", purchase.Id);
        return new CreatePurchaseResponse(purchase.Id);
    }
}

