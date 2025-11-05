using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.MarkAsObsolete.v1;

public sealed class MarkAsObsoleteHandler : IRequestHandler<MarkAsObsoleteCommand, MarkAsObsoleteResponse>
{
    private readonly IRepository<Inventory> _repository;

    public MarkAsObsoleteHandler([FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    {
        _repository = repository;
    }

    public async Task<MarkAsObsoleteResponse> Handle(MarkAsObsoleteCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.GetByIdAsync(request.InventoryId, cancellationToken)
            ?? throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        inventory.MarkAsObsolete();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new MarkAsObsoleteResponse(
            inventory.Id,
            inventory.StockStatus,
            "Inventory marked as obsolete successfully."
        );
    }
}
