using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Specifications;

public sealed class AcceptancesByInspectionIdSpec : Specification<Acceptance>
{
    public AcceptancesByInspectionIdSpec(Guid inspectionId)
    {
        Query.Where(a => a.InspectionId == inspectionId);
    }
}
