using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.SetLocation.v1;

public sealed class SetLocationHandler : IRequestHandler<SetLocationCommand, SetLocationResponse>
{
    private readonly IRepository<Inventory> _repository;

    public SetLocationHandler([FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    {
        _repository = repository;
    }

    public async Task<SetLocationResponse> Handle(SetLocationCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.GetByIdAsync(request.InventoryId, cancellationToken)
            ?? throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        inventory.SetLocation(request.Location);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new SetLocationResponse(
            inventory.Id,
            request.Location,
            "Inventory location updated successfully."
        );
    }
}
