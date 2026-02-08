using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Create.v1;

public sealed record CreateAssetClassificationRuleResponse(
    Guid Id,
    string Name,
    PropertyClassification Classification,
    decimal MinimumCost,
    decimal MaximumCost,
    DateTime EffectiveDate,
    DateTime? ExpiryDate,
    string RCAAccountCode,
    int Priority,
    bool IsActive);
