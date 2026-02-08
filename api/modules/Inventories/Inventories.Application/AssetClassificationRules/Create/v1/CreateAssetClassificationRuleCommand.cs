using MediatR;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Create.v1;

public sealed record CreateAssetClassificationRuleCommand(
    string Name,
    PropertyClassification Classification,
    decimal MinimumCost,
    decimal MaximumCost,
    DateTime EffectiveDate,
    string RCAAccountCode,
    int Priority,
    DateTime? ExpiryDate = null) : IRequest<CreateAssetClassificationRuleResponse>;
