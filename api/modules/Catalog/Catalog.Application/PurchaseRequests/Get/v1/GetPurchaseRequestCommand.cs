using MediatR;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;

public sealed record GetPurchaseRequestCommand(Guid Id) : IRequest<PurchaseRequestResponse?>;
