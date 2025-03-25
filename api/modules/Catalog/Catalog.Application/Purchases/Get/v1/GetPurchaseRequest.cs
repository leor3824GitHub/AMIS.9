using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
public class GetPurchaseRequest : IRequest<PurchaseResponse>
{
    public Guid Id { get; set; }
    public GetPurchaseRequest(Guid id) => Id = id;
}
