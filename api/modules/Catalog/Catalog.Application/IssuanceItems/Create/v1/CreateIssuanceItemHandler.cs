using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Create.v1;
public sealed class CreateIssuanceItemHandler(
    ILogger<CreateIssuanceItemHandler> logger,
    [FromKeyedServices("catalog:issuanceItems")] IRepository<IssuanceItem> repository,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepository)
    : IRequestHandler<CreateIssuanceItemCommand, CreateIssuanceItemResponse>
{
    public async Task<CreateIssuanceItemResponse> Handle(CreateIssuanceItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        //check if inventory exists
        var inventory = await inventoryRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (inventory == null)
        {
            // Log the warning and return an error if the inventory does not exist
            logger.LogWarning("Inventory not found for ProductId {ProductId}", request.ProductId);
            return new CreateIssuanceItemResponse(Id: null, Success: false, ErrorMessage: "Inventory not found.");
        }

        if (inventory.Qty < request.Qty)
        {
            // Log the warning and return an error if the inventory does not have enough stock
            logger.LogWarning("Inventory does not have enough stock for ProductId {ProductId}", request.ProductId);
            return new CreateIssuanceItemResponse(Id: null, Success: false, ErrorMessage: "Inventory does not have enough stock.");
        }
            // Deduct stock from inventory
            inventory.DeductStock(request.Qty);
            await inventoryRepository.UpdateAsync(inventory, cancellationToken);
            logger.LogInformation("Inventory updated for ProductId {ProductId}, deducted {Qty} units.", request.ProductId, request.Qty);

        //create purchase item
        var purchaseItem = IssuanceItem.Create(request.IssuanceId!, request.ProductId!, request.Qty, request.UnitPrice, request.Status);
        await repository.AddAsync(purchaseItem, cancellationToken);
        logger.LogInformation("purchaseItem created {IssuanceItemId}", purchaseItem.Id);

        return new CreateIssuanceItemResponse(Id: purchaseItem.Id, Success: true, ErrorMessage: null);
    }
}
