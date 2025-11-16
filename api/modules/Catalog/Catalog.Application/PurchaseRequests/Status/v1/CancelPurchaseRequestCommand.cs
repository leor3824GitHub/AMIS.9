using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;

public sealed record CancelPurchaseRequestCommand(Guid PurchaseRequestId, string? Reason) : IRequest;
