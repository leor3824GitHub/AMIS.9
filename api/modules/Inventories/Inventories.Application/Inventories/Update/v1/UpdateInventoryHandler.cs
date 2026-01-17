using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Inventories.Update.v1;
public sealed class UpdateInventoryHandler(
    ILogger<UpdateInventoryHandler> logger,
    [FromKeyedServices("inventories:inventories")] IRepository<Inventory> repository)
    : IRequestHandler<UpdateInventoryCommand, UpdateInventoryResponse>
{
    public async Task<UpdateInventoryResponse> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var inventory = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = inventory ?? throw new InventoryNotFoundException(request.Id);
        var updatedInventory = inventory.Update(request.ProductId, request.Qty, request.AvePrice);
        await repository.UpdateAsync(updatedInventory, cancellationToken);
        logger.LogInformation("inventory with id : {InventoryId} updated.", inventory.Id);
        return new UpdateInventoryResponse(inventory.Id);
    }
}

