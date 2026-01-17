using MediatR;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.Status.v1;

public sealed record CancelPurchaseRequestCommand(Guid PurchaseRequestId, string? Reason) : IRequest;

