using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.PutOnHold.v1;

public sealed record PutPurchaseOnHoldResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message,
    string Reason
);
