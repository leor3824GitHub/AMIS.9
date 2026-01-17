using MediatR;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.Get.v1;

public class GetInspectionRequestRequest : IRequest<InspectionRequestResponse>
{
    public Guid Id { get; set; }

    public GetInspectionRequestRequest(Guid id) => Id = id;
}

