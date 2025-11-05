using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkPendingPayment.v1;

public sealed record MarkPendingPaymentResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
