using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Create.v2;
public sealed class CreatePurchaseItemHandler(
    ILogger<CreatePurchaseItemHandler> logger,
    [FromKeyedServices("catalog:purchaseItems")] IRepository<PurchaseItem> repository,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepository)
    : IRequestHandler<CreatePurchaseItemCommand, CreatePurchaseItemResponse>
{
    public async Task<CreatePurchaseItemResponse> Handle(CreatePurchaseItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        //create purchase item
        var purchaseItem = PurchaseItem.Create(request.PurchaseId!, request.ProductId!, request.Qty, request.UnitPrice, request.ItemStatus);

        await repository.AddAsync(purchaseItem, cancellationToken);
        logger.LogInformation("purchaseItem created {PurchaseItemId}", purchaseItem.Id);

        //check if inventory exists
        var spec = new GetInventoryProductIdSpecs(request.ProductId);
        var inventory = await inventoryRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (inventory == null)
        {
            //create new inventory entry
            inventory = Inventory.Create(request.ProductId!, request.Qty, request.UnitPrice);
            await inventoryRepository.AddAsync(inventory, cancellationToken);
            logger.LogInformation("New inventory created for ProductId {ProductId}", request.ProductId);
        }
        else
        {
            //update existing inventory
            inventory.AddStock(request.Qty, request.UnitPrice);
            await inventoryRepository.UpdateAsync(inventory, cancellationToken);
            logger.LogInformation("Inventory updated for ProductId {ProductId}", request.ProductId);
        }

        return new CreatePurchaseItemResponse(purchaseItem.Id);
    }
}
