using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Create.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Update.v1;
public sealed class UpdateIssuanceItemHandler(
    ILogger<UpdateIssuanceItemHandler> logger,
    [FromKeyedServices("catalog:issuanceItems")] IRepository<IssuanceItem> repository,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepository)
    : IRequestHandler<UpdateIssuanceItemCommand, UpdateIssuanceItemResponse>
{
    public async Task<UpdateIssuanceItemResponse> Handle(UpdateIssuanceItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
 
        //update issuance item
        var issuanceItem = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = issuanceItem ?? throw new IssuanceItemNotFoundException(request.Id);
        var oldqty = issuanceItem.Qty;
        var updatedIssuanceItem = issuanceItem.Update(request.IssuanceId, request.ProductId, request.Qty, request.UnitPrice, request.Status);
        await repository.UpdateAsync(updatedIssuanceItem, cancellationToken);
        logger.LogInformation("issuanceItem with id : {IssuanceItemId} updated.", issuanceItem.Id);

        //check if inventory exists
        var inventory = await inventoryRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (inventory == null)
        {
            // Log the warning and return an error if the inventory does not exist
            logger.LogWarning("Inventory not found for ProductId {ProductId}", request.ProductId);
            return new UpdateIssuanceItemResponse(Id: null, Success: false, ErrorMessage: "Inventory not found.");
        }

        // Deduct stock from inventory
        inventory.UpdateStock(oldqty,request.Qty, request.UnitPrice);
        await inventoryRepository.UpdateAsync(inventory, cancellationToken);
        logger.LogInformation("Inventory updated for ProductId {ProductId}, updated {Qty} units.", request.ProductId, request.Qty);

        return new UpdateIssuanceItemResponse(Id: issuanceItem.Id);
    }
}
