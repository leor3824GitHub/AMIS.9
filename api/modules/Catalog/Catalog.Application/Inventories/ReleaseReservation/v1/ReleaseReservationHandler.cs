using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.ReleaseReservation.v1;

public class ReleaseReservationHandler(
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    : IRequestHandler<ReleaseReservationCommand, ReleaseReservationResponse>
{
    public async Task<ReleaseReservationResponse> Handle(ReleaseReservationCommand request, CancellationToken cancellationToken)
    {
        var inventory = await repository.GetByIdAsync(request.InventoryId, cancellationToken);

        if (inventory == null)
            throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        inventory.ReleaseReservation(request.Quantity);
        await repository.SaveChangesAsync(cancellationToken);

        return new ReleaseReservationResponse(
            inventory.Id,
            request.Quantity,
            inventory.ReservedQty,
            inventory.AvailableQty,
            $"Released {request.Quantity} units from reservation successfully."
        );
    }
}
