using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkShipped.v1;

public record MarkShippedCommand(Guid PurchaseId) : IRequest<MarkShippedResponse>;
