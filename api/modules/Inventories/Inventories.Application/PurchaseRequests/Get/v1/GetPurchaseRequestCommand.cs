using MediatR;
using AMIS.WebApi.Inventories.Application.PurchaseRequests.Get.v1;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.Get.v1;

public sealed record GetPurchaseRequestCommand(Guid Id) : IRequest<PurchaseRequestResponse?>;

