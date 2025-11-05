using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.AllocateToProduction.v1;

public sealed record AllocateToProductionCommand(
    Guid InventoryId,
    int Quantity
) : IRequest<AllocateToProductionResponse>;
