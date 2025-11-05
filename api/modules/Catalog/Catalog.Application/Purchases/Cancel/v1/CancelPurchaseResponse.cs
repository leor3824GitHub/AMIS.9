using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.Cancel.v1;

public record CancelPurchaseResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Reason,
    string Message
);
