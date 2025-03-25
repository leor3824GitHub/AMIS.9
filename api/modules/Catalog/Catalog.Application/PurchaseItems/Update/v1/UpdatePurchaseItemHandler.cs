using AMIS.Framework.Core.Persistence;
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
        var purchaseItem = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = purchaseItem ?? throw new PurchaseItemNotFoundException(request.Id);
        var updatedPurchaseItem = purchaseItem.Update(request.PurchaseId, request.ProductId, request.Qty, request.UnitPrice, request.Status);
        await repository.UpdateAsync(updatedPurchaseItem, cancellationToken);
        logger.LogInformation("purchaseItem with id : {PurchaseItemId} updated.", purchaseItem.Id);
        return new UpdatePurchaseItemResponse(purchaseItem.Id);
    }
}
