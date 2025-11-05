using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkPendingPayment.v1;

public sealed record MarkPendingPaymentCommand(Guid PurchaseId) : IRequest<MarkPendingPaymentResponse>;
