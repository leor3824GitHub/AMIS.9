using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Update.v1;

public sealed class UpdateAssetClassificationRuleHandler(
    [FromKeyedServices("inventories:assetclassificationrules")] IRepository<AssetClassificationRule> repository)
    : IRequestHandler<UpdateAssetClassificationRuleCommand, UpdateAssetClassificationRuleResponse>
{
    public async Task<UpdateAssetClassificationRuleResponse> Handle(
        UpdateAssetClassificationRuleCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rule = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AssetClassificationRuleNotFoundException(request.Id);

        // Update properties
        rule.SetName(request.Name);
        rule.SetMinimumCost(request.MinimumCost);
        rule.SetMaximumCost(request.MaximumCost);
        rule.SetEffectiveDate(request.EffectiveDate);
        rule.SetExpiryDate(request.ExpiryDate);
        rule.SetRCAAccountCode(request.RCAAccountCode);
        rule.SetPriority(request.Priority);

        await repository.UpdateAsync(rule, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return MapToResponse(rule);
    }

    private static UpdateAssetClassificationRuleResponse MapToResponse(AssetClassificationRule rule)
    {
        return new UpdateAssetClassificationRuleResponse(
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
