using MediatR;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Create.v1;

public sealed record PurchaseRequestItemCreateDto(
    Guid? ProductId,
    int Qty,
    string? Description,
    string? Justification
);

public sealed record CreatePurchaseRequestCommand(
    Guid RequestedBy,
    string Purpose,
    ICollection<PurchaseRequestItemCreateDto>? Items = null
) : IRequest<CreatePurchaseRequestResponse>;
