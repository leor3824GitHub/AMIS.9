using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Search.v1;

public class SearchAssetClassificationRulesSpecs : Specification<AssetClassificationRule>
{
    public SearchAssetClassificationRulesSpecs(SearchAssetClassificationRulesCommand command)
    {
        var query = Query;

        if (!string.IsNullOrWhiteSpace(command.Name))
        {
            query = query.Where(x => x.Name.Contains(command.Name));
        }

        query
            .OrderByDescending(x => x.Id)
            .Skip((command.PageNumber - 1) * command.PageSize)
            .Take(command.PageSize);
    }
}
