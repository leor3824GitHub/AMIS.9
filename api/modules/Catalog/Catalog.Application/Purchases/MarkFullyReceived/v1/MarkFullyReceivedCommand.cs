using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkFullyReceived.v1;

public record MarkFullyReceivedCommand(Guid PurchaseId) : IRequest<MarkFullyReceivedResponse>;
