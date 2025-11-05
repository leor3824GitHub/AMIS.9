using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkInProgress.v1;

public sealed record MarkInProgressResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
