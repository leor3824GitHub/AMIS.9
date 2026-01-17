using MediatR;

namespace AMIS.WebApi.Inventories.Application.Purchases.Get.v1;
public class GetPurchaseRequest : IRequest<PurchaseResponse>
{
    public Guid Id { get; set; }
    public GetPurchaseRequest(Guid id) => Id = id;
}

