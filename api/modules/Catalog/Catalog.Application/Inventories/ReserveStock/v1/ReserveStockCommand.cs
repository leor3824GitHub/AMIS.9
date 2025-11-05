using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.ReserveStock.v1;

public sealed record ReserveStockCommand(
    Guid InventoryId,
    int Quantity,
    string? ReservationReference) : IRequest<ReserveStockResponse>;
