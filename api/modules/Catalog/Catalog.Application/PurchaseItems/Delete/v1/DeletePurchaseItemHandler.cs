using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Delete.v1;
public sealed class DeletePurchaseItemHandler(
    ILogger<DeletePurchaseItemHandler> logger,
    [FromKeyedServices("catalog:purchaseItems")] IRepository<PurchaseItem> repository)
    : IRequestHandler<DeletePurchaseItemCommand>
{
    public async Task Handle(DeletePurchaseItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var purchaseItem = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = purchaseItem ?? throw new PurchaseItemNotFoundException(request.Id);
        await repository.DeleteAsync(purchaseItem, cancellationToken);
        logger.LogInformation("purchaseItem with id : {PurchaseItemId} deleted", purchaseItem.Id);
    }
}
