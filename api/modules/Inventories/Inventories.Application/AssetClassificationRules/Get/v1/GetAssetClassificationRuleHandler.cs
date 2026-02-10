using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Get.v1;

public sealed class GetAssetClassificationRuleHandler(
    [FromKeyedServices("inventories:assetclassificationrules")] IReadRepository<AssetClassificationRule> repository)
    : IRequestHandler<GetAssetClassificationRuleRequest, AssetClassificationRuleResponse>
{
    public async Task<AssetClassificationRuleResponse> Handle(
        GetAssetClassificationRuleRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetAssetClassificationRuleSpecs(request.Id);
        var rule = await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new AssetClassificationRuleNotFoundException(request.Id);

        return MapToResponse(rule);
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
