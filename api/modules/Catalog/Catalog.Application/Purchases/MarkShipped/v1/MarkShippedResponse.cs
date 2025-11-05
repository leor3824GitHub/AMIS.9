using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkShipped.v1;

public record MarkShippedResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
