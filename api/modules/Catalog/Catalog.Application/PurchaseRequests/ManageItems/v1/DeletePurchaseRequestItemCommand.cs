using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.ManageItems.v1;

public sealed record DeletePurchaseRequestItemCommand(
    Guid PurchaseRequestId,
    Guid ItemId
) : IRequest;
