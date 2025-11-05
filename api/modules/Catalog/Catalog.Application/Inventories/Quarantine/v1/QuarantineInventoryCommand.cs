using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.Quarantine.v1;

public sealed record QuarantineInventoryCommand(
    Guid InventoryId,
    string? Location) : IRequest<QuarantineInventoryResponse>;

public sealed record QuarantineInventoryResponse(
    Guid InventoryId,
    string Status,
    string? Location,
    string Message);
