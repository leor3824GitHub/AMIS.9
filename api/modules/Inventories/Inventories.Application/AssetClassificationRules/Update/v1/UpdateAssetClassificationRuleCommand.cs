using MediatR;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Update.v1;

public sealed record UpdateAssetClassificationRuleCommand(
    Guid Id,
    string Name,
    PropertyClassification Classification,
    decimal MinimumCost,
    decimal MaximumCost,
    DateTime EffectiveDate,
    string RCAAccountCode,
    int Priority,
    DateTime? ExpiryDate = null) : IRequest<UpdateAssetClassificationRuleResponse>;
