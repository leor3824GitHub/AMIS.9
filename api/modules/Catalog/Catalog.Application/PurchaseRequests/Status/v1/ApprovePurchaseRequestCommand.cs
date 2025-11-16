using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;

public sealed record ApprovePurchaseRequestCommand(Guid PurchaseRequestId, Guid ApprovedBy, string? Remarks) : IRequest;
