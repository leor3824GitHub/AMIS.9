using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.SetCostingMethod.v1;

public sealed class SetCostingMethodHandler : IRequestHandler<SetCostingMethodCommand, SetCostingMethodResponse>
{
    private readonly IRepository<Inventory> _repository;

    public SetCostingMethodHandler([FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    {
        _repository = repository;
    }

    public async Task<SetCostingMethodResponse> Handle(SetCostingMethodCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.GetByIdAsync(request.InventoryId, cancellationToken)
            ?? throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        inventory.SetCostingMethod(request.Method);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new SetCostingMethodResponse(
            inventory.Id,
            request.Method,
            $"Costing method set to {request.Method} successfully."
        );
    }
}
