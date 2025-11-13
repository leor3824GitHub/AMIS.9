using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.ManageItems.v1;

public sealed class UpdatePurchaseItemHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<UpdatePurchaseItemCommand>
{
    public async Task Handle(UpdatePurchaseItemCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken);
        if (purchase is null)
            throw new Exception($"Purchase {request.PurchaseId} not found");

        purchase.UpdateItem(request.ItemId, request.ProductId, request.Qty, request.UnitPrice, request.ItemStatus);
        await repository.UpdateAsync(purchase, cancellationToken);
    }
}
