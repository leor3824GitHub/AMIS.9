using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Get.v1;

public class GetAssetClassificationRuleSpecs : Specification<AssetClassificationRule>
{
    public GetAssetClassificationRuleSpecs(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}
