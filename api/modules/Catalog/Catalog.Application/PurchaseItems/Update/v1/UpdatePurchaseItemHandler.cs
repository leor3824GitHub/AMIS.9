using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Update.v1;
public sealed class UpdatePurchaseItemHandler(
    ILogger<UpdatePurchaseItemHandler> logger,
    [FromKeyedServices("catalog:purchaseItems")] IRepository<PurchaseItem> repository)
    : IRequestHandler<UpdatePurchaseItemCommand, UpdatePurchaseItemResponse>
{
    public async Task<UpdatePurchaseItemResponse> Handle(UpdatePurchaseItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Fetch purchase item, throw if not found
        var purchaseItem = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new PurchaseItemNotFoundException(request.Id);

        // Update purchase item
        var updatedPurchaseItem = purchaseItem.Update(request.ProductId, request.Qty, request.UnitPrice, request.ItemStatus);

        await repository.UpdateAsync(updatedPurchaseItem, cancellationToken);
        logger.LogInformation("Purchase item with Id {PurchaseItemId} updated successfully.", purchaseItem.Id);
                
        return new UpdatePurchaseItemResponse(Id: purchaseItem.Id, Success: true, ErrorMessage: null);
    }

}
