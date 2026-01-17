using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.Get.v1;

public class GetInspectionRequestSpecs : Specification<InspectionRequest, InspectionRequestResponse>
{
    public GetInspectionRequestSpecs(Guid id)
    {
        Query
            .Where(i => i.Id == id);
    }
}

