using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Update.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Update.v1;
public sealed class UpdatePurchaseItemHandler(
    ILogger<UpdatePurchaseItemHandler> logger,
    [FromKeyedServices("catalog:purchaseItems")] IRepository<PurchaseItem> repository,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepository)
    : IRequestHandler<UpdatePurchaseItemCommand, UpdatePurchaseItemResponse>
{
    public async Task<UpdatePurchaseItemResponse> Handle(UpdatePurchaseItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Fetch purchase item, throw if not found
        var purchaseItem = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new PurchaseItemNotFoundException(request.Id);

        int oldQty = purchaseItem.Qty;

        // Update purchase item
        var updatedPurchaseItem = purchaseItem.Update(
            request.PurchaseId, request.ProductId, request.Qty, request.UnitPrice, request.Status);

        await repository.UpdateAsync(updatedPurchaseItem, cancellationToken);
        logger.LogInformation("Purchase item with ID {PurchaseItemId} updated successfully.", purchaseItem.Id);

        // Fetch inventory, return an error response if not found
        var inventory = await inventoryRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (inventory is null)
        {
            logger.LogWarning("Inventory not found for Product ID {ProductId}.", request.ProductId);
            return new UpdatePurchaseItemResponse(Id: null, Success: false, ErrorMessage: "Inventory not found.");
        }

        // Adjust inventory stock
        inventory.UpdateStock(oldQty, request.Qty, request.UnitPrice);
        await inventoryRepository.UpdateAsync(inventory, cancellationToken);
        logger.LogInformation(
            "Inventory updated for Product ID {ProductId}. Previous Qty: {OldQty}, New Qty: {NewQty}.",
            request.ProductId, oldQty, request.Qty
        );

        return new UpdatePurchaseItemResponse(Id: purchaseItem.Id, Success: true, ErrorMessage: null);
    }

}
