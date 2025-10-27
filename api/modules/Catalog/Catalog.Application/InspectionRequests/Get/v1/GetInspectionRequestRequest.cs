using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;

public class GetInspectionRequestRequest : IRequest<InspectionRequestResponse>
{
    public Guid Id { get; set; }

    public GetInspectionRequestRequest(Guid id) => Id = id;
}
