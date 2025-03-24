using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
public class GetInventoryRequest : IRequest<InventoryResponse>
{
    public Guid Id { get; set; }
    public GetInventoryRequest(Guid id) => Id = id;
}
