using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Approve.v1;

public sealed record ApprovePurchaseCommand(Guid PurchaseId) : IRequest<ApprovePurchaseResponse>;

public sealed record ApprovePurchaseResponse(
    Guid PurchaseId,
    string Status,
    string Message);
