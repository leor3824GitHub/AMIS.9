using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkPartiallyReceived.v1;

public sealed record MarkPartiallyReceivedResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
