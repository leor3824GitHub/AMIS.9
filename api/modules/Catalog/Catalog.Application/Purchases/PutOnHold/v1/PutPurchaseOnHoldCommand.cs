using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.PutOnHold.v1;

public sealed record PutPurchaseOnHoldCommand(
    Guid PurchaseId,
    string Reason
) : IRequest<PutPurchaseOnHoldResponse>;
