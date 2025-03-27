using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
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

        // Retrieve the issuance item from the repository
        var issuanceItem = await repository.GetByIdAsync(request.Id, cancellationToken);

        // If the issuance item does not exist, throw an exception to stop further processing
        if (issuanceItem == null)
        {
            logger.LogWarning("IssuanceItem with id {IssuanceItemId} not found.", request.Id);
            return; // No need to proceed if the item does not exist
        }

        // Try to delete the issuance item from the repository
        try
        {
            await repository.DeleteAsync(issuanceItem, cancellationToken);
            logger.LogInformation("IssuanceItem with id : {IssuanceItemId} deleted successfully", issuanceItem.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting IssuanceItem with id : {IssuanceItemId}", issuanceItem.Id);
            return; // Stop execution if deletion failed
        }

        // Check if we need to update the inventory
        var spec = new GetInventoryProductIdSpecs(issuanceItem.ProductId);
        var inventory = await inventoryRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (inventory == null)
        {
            // Log a warning if inventory is not found for the product
            logger.LogWarning("Inventory not found for ProductId {ProductId}", issuanceItem.ProductId);
            return; // No need to proceed if inventory doesn't exist
        }

        try
        {
            // Update the stock based on the deleted issuance item quantity
            inventory.UpdateStock(issuanceItem.Qty);

            // Save the updated inventory
            await inventoryRepository.UpdateAsync(inventory, cancellationToken);
            logger.LogInformation("{Qty} quantity of ProductId {ProductId} returned to inventory successfully.", issuanceItem.Qty, issuanceItem.ProductId);
        }
        catch (Exception ex)
        {
            // Log an error if updating inventory fails
            logger.LogError(ex, "An error occurred while updating inventory for ProductId {ProductId}", issuanceItem.ProductId);
        }
    }

}
