using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Create.v1;

public sealed class CreateAssetClassificationRuleHandler(
    [FromKeyedServices("inventories:assetclassificationrules")] IRepository<AssetClassificationRule> repository)
    : IRequestHandler<CreateAssetClassificationRuleCommand, CreateAssetClassificationRuleResponse>
{
    public async Task<CreateAssetClassificationRuleResponse> Handle(
        CreateAssetClassificationRuleCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rule = AssetClassificationRule.Create(
            name: request.Name,
            classification: request.Classification,
            minimumCost: request.MinimumCost,
            maximumCost: request.MaximumCost,
            effectiveDate: request.EffectiveDate,
            rcaAccountCode: request.RCAAccountCode,
            priority: request.Priority,
            expiryDate: request.ExpiryDate);

        await repository.AddAsync(rule, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return MapToResponse(rule);
    }

    private static CreateAssetClassificationRuleResponse MapToResponse(AssetClassificationRule rule)
    {
        return new CreateAssetClassificationRuleResponse(
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
