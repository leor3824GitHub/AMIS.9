using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.AllocateToProduction.v1;

public sealed class AllocateToProductionHandler : IRequestHandler<AllocateToProductionCommand, AllocateToProductionResponse>
{
    private readonly IRepository<Inventory> _repository;

    public AllocateToProductionHandler([FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    {
        _repository = repository;
    }

    public async Task<AllocateToProductionResponse> Handle(AllocateToProductionCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.GetByIdAsync(request.InventoryId, cancellationToken)
            ?? throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        inventory.AllocateToProduction(request.Quantity);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new AllocateToProductionResponse(
            inventory.Id,
            request.Quantity,
            $"Allocated {request.Quantity} units to production successfully."
        );
    }
}
