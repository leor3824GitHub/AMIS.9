using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.Acknowledge.v1;

public sealed record AcknowledgePurchaseResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
