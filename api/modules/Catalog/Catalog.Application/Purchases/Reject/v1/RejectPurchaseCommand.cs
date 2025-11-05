using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Reject.v1;

public sealed record RejectPurchaseCommand(
    Guid PurchaseId,
    string Reason) : IRequest<RejectPurchaseResponse>;

public sealed record RejectPurchaseResponse(
    Guid PurchaseId,
    string Status,
    string Message);
