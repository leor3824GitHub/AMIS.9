using Ardalis.Specification;
using SemexRegistryDomain = AMIS.WebApi.Inventories.Domain.SemexRegistry;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Post.v1;

public sealed class AllSemexRegistriesSpec : Specification<SemexRegistryDomain>
{
    public AllSemexRegistriesSpec()
    {
        // No filters - get all registries
    }
}
