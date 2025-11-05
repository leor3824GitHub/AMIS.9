using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.ReleaseFromHold.v1;

public sealed record ReleasePurchaseFromHoldResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
