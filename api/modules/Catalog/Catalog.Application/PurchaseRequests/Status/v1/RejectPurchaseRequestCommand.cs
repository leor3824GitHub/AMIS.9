using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;

public sealed record RejectPurchaseRequestCommand(Guid PurchaseRequestId, Guid RejectedBy, string Reason) : IRequest;
