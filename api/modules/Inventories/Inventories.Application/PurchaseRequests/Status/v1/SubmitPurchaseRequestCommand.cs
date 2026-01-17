using MediatR;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.Status.v1;

public sealed record SubmitPurchaseRequestCommand(Guid PurchaseRequestId) : IRequest;

