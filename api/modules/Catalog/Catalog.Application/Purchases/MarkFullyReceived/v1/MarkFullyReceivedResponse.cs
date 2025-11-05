using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkFullyReceived.v1;

public record MarkFullyReceivedResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
