using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.ProcurementProjects.Get.v1;

public sealed class GetProcurementProjectSpec : Specification<ProcurementProject>
{
    public GetProcurementProjectSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
