using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.AssetClassificationRules.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Search.v1;

public class SearchAssetClassificationRulesCommand : PaginationFilter, IRequest<PagedList<AssetClassificationRuleResponse>>
{
    public string? Name { get; set; }
}
