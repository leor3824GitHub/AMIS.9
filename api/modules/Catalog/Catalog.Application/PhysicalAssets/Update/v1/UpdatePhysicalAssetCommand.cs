using MediatR;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.Update.v1;

public sealed record UpdatePhysicalAssetCommand(
    Guid Id,
    string? Location = null,
    string? Condition = null,
    Guid? CurrentCustodianId = null) : IRequest<UpdatePhysicalAssetResponse>;

public sealed record UpdatePhysicalAssetResponse(
    Guid Id,
    string? Location,
    string Condition,
    Guid? CurrentCustodianId);
