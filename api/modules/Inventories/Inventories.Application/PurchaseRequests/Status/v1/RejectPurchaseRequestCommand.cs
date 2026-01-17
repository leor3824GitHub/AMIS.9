using MediatR;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.Status.v1;

public sealed record RejectPurchaseRequestCommand(Guid PurchaseRequestId, Guid RejectedBy, string Reason) : IRequest;

