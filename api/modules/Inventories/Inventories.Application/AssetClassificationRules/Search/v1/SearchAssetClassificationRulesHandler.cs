using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.AssetClassificationRules.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Search.v1;

public sealed class SearchAssetClassificationRulesHandler(
    [FromKeyedServices("inventories:assetclassificationrules")] IReadRepository<AssetClassificationRule> repository)
    : IRequestHandler<SearchAssetClassificationRulesCommand, PagedList<AssetClassificationRuleResponse>>
{
    public async Task<PagedList<AssetClassificationRuleResponse>> Handle(
        SearchAssetClassificationRulesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAssetClassificationRulesSpecs(request);
        var total = await repository.CountAsync(cancellationToken);
        var rules = await repository.ListAsync(spec, cancellationToken);

        var responses = rules
            .Select(MapToResponse)
            .ToList();

        return new PagedList<AssetClassificationRuleResponse>(
            responses, 
            request.PageNumber, 
            request.PageSize, 
            total);
    }

    private static AssetClassificationRuleResponse MapToResponse(AssetClassificationRule rule)
    {
        return new AssetClassificationRuleResponse(
            Id: rule.Id,
            Name: rule.Name,
            Classification: rule.Classification,
            MinimumCost: rule.MinimumCost,
            MaximumCost: rule.MaximumCost,
            EffectiveDate: rule.EffectiveDate,
            ExpiryDate: rule.ExpiryDate,
            RCAAccountCode: rule.RCAAccountCode,
            Priority: rule.Priority,
            IsActive: rule.IsActive);
    }
}
