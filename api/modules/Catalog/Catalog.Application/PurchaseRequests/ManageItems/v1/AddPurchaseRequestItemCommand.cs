using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.ManageItems.v1;

public sealed record AddPurchaseRequestItemCommand(
    Guid PurchaseRequestId,
    Guid? ProductId,
    int Qty,
    string? Description,
    string? Justification
) : IRequest<Guid>; // returns new item Id
