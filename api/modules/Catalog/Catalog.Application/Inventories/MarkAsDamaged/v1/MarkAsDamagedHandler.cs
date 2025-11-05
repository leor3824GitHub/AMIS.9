using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.MarkAsDamaged.v1;

public sealed class MarkAsDamagedHandler : IRequestHandler<MarkAsDamagedCommand, MarkAsDamagedResponse>
{
    private readonly IRepository<Inventory> _repository;

    public MarkAsDamagedHandler([FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    {
        _repository = repository;
    }

    public async Task<MarkAsDamagedResponse> Handle(MarkAsDamagedCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.GetByIdAsync(request.InventoryId, cancellationToken)
            ?? throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        inventory.MarkAsDamaged();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new MarkAsDamagedResponse(
            inventory.Id,
            inventory.StockStatus,
            "Inventory marked as damaged successfully."
        );
    }
}
