using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Cancel.v1;

public record CancelPurchaseCommand(Guid PurchaseId, string Reason) : IRequest<CancelPurchaseResponse>;
