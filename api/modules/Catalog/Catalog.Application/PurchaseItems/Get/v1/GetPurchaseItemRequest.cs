using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
public class GetPurchaseItemRequest : IRequest<PurchaseItemResponse>
{
    public Guid Id { get; set; }
    public GetPurchaseItemRequest(Guid id) => Id = id;
}
