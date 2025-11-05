using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.ReleaseFromHold.v1;

public sealed record ReleasePurchaseFromHoldCommand(Guid PurchaseId) : IRequest<ReleasePurchaseFromHoldResponse>;
