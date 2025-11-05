using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.ReleaseReservation.v1;

public record ReleaseReservationCommand(Guid InventoryId, int Quantity) : IRequest<ReleaseReservationResponse>;
