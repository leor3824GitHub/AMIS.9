using MediatR;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;
public class GetIssuanceItemRequest : IRequest<IssuanceItemResponse>
{
    public Guid Id { get; set; }
    public GetIssuanceItemRequest(Guid id) => Id = id;
}
