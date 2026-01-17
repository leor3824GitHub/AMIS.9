using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.ProcurementProjects.Get.v1;

public sealed class GetProcurementProjectSpec : Specification<ProcurementProject>
{
    public GetProcurementProjectSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}

