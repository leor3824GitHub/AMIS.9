using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Inventories.Delete.v1;
public sealed class DeleteInventoryHandler(
    ILogger<DeleteInventoryHandler> logger,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    : IRequestHandler<DeleteInventoryCommand>
{
    public async Task Handle(DeleteInventoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var inventory = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = inventory ?? throw new InventoryNotFoundException(request.Id);
        await repository.DeleteAsync(inventory, cancellationToken);
        logger.LogInformation("inventory with id : {InventoryId} deleted", inventory.Id);
    }
}
