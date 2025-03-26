using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Delete.v1;
public sealed class DeleteIssuanceItemHandler(
    ILogger<DeleteIssuanceItemHandler> logger,
    [FromKeyedServices("catalog:issuanceItems")] IRepository<IssuanceItem> repository,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepository)
    : IRequestHandler<DeleteIssuanceItemCommand>
{
    public async Task Handle(DeleteIssuanceItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var issuanceItem = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = issuanceItem ?? throw new IssuanceItemNotFoundException(request.Id);

        var productId = issuanceItem.ProductId;
        var qty = issuanceItem.Qty;

        await repository.DeleteAsync(issuanceItem, cancellationToken);
        logger.LogInformation("issuanceItem with id : {IssuanceItemId} deleted", issuanceItem.Id);

        //check if update inventory
        var inventory = await inventoryRepository.GetByIdAsync(productId, cancellationToken);
        if (inventory == null)
        {
            logger.LogWarning("Inventory not found for ProductId {ProductId}", productId);
            return;
        }

        inventory.UpdateStock(qty);
        await inventoryRepository.UpdateAsync(inventory, cancellationToken);
        logger.LogInformation(" {Qty} quantity of ProductId {ProductId} returned to Inventory.", qty, productId);
    }
}
