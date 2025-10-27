using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;

public class GetInspectionRequestSpecs : Specification<InspectionRequest, InspectionRequestResponse>
{
    public GetInspectionRequestSpecs(Guid id)
    {
        Query
            .Where(i => i.Id == id);
    }
}
