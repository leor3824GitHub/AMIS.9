using MediatR;

namespace AMIS.WebApi.Inventories.Application.AssetMovement.Queries;

public record GetAssetMovementHistoryQuery(Guid AssetId) : IRequest<AssetMovementSummaryDto?>;
