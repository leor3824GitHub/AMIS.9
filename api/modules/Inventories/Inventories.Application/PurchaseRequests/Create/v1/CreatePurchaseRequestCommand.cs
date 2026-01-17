using MediatR;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.Create.v1;

public sealed record PurchaseRequestItemCreateDto(
    Guid? ProductId,
    string? ManualProductName,
    int Qty,
    string Unit,
    string? Description
);

public sealed record CreatePurchaseRequestCommand(
    DateTime RequestDate,
    Guid RequestedBy,
    string Purpose,
    ICollection<PurchaseRequestItemCreateDto>? Items = null
) : IRequest<CreatePurchaseRequestResponse>;

