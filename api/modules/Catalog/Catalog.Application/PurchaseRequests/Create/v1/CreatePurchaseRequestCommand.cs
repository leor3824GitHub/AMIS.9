using MediatR;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Create.v1;

public sealed record PurchaseRequestItemCreateDto(
    Guid? ProductId,
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
