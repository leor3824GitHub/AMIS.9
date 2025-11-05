using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkClosed.v1;

public sealed record MarkClosedCommand(Guid PurchaseId) : IRequest<MarkClosedResponse>;
