using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
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
        Guid? issuanceItemId = Guid.Empty;

        ArgumentNullException.ThrowIfNull(request);

        //check if inventory exists
        var spec = new GetInventoryProductIdSpecs(request.ProductId);
        var inventory = await inventoryRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (inventory == null)
        {
            logger.LogWarning("Inventory not found for ProductId {ProductId}", request.ProductId);
            return new CreateIssuanceItemResponse(null, false, "Inventory not found.");
        }

        if (inventory.Qty < request.Qty)
        {
            logger.LogWarning("Not enough stock for ProductId {ProductId}", request.ProductId);
            return new CreateIssuanceItemResponse(null, false, "Not enough stock.");
        }
        try
        {
            // Create issuance item
            var issuanceItem = IssuanceItem.Create(request.IssuanceId!, request.ProductId!, request.Qty, request.UnitPrice, request.Status);
            await repository.AddAsync(issuanceItem, cancellationToken);
            issuanceItemId = issuanceItem.Id;

            logger.LogInformation("IssuanceItem created with Id {IssuanceItemId}", issuanceItem.Id);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating issuance item.");
            return new CreateIssuanceItemResponse(null, false, "Error creating issuance item.");
        }
        finally
        {
        // Deduct stock
            inventory.DeductStock(request.Qty);
            await inventoryRepository.UpdateAsync(inventory, cancellationToken);
            logger.LogInformation("Inventory updated for ProductId {ProductId}, deducted {Qty} units.", request.ProductId, request.Qty);
            
        }
        return new CreateIssuanceItemResponse(issuanceItemId, true, null);
    }


}
