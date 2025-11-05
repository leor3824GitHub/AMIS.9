using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.SubmitForApproval.v1;

public sealed record SubmitPurchaseForApprovalCommand(Guid PurchaseId) : IRequest<SubmitPurchaseForApprovalResponse>;

public sealed record SubmitPurchaseForApprovalResponse(
    Guid PurchaseId,
    string Status,
    string Message);
