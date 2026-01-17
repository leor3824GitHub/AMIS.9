using MediatR;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.ManageItems.v1;

public sealed record DeletePurchaseRequestItemCommand(
    Guid PurchaseRequestId,
    Guid ItemId
) : IRequest;

