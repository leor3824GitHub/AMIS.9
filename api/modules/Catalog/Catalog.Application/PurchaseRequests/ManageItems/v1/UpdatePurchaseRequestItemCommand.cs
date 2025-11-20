using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.ManageItems.v1;

public sealed record UpdatePurchaseRequestItemCommand(
    Guid PurchaseRequestId,
    Guid ItemId,
    Guid? ProductId,
    int Qty,
    string Unit,
    string? Description
) : IRequest;
