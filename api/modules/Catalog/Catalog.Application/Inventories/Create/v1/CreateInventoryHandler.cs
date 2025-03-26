using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Inventories.Create.v1;
public sealed class CreateInventoryHandler(
    ILogger<CreateInventoryHandler> logger,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    : IRequestHandler<CreateInventoryCommand, CreateInventoryResponse>
{
    public async Task<CreateInventoryResponse> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var inventory = Inventory.Create(request.ProductId, request.Qty, request.AvePrice);
        await repository.AddAsync(inventory, cancellationToken);
        logger.LogInformation("inventory created {InventoryId}", inventory.Id);
        return new CreateInventoryResponse(inventory.Id);
    }
}
