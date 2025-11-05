using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkPartiallyReceived.v1;

public sealed record MarkPartiallyReceivedCommand(Guid PurchaseId) : IRequest<MarkPartiallyReceivedResponse>;
