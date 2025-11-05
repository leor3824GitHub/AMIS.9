using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkPendingInvoice.v1;

public sealed record MarkPendingInvoiceResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
