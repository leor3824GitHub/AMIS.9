using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkPendingInvoice.v1;

public sealed record MarkPendingInvoiceCommand(Guid PurchaseId) : IRequest<MarkPendingInvoiceResponse>;
