using MediatR;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.RecordDepreciation.v1;

public sealed record RecordDepreciationCommand(
    Guid AssetId,
    decimal Amount,
    DateTime DepreciationDate) : IRequest<RecordDepreciationResponse>;

public sealed record RecordDepreciationResponse(
    Guid AssetId,
    decimal Amount,
    decimal AccumulatedDepreciation,
    decimal BookValue,
    DateTime DepreciationDate);
