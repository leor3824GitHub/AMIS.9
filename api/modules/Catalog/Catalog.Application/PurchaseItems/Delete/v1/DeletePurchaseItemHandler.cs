using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Delete.v1;
public sealed class DeletePurchaseItemHandler(
    ILogger<DeletePurchaseItemHandler> logger,
    [FromKeyedServices("catalog:purchaseItems")] IRepository<PurchaseItem> repository,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepository)
    : IRequestHandler<DeletePurchaseItemCommand>
{
    public async Task Handle(DeletePurchaseItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var purchaseItem = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = purchaseItem ?? throw new IssuanceItemNotFoundException(request.Id);

        var productId = purchaseItem.ProductId;
        var qty = purchaseItem.Qty;
        var unitPrice = purchaseItem.UnitPrice;

        await repository.DeleteAsync(purchaseItem, cancellationToken);
        logger.LogInformation("purchaseItem with id : {PurchaseItemId} deleted", purchaseItem.Id);

        //check if update inventory
        var spec = new GetInventoryProductIdSpecs(productId);
        var inventory = await inventoryRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (inventory == null)
        {
            logger.LogWarning("Inventory not found for ProductId {ProductId}", productId);
            return;
        }

        inventory.DeductStock(qty, unitPrice);
        await inventoryRepository.UpdateAsync(inventory, cancellationToken);
        logger.LogInformation(" {Qty} quantity of ProductId {ProductId} deducted to Inventory.", qty, productId);

    }
}
