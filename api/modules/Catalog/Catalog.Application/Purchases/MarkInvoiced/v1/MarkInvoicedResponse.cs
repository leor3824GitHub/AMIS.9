using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkInvoiced.v1;

public record MarkInvoicedResponse(
    Guid PurchaseId,
    PurchaseStatus Status,
    string Message
);
