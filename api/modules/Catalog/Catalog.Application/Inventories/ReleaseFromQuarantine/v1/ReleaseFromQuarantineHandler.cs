using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.ReleaseFromQuarantine.v1;

public sealed class ReleaseFromQuarantineHandler : IRequestHandler<ReleaseFromQuarantineCommand, ReleaseFromQuarantineResponse>
{
    private readonly IRepository<Inventory> _repository;

    public ReleaseFromQuarantineHandler([FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    {
        _repository = repository;
    }

    public async Task<ReleaseFromQuarantineResponse> Handle(ReleaseFromQuarantineCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.GetByIdAsync(request.InventoryId, cancellationToken)
            ?? throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        inventory.ReleaseFromQuarantine();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new ReleaseFromQuarantineResponse(
            inventory.Id,
            inventory.StockStatus,
            "Inventory released from quarantine successfully."
        );
    }
}
