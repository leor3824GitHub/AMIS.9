using MediatR;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.Status.v1;

public sealed record ApprovePurchaseRequestCommand(Guid PurchaseRequestId, Guid ApprovedBy, string? Remarks) : IRequest;

