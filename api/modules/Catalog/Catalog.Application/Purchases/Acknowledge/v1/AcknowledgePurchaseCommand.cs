using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Acknowledge.v1;

public sealed record AcknowledgePurchaseCommand(Guid PurchaseId) : IRequest<AcknowledgePurchaseResponse>;
