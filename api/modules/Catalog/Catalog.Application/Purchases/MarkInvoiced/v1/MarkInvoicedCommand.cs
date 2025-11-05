using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkInvoiced.v1;

public record MarkInvoicedCommand(Guid PurchaseId) : IRequest<MarkInvoicedResponse>;
