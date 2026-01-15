using MediatR;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.AssignCustodian.v1;

public sealed record AssignCustodianCommand(
    Guid AssetId,
    Guid CustodianId) : IRequest<AssignCustodianResponse>;

public sealed record AssignCustodianResponse(
    Guid AssetId,
    Guid CustodianId,
    DateTime AssignmentDate);
