using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Specifications;

public sealed class AcceptancesByInspectionIdSpec : Specification<Acceptance>
{
    public AcceptancesByInspectionIdSpec(Guid inspectionId)
    {
        Query.Where(a => a.InspectionId == inspectionId);
    }
}

