using MediatR;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.UpdateCondition.v1;

public sealed record UpdateConditionCommand(
    Guid AssetId,
    string Condition,
    string? Remarks = null) : IRequest<UpdateConditionResponse>;

public sealed record UpdateConditionResponse(
    Guid AssetId,
    string Condition,
    DateTime UpdatedDate);

