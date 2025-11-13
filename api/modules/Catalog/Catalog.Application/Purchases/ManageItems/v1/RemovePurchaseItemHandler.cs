using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.ManageItems.v1;

public sealed class RemovePurchaseItemHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<RemovePurchaseItemCommand>
{
    public async Task Handle(RemovePurchaseItemCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken);
        if (purchase is null)
            throw new Exception($"Purchase {request.PurchaseId} not found");

        purchase.RemoveItem(request.ItemId);
        await repository.UpdateAsync(purchase, cancellationToken);
    }
}
