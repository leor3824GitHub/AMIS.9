using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.ManageItems.v1;

public sealed class AddPurchaseItemHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<AddPurchaseItemCommand, AddPurchaseItemResponse>
{
    public async Task<AddPurchaseItemResponse> Handle(AddPurchaseItemCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken);
        if (purchase is null)
            throw new Exception($"Purchase {request.PurchaseId} not found");

        purchase.AddItem(request.ProductId, request.Qty, request.UnitPrice, request.ItemStatus);
        var item = purchase.Items.Last();
        await repository.UpdateAsync(purchase, cancellationToken);
        return new AddPurchaseItemResponse(item.Id);
    }
}
