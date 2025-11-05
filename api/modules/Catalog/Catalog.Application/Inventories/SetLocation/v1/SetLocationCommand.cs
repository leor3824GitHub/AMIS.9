using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.SetLocation.v1;

public sealed record SetLocationCommand(
    Guid InventoryId,
    string Location
) : IRequest<SetLocationResponse>;
