using MediatR;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Create.v1;

public sealed record CreatePhysicalAssetCommand(
    string PropertyCode,
    Guid ProductId,
    decimal AcquisitionCost,
    DateTime AcquisitionDate,
    int Quantity = 1,
    string? SerialNumber = null,
    string? ModelNumber = null,
    Guid? ParentAssetId = null) : IRequest<CreatePhysicalAssetResponse>;

public sealed record CreatePhysicalAssetResponse(
    Guid Id,
    string PropertyCode,
    decimal AcquisitionCost,
    PropertyClassification Classification,
    Guid? ParentAssetId);

