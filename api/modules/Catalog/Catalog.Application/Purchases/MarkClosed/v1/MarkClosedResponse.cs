using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkClosed.v1;

public sealed record MarkClosedResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
