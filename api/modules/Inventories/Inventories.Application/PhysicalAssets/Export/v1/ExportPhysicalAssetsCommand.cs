using MediatR;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Export.v1;

public sealed record ExportPhysicalAssetsCommand(
    string? Classification = null,
    string? Location = null,
    string? Condition = null,
    bool? IsDisposed = null) : IRequest<ExportPhysicalAssetsResponse>;

public sealed record ExportPhysicalAssetsResponse(
    string FileName,
    byte[] FileContent,
    string ContentType);

