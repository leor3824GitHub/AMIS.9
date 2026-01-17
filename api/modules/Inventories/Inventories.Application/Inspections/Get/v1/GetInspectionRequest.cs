using MediatR;

namespace AMIS.WebApi.Inventories.Application.Inspections.Get.v1;

public class GetInspectionRequest : IRequest<InspectionResponse>
{
    public Guid Id { get; set; }

    public GetInspectionRequest(Guid id) => Id = id;
}

