using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;

public class GetInspectionItemRequest : IRequest<InspectionItemResponse>
{
    public Guid Id { get; set; }

    public GetInspectionItemRequest(Guid id) => Id = id;
}
