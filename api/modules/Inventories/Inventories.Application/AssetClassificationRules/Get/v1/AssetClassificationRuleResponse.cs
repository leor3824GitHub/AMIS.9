using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Get.v1;

public sealed record AssetClassificationRuleResponse(
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
