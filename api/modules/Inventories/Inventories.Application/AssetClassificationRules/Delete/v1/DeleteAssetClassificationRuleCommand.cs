using MediatR;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Delete.v1;

public sealed record DeleteAssetClassificationRuleCommand(Guid Id) : IRequest;
