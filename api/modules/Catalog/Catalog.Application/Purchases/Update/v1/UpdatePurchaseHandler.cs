using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
using AMIS.WebApi.Catalog.Domain;
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
        var purchase = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = purchase ?? throw new PurchaseNotFoundException(request.Id);
        
        var updatedPurchase = purchase.Update(request.SupplierId, request.PurchaseDate, request.TotalAmount, request.Status);

        // Use the SyncItems method to update, add, and remove items inside the aggregate
        var itemUpdates = request.Items.Select(i => new PurchaseItemUpdate(
            i.Id, // Pass the Id
            i.ProductId,
            i.Qty,
            i.UnitPrice,
            i.ItemStatus
        )).ToList();

        purchase.SyncItems(itemUpdates);

        // Save the updated aggregate root
        await repository.UpdateAsync(updatedPurchase, cancellationToken);
        // ? Convert domain PurchaseItem to DTO
        var itemDtos = purchase.Items.Select(i => new PurchaseItemDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            Qty = i.Qty,
            UnitPrice = i.UnitPrice,
            ItemStatus = i.ItemStatus
        }).ToList();

        logger.LogInformation("Purchase with id {PurchaseId} updated.", purchase.Id);

        return new UpdatePurchaseResponse(purchase.Id, itemDtos);
    }
}
